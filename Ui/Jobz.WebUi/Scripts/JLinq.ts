class SortOption {
    propSelector: (item: any) => any;
    isAsc: boolean;
    constructor(propSelector: (item: any) => any, isAsc: boolean) {
        this.propSelector = propSelector;
        this.isAsc = isAsc;
    }
}

class JLinq<T> {
    items: any[];

    constructor(list: T[]) {
        this.items = [];
        for (var i = 0; i < list.length; i++) {
            this.items.push(list[i]);
        }
    }
    where(clause: (item: T) => boolean): JLinq<T> {
        var result: T[] = [];
        this.foreach((x: any): void => {
            if (clause(x)) {
                result.push(x);
            }
        });
        return new JLinq<T>(result);
    };
    //This does not compare complex objects properly, its fine for 1 dimensional arrays
    distinct(clause: (item: T) => any): JLinq<T> {
        var obj = {};
        var result: T[] = [];
        this.foreach((x: any): void => {
            var item = clause(x);
            if (obj[item] == null) {
                obj[item] = true;
                result.push(item);
            }
        });
        return new JLinq<T>(result);
    };
    orderBy(clause: (item: T) => any): JLinq<T> {
        var sortFunc = (a, b) => {
            var x = clause(a);
            var y = clause(b);
            if (typeof (x) === typeof (1) && typeof (y) === typeof (1)) {
                return ((x - y) < 0) ? -1 : ((x - y) > 0) ? 1 : 0;
            } else {//sort alphanumeric
                return ((x < y) ? -1 : ((x > y) ? 1 : 0));
            }
        };
        return new JLinq<T>(this.items.sort(sortFunc));
    };
    orderByDescending(clause: (item: T) => any): JLinq<T> {
        const sortFunc = (a, b) => {
            var x = clause(b);
            var y = clause(a);
            if (typeof (x) === typeof (1) && typeof (y) === typeof (1)) {
                return ((x - y) < 0) ? -1 : ((x - y) > 0) ? 1 : 0;
            } else {//sort alphanumeric
                return ((x < y) ? -1 : ((x > y) ? 1 : 0));
            }
        };
        return new JLinq<T>(this.items.sort(sortFunc));
    };
    orderBySortOptions(sortOptions: SortOption[]): JLinq<T> {
        var sortFunc = (a, b) => {
            var result = 0;
            for (let i = 0; i < sortOptions.length; i++) {
                let x;
                let y;
                const currentOption = sortOptions[i];
                if (currentOption.isAsc) {
                    x = currentOption.propSelector(a);
                    y = currentOption.propSelector(b);
                } else {
                    x = currentOption.propSelector(b);
                    y = currentOption.propSelector(a);
                }
                if (x !== y) {
                    if (typeof (x) === typeof (1) && typeof (y) === typeof (1)) {
                        //sort numeric
                        result = ((x - y) < 0) ? -1 : ((x - y) > 0) ? 1 : 0;
                    } else {
                        //sort alphanumeric
                        result = ((x < y) ? -1 : ((x > y) ? 1 : 0));
                    }
                    return result;
                }
            }
            return result;
        };
        return new JLinq<T>(this.items.sort(sortFunc));
    };
    any(clause?: (item: T) => boolean): boolean {
        if (clause != null) {
            return this.where(clause).any();
        } else {
            return this.items.length > 0;
        }
    };
    average(clause: (item: T) => number, numberOfDecimalPlaces: number): number {
        var result = 0;
        if (clause != null) {
            var jl = new JLinq(this.select(clause).items);
            var numAry = jl.items;
            var sum = jl.sum(function (x) { return x; });
            var count = numAry.length;
            result = (sum / count);
            result = Math.round(result * Math.pow(10, numberOfDecimalPlaces)) / Math.pow(10, numberOfDecimalPlaces);
        }
        return result;
    };
    select<TR>(clause: (item: T) => TR): JLinq<TR> {
        var result: TR[] = [];
        this.foreach((x: T): void => {
            result.push(clause(x));
        });
        return new JLinq<TR>(result);
    };
    first(clause?: (item: any) => boolean): T {
        if (clause != null) {
            return this.where(clause).first(null);
        } else {
            return (this.items.length > 0
                ? this.items[0]
                : null);
        }
    };
    last(clause?: (item: any) => boolean): T {
        if (clause != null) {
            return this.where(clause).last();
        } else {
            return (this.items.length > 0
                ? this.items[this.items.length - 1]
                : null);
        }
    };
    min(clause: (item: any) => number): number {
        const result = this.select(clause).orderBy(x => x).first();
        return result;
    };
    max(clause: (item: any) => number): number {
        const result = this.select(clause).orderBy(x => x).last();
        return result;
    };
    sum(clause: (item: any) => number): number {
        var ary = this.select(clause).items;
        var sum = 0;
        for (var i in ary) {
            sum += ary[i];
        }
        return sum;
    };
    foreach(func: (item: T) => void): void {
        for (let i = 0; i < this.items.length; i++) {
            func(this.items[i]);
        }
    }
}
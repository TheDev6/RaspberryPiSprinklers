var SortOption = /** @class */ (function () {
    function SortOption(propSelector, isAsc) {
        this.propSelector = propSelector;
        this.isAsc = isAsc;
    }
    return SortOption;
}());
var JLinq = /** @class */ (function () {
    function JLinq(list) {
        this.items = [];
        for (var i = 0; i < list.length; i++) {
            this.items.push(list[i]);
        }
    }
    JLinq.prototype.where = function (clause) {
        var result = [];
        this.foreach(function (x) {
            if (clause(x)) {
                result.push(x);
            }
        });
        return new JLinq(result);
    };
    ;
    //This does not compare complex objects properly, its fine for 1 dimensional arrays
    JLinq.prototype.distinct = function (clause) {
        var obj = {};
        var result = [];
        this.foreach(function (x) {
            var item = clause(x);
            if (obj[item] == null) {
                obj[item] = true;
                result.push(item);
            }
        });
        return new JLinq(result);
    };
    ;
    JLinq.prototype.orderBy = function (clause) {
        var sortFunc = function (a, b) {
            var x = clause(a);
            var y = clause(b);
            if (typeof (x) === typeof (1) && typeof (y) === typeof (1)) {
                return ((x - y) < 0) ? -1 : ((x - y) > 0) ? 1 : 0;
            }
            else { //sort alphanumeric
                return ((x < y) ? -1 : ((x > y) ? 1 : 0));
            }
        };
        return new JLinq(this.items.sort(sortFunc));
    };
    ;
    JLinq.prototype.orderByDescending = function (clause) {
        var sortFunc = function (a, b) {
            var x = clause(b);
            var y = clause(a);
            if (typeof (x) === typeof (1) && typeof (y) === typeof (1)) {
                return ((x - y) < 0) ? -1 : ((x - y) > 0) ? 1 : 0;
            }
            else { //sort alphanumeric
                return ((x < y) ? -1 : ((x > y) ? 1 : 0));
            }
        };
        return new JLinq(this.items.sort(sortFunc));
    };
    ;
    JLinq.prototype.orderBySortOptions = function (sortOptions) {
        var sortFunc = function (a, b) {
            var result = 0;
            for (var i = 0; i < sortOptions.length; i++) {
                var x = void 0;
                var y = void 0;
                var currentOption = sortOptions[i];
                if (currentOption.isAsc) {
                    x = currentOption.propSelector(a);
                    y = currentOption.propSelector(b);
                }
                else {
                    x = currentOption.propSelector(b);
                    y = currentOption.propSelector(a);
                }
                if (x !== y) {
                    if (typeof (x) === typeof (1) && typeof (y) === typeof (1)) {
                        //sort numeric
                        result = ((x - y) < 0) ? -1 : ((x - y) > 0) ? 1 : 0;
                    }
                    else {
                        //sort alphanumeric
                        result = ((x < y) ? -1 : ((x > y) ? 1 : 0));
                    }
                    return result;
                }
            }
            return result;
        };
        return new JLinq(this.items.sort(sortFunc));
    };
    ;
    JLinq.prototype.any = function (clause) {
        if (clause != null) {
            return this.where(clause).any();
        }
        else {
            return this.items.length > 0;
        }
    };
    ;
    JLinq.prototype.average = function (clause, numberOfDecimalPlaces) {
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
    ;
    JLinq.prototype.select = function (clause) {
        var result = [];
        this.foreach(function (x) {
            result.push(clause(x));
        });
        return new JLinq(result);
    };
    ;
    JLinq.prototype.first = function (clause) {
        if (clause != null) {
            return this.where(clause).first(null);
        }
        else {
            return (this.items.length > 0
                ? this.items[0]
                : null);
        }
    };
    ;
    JLinq.prototype.last = function (clause) {
        if (clause != null) {
            return this.where(clause).last();
        }
        else {
            return (this.items.length > 0
                ? this.items[this.items.length - 1]
                : null);
        }
    };
    ;
    JLinq.prototype.min = function (clause) {
        var result = this.select(clause).orderBy(function (x) { return x; }).first();
        return result;
    };
    ;
    JLinq.prototype.max = function (clause) {
        var result = this.select(clause).orderBy(function (x) { return x; }).last();
        return result;
    };
    ;
    JLinq.prototype.sum = function (clause) {
        var ary = this.select(clause).items;
        var sum = 0;
        for (var i in ary) {
            sum += ary[i];
        }
        return sum;
    };
    ;
    JLinq.prototype.foreach = function (func) {
        for (var i = 0; i < this.items.length; i++) {
            func(this.items[i]);
        }
    };
    return JLinq;
}());
//# sourceMappingURL=JLinq.js.map
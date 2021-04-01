namespace SprinklerApi.Data.Tables
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class WaterEventHistory
    {
        public Guid WaterEventHistoryUid { get; set; }
        public string ZoneName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public TimeSpan? Duration
        {
            get
            {
                TimeSpan? result = null;
                if (this.End != null)
                {
                    result = this.End - this.Start;
                }
                return result;
            }
        }

        public string Message { get; set; }

        public static void RegisterForSqlLite(ModelBuilder builder)
        {
            builder.Entity<WaterEventHistory>()
                .HasKey(e => e.WaterEventHistoryUid);

            builder.Entity<WaterEventHistory>()
                .Property<Guid>(e => e.WaterEventHistoryUid)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<WaterEventHistory>()
                .Property<string>(e => e.ZoneName)
                .HasColumnType(SqlLiteTypes.TEXT);

            builder.Entity<WaterEventHistory>()
                .Property<DateTime>(e => e.Start)
                .HasColumnType(SqlLiteTypes.TEXT);

            builder.Entity<WaterEventHistory>()
                .Property<DateTime?>(e => e.End)
                .HasColumnType(SqlLiteTypes.TEXT);

            builder.Entity<WaterEventHistory>()
                .Property<string>(e => e.Message)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();
        }
    }
}

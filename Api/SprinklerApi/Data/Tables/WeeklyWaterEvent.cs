namespace SprinklerApi.Data.Tables
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class WeeklyWaterEvent
    {
        public Guid WeeklyWaterEventUid { get; set; }
        public string ZoneName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public static void RegisterForSqlLite(ModelBuilder builder)
        {
            //The primary key
            builder.Entity<WeeklyWaterEvent>()
                .HasKey(e => e.WeeklyWaterEventUid);

            builder.Entity<WeeklyWaterEvent>()
                .Property<Guid>(e => e.WeeklyWaterEventUid)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<WeeklyWaterEvent>()
                .Property<string>(e => e.ZoneName)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<WeeklyWaterEvent>()
                .Property<DayOfWeek>(e => e.DayOfWeek)
                .HasColumnType(SqlLiteTypes.INT)
                .IsRequired();

            builder.Entity<WeeklyWaterEvent>()
                .Property<TimeSpan>(e => e.Start)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<WeeklyWaterEvent>()
                .Property<TimeSpan>(e => e.End)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();
        }
    }
}

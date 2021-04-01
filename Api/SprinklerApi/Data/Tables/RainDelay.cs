namespace SprinklerApi.Data.Tables
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class RainDelay
    {
        public Guid RainDelayUid { get; set; }
        public DateTime RainDelayExpireDate { get; set; }

        public static void RegisterForSqlLite(ModelBuilder builder)
        {
            builder.Entity<RainDelay>()
                .HasKey(e => e.RainDelayUid);

            builder.Entity<RainDelay>()
                .Property<Guid>(e => e.RainDelayUid)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<RainDelay>()
                .Property<DateTime>(e => e.RainDelayExpireDate)
                .HasColumnType("TEXT")
                .IsRequired();
        }
    }
}

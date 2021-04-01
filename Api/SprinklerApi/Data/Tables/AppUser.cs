namespace SprinklerApi.Data.Tables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class AppUser
    {
        public Guid UserUid { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string PasswordBase64Hash { get; set; }
        public string CommaSeparatedClaims { get; set; }

        public List<string> Claims
        {
            get
            {
                var result = new List<string>();
                if (!string.IsNullOrWhiteSpace(CommaSeparatedClaims))
                {
                    result = CommaSeparatedClaims.Split(',').ToList();
                }
                return result;
            }
        }

        public static void RegisterForSqlLite(ModelBuilder builder)
        {
            builder.Entity<AppUser>()
                .HasKey(e => e.UserUid);

            builder.Entity<AppUser>()
                .Property<Guid>(e => e.UserUid)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<AppUser>()
                .Property<string>(e => e.Username)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<AppUser>()
                .Property<string>(e => e.Salt)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<AppUser>()
                .Property<string>(e => e.PasswordBase64Hash)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();

            builder.Entity<AppUser>()
                .Property<string>(e => e.CommaSeparatedClaims)
                .HasColumnType(SqlLiteTypes.TEXT)
                .IsRequired();
        }
    }
}

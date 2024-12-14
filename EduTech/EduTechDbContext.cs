﻿using System.Security.Claims;
using EduTech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduTech
{
    public class EduTechDbContext : IdentityDbContext<ApplicationUser>
    {
        public EduTechDbContext(DbContextOptions<EduTechDbContext> options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Class>()
                    .HasMany(c => c.Lecturers)
                    .WithMany(u => u.ClassesTeaching)
                    .UsingEntity(j => j.ToTable("ClassLecturers"));

            builder.Entity<Class>()
                    .HasMany(c => c.Students)
                    .WithMany(u => u.ClassesAttending)
                    .UsingEntity(j => j.ToTable("ClassStudents"));


            // Rename Identity tables
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }

    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Client>()
                .Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.Address).IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.City).IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.PostalCode).IsRequired();
            modelBuilder.Entity<Client>()
                .Property(c => c.Country).IsRequired();

            modelBuilder.Entity<Status>().HasKey(s => s.Id);
            modelBuilder.Entity<Status>()
                .Property(s => s.StatusName).IsRequired();

            modelBuilder.Entity<TeamMember>().HasKey(t => t.Id);
            modelBuilder.Entity<TeamMember>()
                .Property(t => t.Name)
                .IsRequired();
            modelBuilder.Entity<TeamMember>()
                .Property(t => t.Username)
                .IsRequired();
            modelBuilder.Entity<TeamMember>()
                .Property(t => t.Password)
                .IsRequired(false);
            modelBuilder.Entity<TeamMember>()
                .Property(t => t.Email)
                .IsRequired();
            modelBuilder.Entity<TeamMember>()
                .Property(t => t.HoursPerWeek)
                .IsRequired();
            modelBuilder.Entity<TeamMember>()
                .Property(t => t.Role)
                .IsRequired()
                .HasConversion<string>();
            modelBuilder.Entity<TeamMember>()
                .HasOne<Status>(t => t.Status)
                .WithMany()
                .HasForeignKey("StatusId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Project>()
                .Property(p => p.Description)
                .IsRequired(false);
            modelBuilder.Entity<Project>()
                .HasOne<Status>(p => p.Status)
                .WithMany()
                .HasForeignKey("StatusId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            modelBuilder.Entity<Project>()
                .HasOne<Client>(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            modelBuilder.Entity<Project>()
                .HasOne<TeamMember>(p => p.Lead)
                .WithMany()
                .HasForeignKey(p => p.LeadId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Activity>().HasKey(a => a.Id);
            modelBuilder.Entity<Activity>()
                .Property(a => a.Date)
                .IsRequired();
            modelBuilder.Entity<Activity>()
                .Property(a => a.Description)
                .IsRequired(false);
            modelBuilder.Entity<Activity>()
                .Property(a => a.Overtime)
                .IsRequired(false);
            modelBuilder.Entity<Activity>()
                .HasOne<Client>(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            modelBuilder.Entity<Activity>()
                .HasOne<Category>(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            modelBuilder.Entity<Activity>()
                .HasOne<Project>(a => a.Project)
                .WithMany()
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            modelBuilder.Entity<Activity>()
                .Property(a => a.Hours)
                .IsRequired();
            modelBuilder.Entity<Activity>()
                .HasOne<TeamMember>(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }

    }
}

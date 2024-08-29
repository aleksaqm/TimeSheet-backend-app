using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ModelBuilders
{
    internal class ProjectModelBuilder
    {
        private readonly ModelBuilder _modelBuilder;
        public ProjectModelBuilder(ModelBuilder builder)
        {
            _modelBuilder = builder;
        }

        public void OnModelCreating()
        {
            _modelBuilder.Entity<Project>().HasKey(p => p.Id);
            _modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired();
            _modelBuilder.Entity<Project>()
                .Property(p => p.Description)
                .IsRequired(false);
            _modelBuilder.Entity<Project>()
                .HasOne<Status>(p => p.Status)
                .WithMany()
                .HasForeignKey("StatusId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            _modelBuilder.Entity<Project>()
                .HasOne<Client>(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            _modelBuilder.Entity<Project>()
                .HasOne<TeamMember>(p => p.Lead)
                .WithMany()
                .HasForeignKey(p => p.LeadId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}

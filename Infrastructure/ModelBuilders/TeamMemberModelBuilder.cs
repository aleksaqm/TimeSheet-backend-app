using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ModelBuilders
{
    internal class TeamMemberModelBuilder
    {
        private readonly ModelBuilder _modelBuilder;
        public TeamMemberModelBuilder(ModelBuilder builder)
        {
            _modelBuilder = builder;
        }

        public void OnModelCreating()
        {
            _modelBuilder.Entity<TeamMember>().HasKey(t => t.Id);
            _modelBuilder.Entity<TeamMember>()
                .Property(t => t.Name)
                .IsRequired();
            _modelBuilder.Entity<TeamMember>()
                .Property(t => t.Username)
                .IsRequired();
            _modelBuilder.Entity<TeamMember>()
                .Property(t => t.Password)
                .IsRequired(false);
            _modelBuilder.Entity<TeamMember>()
                .Property(t => t.Email)
                .IsRequired();
            _modelBuilder.Entity<TeamMember>()
                .Property(t => t.HoursPerWeek)
                .IsRequired();
            _modelBuilder.Entity<TeamMember>()
                .Property(t => t.Role)
                .IsRequired()
                .HasConversion<string>();
            _modelBuilder.Entity<TeamMember>()
                .HasOne<Status>(t => t.Status)
                .WithMany()
                .HasForeignKey("StatusId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            _modelBuilder.Entity<TeamMember>()
                .HasIndex(a => a.Username);
            _modelBuilder.Entity<TeamMember>()
                .HasIndex(a => a.Email);
        }
    }
}

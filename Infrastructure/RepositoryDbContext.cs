using Domain.Entities;
using Infrastructure.ModelBuilders;
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
            var categoryModelBuilder = new CategoryModelBuilder(modelBuilder);
            categoryModelBuilder.OnModelCreating();

            var clientModelBuilder = new ClientModelBuilder(modelBuilder);
            clientModelBuilder.OnModelCreating();

            var statusModelBuilder = new StatusModelBuilder(modelBuilder);
            statusModelBuilder.OnModelCreating();

            var teamMemberModelBuilder = new TeamMemberModelBuilder(modelBuilder);
            teamMemberModelBuilder.OnModelCreating();

            var projectModelBuilder = new ProjectModelBuilder(modelBuilder);
            projectModelBuilder.OnModelCreating();

            var activityModelBuilder = new ActivityModelBuilder(modelBuilder);
            activityModelBuilder.OnModelCreating();

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }

    }
}

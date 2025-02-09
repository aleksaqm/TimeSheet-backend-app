using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ModelBuilders
{
    internal class StatusModelBuilder
    {
        private readonly ModelBuilder _modelBuilder;
        public StatusModelBuilder(ModelBuilder builder)
        {
            _modelBuilder = builder;
        }

        public void OnModelCreating()
        {
            _modelBuilder.Entity<Status>().HasKey(s => s.Id);
            _modelBuilder.Entity<Status>()
                .Property(s => s.StatusName).IsRequired();
        }
    }
}

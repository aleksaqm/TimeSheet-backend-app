using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ModelBuilders
{
    internal class CategoryModelBuilder
    {
        private readonly ModelBuilder _modelBuilder;
        public CategoryModelBuilder(ModelBuilder builder)
        {
            _modelBuilder = builder;
        }

        public void OnModelCreating()
        {
            _modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            _modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();
        }
    }
}

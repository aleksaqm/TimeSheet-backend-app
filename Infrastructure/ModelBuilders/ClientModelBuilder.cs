using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ModelBuilders
{
    internal class ClientModelBuilder
    {
        private readonly ModelBuilder _modelBuilder;
        public ClientModelBuilder(ModelBuilder builder)
        {
            _modelBuilder = builder;
        }

        public void OnModelCreating()
        {
            _modelBuilder.Entity<Client>().HasKey(c => c.Id);
            _modelBuilder.Entity<Client>()
                .Property(c => c.Name).IsRequired();
            _modelBuilder.Entity<Client>()
                .Property(c => c.Address).IsRequired();
            _modelBuilder.Entity<Client>()
                .Property(c => c.City).IsRequired();
            _modelBuilder.Entity<Client>()
                .Property(c => c.PostalCode).IsRequired();
            _modelBuilder.Entity<Client>()
                .Property(c => c.Country).IsRequired();
            _modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
        }
    }
}

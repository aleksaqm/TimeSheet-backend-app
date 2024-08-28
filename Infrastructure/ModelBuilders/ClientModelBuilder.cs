using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}

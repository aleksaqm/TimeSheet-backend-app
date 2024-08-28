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

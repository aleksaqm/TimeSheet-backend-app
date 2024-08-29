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

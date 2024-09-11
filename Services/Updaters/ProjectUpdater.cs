using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Updaters
{
    public class ProjectUpdater
    {
        public static void Update(Project source, Project target)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            target.CustomerId = source.CustomerId;
            target.LeadId = source.LeadId;
            target.Status.StatusName = source.Status.StatusName;
        }
    }
}

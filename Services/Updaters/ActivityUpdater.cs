using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Updaters
{
    public class ActivityUpdater
    {
        public static void Update(Activity source, Activity targer)
        {
            targer.Date = source.Date;
            targer.CategoryId = source.CategoryId;
            targer.ClientId = source.ClientId;
            targer.ProjectId = source.ProjectId;
            targer.UserId = source.UserId;
            targer.Description = source.Description;
            targer.Hours = source.Hours;
            targer.Overtime = source.Overtime;
        }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore.Update;
using Services.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Updaters
{
    public class TeamMemberUpdater
    {
        public static void Update(TeamMember source, TeamMember target)
        {
            target.Name = source.Name;
            target.Username = source.Username;
            target.Email = source.Email;
            target.HoursPerWeek = source.HoursPerWeek;
            target.Status.StatusName = source.Status.StatusName;
            target.Role = source.Role;
        }
    }
}

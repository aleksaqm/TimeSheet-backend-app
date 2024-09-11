using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IClientRepository ClientRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITeamMemberRepository TeamMemberRepository { get; }
        IActivityRepository ActivityRepository { get; }
        Task SaveChangesAsync();
    }
}

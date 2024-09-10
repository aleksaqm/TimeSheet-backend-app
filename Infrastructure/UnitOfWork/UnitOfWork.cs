using Domain.Repositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly RepositoryDbContext _context;
        private ICategoryRepository _categoryRepository;
        private IClientRepository _clientRepository;
        private IProjectRepository _projectRepository;
        private ITeamMemberRepository _teamMemberRepository;
        private IActivityRepository _activityRepository;
        

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository is null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }

        public IClientRepository ClientRepository
        {
            get
            {
                if (_clientRepository is null)
                {
                    _clientRepository = new ClientRepository(_context);
                }
                return _clientRepository;
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                if (_projectRepository is null)
                {
                    _projectRepository = new ProjectRepository(_context);
                }
                return _projectRepository;
            }
        }

        public ITeamMemberRepository TeamMemberRepository
        {
            get
            {
                if (_teamMemberRepository is null)
                {
                    _teamMemberRepository = new TeamMemberRepository(_context);
                }
                return _teamMemberRepository;
            }
        }

        public IActivityRepository ActivityRepository
        {
            get
            {
                if (_activityRepository is null)
                {
                    _activityRepository = new ActivityRepository(_context);
                }
                return _activityRepository;
            }
        }

        public UnitOfWork(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

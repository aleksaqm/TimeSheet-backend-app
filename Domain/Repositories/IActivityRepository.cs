﻿using Domain.Entities;
using Shared;

namespace Domain.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(Guid id);
        Task<IEnumerable<Activity>> GetAllAsync();
        Task<IEnumerable<Activity>> GetForOneDay(DateTime day, Guid userId);
        Task<IEnumerable<Activity>> GetForPeriod(DateTime startDate, DateTime endDate, Guid userId);
        Task AddAsync(Activity activity);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Activity>> GetForReport(GetReportDto reportFilter);
    }
}

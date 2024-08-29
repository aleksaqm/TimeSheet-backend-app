using Shared;

namespace Service.Abstractions
{
    public interface ICategoryService
    {
        Task<CategoryUpdateDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<CategoryUpdateDto>> GetAllAsync();
        Task<CategoryUpdateDto?> AddAsync(CategoryCreateDto categoryDto);
        Task<CategoryUpdateDto?> UpdateAsync(CategoryUpdateDto categoryDto);
        Task<bool> DeleteAsync(Guid id);
    }
}

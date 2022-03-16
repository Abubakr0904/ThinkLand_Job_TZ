using webapp.Core.IRepositories;
using webapp.Data;
using webapp.Entities;
using Microsoft.EntityFrameworkCore;

namespace webapp.Core.Repositories;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context, ILogger logger)
        :base(context, logger) {  }

    public override async Task<bool> UpdateAsync(Category entity)
    {
        try
        {
            var category = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if(category != null)
            {
                category.Name = entity.Name;

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} UpdateAsync method", typeof(CategoryRepository));
            return false;
        }
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var category = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null) 
            {
                _dbSet.Remove(category);
                return true;
            }
            return false;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} DeleteAsync method", typeof(CategoryRepository));
            return false;
        }
    }

    public override async Task<Category> GetByIdAsync(Guid id)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} GetByIdAsync method", typeof(CategoryRepository));
            return null;
        }
    }
    public override async Task<IEnumerable<Category>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} GetAllAsync method", typeof(CategoryRepository));
            return null;
        }
    }
    public async override Task<bool> AddAsync(Category entity)
    {
        try
        {
            var existing = await GetByIdAsync(entity.Id);
            if(existing != null)
                return false;
            
            await _dbSet.AddAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} AddAsync method", typeof(CategoryRepository));
            return false;
        }
    }
}
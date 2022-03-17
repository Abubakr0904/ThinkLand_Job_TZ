using webapp.Core.IRepositories;
using webapp.Data;
using webapp.Entities;
using Microsoft.EntityFrameworkCore;

namespace webapp.Core.Repositories;
public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context, ILogger logger)
        :base(context, logger) {  }

    public override async Task<Expense> GetByIdAsync(Guid id)
    {
        try
        {
            return await _dbSet.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} GetByIdAsync method", typeof(ExpenseRepository));
            return null;
        }
    }
    public override async Task<IEnumerable<Expense>> GetAllAsync()
    {
        try
        {
            return await _dbSet.AsNoTracking().Include(x => x.Category).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} GetAllAsync method", typeof(ExpenseRepository));
            return null;
        }
    }
    public async override Task<bool> AddAsync(Expense entity)
    {
        try
        {
            if(_dbSet.Any(x => x.Id == entity.Id))
                return false;
            await _dbSet.AddAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} AddAsync method", typeof(ExpenseRepository));
            return false;
        }
    }
    public override async Task<bool> UpdateAsync(Expense entity)
    {
        try
        {
            var expense = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if(expense != null)
            {
                if( expense.Id == entity.Id && expense.CategoryId == entity.CategoryId 
                    && expense.Name == entity.Name && expense.Amount == entity.Amount)
                {
                    return false;
                }
                expense.Name = entity.Name;
                expense.Amount = entity.Amount;
                expense.UpdatedAt = DateTimeOffset.UtcNow;
                expense.CategoryId = entity.CategoryId;
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} UpdateAsync method", typeof(ExpenseRepository));
            return false;
        }
    }
    public override async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var expense = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (expense != null) 
            {
                _dbSet.Remove(expense);
                return true;
            }
            return false;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, "{Repo} DeleteAsync method", typeof(ExpenseRepository));
            return false;
        }
    }
}
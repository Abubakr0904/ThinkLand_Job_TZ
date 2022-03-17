using webapp.Core.IRepositories;

namespace webapp.Core.IConfiguration;
public interface IUnitOfWork
{
    ICategoryRepository Categories { get; }
    IExpenseRepository Expenses { get; }

    Task<int> CompleteAsync();
}

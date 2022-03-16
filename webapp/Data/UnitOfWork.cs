using webapp.Core.IConfiguration;
using webapp.Core.IRepositories;
using webapp.Core.Repositories;

namespace webapp.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public ICategoryRepository Categories { get; private set; }

        public IExpenseRepository Expenses { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger= logger;

            Expenses = new ExpenseRepository(_context, _logger);
            Categories = new CategoryRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
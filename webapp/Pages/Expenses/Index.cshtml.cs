using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapp.Core.IConfiguration;
using webapp.Data;
using webapp.Entities;

namespace webapp.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Expense> Expenses { get;set; }

        public async Task OnGetAsync()
        {
            if(User.Identity.Name != "admin")
            {
                Expenses = (await _unitOfWork.Expenses.GetAllAsync()).Where(x => x.AuthorName == User.Identity.Name);
            }
            else
            {
                Expenses = await _unitOfWork.Expenses.GetAllAsync();
            }
        }
        public async Task<Category> GetCategory(Guid id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }
    }
}

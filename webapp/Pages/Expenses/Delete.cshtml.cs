using System.Globalization;
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
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string Culture { get; set; } = "dd MMM yyyy HH:mm:ss";
        
        
        
        [BindProperty]
        public Expense Expense { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Expense = await _unitOfWork.Expenses.GetByIdAsync(id.Value);

            if (Expense == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Expense = await _unitOfWork.Expenses.GetByIdAsync(id.Value);

            if (Expense != null)
            {
                await _unitOfWork.Expenses.DeleteAsync(id.Value);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

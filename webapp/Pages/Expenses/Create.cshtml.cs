using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using webapp.Core.IConfiguration;
using webapp.Data;
using webapp.Entities;
using webapp.ViewModels;

namespace webapp.Pages.Expenses
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var res = JsonConvert.SerializeObject(await _unitOfWork.Categories.GetAllAsync(), Formatting.None,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            CategoriesModel = JsonConvert.DeserializeObject<IEnumerable<Category>>(res);

            // System.Console.WriteLine();
            // Console.WriteLine($"{JsonConvert.SerializeObject(CategoriesModel)}");
            // System.Console.WriteLine();

            return Page();
        }

        [BindProperty]
        public Expense Expense { get; set; }
        public IEnumerable<Category> CategoriesModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Category", "Category should be selected");
                return Page();
            }
            var categEntity = await _unitOfWork.Categories.GetByIdAsync(Expense.CategoryId);
            
            Expense.Id = Guid.NewGuid();
            Expense.UpdatedAt = Expense.CreatedAt = DateTimeOffset.UtcNow;
            Expense.CategoryId = categEntity.Id;
            Expense.AuthorName = User.Identity.Name;

            await _unitOfWork.Expenses.AddAsync(Expense);
            await _unitOfWork.CompleteAsync();

            return RedirectToPage("./Index");
        }
    }
}

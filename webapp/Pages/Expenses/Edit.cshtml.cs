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
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IUnitOfWork unitOfWork, ILogger<EditModel> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            var res = JsonConvert.SerializeObject(await _unitOfWork.Categories.GetAllAsync(), Formatting.None,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            CategoriesModel = JsonConvert.DeserializeObject<IEnumerable<Category>>(res);
            
            var expenseRes = JsonConvert.SerializeObject(await _unitOfWork.Expenses.GetByIdAsync(id.Value), 
                Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var entityRes = JsonConvert.DeserializeObject<Expense>(expenseRes);
            if(entityRes != null) Expense = entityRes;
            else return NotFound("No issued Expense with this id");

            return Page();
        }

        [BindProperty]
        public Expense Expense { get; set; }
        public IEnumerable<Category> CategoriesModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _unitOfWork.Expenses.UpdateAsync(Expense);
                await _unitOfWork.CompleteAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "{Page} OnPostAsync method", typeof(EditModel));
            }
            return RedirectToPage("./Index");
        }
    }
}

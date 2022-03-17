using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Core.IConfiguration;
using webapp.Entities;

namespace webapp.Pages.Categories
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

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound("Bu id bilan qo'shilgan kategoriya mavjud emas");
            }

            Category = await _unitOfWork.Categories.GetByIdAsync(id.Value);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _unitOfWork.Categories.UpdateAsync(Category);
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Core.IConfiguration;
using webapp.Entities;

namespace webapp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string CreatedAt { get; set; } = "";
        public string UpdatedAt { get; set; } = "";
        
        

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _unitOfWork.Categories.GetByIdAsync(id.Value);
            CreatedAt = Category?.CreatedAt.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss");
            UpdatedAt = Category?.UpdatedAt.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss");

            if (Category == null)
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

            await _unitOfWork.Categories.DeleteAsync(id.Value);
            await _unitOfWork.CompleteAsync();

            return RedirectToPage("./Index");
        }
    }
}

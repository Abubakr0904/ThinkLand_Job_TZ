using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Core.IConfiguration;
using webapp.Entities;

namespace webapp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Category.UpdatedAt = Category.CreatedAt = DateTimeOffset.UtcNow;
            Category.Id = Guid.NewGuid();
            await _unitOfWork.Categories.AddAsync(Category);
            await _unitOfWork.CompleteAsync();

            return RedirectToPage("./Index");
        }
    }
}

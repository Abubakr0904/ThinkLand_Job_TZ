using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using webapp.Core.IConfiguration;
using webapp.Data;
using webapp.Entities;

namespace webapp.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userM;

        public IndexModel(IUnitOfWork unitOfWork, UserManager<AppUser> userM)
        {
            _unitOfWork = unitOfWork;
            _userM = userM;
        }

        public IEnumerable<Expense> Expenses { get;set; } = new List<Expense>();
        [BindProperty]
        public string SelectedCategoryName { get; set; }
        public IEnumerable<Category> CategoriesModel { get; set; }
        public Guid userId { get; set; }
        
        
        

        public async Task OnGetAsync(Guid userId)
        {
            if(userId != default && !string.IsNullOrWhiteSpace(userId.ToString()) && !string.IsNullOrEmpty(userId.ToString()))
            {
                this.userId = userId;
                Expenses = (await _unitOfWork.Expenses.GetAllAsync()).Where(x => x.AuthorName == _userM.Users.FirstOrDefault(x => x.Id == userId).UserName);
            }
            else if(User.Identity.Name != "admin" && User.Identity.Name != "superadmin")
            {
                Expenses = (await _unitOfWork.Expenses.GetAllAsync()).Where(x => x.AuthorName == User.Identity.Name);
            }
            else
            {
                Expenses = await _unitOfWork.Expenses.GetAllAsync();
            }

            var res = JsonConvert.SerializeObject(await _unitOfWork.Categories.GetAllAsync(), Formatting.None,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            CategoriesModel = JsonConvert.DeserializeObject<IEnumerable<Category>>(res);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(SelectedCategoryName is not null)
            {
                var entity = await _unitOfWork.Categories.GetByNameAsync(SelectedCategoryName);
                Expenses = User.Identity.Name == "admin"            ? 
                    (await _unitOfWork.Expenses.GetAllAsync())
                    .Where(x => x.CategoryId == entity.Id).ToList() : 
                    (await _unitOfWork.Expenses.GetAllAsync())
                    .Where(x => x.AuthorName == User.Identity.Name)
                    .Where(x => x.CategoryId == entity.Id).ToList();
            }
            else
            {
                Expenses = User.Identity.Name == "admin" ? await _unitOfWork.Expenses.GetAllAsync() : (await _unitOfWork.Expenses.GetAllAsync()).Where(x => x.AuthorName == User.Identity.Name).ToList();
            }
            
            var res = JsonConvert.SerializeObject(await _unitOfWork.Categories.GetAllAsync(), Formatting.None,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            CategoriesModel = JsonConvert.DeserializeObject<IEnumerable<Category>>(res);
            return Page();
        }
    }
}

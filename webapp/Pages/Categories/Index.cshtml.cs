using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using webapp.Core.IConfiguration;
using webapp.Entities;

namespace webapp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> Categories { get;set; }

        public async Task OnGetAsync(int page = 1, int offset = 0)
        {
            var res = JsonConvert.SerializeObject(await _unitOfWork.Categories.GetAllAsync(), Formatting.None,
            new JsonSerializerSettings()
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(res).Skip(offset * 10).Take(10);
        }
    }
}

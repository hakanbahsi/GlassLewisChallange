using GlassLewisChallange.UI.Models;
using GlassLewisChallange.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlassLewisChallange.UI.Pages.Companies
{
    public class DetailsModel : PageModel
    {
        private readonly CompanyService _companyService;
        public string ErrorMessage { get; set; }

        public CompanyDetailModel Company { get; set; }

        public DetailsModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                Company = await _companyService.GetCompanyByIdAsync(id);
                if (Company == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }
    }
}

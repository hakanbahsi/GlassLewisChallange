using GlassLewisChallange.UI.Models;
using GlassLewisChallange.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlassLewisChallange.UI.Pages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly CompanyService _companyService;
        public string ErrorMessage { get; set; }

        [BindProperty]
        public CreateCompanyModel Company { get; set; }

        public CreateModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var success = await _companyService.CreateCompanyAsync(Company);
                if (success)
                {
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, "Something went wrong");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}

using GlassLewisChallange.UI.Models;
using GlassLewisChallange.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlassLewisChallange.UI.Pages.Companies
{
    public class EditModel : PageModel
    {
        private readonly CompanyService _companyService;
        public string ErrorMessage { get; set; }

        [BindProperty]
        public CompanyDetailModel Company { get; set; }

        public EditModel(CompanyService companyService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var success = await _companyService.UpdateCompanyAsync(Company);
                if (success)
                {
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError("", "Something went wrong");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}

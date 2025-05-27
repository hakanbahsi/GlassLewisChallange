using GlassLewisChallange.UI.Models;
using GlassLewisChallange.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlassLewisChallange.UI.Pages;

public class IndexModel : PageModel
{
    private readonly CompanyService _companyService;
    public string ErrorMessage { get; set; }
    public List<CompanyListItemModel> Companies { get; set; } = new();

    public IndexModel(CompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task OnGetAsync()
    {
        try
        {
            Companies = await _companyService.GetAllCompaniesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}

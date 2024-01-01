using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ThreeAmigosWebPage.Pages;

public class ClaimsModel : PageModel
{
    private readonly ILogger<ClaimsModel> _logger;

    public ClaimsModel(ILogger<ClaimsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}


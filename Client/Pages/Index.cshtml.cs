using Client.WebApiService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWebApiService _webApi;
    private readonly ExceptionHandler _exceptionHandler;
    public string Confirmation { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IWebApiService webApi, ExceptionHandler exceptionHandler)
    {
        _logger = logger;
        _webApi = webApi;
        _exceptionHandler = exceptionHandler;
    }

    public void OnGet()
    {
    }

    public async Task OnGetCallTheService()
    {
        try
        {
            await _webApi.ApiPersonSystemExceptionAsync();
            Confirmation = "Success";
        }
        catch (Exception e)
        {
            Confirmation = e.Message;
        }
    }

    private void Waiting(bool obj)
    {
        Confirmation = obj ? "Loading..." : string.Empty;
    }

    private void ExceptionRaised(Exception obj)
    {
        Confirmation = obj.Message;
    }

    public async Task OnGetCallTheDomain()
    {
        var result =
            await _exceptionHandler.ExecuteInTryCatch(() => _webApi.ApiPersonGetAsync(125), ExceptionRaised, Waiting);
        Confirmation = JsonConvert.SerializeObject(result, JsonHelper.GetDefaultSettings());
    }
}
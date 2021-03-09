using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiServer.Controllers
{
    // Documenting apis with ApiExplorer
    public class DocumentationController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public DocumentationController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        public IActionResult Index()
        {
            // needs services.AddMvc(); to be aded in Startup / ConfigureServices
            return View(_apiExplorer);
        }
    }
}

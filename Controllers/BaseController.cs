using System.Diagnostics;
using System.Threading.Tasks;
using anvireco_reviews_preprocessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace anvireco_reviews_preprocessor.Controllers
{
    public class BaseController : Controller
    {

        [HttpGet]
        public IActionResult Index() => View();

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
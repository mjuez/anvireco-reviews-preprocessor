using System.Diagnostics;
using System.Threading.Tasks;
using anvireco_reviews_preprocessor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace anvireco_reviews_preprocessor.Controllers
{
    public class BaseController : Controller
    {

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [Produces("text/csv")]
        public Task<IActionResult> Index([FromBody]IList<Repository> value){
            return Ok();
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
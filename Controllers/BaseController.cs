using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using anvireco_reviews_preprocessor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace anvireco_reviews_preprocessor.Controllers
{
    public class BaseController : Controller
    {

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        //[Produces("text/csv")]
        public async Task<IActionResult> Index(IFormFile reviewsFile)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(ProcessedData));
            var processedData = tc.ConvertFrom(reviewsFile) as ProcessedData;

            var reviewers = processedData.Repositories.SelectMany(r => r.PullRequests.SelectMany(p => p.Reviews.Select(review => review.Reviewer))).Distinct().ToList();

            return Ok();
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
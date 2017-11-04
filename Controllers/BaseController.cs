using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using anvireco_reviews_preprocessor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace anvireco_reviews_preprocessor.Controllers
{
    public class BaseController : Controller
    {

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile reviewsFile)
        {
            var processedData = await ConvertFromFileAsync(reviewsFile);
            var memoryStream = await ConvertToMemoryStreamAsync(processedData);
        
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "export.csv" };
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        private async Task<ProcessedData> ConvertFromFileAsync(IFormFile reviewsFile){
            var converter = TypeDescriptor.GetConverter(typeof(ProcessedData));
            return await Task.Run(() => converter.ConvertFrom(reviewsFile) as ProcessedData);
        }

        private async Task<MemoryStream> ConvertToMemoryStreamAsync(ProcessedData processedData){
            var converter = TypeDescriptor.GetConverter(typeof(ProcessedData));
            return await Task.Run(() => converter.ConvertTo(processedData, typeof(MemoryStream)) as MemoryStream);
        }

    }

}
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using anvireco_reviews_preprocessor.ClassMaps;
using anvireco_reviews_preprocessor.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            IList<Repository> repositories = new List<Repository>();

            var reader = new StreamReader(reviewsFile.OpenReadStream());
            var csv = new CsvReader(reader);
            bool badData = false;
            int badDataCount = 0;
            csv.Configuration.BadDataFound = context =>
            {
                badData = true;
                badDataCount++;
            };
            csv.Configuration.RegisterClassMap<RepositoryMap>();
            csv.Configuration.RegisterClassMap<PullRequestMap>();
            csv.Configuration.RegisterClassMap<ReviewMap>();

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                if (!badData)
                {
                    var repository = csv.GetRecord<Repository>();
                    var pullRequest = csv.GetRecord<PullRequest>();
                    var review = csv.GetRecord<Review>();

                    if (!repositories.Contains(repository))
                    {
                        repositories.Add(repository);
                    }
                    else
                    {
                        repository = repositories.Where(r => r.Id == repository.Id).Single();
                    }

                    var pullRequests = repository.PullRequests;
                    if (!pullRequests.Contains(pullRequest))
                    {
                        pullRequests.Add(pullRequest);
                    }
                    else
                    {
                        pullRequest = pullRequests.Where(p => p.Id == pullRequest.Id).Single();
                    }
                    pullRequest.Reviews.Add(review);
                }

                badData = false;

            }

            return Ok();
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
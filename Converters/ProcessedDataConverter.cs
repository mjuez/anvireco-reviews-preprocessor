using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using anvireco_reviews_preprocessor.ClassMaps;
using anvireco_reviews_preprocessor.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;

namespace anvireco_reviews_preprocessor.Converters
{

    public class ProcessedDataConverter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(IFormFile)) return true;
            return base.CanConvertTo(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var fileValue = value as IFormFile;
            var processedData = new ProcessedData();

            var reader = new StreamReader(fileValue.OpenReadStream());
            var csv = new CsvReader(reader);
            bool badData = false;
            csv.Configuration.BadDataFound = ctx =>
            {
                badData = true;
                processedData.BadRecords++;
            };
            csv.Configuration.RegisterClassMap<RepositoryMap>();
            csv.Configuration.RegisterClassMap<PullRequestMap>();
            csv.Configuration.RegisterClassMap<ReviewMap>();

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                processedData.TotalRecords++;
                if (!badData)
                {
                    var repository = csv.GetRecord<Repository>();
                    var pullRequest = csv.GetRecord<PullRequest>();
                    var review = csv.GetRecord<Review>();

                    if (!processedData.Repositories.Contains(repository))
                    {
                        processedData.Repositories.Add(repository);
                    }
                    else
                    {
                        repository = processedData.Repositories.Where(r => r.Id == repository.Id).Single();
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

            return processedData;

        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MemoryStream)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var processedData = value as ProcessedData;
            var reviewers = GetReviewersList(processedData.Repositories);
            MemoryStream destinationMemoryStream = GetCsvMemoryStream(processedData.Repositories, reviewers);
            return destinationMemoryStream;
        }

        private IList<string> GetReviewersList(IList<Repository> repositories)
        {
            return repositories.SelectMany(r => r.PullRequests.SelectMany(p => p.Reviews.Select(review => review.Reviewer))).Distinct().ToList();
        }

        private MemoryStream GetCsvMemoryStream(IList<Repository> repositories, IList<string> reviewers)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var csv = new CsvWriter(streamWriter);

            foreach (var repository in repositories)
            {
                foreach (var pullRequest in repository.PullRequests)
                {
                    dynamic csvRecord = new ExpandoObject();
                    csvRecord.repository_id = repository.Id;
                    csvRecord.repository_owner = repository.Owner;
                    csvRecord.repository_name = repository.Name;
                    csvRecord.repository_language = repository.Language;
                    csvRecord.repository_creation_date = repository.CreationDate;
                    csvRecord.repository_update_date = repository.UpdateDate;
                    csvRecord.pull_request_id = pullRequest.Id;
                    csvRecord.pull_request_title = pullRequest.Title;
                    csvRecord.pull_request_body = pullRequest.Body;
                    csvRecord.pull_request_state = pullRequest.State;
                    csvRecord.pull_request_locked = pullRequest.Locked;
                    csvRecord.pull_request_creation_date = pullRequest.CreationDate;
                    csvRecord.pull_request_update_date = pullRequest.UpdateDate;
                    csvRecord.pull_request_close_date = pullRequest.CloseDate;
                    csvRecord.pull_request_merged = pullRequest.Merged;
                    csvRecord.pull_request_mergeable = pullRequest.Mergeable;
                    csvRecord.pull_request_comments = pullRequest.Comments;
                    csvRecord.pull_request_reviews = pullRequest.Reviews.Count;
                    csvRecord.pull_request_review_comments = pullRequest.ReviewComments;
                    csvRecord.pull_request_commits = pullRequest.Commits;
                    csvRecord.pull_request_additions = pullRequest.Additions;
                    csvRecord.pull_request_deletions = pullRequest.Deletions;
                    csvRecord.pull_request_changed_files = pullRequest.ChangedFiles;

                    foreach (var reviewer in reviewers)
                    {
                        var commented = GetReviewsByReviewerAndState(pullRequest.Reviews, reviewer, "COMMENTED");
                        var approved = GetReviewsByReviewerAndState(pullRequest.Reviews, reviewer, "APPROVED");
                        var changesRequested = GetReviewsByReviewerAndState(pullRequest.Reviews, reviewer, "CHANGES_REQUESTED");
                        var dismissed = GetReviewsByReviewerAndState(pullRequest.Reviews, reviewer, "DISMISSED");

                        var numCommented = commented.Count();
                        var numApproved = approved.Count();
                        var numChangesRequested = changesRequested.Count();
                        var numDismissed = dismissed.Count();

                        var strCommented = ConcatenateReviewsBody(commented);
                        var strApproved = ConcatenateReviewsBody(approved);
                        var strChangesRequested = ConcatenateReviewsBody(changesRequested);
                        var strDismissed = ConcatenateReviewsBody(dismissed);

                        var dict = csvRecord as IDictionary<string, Object>;
                        dict.Add(reviewer + "_commented", numCommented);
                        dict.Add(reviewer + "_commented_str", strCommented);
                        dict.Add(reviewer + "_approved", numApproved);
                        dict.Add(reviewer + "_approved_str", strApproved);
                        dict.Add(reviewer + "_changes_requested", numChangesRequested);
                        dict.Add(reviewer + "_changes_requested_str", strChangesRequested);
                        dict.Add(reviewer + "_dismissed", numDismissed);
                        dict.Add(reviewer + "_dismissed_str", strDismissed);
                    }

                    csv.WriteRecord(csvRecord);
                    csv.NextRecord();
                }
            }
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }

        private IEnumerable<Review> GetReviewsByReviewerAndState(IList<Review> reviews, string reviewer, string state) => reviews.Where(r => r.Reviewer == reviewer).Where(r => r.State == state);

        private string ConcatenateReviewsBody(IEnumerable<Review> reviews) => reviews.Select(r => r.Body).Aggregate("", (concatenated, body) => concatenated + " " + body);
    }

}
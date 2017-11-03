using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => false;

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            throw new InvalidOperationException("Can't convert from Repository list."); // one way converter.
        }
    }

}
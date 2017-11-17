using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using anvireco_reviews_preprocessor.Converters;

namespace anvireco_reviews_preprocessor.Models
{

    [TypeConverter(typeof(ProcessedDataConverter))]
    public class ProcessedData
    {

        public IList<Repository> Repositories { get; set; } = new List<Repository>();

        public int BadRecords { get; set; } = 0;

        public int TotalRecords { get; set; } = 0;

        public string GetExportFileName() {
            return Repositories.Select(r => r.Name).Aggregate("", (concatenated, body) => concatenated + "_" + body);
        }

    }

}
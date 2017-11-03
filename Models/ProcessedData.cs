using System.Collections.Generic;
using System.ComponentModel;
using anvireco_reviews_preprocessor.Converters;

namespace anvireco_reviews_preprocessor.Models
{

    [TypeConverter(typeof(ProcessedDataConverter))]
    public class ProcessedData
    {

        public IList<Repository> Repositories { get; set; } = new List<Repository>();

        public int BadRecords { get; set; } = 0;

        public int TotalRecords { get; set; } = 0;

    }

}
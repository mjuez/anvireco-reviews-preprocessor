using anvireco_reviews_preprocessor.Models;
using CsvHelper.Configuration;

namespace anvireco_reviews_preprocessor.ClassMaps
{
    public sealed class ReviewMap : ClassMap<Review>
    {

        public ReviewMap()
        {            
            Map(m => m.Id).Name("review_id");
            Map(m => m.State).Name("review_state");
            Map(m => m.Body).Name("review_body");
            Map(m => m.Reviewer).Name("reviewer_login");
        }

    }

}
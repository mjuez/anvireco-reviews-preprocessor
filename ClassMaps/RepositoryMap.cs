using anvireco_reviews_preprocessor.Models;
using CsvHelper.Configuration;

namespace anvireco_reviews_preprocessor.ClassMaps
{
    public sealed class RepositoryMap : ClassMap<Repository>
    {

        public RepositoryMap()
        {            
            Map(m => m.Id).Name("repository_id");
            Map(m => m.Owner).Name("repository_owner");
            Map(m => m.Name).Name("repository_name");
            Map(m => m.Language).Name("language");
            Map(m => m.CreationDate).Name("repository_creation_date");
            Map(m => m.UpdateDate).Name("repository_update_date");
        }

    }

}
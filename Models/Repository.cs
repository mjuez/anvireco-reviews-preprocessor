using System.Collections.Generic;

namespace anvireco_reviews_preprocessor.Models
{

    public class Repository
    {
        public int Id { get; set; }

        public string Owner { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public string CreationDate { get; set; }

        public string UpdateDate { get; set; }

        public List<PullRequest> PullRequests { get; } = new List<PullRequest>();

        public override bool Equals(object obj)
        {
            var item = obj as Repository;
            if (item == null) return false;
            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode() => this.Id.GetHashCode();

    }

}
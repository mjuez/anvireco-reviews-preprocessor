using System.Collections.Generic;

namespace anvireco_reviews_preprocessor.Models {

    public class PullRequest {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string State { get; set; }

        public string Locked { get; set; }

        public string CreationDate { get; set; }

        public string UpdateDate { get; set; }

        public string CloseDate { get; set; }

        public string Merged { get; set; }

        public string Mergeable { get; set; }

        public int Comments { get; set; }

        public int ReviewComments { get; set; }

        public int Commits { get; set; }

        public int Additions { get; set; }

        public int Deletions { get; set; }

        public int ChangedFiles { get; set; }

        public List<Review> Reviews { get; } = new List<Review>();

        public override bool Equals(object obj)
        {
            var item = obj as PullRequest;
            if (item == null) return false;
            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode() => this.Id.GetHashCode();

    }

}
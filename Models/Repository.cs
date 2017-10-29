using System.Collections.Generic;

namespace anvireco_reviews_preprocessor.Models {

    public class Repository {

        public Repository() => this.PullRequests = new List<PullRequest>();

        public int Id { get; set; }

        public string Owner { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public string CreationDate { get; set; }

        public string UpdateDate { get; set; }

        public List<PullRequest> PullRequests { get; }

    }

}
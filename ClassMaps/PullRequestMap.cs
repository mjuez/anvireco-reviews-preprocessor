using anvireco_reviews_preprocessor.Models;
using CsvHelper.Configuration;

namespace anvireco_reviews_preprocessor.ClassMaps
{
    public sealed class PullRequestMap : ClassMap<PullRequest>
    {

        public PullRequestMap()
        {            
            Map(m => m.Id).Name("pull_request_id");
            Map(m => m.Title).Name("pull_request_title");
            Map(m => m.Body).Name("pull_request_body");
            Map(m => m.State).Name("pull_request_state");
            Map(m => m.Locked).Name("pull_request_locked");
            Map(m => m.CreationDate).Name("pull_request_creation_date");
            Map(m => m.UpdateDate).Name("pull_request_update_date");
            Map(m => m.CloseDate).Name("pull_request_close_date");
            Map(m => m.Merged).Name("pull_request_merged");
            Map(m => m.Mergeable).Name("pull_request_mergeable");
            Map(m => m.Comments).Name("pull_request_comments");
            Map(m => m.Reviews).Name("pull_request_reviews");
            Map(m => m.ReviewComments).Name("pull_request_review_comments");
            Map(m => m.Commits).Name("pull_request_commits");
            Map(m => m.Additions).Name("pull_request_additions");
            Map(m => m.Deletions).Name("pull_request_deletions");
            Map(m => m.ChangedFiles).Name("pull_request_changed_files");
        }

    }

}
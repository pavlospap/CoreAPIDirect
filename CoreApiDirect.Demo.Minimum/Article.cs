using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Minimum
{
    public class Article : Entity<int>
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}

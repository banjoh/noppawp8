using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.DataModel
{
    public class NewsItem
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<NewsItemLink> Links { get; set; }
    }

    public class NewsItemLink
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
    }
}

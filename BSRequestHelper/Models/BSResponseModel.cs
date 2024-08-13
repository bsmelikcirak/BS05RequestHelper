using System.Collections.Generic;

namespace BSRequestHelper.Models
{
    public class BSRequestErrorDetail
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseContent { get; set; }
        public List<string> Headers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HttpRquestPlayer
{
   public  class RequestModel
    {
        public string baseUrl { get; set; }
        public List<string> originalHeaders { get; set; }
        public List<string> newHeaders { get; set; }
        public   Dictionary<string,List<string>>  requests { get; set; }
    }
}

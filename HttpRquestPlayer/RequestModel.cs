using System.Collections.Generic;

namespace HttpRquestPlayer
{
    public class RequestModel
    {
        public string baseUrl { get; set; }
        public List<string> originalHeaders { get; set; }
        public List<string> newHeaders { get; set; }
        public Requests requests { get; set; }
    }
    public class WithBodies
    {
        public Post post { get; set; }
        public Put put { get; set; }
    }
    public class Post
    {
        public List<string> urls { get; set; }
        public List<string> bodies { get; set; }
    }
    public class Put
    {
        public List<string> urls { get; set; }
        public List<string> bodies { get; set; }
    }
    public class Requests
    {
        public List<string> get { get; set; }
        public WithBodies withBodies { get; set; }
    }
}

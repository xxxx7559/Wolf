using System.Collections.Generic;

namespace SignalRSample.Models
{
    public class WolfType
    {
        public string Id {set;get;}
        public string Type { set; get; }
        public List<string> Value { set; get; }
    }
    public class Wolf
    {
        public int Id { set; get; }
        public string value { set; get; }
        public bool checke { set; get; }
        public string SelectEmp { set; get; }
    }
}
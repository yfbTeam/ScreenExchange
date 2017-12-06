using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectorModel
{
    public class JsonModel
    {
        public object Data { get; set; }
        public string Msg { get; set; }
        public int errNum { get; set; }
        public string Status { get; set; }
        public string BackUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApis
{
    public class AppConfig
    {
        public CRICKET CRICKET { get; set; }
    }
    public class CRICKET
    {
        public string DropdwonKey { get; set; }
        public PlayerDetails[] PlayerDetails { get; set; }
    }

    public class PlayerDetails
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}

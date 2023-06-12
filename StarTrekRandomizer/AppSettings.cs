using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTrekRandomizer
{
    public class AppSettings
    {
        public string Root { get; set; } = null!;
        public Show[] Shows { get; set; } = new Show[0];
    }
}

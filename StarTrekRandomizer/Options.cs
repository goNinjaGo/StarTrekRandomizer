using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTrekRandomizer
{
    public class Options
    {
        [Option('t', "types", HelpText = "A list of ShowTypes enum values to choose an episode from", Required = true, SetName = "types")]
        public IEnumerable<ShowType>? ShowTypes { get; set; }

        [Option('s', "show", HelpText = "The show to choose an episode from", Required = true, SetName = "show")]
        public string Show { get; set; } = string.Empty;
    }
}

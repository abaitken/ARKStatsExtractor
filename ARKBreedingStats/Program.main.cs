using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARKBreedingStats
{
    partial class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length != 0)
                return RunCommandLine(args);

            RunGraphical();
            return 0;
        }
    }
}

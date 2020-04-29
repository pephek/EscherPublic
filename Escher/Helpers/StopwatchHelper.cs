using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public static class StopwatchHelper
    {
        private static Stopwatch stopwatch;
        private static string stopwatchWhat;

        public static void Start(string what)
        {
            stopwatchWhat = what;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public static void Stop()
        {
            stopwatch.Stop();
            Debug.WriteLine(string.Format("{0} took {1} milliseonds", stopwatchWhat, stopwatch.ElapsedMilliseconds));
        }
    }
}

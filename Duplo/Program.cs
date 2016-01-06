using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;
using Newtonsoft.Json;

using Duplo;
using System.IO;
using Newtonsoft.Json.Converters;

namespace Duplo.Cli
{
    enum ExitCode : int
    {
        Success = 0,
        NoTargets = 1,
        UnknownError = 99
    }

    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static int Main(string[] args)
        {

            if (args.Length == 0)
            {
                Usage();
                return (int)ExitCode.NoTargets;                
            }

            var targets = args.ToList<String>();

            var finder = new DupeFinder(targets);
            finder.ExamineFiles();

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter("duplicate-names.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, finder.DuplicateNames());
            }

            using (StreamWriter sw = new StreamWriter("duplicate-sizes.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, finder.DuplicateSizes());
            }

            return (int)ExitCode.Success;
        }

        static void Usage()
        {
            Console.WriteLine("Duplo.exe <dir1> <dir2> ...");
        }
    }
}

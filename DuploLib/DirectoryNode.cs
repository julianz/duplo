using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace Duplo
{
    public class DirectoryNode
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private DirectoryInfo _directory;

        public string Path {  get { return _directory.FullName; } }

        public int FileCount { get; set; }

        public int DuplicateCount { get; set; }

        public DirectoryNode(DirectoryInfo directory)
        {
            _directory = directory;
            FileCount = 0;
            DuplicateCount = 0;
        }
    }
}

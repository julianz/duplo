using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace Duplo
{
    public class FileNode
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private FileInfo _file;
        private String _hash;

        private const int BYTES_TO_HASH = 5000;

        // Public properties will be serialized to JSON

        public String Filename { get { return File.FullName; } }

        internal FileInfo File { get { return _file; } }

        internal String Hash
        {
            get
            {
                // Computer hash as needed.

                if (String.IsNullOrEmpty(_hash))
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    using (var stream = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sha256 = new SHA256CryptoServiceProvider())
                    {
                        var buffer = new byte[BYTES_TO_HASH];
                        stream.Read(buffer, 0, BYTES_TO_HASH);
                        var hash = sha256.ComputeHash(buffer);
                        _hash = Convert.ToBase64String(hash).TrimEnd('=');

                        _logger.Debug("Hashed '{0}' to {1} in {2} ms", File.Name, _hash, stopwatch.ElapsedMilliseconds);

                        return _hash;
                    }
                }
                else
                {
                    return _hash;
                }
            }
        }

        public FileNode(FileInfo file)
        {
            _file = file;
        }
    }
}

using System;
using System.Collections.Generic;
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

        public FileInfo File { get { return _file; } }

        public String Hash
        {
            get
            {
                if (_hash == null)
                {
                    using (var stream = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var sha256 = new SHA256CryptoServiceProvider())
                        {
                            var hash = sha256.ComputeHash(stream);
                            _hash = Convert.ToBase64String(hash).TrimEnd('=');
                            return _hash;
                        }
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

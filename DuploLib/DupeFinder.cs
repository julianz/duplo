using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace Duplo
{
    public class DupeFinder
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private List<String> _targetDirs;

        private List<FileNode> _allFiles;

        private Dictionary<long, List<FileNode>> _filesBySize;
        private Dictionary<String, List<FileNode>> _filesByName;
        private Dictionary<String, List<FileNode>> _filesByHash;

        public DupeFinder(List<String> targetDirs)
        {
            _targetDirs = targetDirs;

            _allFiles = new List<FileNode>();
            _filesBySize = new Dictionary<long, List<FileNode>>();
            _filesByName = new Dictionary<string, List<FileNode>>();
            _filesByHash = new Dictionary<string, List<FileNode>>();
        }

        public void ExamineFiles()
        {
            foreach (var target in _targetDirs)
            {
                if (Directory.Exists(target))
                {
                    _logger.Info("Examining files in '{0}'", target);

                    foreach (var file in Directory.EnumerateFiles(target, "*.*", SearchOption.AllDirectories).Select(f => new FileInfo(f)))
                    {
                        if (IncludeFile(file))
                        {
                            _logger.Debug("Adding file '{0}'", file.Name);

                            var filenode = new FileNode(file);
                            _allFiles.Add(filenode);

                            // Filename
                            if (_filesByName.ContainsKey(filenode.File.Name))
                            {
                                _logger.Debug("Found duplicate name '{0}'", filenode.File.Name);
                                _filesByName[filenode.File.Name].Add(filenode);
                            }
                            else
                            {
                                _filesByName[filenode.File.Name] = new List<FileNode> { filenode };
                            }

                            // File size
                            if (_filesBySize.ContainsKey(filenode.File.Length))
                            {
                                _logger.Debug("Found duplicate size '{0}'", filenode.File.Length);
                                _filesBySize[filenode.File.Length].Add(filenode);
                            }
                            else
                            {
                                _filesBySize[filenode.File.Length] = new List<FileNode> { filenode };
                            }
                        }
                    }
                }
                else
                {
                    _logger.Warn("Target directory '{0}' does not exist or is not a directory", target);
                }
            }
        }

        /// <summary>
        /// Whether to include a file in our search or not.
        /// </summary>
        private bool IncludeFile(FileInfo file)
        {
            if (file.Length == 0) { return false; }

            if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) { return false; }
            if ((file.Attributes & FileAttributes.System) == FileAttributes.System) { return false; }

            if (file.Name.Equals("thumbs.db", StringComparison.InvariantCultureIgnoreCase)) { return false; }

            if (file.Extension.Equals(".ini", StringComparison.InvariantCultureIgnoreCase)) { return false; }

            return true;
        }

        public IEnumerable<KeyValuePair<string, List<FileNode>>> DuplicateNames()
        {
            return _filesByName.Where(entry => entry.Value.Count > 1);
        }

        public IEnumerable<KeyValuePair<long, List<FileNode>>> DuplicateSizes()
        {
            return _filesBySize.Where(entry => entry.Value.Count > 1);
        }
    }
}

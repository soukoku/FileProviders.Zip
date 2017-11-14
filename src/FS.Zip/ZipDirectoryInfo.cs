using Microsoft.Extensions.FileProviders;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// A folder <see cref="IFileInfo"/> that's manually generated.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IFileInfo" />
    class ZipDirectoryInfo : IFileInfo
    {
        private string _path;
        private string _name;
        private DateTimeOffset _modified;

        public ZipDirectoryInfo(ZipArchiveEntry entry)
        {
            _modified = entry.LastWriteTime;
            _path = Path.GetDirectoryName(entry.FullName).Replace('\\', '/');
            _name = Path.GetFileName(_path);
        }

        public ZipDirectoryInfo(string path)
        {
            _name = Path.GetFileName(_path) ?? string.Empty;
            _path = path.Replace('\\', '/');
        }

        public bool Exists => true;

        public long Length => -1;

        public string PhysicalPath => _path;

        public string Name => _name;

        public DateTimeOffset LastModified => _modified;

        public bool IsDirectory => true;

        public Stream CreateReadStream()
        {
            throw new InvalidOperationException("Cannot create a stream for a directory.");
        }

        public override string ToString() => PhysicalPath;
    }
}
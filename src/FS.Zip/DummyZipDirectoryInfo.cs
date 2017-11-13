using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// A folder <see cref="IFileInfo"/> that's manually generated.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IFileInfo" />
    class DummyZipDirectoryInfo : IFileInfo
    {
        private string _path;
        private string _name;
        private DateTimeOffset _modified;

        public DummyZipDirectoryInfo(string path, DateTimeOffset modified)
        {
            _path = path.Replace('\\', '/');
            _name = Path.GetFileName(_path);
            _modified = modified;
        }

        public bool Exists => true;

        public long Length => 0;

        public string PhysicalPath => _path;

        public string Name => _name;

        public DateTimeOffset LastModified => _modified;

        public bool IsDirectory => true;

        public Stream CreateReadStream()
        {
            throw new NotSupportedException();
        }

        public override string ToString() => PhysicalPath;
    }
}
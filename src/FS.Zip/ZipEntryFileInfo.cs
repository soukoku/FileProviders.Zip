using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.IO.Compression;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// Represents an entry in the zip file.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IFileInfo" />
    class ZipEntryFileInfo : IFileInfo
    {
        private ZipArchiveEntry _entry;
        private ZipArchive _archive;

        public ZipEntryFileInfo(ZipArchiveEntry entry, ZipArchive archive)
        {
            _entry = entry;
            _archive = archive;
        }


        public bool Exists => true;

        public long Length => _entry.Length;

        // normalize to use /
        public string PhysicalPath => _entry.FullName.Replace('\\', '/');

        public string Name => _entry.Name;

        public DateTimeOffset LastModified => _entry.LastWriteTime.UtcDateTime;

        public bool IsDirectory => _entry.IsDirectory();

        public Stream CreateReadStream()
        {
            try
            {
                return new StreamWithDisposables(_entry.Open(), _archive);
            }
            catch
            {
                _archive.Dispose();
                throw;
            }
        }
    }
}

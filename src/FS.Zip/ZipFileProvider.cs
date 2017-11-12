using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// Provides read-only file system view over a zip file's contents.
    /// </summary>
    public class ZipFileProvider : IFileProvider
    {
        DateTimeOffset _defaultLastModifed;
        private byte[] _zipData;
        IList<IFileInfo> _folderEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileProvider" /> class.
        /// </summary>
        /// <param name="zipData">The zip file's data.</param>
        public ZipFileProvider(byte[] zipData)
        {
            _defaultLastModifed = DateTimeOffset.UtcNow;
            _zipData = zipData;
            using (var archive = GetArchive())
            {
                _folderEntries = archive.ReadFolders(_defaultLastModifed);
            }
        }

        private ZipArchive GetArchive()
            => new ZipArchive(stream: new MemoryStream(_zipData),
                              mode: ZipArchiveMode.Read,
                              leaveOpen: false);

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the directory.</param>
        /// <returns>
        /// Returns the contents of the directory.
        /// </returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (string.IsNullOrEmpty(subpath) && !string.Equals(subpath, "/", StringComparison.Ordinal))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            using (var archive = GetArchive())
            {
                var files = archive.ReadFiles(_defaultLastModifed)
                                .Union(_folderEntries)
                                .Where(entry => string.Equals(entry.PhysicalPath.GetZipDirectoryPath(), subpath, StringComparison.OrdinalIgnoreCase))
                                .ToList();
                return new ZipDirectoryContents(files);
            }
        }

        /// <summary>
        /// Locate a file at the given path.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the file.</param>
        /// <returns>
        /// The file information. Caller must check Exists property.
        /// </returns>
        /// <exception cref="NotFoundFileInfo"></exception>
        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            ZipArchive archive = GetArchive();
            IFileInfo file = null;
            try
            {
                file = archive.ReadFiles(_defaultLastModifed)
                        //.Union(_folderEntries)
                        .FirstOrDefault(entry => string.Equals(entry.PhysicalPath, subpath, StringComparison.OrdinalIgnoreCase));
            }
            finally
            {
                if (file == null && archive != null)
                {
                    archive.Dispose();
                }
            }
            return file ?? new NotFoundFileInfo(subpath);
        }

        /// <summary>
        /// Always returns a <see cref="NullChangeToken"/>.
        /// </summary>
        /// <param name="filter">Not used since zip file is read-only.</param>
        /// <returns>
        /// A <see cref="NullChangeToken"/>.
        /// </returns>
        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}

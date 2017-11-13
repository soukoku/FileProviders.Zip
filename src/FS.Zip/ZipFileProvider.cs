using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        /// <exception cref="ArgumentNullException">zipData</exception>
        public ZipFileProvider(byte[] zipData)
        {
            _zipData = zipData ?? throw new ArgumentNullException(nameof(zipData));
            _defaultLastModifed = DateTimeOffset.UtcNow;

            using (var archive = _zipData.GetArchive())
            {
                _folderEntries = archive.ReadFolders(_defaultLastModifed);
            }
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the directory.</param>
        /// <returns>
        /// Returns the contents of the directory.
        /// </returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            Debug.WriteLine($"GetDirectoryContents({subpath})");

            var isRoot = string.Equals(subpath, "/", StringComparison.Ordinal);

            if (string.IsNullOrEmpty(subpath) && !isRoot)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            subpath = subpath.Trim('/');
            using (var archive = _zipData.GetArchive())
            {
                var folder = _folderEntries
                    .FirstOrDefault(entry => string.Equals(entry.PhysicalPath, subpath, StringComparison.OrdinalIgnoreCase));
                if (folder == null && !isRoot)
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                var all = archive.ReadFiles(_defaultLastModifed)
                                .Union(_folderEntries);
                var matchItems = all.Where(entry => string.Equals(Path.GetDirectoryName(entry.PhysicalPath).Replace('\\', '/'), subpath, StringComparison.OrdinalIgnoreCase))
                                .ToList();
                return new ZipDirectoryContents(matchItems);
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
            Debug.WriteLine($"GetFileInfo({subpath})");

            var isRoot = string.Equals(subpath, "/", StringComparison.Ordinal);

            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }
            else if (isRoot)
            {
                return new DummyZipDirectoryInfo("", _defaultLastModifed);
            }

            subpath = subpath.Trim('/');

            var archive = _zipData.GetArchive();
            IFileInfo file = null;
            try
            {
                file = archive.ReadFiles(_defaultLastModifed)
                        .Union(_folderEntries)
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
        public IChangeToken Watch(string filter) => NullChangeToken.Singleton;
    }
}

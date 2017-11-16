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
        private byte[] _zipData;
        IList<ZipEntryInfo> _folderEntries;
        StringComparison _comparison;

#if !NETSTD11
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileProvider" /> class.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="caseSensitive">if set to <c>true</c> then path comparisons will be case sensitive.</param>
        public ZipFileProvider(string zipFilePath, bool caseSensitive = false) : this(File.ReadAllBytes(zipFilePath), caseSensitive)
        {

        }
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileProvider" /> class.
        /// </summary>
        /// <param name="zipData">The zip file's data.</param>
        /// <param name="caseSensitive">if set to <c>true</c> then path comparisons will be case sensitive.</param>
        /// <exception cref="System.ArgumentNullException">zipData</exception>
        public ZipFileProvider(byte[] zipData, bool caseSensitive = false)
        {
            _zipData = zipData ?? throw new ArgumentNullException(nameof(zipData));
            _comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            using (var archive = _zipData.GetArchive())
            {
                _folderEntries = archive.ReadFolders(_comparison);
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

            if (string.IsNullOrEmpty(subpath)) { return NotFoundDirectoryContents.Singleton; }

            var isRoot = string.Equals(subpath, "/", StringComparison.Ordinal);

            subpath = subpath.Trim('/');
            var folder = _folderEntries
                .FirstOrDefault(entry => string.Equals(entry.PhysicalPath, subpath, _comparison));
            if (folder == null && !isRoot)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            using (var archive = _zipData.GetArchive())
            {
                var all = archive.ReadFiles()
                                .Union(_folderEntries);
                var matchItems = all.Where(entry => string.Equals(Path.GetDirectoryName(entry.PhysicalPath).Replace('\\', '/'), subpath, _comparison))
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

            if (string.IsNullOrEmpty(subpath) || isRoot)
            {
                return new NotFoundFileInfo(subpath);
            }

            subpath = subpath.Trim('/');

            var archive = _zipData.GetArchive();
            IFileInfo file = null;
            try
            {
                file = archive.ReadFiles()
                        .FirstOrDefault(entry => string.Equals(entry.PhysicalPath, subpath, _comparison));
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

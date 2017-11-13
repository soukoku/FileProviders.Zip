using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// Contains utility extension methods for zip.
    /// </summary>
    static class ZipExtensions
    {
        /// <summary>
        /// Determines whether this zip entry is a directory.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>
        ///   <c>true</c> if the specified entry is directory; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDirectory(this ZipArchiveEntry entry)
            => string.IsNullOrEmpty(entry.Name);

        /// <summary>
        /// Generate the all folder listing of a zip archive.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="lastModified">The last modified value to use.</param>
        /// <returns></returns>
        public static IList<IFileInfo> ReadFolders(this ZipArchive archive, DateTimeOffset lastModified)
            // why need this?, cuz some zip files don't actually have folder entries
            // so just always make them ourselves based on file paths.
            => archive.Entries
                .Where(e=>e.IsDirectory())
                .Select(e=>Path.GetDirectoryName(e.FullName))
                // union real folders entries with calculated folders from files
                .Union(archive.Entries
                        .Where(e => !e.IsDirectory())
                        .Select(e => Path.GetDirectoryName(e.FullName))
                        .Where(path => !string.IsNullOrEmpty(path)))
                .Distinct()
                .Select(path => (IFileInfo)new DummyZipDirectoryInfo(path, lastModified))
                .ToList();


        /// <summary>
        /// Reads the file entries from the zip archive.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="lastModified">The last modified.</param>
        /// <returns></returns>
        public static IEnumerable<IFileInfo> ReadFiles(this ZipArchive archive, DateTimeOffset lastModified)
            => archive.Entries
                .Where(e => !e.IsDirectory())
                .Select(e => new ZipEntryFileInfo(e, archive));

        /// <summary>
        /// Generates the directory path from a full zip entry path.
        /// </summary>
        /// <param name="zipEntryFullPath">The zip entry full path.</param>
        /// <returns></returns>
        public static string GetZipDirectoryPath(this string zipEntryFullPath)
        {
            // can't use Path.GetDirectoryName since we need '/'
            var trimmed = zipEntryFullPath.Trim('/');
            var idx = trimmed.LastIndexOf('/');
            if (idx > -1)
            {
                return trimmed.Substring(0, idx);
            }
            return string.Empty;
        }
    }
}

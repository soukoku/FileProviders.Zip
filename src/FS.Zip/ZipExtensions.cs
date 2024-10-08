﻿using System;
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
        /// Generate all folder entries of a zip archive.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static IList<ZipEntryInfo> ReadFolders(this ZipArchive archive, StringComparison comparison)
        {
            // why need this?, cuz some zip files don't actually have folder entries 
            // (ZipFile.CreateFromDirectory can create this)
            // so always remake them ourselves based on file paths.
            
            var folders =  archive.Entries.Select(entry=>
            {
                if (entry.IsDirectory())
                {
                    return new ZipEntryInfo(entry);
                }
                else
                {
                    var parent = Path.GetDirectoryName(entry.FullName.Replace('\\','/'));
                    if (!string.IsNullOrEmpty(parent))
                    {
                        return new ZipEntryInfo(parent);
                    }
                }

                return null;
            }).Where(e=>e!=null)
            .Distinct(new PhyPathEqualityComparer(comparison))
            .ToList();

            return folders;
        }

        /// <summary>
        /// Reads only the file entries from the zip archive.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <returns></returns>
        public static IEnumerable<ZipEntryInfo> ReadFiles(this ZipArchive archive)
            => archive.Entries
                .Where(e => !e.IsDirectory())
                .Select(e => new ZipEntryInfo(e));


        /// <summary>
        /// Wraps the zip data with a <see cref="ZipArchive"/> for processing.
        /// </summary>
        /// <param name="zipData">The zip data.</param>
        /// <returns></returns>
        public static ZipArchive GetArchive(this byte[] zipData)
            => new ZipArchive(stream: new MemoryStream(zipData),
                              mode: ZipArchiveMode.Read,
                              leaveOpen: false);


        /// <summary>
        /// Makes the stream seekable if necessary.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static Stream MakeSeekable(this Stream stream)
        {
            if (stream.CanSeek) { return stream; }

            var ms = new MemoryStream();
            try
            {
                stream.CopyTo(ms);
                ms.Position = 0;
                return ms;
            }
            catch
            {
                ms.Dispose();
                throw;
            }
            finally
            {
                stream.Dispose();
            }
        }
    }
}

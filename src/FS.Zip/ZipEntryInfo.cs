﻿using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.IO.Compression;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// An <see cref="IFileInfo"/> for a zip entry.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IFileInfo" />
    class ZipEntryInfo : IFileInfo
    {
        private string _name;
        private DateTimeOffset _modified;
        private bool _isDir;
        private long _lenth;
        private ZipArchiveEntry _fileEntry;

        public ZipEntryInfo(ZipArchiveEntry entry)
        {
            _modified = entry.LastWriteTime;
            _isDir = string.IsNullOrEmpty(entry.Name);
            if (_isDir)
            {
                ZipPath = Path.GetDirectoryName(entry.FullName.Replace('\\', '/'));
                _name = Path.GetFileName(ZipPath);
                _lenth = -1;
            }
            else
            {
                _fileEntry = entry;
                ZipPath = entry.FullName.Replace('\\', '/');
                _name = entry.Name;
                _lenth = entry.Length;
            }
        }

        public ZipEntryInfo(string folderPath)
        {
            _isDir = true;
            ZipPath = folderPath.Replace('\\', '/');
            _name = Path.GetFileName(ZipPath);
            _lenth = -1;
        }

        public bool Exists => true;

        public long Length => _lenth;

        public string PhysicalPath => null; // _path;

        /// <summary>
        /// Path of zip entry from root of zip file.
        /// </summary>
        public string ZipPath { get; }

        public string Name => _name;

        public DateTimeOffset LastModified => _modified;

        public bool IsDirectory => _isDir;

        public Stream CreateReadStream()
        {
            if (_isDir) throw new InvalidOperationException("Cannot create a stream for a directory.");

            try
            {
                return new StreamWithDisposables(_fileEntry.Open().MakeSeekable(), _fileEntry.Archive);
            }
            catch
            {
                _fileEntry.Archive.Dispose();
                throw;
            }
        }

        public override string ToString() => $"{(_isDir ? "[Folder]" : "[File]")} {ZipPath}";
    }
}
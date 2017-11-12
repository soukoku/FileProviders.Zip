using System;
using System.IO;

namespace ZipTests
{
    static class TestData
    {
        /// <summary>
        /// Zip file with file hierarchy but no directory entries.
        /// </summary>
        public static readonly byte[] No_Folder_Zip_File = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, @"data\zip-with-file-no-folders.zip"));

        /// <summary>
        /// Zip file with normal file/folder hierarchy.
        /// </summary>
        public static readonly byte[] Standard_Zip_File = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, @"data\zip-with-normal-file-folders.zip"));

        /// <summary>
        /// Zip file with no entries.
        /// </summary>
        public static readonly byte[] Empty_Zip_File = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, @"data\zip-without-anything.zip"));
    }
}

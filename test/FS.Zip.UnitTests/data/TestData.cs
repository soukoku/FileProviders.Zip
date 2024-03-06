using System;
using System.IO;

namespace ZipTests
{
    static class TestData
    {
        /// <summary>
        /// Contains 2 test zip files, one with with normal file/folder hierarchy, and one file hierarchy but no directory entries.
        /// </summary>
        public static readonly System.Collections.Generic.IEnumerable<object[]> With_Data_Zip_Files = new[] {
            new object[]{Path.Combine(Environment.CurrentDirectory, "data", "zip-with-normal-file-folders.zip") },
            new object[]{Path.Combine(Environment.CurrentDirectory, "data", "zip-with-no-folders.zip") },
        };

        /// <summary>
        /// Zip file with no entries.
        /// </summary>
        public static readonly string No_Data_Zip_File = Path.Combine(Environment.CurrentDirectory, "data", "zip-without-anything.zip");
    }
}

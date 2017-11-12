using Soukoku.Extensions.FileProviders;
using System;
using Xunit;

namespace ZipTests
{
    public class NoFolderZip
    {
        [Fact]
        public void Test1()
        {
            var provider = new ZipFileProvider(TestData.No_Folder_Zip_File);

            throw new NotImplementedException("To be tested.");
        }
    }
}

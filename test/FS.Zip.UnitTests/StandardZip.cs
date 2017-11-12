using Soukoku.Extensions.FileProviders;
using System;
using Xunit;

namespace ZipTests
{
    public class StandardZip
    {
        [Fact]
        public void Test1()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            throw new NotImplementedException("To be tested.");
        }
    }
}

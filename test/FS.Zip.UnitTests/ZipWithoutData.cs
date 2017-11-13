using Soukoku.Extensions.FileProviders;
using System.Linq;
using Xunit;

namespace ZipTests
{
    public class ZipWithoutData
    {
        [Fact]
        public void GetDirectoryContents_Given_Root_Exists_With_No_Files()
        {
            var provider = new ZipFileProvider(TestData.No_Data_Zip_File);

            var result = provider.GetDirectoryContents("/");
            Assert.True(result.Exists, "Doesn't exist.");
            Assert.Empty(result);
        }

        [Fact]
        public void GetDirectoryContents_Given_Non_root_Returns_no_exists()
        {
            var provider = new ZipFileProvider(TestData.No_Data_Zip_File);

            var result = provider.GetDirectoryContents("/file");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("/folder/");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("/folder/file");
            Assert.False(result.Exists, "Says exists.");
        }

        [Fact]
        public void GetFileInfo_Always_Returns_Not_Exists_For_Non_Root_Paths()
        {
            var provider = new ZipFileProvider(TestData.No_Data_Zip_File);

            var result = provider.GetFileInfo("/file");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetFileInfo("/folder/");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetFileInfo("/folder/file");
            Assert.False(result.Exists, "Says exists.");
        }
    }
}

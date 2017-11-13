using Microsoft.Extensions.FileProviders;
using Soukoku.Extensions.FileProviders;
using System;
using Xunit;

namespace ZipTests
{
    public class FixedCases
    {
        [Fact]
        public void Ctor_When_Given_null_Throws_ArgNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ZipFileProvider(null));
        }

        [Fact]
        public void Watch_Always_Returns_NullChangeToken()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            // prolly a bad test case but anyway
            var result = provider.Watch(null);
            Assert.IsType<NullChangeToken>(result);

            result = provider.Watch("");
            Assert.IsType<NullChangeToken>(result);

            result = provider.Watch("/");
            Assert.IsType<NullChangeToken>(result);

            result = provider.Watch("/asdf");
            Assert.IsType<NullChangeToken>(result);
        }

        [Fact]
        public void GetDirectoryContents_Given_null_Or_Empty_Returns_Not_Exists()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetDirectoryContents(null);
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("");
            Assert.False(result.Exists, "Says exists.");
        }

        [Fact]
        public void GetFileInfo_Given_null_Or_Empty_Returns_Not_Exists()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo(null);
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetFileInfo("");
            Assert.False(result.Exists, "Says exists.");
        }

        [Fact]
        public void GetFileInfo_Given_Root_Exists()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo("/");
            Assert.True(result.Exists, "Root don't exist.");
            Assert.True(result.IsDirectory, "Not a folder");
        }
    }
}

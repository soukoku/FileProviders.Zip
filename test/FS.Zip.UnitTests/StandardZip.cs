using Soukoku.Extensions.FileProviders;
using System;
using System.Linq;
using Xunit;

namespace ZipTests
{
    public class StandardZip
    {
        [Fact]
        public void GetDirectoryContents_Given_Root_Exists_With_Files()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetDirectoryContents("/");
            Assert.True(result.Exists, "Doesn't exist.");
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetDirectoryContents_Given_Folder_With_Files_Works()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetDirectoryContents("/folder with files");
            Assert.True(result.Exists, "Doesn't exists.");
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void GetDirectoryContents_Given_Folder_Without_Files_Works()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetDirectoryContents("/folder without files");
            Assert.True(result.Exists, "Doesn't exists.");
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetDirectoryContents_Given_Bad_Folder_Path_Returns_no_exists()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetDirectoryContents("/file");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("/folder/");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("/folder/file");
            Assert.False(result.Exists, "Says exists.");
        }

        [Fact]
        public void GetFileInfo_For_Real_File_In_Root_Works()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo("/file 0.txt");
            Assert.True(result.Exists, "File 0 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(0, result.Length);

            result = provider.GetFileInfo("/file 1.txt");
            Assert.True(result.Exists, "File 1 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(17, result.Length);
        }

        [Fact]
        public void GetFileInfo_For_Real_File_In_SubFolder_Works()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo("/folder with files/sub file 1.txt");
            Assert.True(result.Exists, "File 1 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(27, result.Length);
        }

        [Fact]
        public void GetFileInfo_For_Bad_File_No_Exist()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo("/bad folder/bad file.txt");
            Assert.False(result.Exists, "Somehow exists.");
        }
        

        [Fact]
        public void GetFileInfo_For_Real_Folder_Works()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo("/folder with files/");
            Assert.True(result.Exists, "Folder don't exist.");
            Assert.True(result.IsDirectory, "Not a folder");
        }

        [Fact]
        public void GetFileInfo_For_Bad_Folder_No_Exist()
        {
            var provider = new ZipFileProvider(TestData.Standard_Zip_File);

            var result = provider.GetFileInfo("/bad folder/");
            Assert.False(result.Exists, "Somehow exists.");
        }
    }
}

using Soukoku.Extensions.FileProviders;
using System.Linq;
using Xunit;

namespace ZipTests
{
    public class ZipWithData
    {
        // other than 1 zip file being without folders, they should logically contain the same structure/files
        // so use MemberDataAttribute to do the same tests on both.


        // GetDirectoryContents tests

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetDirectoryContents_Given_Root_Exists_With_Files(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetDirectoryContents("/");
            Assert.True(result.Exists, "Doesn't exist.");
            Assert.Equal(4, result.Count());
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetDirectoryContents_Given_Top_Folder_With_Files_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetDirectoryContents("/folder with files");
            Assert.True(result.Exists, "Doesn't exists.");
            Assert.Equal(2, result.Count()); // both file and subfolder
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetDirectoryContents_Given_Top_Folder_Without_Files_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetDirectoryContents("/folder without files");
            Assert.True(result.Exists, "Doesn't exists.");
            Assert.Empty(result);
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetDirectoryContents_Given_Sub_Folder_With_Files_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetDirectoryContents("/folder with files/sub folder");
            Assert.True(result.Exists, "Doesn't exists.");
            Assert.Single(result);
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetDirectoryContents_Given_Bad_Folder_Path_Returns_no_exists(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetDirectoryContents("/file");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("/folder/");
            Assert.False(result.Exists, "Says exists.");

            result = provider.GetDirectoryContents("/folder/file");
            Assert.False(result.Exists, "Says exists.");
        }




        // GetFileInfo tests

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Real_File_In_Root_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/file 0.txt");
            Assert.True(result.Exists, "File 0 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(0, result.Length);

            result = provider.GetFileInfo("/file 1.txt");
            Assert.True(result.Exists, "File 1 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(17, result.Length);
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Real_File_In_TopFolder_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/folder with files/sub file 1.txt");
            Assert.True(result.Exists, "File 1 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(27, result.Length);
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Bad_File_No_Exist(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/bad folder/bad file.txt");
            Assert.False(result.Exists, "Somehow exists.");
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Real_Top_Folder_With_Files_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/folder with files/");
            Assert.True(result.Exists, "Folder don't exist.");
            Assert.True(result.IsDirectory, "Not a folder");
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Real_Top_Folder_Without_Files_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/folder without files/");
            Assert.True(result.Exists, "Folder don't exist.");
            Assert.True(result.IsDirectory, "Not a folder");
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Real_Sub_Folder_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/folder with files/sub folder");
            Assert.True(result.Exists, "Folder don't exist.");
            Assert.True(result.IsDirectory, "Not a folder");
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Real_File_In_SubFolder_Works(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/folder with files/sub folder/sub file 2.txt");
            Assert.True(result.Exists, "File 2 don't exist.");
            Assert.False(result.IsDirectory, "Not a file");
            Assert.Equal(34, result.Length);
        }

        [MemberData(nameof(TestData.With_Data_Zip_Files), MemberType = typeof(TestData))]
        [Theory]
        public void GetFileInfo_For_Bad_Folder_No_Exist(byte[] zipData)
        {
            var provider = new ZipFileProvider(zipData);

            var result = provider.GetFileInfo("/bad folder/");
            Assert.False(result.Exists, "Somehow exists.");
        }
    }
}

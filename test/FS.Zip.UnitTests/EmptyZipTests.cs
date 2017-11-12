using System;
using Xunit;

namespace Soukoku.Extensions.FileProviders
{
    public class EmptyZipTests
    {
        [Fact]
        public void Test1()
        {
            var provider = new ZipFileProvider(null);
        }
    }
}

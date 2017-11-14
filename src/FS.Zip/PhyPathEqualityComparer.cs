using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;

namespace Soukoku.Extensions.FileProviders
{
    class PhyPathEqualityComparer : IEqualityComparer<IFileInfo>
    {
        public static IEqualityComparer<ZipDirectoryInfo> Instance { get; } = new PhyPathEqualityComparer();

        public bool Equals(IFileInfo x, IFileInfo y)
        {
            return string.Equals(x.PhysicalPath, y.PhysicalPath, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(IFileInfo obj)
        {
            return obj.PhysicalPath.ToUpperInvariant().GetHashCode();
        }
    }
}
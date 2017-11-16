using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;

namespace Soukoku.Extensions.FileProviders
{
    class PhyPathEqualityComparer : IEqualityComparer<ZipEntryInfo>
    {
        private StringComparison _comparison;


        public PhyPathEqualityComparer(StringComparison comparison)
        {
            _comparison = comparison;
        }

        public bool Equals(ZipEntryInfo x, ZipEntryInfo y)
        {
            return string.Equals(x.PhysicalPath, y.PhysicalPath, _comparison);
        }

        public int GetHashCode(ZipEntryInfo obj)
        {
            if (_comparison == StringComparison.Ordinal)
                return obj.PhysicalPath.GetHashCode();

            return obj.PhysicalPath.ToUpperInvariant().GetHashCode();
        }
    }
}
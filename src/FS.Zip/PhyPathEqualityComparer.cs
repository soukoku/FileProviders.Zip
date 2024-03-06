using System;
using System.Collections.Generic;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// Compares two <see cref="ZipEntryInfo"/> using <see cref="ZipEntryInfo.PhysicalPath"/>.
    /// </summary>
    class PhyPathEqualityComparer : IEqualityComparer<ZipEntryInfo>
    {
        private StringComparison _comparison;


        /// <summary>
        /// Initializes a new instance of the <see cref="PhyPathEqualityComparer"/> class.
        /// </summary>
        /// <param name="comparison">The comparison rule.</param>
        public PhyPathEqualityComparer(StringComparison comparison)
        {
            _comparison = comparison;
        }

        public bool Equals(ZipEntryInfo x, ZipEntryInfo y)
        {
            return string.Equals(x.ZipPath, y.ZipPath, _comparison);
        }


        public int GetHashCode(ZipEntryInfo obj)
        {
            if (_comparison == StringComparison.Ordinal)
                return obj.ZipPath.GetHashCode();

            return obj.ZipPath.ToUpperInvariant().GetHashCode();
        }
    }
}
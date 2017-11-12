using Microsoft.Extensions.FileProviders;
using System.Collections;
using System.Collections.Generic;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// Provides file contents of a zip folder entry.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IDirectoryContents" />
    class ZipDirectoryContents : IDirectoryContents
    {
        private IEnumerable<IFileInfo> _files;

        public ZipDirectoryContents(IEnumerable<IFileInfo> files)
        {
            _files = files;
        }

        public bool Exists => true;

        public IEnumerator<IFileInfo> GetEnumerator()
            => _files.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _files.GetEnumerator();
    }
}

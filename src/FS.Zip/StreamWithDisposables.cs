using System;
using System.IO;

namespace Soukoku.Extensions.FileProviders
{
    /// <summary>
    /// A <see cref="Stream"/> wrapper that holds other things that will be disposed
    /// when this is disposed.
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    sealed class StreamWithDisposables : Stream
    {
        readonly IDisposable[] _disposables;
        readonly Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamWithDisposables"/> class.
        /// </summary>
        /// <param name="stream">The stream to warp.</param>
        /// <param name="disposables">Other disposable objects to track.</param>
        public StreamWithDisposables(Stream stream, params IDisposable[] disposables)
        {
            _stream = stream;
            _disposables = disposables;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _stream.Dispose();
                foreach (var d in _disposables) { d.Dispose(); }
            }
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
            => _stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
            => _stream.Seek(offset, origin);

        public override void SetLength(long value)
            => _stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
            => _stream.Write(buffer, offset, count);
    }
}

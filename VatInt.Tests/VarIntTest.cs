using System;
using System.IO;
using Xunit;

namespace VarInt.Tests
{
    public class VarIntTest
    {
        private MemoryStream _stream;

        public VarIntTest()
        {
            _stream = new MemoryStream();
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData(-10, 1)]
        [InlineData(byte.MaxValue, 2)]
        [InlineData(-byte.MaxValue, 2)]
        [InlineData(short.MaxValue, 3)]
        [InlineData(-short.MaxValue, 3)]
        [InlineData(int.MaxValue, 5)]
        [InlineData(-int.MaxValue, 5)]
        [InlineData(25_000_000_000, 6)]
        [InlineData(-25_000_000_000, 6)]
        [InlineData(long.MaxValue, 10)]
        [InlineData(-long.MaxValue, 10)]
        public void Signed(long digit, int sizeInBytes)
        {
            _stream.WriteVarInt(digit);
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.Equal(sizeInBytes, _stream.Length);

            var newDigit = _stream.ReadVarInt();

            Assert.Equal(digit, newDigit);
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData(byte.MaxValue, 2)]
        [InlineData(ushort.MaxValue, 3)]
        [InlineData(uint.MaxValue, 5)]
        [InlineData(250_000_000_000, 6)]
        [InlineData(ulong.MaxValue, 10)]
        public void UnSigned(ulong digit, int sizeInBytes)
        {
            _stream.WriteVarInt(digit);
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.Equal(sizeInBytes, _stream.Length);

            var newDigit = _stream.ReadVarUInt();

            Assert.Equal(digit, newDigit);
        }
    }
}

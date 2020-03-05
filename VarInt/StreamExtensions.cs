using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static ulong ReadVarUInt(this Stream stream)
        {
            int shift = 0;
            ulong result = 0;

            while (true)
            {
                var byteValue = (ulong)stream.ReadByte();
                var tmp = byteValue & 0x7f;
                result |= tmp << shift;

                if (shift > 64)
                {
                    throw new ArgumentOutOfRangeException("bytes", "Byte array is too large.");
                }

                if ((byteValue & 0x80) != 0x80)
                {
                    return result;
                }

                shift += 7;
            }

            throw new ArgumentException("Cannot decode varint.", "bytes");
        }
        public static long ReadVarInt(this Stream stream)
        {
            return DecodeZigZag(stream.ReadVarUInt());
        }

        public static void WriteVarInt(this Stream stream, long value)
        {
            var bytes = value.VarintBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteVarInt(this Stream stream, ulong value)
        {
            var bytes = value.VarintBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        public static byte[] VarintBytes(this long value)
        {
            return ((ulong)value.EncodeZigZag(64)).VarintBytes();
        }
        public static byte[] VarintBytes(this ulong value)
        {
            var buffer = new byte[10];
            var currentIndex = 0;
            do
            {
                var @byte = value & 0x7f;
                value >>= 7;

                if (value != 0)
                {
                    @byte |= 0x80;
                }

                buffer[currentIndex++] = (byte)@byte;

            } while (value != 0);

            var result = new byte[currentIndex];
            Buffer.BlockCopy(buffer, 0, result, 0, currentIndex);

            return result;
        }

       

        private static long EncodeZigZag(this long value, int bitLength)
        {
            return (value << 1) ^ (value >> (bitLength - 1));
        }
        private static long DecodeZigZag(ulong value)
        {
            if ((value & 0x1) == 0x1)
            {
                return (-1 * ((long)(value >> 1) + 1));
            }

            return (long)(value >> 1);
        }
    }
}

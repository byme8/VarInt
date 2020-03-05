using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace VarInt.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<VarInt>();
        }
    }


    public class VarInt
    {
        [Benchmark]
        public void UnSigned()
        {
            var stream = new MemoryStream();
            for (int i = 0; i < 1_000_000; i++)
            {
                stream.WriteVarInt(100);
            }
        }

        [Benchmark]
        public void Signed()
        {
            var stream = new MemoryStream();
            for (int i = 0; i < 1_000_000; i++)
            {
                stream.WriteVarInt(-100);
            }
        }
    }
}

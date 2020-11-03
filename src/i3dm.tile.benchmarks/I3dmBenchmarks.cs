using BenchmarkDotNet.Attributes;
using I3dm.Tile;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace i3dm.tile.benchmarks
{
    [MemoryDiagnoser]
    public class I3dmBenchmarks
    {
        private I3dm.Tile.I3dm i3dm;
        private byte[] GlbModel ;

        [GlobalSetup]
        public void Setup()
        {
            GlbModel = File.ReadAllBytes(@"barrel.glb");

            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };

            i3dm = new I3dm.Tile.I3dm(positions, GlbModel);
            i3dm.RtcCenter = new Vector3(100, 100, 100);


        }

        [Benchmark]
        public void ReadI3dm()
        {
            var Stream = File.OpenRead("barrel.i3dm");
            var i3dm1 = I3dmReader.Read(Stream);
        }

        [Benchmark]
        public void WriteI3dm()
        {
            var bytes = I3dmWriter.Write(i3dm);
        }
    }
}

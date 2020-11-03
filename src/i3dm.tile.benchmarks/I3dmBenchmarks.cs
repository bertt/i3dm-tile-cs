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
        [Benchmark]
        public void ReadI3dm()
        {
            var stream = File.OpenRead("barrel.i3dm");
            var i3dm = I3dmReader.Read(stream);
        }

        [Benchmark]
        public void WriteI3dm()
        {
            var treeGlb = File.ReadAllBytes(@"barrel.glb");
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);
            i3dm.RtcCenter = new Vector3(100, 100, 100);
            I3dmWriter.Write(i3dm);
        }
    }
}

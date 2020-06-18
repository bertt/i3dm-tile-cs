using I3dm.Tile;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace i3dm.tile.tests
{
    public class Tests
    {
        Stream i3dmfile;
        string expectedMagicHeader = "i3dm";
        int expectedVersionHeader = 1;

        [SetUp]
        public void Setup()
        {
            i3dmfile = File.OpenRead(@"testfixtures/cube.i3dm");
            Assert.IsTrue(i3dmfile != null);
        }

        [Test]
        public void Test1()
        {
            // act
            var i3dm = I3dmReader.ReadI3dm(i3dmfile);

            // assert
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 16);
            var expectedFeatureTableBinary = new byte[16];
            Assert.IsTrue(i3dm.FeatureTableBinary.SequenceEqual(expectedFeatureTableBinary));
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"type\":\"Buffer\",\"data\":[123,34,73,78,83,84,65,78,67,69,83,95,76,69,78,71,84,72,34,58,49,44,34,80,79,83,73,84,73,79,78,34,58,123,34,98,121,116,101,79,102,102,115,101,116,34,58,48,125,125,32,32,32,32,32,32]}  ");
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "SharpGLTF 1.0.0");
        }
    }
}
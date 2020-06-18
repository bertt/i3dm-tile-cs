using I3dm.Tile;
using NUnit.Framework;
using System.IO;

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

            // arrange

            // act
            var i3dm = I3dmReader.ReadI3dm(i3dmfile);
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "SharpGLTF 1.0.0-alpha0009");


            // assert
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            // todo: read i3dm file

            Assert.Pass();
        }
    }
}
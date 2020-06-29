using I3dm.Tile;
using NUnit.Framework;
using System.IO;

namespace i3dm.tile.tests
{
    public class I3dmCompareTests
    {
        [Test]
        public void I3dmComparerTests()
        {
            var i3dmfile = File.OpenRead(@"testfixtures/tree.i3dm");
            var treeI3dm = I3dmReader.Read(i3dmfile);

            var i3dmAfterfile = File.OpenRead(@"testfixtures/tree.i3dm");
            var treeI3dmAfter = I3dmReader.Read(i3dmAfterfile);

            Assert.IsTrue(treeI3dm.FeatureTableBinary.Length == treeI3dmAfter.FeatureTableBinary.Length);

            for(var i=0;i< treeI3dmAfter.FeatureTableBinary.Length; i++)
            {
                Assert.IsTrue(treeI3dm.FeatureTableBinary[i] == treeI3dmAfter.FeatureTableBinary[i]);
            }
        }
    }
}

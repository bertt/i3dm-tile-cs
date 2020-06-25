using I3dm.Tile;
using NUnit.Framework;
using System.IO;
using System.Numerics;

namespace i3dm.tile.tests
{
    public class I3dmWriterTest
    {
        [Test]
        public void WriteBarrelI3dmTest()
        {
            // arrange
            var i3dmExpectedfile = File.OpenRead(@"testfixtures/barrel.i3dm");
            var i3dmExpected = I3dmReader.ReadI3dm(i3dmExpectedfile);
            var positions = i3dmExpected.Positions;
            Assert.IsTrue(positions.Count == 10);
            Assert.IsTrue(i3dmExpected.FeatureTableJson == "{\"INSTANCES_LENGTH\":10,\"POSITION\":{\"byteOffset\":0},\"BATCH_ID\":{\"byteOffset\":120,\"componentType\":\"UNSIGNED_BYTE\"},\"NORMAL_UP\":{\"byteOffset\":132},\"NORMAL_RIGHT\":{\"byteOffset\":252},\"SCALE_NON_UNIFORM\":{\"byteOffset\":372}}       ");

            var barrelGlb = File.ReadAllBytes(@"testfixtures/barrel.glb");
            var i3dm = new I3dm.Tile.I3dm(positions, barrelGlb);
            i3dm.NormalUps = i3dmExpected.NormalUps;
            i3dm.NormalRights = i3dmExpected.NormalRights;
            i3dm.ScaleNonUniforms = i3dmExpected.ScaleNonUniforms;

            i3dm.BatchTableJson = @"{""Height"":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ";
            i3dm.I3dmHeader.GltfFormat = 1;
            //var result = @"testfixtures/barrel_actual.i3dm";
            var result = @"d:\aaabarrel_actual.i3dm";
            I3dmWriter.WriteI3dm(result, i3dm);

            var i3dmActualStream = File.OpenRead(@"testfixtures/barrel_actual.i3dm");
            var i3dmActual = I3dmReader.ReadI3dm(i3dmActualStream);

            Assert.IsTrue(i3dmActual.I3dmHeader.Version == 1);
            Assert.IsTrue(i3dmActual.I3dmHeader.Magic == "i3dm");
            Assert.IsTrue(i3dmActual.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dmActual.I3dmHeader.BatchTableJsonByteLength == 88);
            Assert.IsTrue(i3dmActual.I3dmHeader.FeatureTableBinaryByteLength == 10 * 4 * 3 * 4); // note: is 304 in original file?
            Assert.IsTrue(i3dmActual.I3dmHeader.BatchTableBinaryByteLength == 0);
            Assert.IsTrue(i3dmActual.Positions.Count == 10);
            Assert.IsTrue(i3dmActual.FeatureTable.IsEastNorthUp == false);
            Assert.IsTrue(i3dmActual.Positions[0].Equals(i3dmExpected.Positions[0]));
            var stream = new MemoryStream(i3dmActual.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "obj2gltf");
        }


        [Test]
        public void WriteTreeI3dmTest()
        {
            // arrange
            var i3dmExpectedfile = File.OpenRead(@"testfixtures/tree.i3dm");
            var i3dmExpected = I3dmReader.ReadI3dm(i3dmExpectedfile);
            var positions = i3dmExpected.Positions;
            Assert.IsTrue(positions.Count == 25);

            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);
            i3dm.I3dmHeader.GltfFormat = 1;
            i3dm.FeatureTable.IsEastNorthUp = true;
            i3dm.BatchTableJson = @"{""Height"":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ";

            // act
            var result = @"testfixtures/tree_actual.i3dm";
            I3dmWriter.WriteI3dm(result, i3dm);

            var i3dmActualStream = File.OpenRead(@"testfixtures/tree_actual.i3dm");
            var i3dmActual = I3dmReader.ReadI3dm(i3dmActualStream);

            // Assert
            Assert.IsTrue(i3dmActual.I3dmHeader.Version== 1);
            Assert.IsTrue(i3dmActual.I3dmHeader.Magic== "i3dm");
            Assert.IsTrue(i3dmActual.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dmActual.I3dmHeader.BatchTableJsonByteLength == 88);
            Assert.IsTrue(i3dmActual.I3dmHeader.FeatureTableJsonByteLength == 72); // 
            Assert.IsTrue(i3dmActual.I3dmHeader.FeatureTableBinaryByteLength == 25*4*3); // note: is 304 in original file?
            Assert.IsTrue(i3dmActual.I3dmHeader.ByteLength == 282064); // Note  is: 282072 originally)
            Assert.IsTrue(i3dmActual.I3dmHeader.BatchTableBinaryByteLength == 0);
            Assert.IsTrue(i3dmActual.Positions.Count == 25);
            Assert.IsTrue(i3dmActual.FeatureTable.IsEastNorthUp== true);
            Assert.IsTrue(i3dmActual.Positions[0].Equals(new Vector3(1214947.2f, -4736379f, 4081540.8f)));
            var stream = new MemoryStream(i3dmActual.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "COLLADA2GLTF");
        }
    }
}

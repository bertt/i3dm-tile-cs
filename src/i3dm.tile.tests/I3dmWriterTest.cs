using I3dm.Tile;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace i3dm.tile.tests
{
    public class I3dmWriterTest
    {
        [Test]
        public void WriteI3dmWithRtcCenterTest()
        {
            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);
            i3dm.RtcCenter = new Vector3(100, 100, 100);
            var result = @"tree_invalid.i3dm";

            I3dmWriter.Write(result, i3dm);

            var headerValidateErrors = i3dm.I3dmHeader.Validate();
            Assert.IsTrue(headerValidateErrors.Count == 0);

            var i3dmActualfile = File.OpenRead(result);
            var i3dmActual = I3dmReader.Read(i3dmActualfile);

            Assert.IsTrue(i3dmActual.RtcCenter.Equals(i3dm.RtcCenter));
        }

        [Test]
        public void WriteI3dmHelloWorld()
        {
            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };
            var batchIds = new List<int>() { 9, 11 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);
            var result = @"tree_basic.i3dm";
            i3dm.BatchIds = batchIds;
            I3dmWriter.Write(result, i3dm);

            var headerValidateErrors = i3dm.I3dmHeader.Validate();
            Assert.IsTrue(headerValidateErrors.Count == 0);

            var i3dmActualfile = File.OpenRead(result);
            var i3dmActual = I3dmReader.Read(i3dmActualfile);
            Assert.IsTrue(i3dmActual.Positions.Count == 2);
            Assert.IsTrue(i3dmActual.Positions[0].Equals(pos1));
            Assert.IsTrue(i3dmActual.Positions[1].Equals(pos2));
            Assert.IsTrue(i3dmActual.BatchIds.Count == 2);
            Assert.IsTrue(i3dmActual.BatchIds[0] == 9);
            Assert.IsTrue(i3dmActual.BatchIds[1] == 11);
            Assert.IsTrue(i3dmActual.FeatureTable.BatchIdOffset.componentType == "UNSIGNED_SHORT");
        }

        [Test]
        public void WriteI3dmHelloWorldWithScales()
        {
            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };
            var scales = new List<float> { 2, 3 };
            var batchIds = new List<int>() { 9, 11 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);
            var result = @"tree_batchid.i3dm";
            i3dm.BatchIds = batchIds;
            i3dm.Scales = scales;
            I3dmWriter.Write(result, i3dm);
            var headerValidateErrors = i3dm.I3dmHeader.Validate();
            Assert.IsTrue(headerValidateErrors.Count == 0);

            var i3dmActualfile = File.OpenRead(result);
            var i3dmActual = I3dmReader.Read(i3dmActualfile);
            Assert.IsTrue(i3dmActual.Positions.Count == 2);
            Assert.IsTrue(i3dmActual.Positions[0].Equals(pos1));
            Assert.IsTrue(i3dmActual.Positions[1].Equals(pos2));
            Assert.IsTrue(i3dmActual.BatchIds.Count == 2);
            Assert.IsTrue(i3dmActual.BatchIds[0] == 9);
            Assert.IsTrue(i3dmActual.BatchIds[1] == 11);
            Assert.IsTrue(i3dmActual.FeatureTable.BatchIdOffset.componentType == "UNSIGNED_SHORT");
            Assert.IsTrue(i3dmActual.Scales[0] == 2);
            Assert.IsTrue(i3dmActual.Scales[1] == 3);
        }

        [Test]
        public void WriteI3dmHelloWorldWithBatchIdType()
        {
            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };
            var batchIds = new List<int>() { 9, 11 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);

            var types = new List<string> { "UNSIGNED_BYTE", "UNSIGNED_SHORT", "UNSIGNED_INT" };

            // write i3dm with every type for batch_id
            foreach (var type in types)
            {
                var result = $"tree_batchid_{type}.i3dm";
                i3dm.BatchIds = batchIds;
                I3dmWriter.Write(result, i3dm, type);

                var headerValidateErrors = i3dm.I3dmHeader.Validate();
                Assert.IsTrue(headerValidateErrors.Count == 0);

                var i3dmActualfile = File.OpenRead(result);
                var i3dmActual = I3dmReader.Read(i3dmActualfile);
                Assert.IsTrue(i3dmActual.Positions.Count == 2);
                Assert.IsTrue(i3dmActual.Positions[0].Equals(pos1));
                Assert.IsTrue(i3dmActual.Positions[1].Equals(pos2));
                Assert.IsTrue(i3dmActual.BatchIds.Count == 2);
                Assert.IsTrue(i3dmActual.BatchIds[0] == 9);
                Assert.IsTrue(i3dmActual.BatchIds[1] == 11);
                Assert.IsTrue(i3dmActual.FeatureTable.BatchIdOffset.componentType == type);

                var stream = new MemoryStream(i3dmActual.GlbData);
                var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
                Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            }
        }

        [Test]
        public void WriteBarrelI3dmWithBatchIdShortTest()
        {
            // arrange
            var originalFile = @"testfixtures/barrel.i3dm";
            var i3dmOriginalfile = File.OpenRead(originalFile);

            var i3dmOriginal = I3dmReader.Read(i3dmOriginalfile);
            Assert.IsTrue(i3dmOriginal.I3dmHeader.FeatureTableBinaryByteLength == 496);
            Assert.IsTrue(i3dmOriginal.FeatureTable.BatchIdOffset.componentType == "UNSIGNED_BYTE");
            i3dmOriginal.FeatureTable.BatchIdOffset.componentType = "UNSIGNED_SHORT";
            var result = @"barrel_actual_short.i3dm";
            I3dmWriter.Write(result, i3dmOriginal);

            var headerValidateErrors = i3dmOriginal.I3dmHeader.Validate();
            Assert.IsTrue(headerValidateErrors.Count == 0);

            // assert
            var i3dmActualfile = File.OpenRead(result);
            var i3dmActualShort = I3dmReader.Read(i3dmActualfile);
            i3dmActualShort.FeatureTable.BatchIdOffset.componentType = "UNSIGNED_SHORT";
        }

        [Test]
        public void WriteBarrelI3dmTest()
        {
            // arrange
            var i3dmExpectedfile = File.OpenRead(@"testfixtures/barrel.i3dm");
            var i3dmExpected = I3dmReader.Read(i3dmExpectedfile);
            var positions = i3dmExpected.Positions;
            Assert.IsTrue(positions.Count == 10);
            Assert.IsTrue(i3dmExpected.FeatureTableJson == "{\"INSTANCES_LENGTH\":10,\"POSITION\":{\"byteOffset\":0},\"BATCH_ID\":{\"byteOffset\":120,\"componentType\":\"UNSIGNED_BYTE\"},\"NORMAL_UP\":{\"byteOffset\":132},\"NORMAL_RIGHT\":{\"byteOffset\":252},\"SCALE_NON_UNIFORM\":{\"byteOffset\":372}}       ");

            var result = @"testfixtures/barrel_actual.i3dm";
            I3dmWriter.Write(result, i3dmExpected);

            var i3dmActualStream = File.OpenRead(@"testfixtures/barrel_actual.i3dm");
            var i3dmActual = I3dmReader.Read(i3dmActualStream);

            // Assert.IsTrue(i3dmExpected.Equals(i3dmActual));
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
            var i3dmExpected = I3dmReader.Read(i3dmExpectedfile);
            var positions = i3dmExpected.Positions;
            Assert.IsTrue(positions.Count == 25);

            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var i3dm = new I3dm.Tile.I3dm(positions, treeGlb);
            i3dm.FeatureTable.IsEastNorthUp = true;
            i3dm.BatchTableJson = @"{""Height"":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ";

            // act
            var result = @"tree.i3dm";
            I3dmWriter.Write(result, i3dm);

            var i3dmActualStream = File.OpenRead(result);
            var i3dmActual = I3dmReader.Read(i3dmActualStream);

            Assert.IsTrue(i3dmActual.I3dmHeader.Version == 1);
            Assert.IsTrue(i3dmActual.I3dmHeader.Magic == "i3dm");
            Assert.IsTrue(i3dmActual.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dmActual.I3dmHeader.BatchTableJsonByteLength == 88);
            Assert.IsTrue(i3dmActual.I3dmHeader.FeatureTableJsonByteLength == 72); // 
            Assert.IsTrue(i3dmActual.I3dmHeader.BatchTableBinaryByteLength == 0);
            Assert.IsTrue(i3dmActual.Positions.Count == 25);
            Assert.IsTrue(i3dmActual.FeatureTable.IsEastNorthUp == true);
            Assert.IsTrue(i3dmActual.Positions[0].Equals(new Vector3(1214947.2f, -4736379f, 4081540.8f)));
            var stream = new MemoryStream(i3dmActual.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "COLLADA2GLTF");
        }


        [Test]
        public void WriteTreeBasicI3dmTest()
        {
            // arrange
            var treeGlb = File.ReadAllBytes(@"testfixtures/tree.glb");
            var mapbox_positions = new List<Vector3>();

            mapbox_positions.Add(new Vector3(-8407346.9596f, 4743739.3031f, 38.29f));
            mapbox_positions.Add(new Vector3(-8406181.2949f, 4744924.0771f, 38.29f));

            var i3dm = new I3dm.Tile.I3dm(mapbox_positions, treeGlb);
            i3dm.BatchTableJson = "{\"Height\":[100,101]}";
            i3dm.FeatureTable.IsEastNorthUp = true;
            var result = @"tree.i3dm";

            // act
            I3dmWriter.Write(result, i3dm);
        }
    }
}

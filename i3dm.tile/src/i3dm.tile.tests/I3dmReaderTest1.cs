using I3dm.Tile;
using NUnit.Framework;
using SharpGLTF.Validation;
using System.IO;
using System.Linq;
using System.Numerics;

namespace i3dm.tile.tests
{
    public class Tests
    {
        string expectedMagicHeader = "i3dm";
        int expectedVersionHeader = 1;


        [Test]
        public void InstancedAnimatedTest()
        {
            // arrange
            var i3dmfile = File.OpenRead(@"testfixtures/instancedAnimated.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":25,\"EAST_NORTH_UP\":true,\"POSITION\":{\"byteOffset\":0}}");
            Assert.IsTrue(i3dm.FeatureTable.InstancesLength == 25);
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.LogicalAnimations.Count == 1);
            Assert.IsTrue(glb.LogicalAnimations[0].Name == "Object_0Action");
        }

        [Test]
        public void InstancedTexturedTest()
        {
            // arrange
            // source: https://github.com/flywave/go-3dtile/tree/master/data/Textured/instancedTextured.i3dm
            var i3dmfile = File.OpenRead(@"testfixtures/instancedTextured.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":1,\"POSITION\":{\"byteOffset\":0}}  ");
            Assert.IsTrue(i3dm.FeatureTable.InstancesLength == 1);
            var stream = new MemoryStream(i3dm.GlbData);
            try
            {
                var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            }
            catch (SchemaException le)
            {
                // we expect schemaexception because no support for gltf1'
                Assert.IsTrue(le != null);
                Assert.IsTrue(le.Message == "Unknown version number: 1");
            }
        }

        [Test]
        public void InstancedBarrelTest()
        {
            // arrange
            // source: https://github.com/PrincessGod/objTo3d-tiles/blob/master/bin/barrel/output/Instancedbarrel/barrel.i3dm
            var i3dmfile = File.OpenRead(@"testfixtures/barrel.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.BatchTableJson == "{\"name\":[\"center\",\"right\",\"left\",\"top\",\"bottom\",\"up\",\"right-top\",\"right-bottom\",\"left-top\",\"left-bottom\"],\"id\":[0,1,2,3,4,5,6,7,8,9]}   ");
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":10,\"POSITION\":{\"byteOffset\":0},\"BATCH_ID\":{\"byteOffset\":120,\"componentType\":\"UNSIGNED_BYTE\"},\"NORMAL_UP\":{\"byteOffset\":132},\"NORMAL_RIGHT\":{\"byteOffset\":252},\"SCALE_NON_UNIFORM\":{\"byteOffset\":372}}       ");
            Assert.IsTrue(i3dm.FeatureTable.InstancesLength == 10);
            Assert.IsTrue(i3dm.Positions[0].Equals(new Vector3(0, 0, 0)));
            Assert.IsTrue(i3dm.Positions[1].Equals(new Vector3(20, 0, 0)));

            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 496);
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            glb.SaveGLB(@"barrel.glb");
        }


        [Test]
        public void InstancedWithBatchTableTest()
        {
            // arrange
            // source: http://vcities.umsl.edu/Cesium1.54/Apps/SampleData/Cesium3DTiles/Instanced/InstancedWithBatchTable/
            var i3dmfile = File.OpenRead(@"testfixtures/instancedWithBatchTable.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.BatchTableJson == "{\"Height\":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ");
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":25,\"EAST_NORTH_UP\":true,\"POSITION\":{\"byteOffset\":0}}");
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            glb.SaveGLB(@"instancedwithbatchtable.glb");
        }

        [Test]
        public void InstancedOrientationTest()
        {
            // arrange
            // source: http://vcities.umsl.edu/Cesium1.54/Apps/SampleData/Cesium3DTiles/Instanced/InstancedOrientation/
            var i3dmfile = File.OpenRead(@"testfixtures/instancedOrientation.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":25,\"POSITION\":{\"byteOffset\":0},\"NORMAL_UP\":{\"byteOffset\":300},\"NORMAL_RIGHT\":{\"byteOffset\":600}}    ");
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.BatchTableJson == "{\"Height\":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ");
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 904);
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb != null);
            glb.SaveGLB(@"instancedorientation.glb");
        }

        [Test]
        public void TreeBillboardTest()
        {
            // arrange
            var i3dmfile = File.OpenRead(@"testfixtures/tree_billboard.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":25,\"EAST_NORTH_UP\":true,\"POSITION\":{\"byteOffset\":0}}");
            Assert.IsTrue(i3dm.BatchTableJson == "{\"Height\":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ");
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
            var stream = new MemoryStream(i3dm.GlbData);
            try
            {
                var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
                //Assert.IsTrue(glb.Asset.Version.Major == 2.0);
                //Assert.IsTrue(glb.Asset.Generator == "COLLADA2GLTF");
            }
            catch (LinkException le)
            {
                // we expect linkexception because SharpGLTF does not support KHR_techniques_webgl'
                Assert.IsTrue(le != null);
                Assert.IsTrue(le.Message == "ModelRoot Extensions: KHR_techniques_webgl");
            }
        }

        [Test]
        public void TreeTest()
        {
            // arrange
            var i3dmfile = File.OpenRead(@"testfixtures/tree.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
            Assert.IsTrue(i3dm.BatchTableJson == "{\"Height\":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ");
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":25,\"EAST_NORTH_UP\":true,\"POSITION\":{\"byteOffset\":0}}");
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
            Assert.IsTrue(i3dm.Positions[0].Equals(new Vector3(1214947.2f, -4736379f, 4081540.8f)));
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "COLLADA2GLTF");
            glb.SaveGLB(@"tree.glb");
        }


        [Test]
        public void CubeTest()
        {
            // arrange
            var i3dmfile = File.OpenRead(@"testfixtures/cube.i3dm");
            Assert.IsTrue(i3dmfile != null);
            // act
            var i3dm = I3dmReader.Read(i3dmfile);

            // assert
            Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
            Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
            Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(i3dm.GlbData.Length > 0);
            Assert.IsTrue(i3dm.FeatureTableBinary.Length == 16);
            var expectedFeatureTableBinary = new byte[16];
            Assert.IsTrue(i3dm.FeatureTableBinary.SequenceEqual(expectedFeatureTableBinary));
            Assert.IsTrue(i3dm.FeatureTableJson == "{\"type\":\"Buffer\",\"data\":[123,34,73,78,83,84,65,78,67,69,83,95,76,69,78,71,84,72,34,58,49,44,34,80,79,83,73,84,73,79,78,34,58,123,34,98,121,116,101,79,102,102,115,101,116,34,58,48,125,125,32,32,32,32,32,32]}  ");
            var stream = new MemoryStream(i3dm.GlbData);
            var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
            Assert.IsTrue(glb.Asset.Version.Major == 2.0);
            Assert.IsTrue(glb.Asset.Generator == "SharpGLTF 1.0.0");
            glb.SaveGLB(@"cube.glb");
        }
    }
}
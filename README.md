# i3dm-tile-cs

Reader/Writer for Instanced 3DTiles (i3dm)

i3dm specs:

https://github.com/CesiumGS/3d-tiles/tree/master/specification/TileFormats/Instanced3DModel

## Nuget

https://www.nuget.org/packages/i3dm.tile/

## Sample code

```
var i3dmfile = File.OpenRead(@"testfixtures/tree.i3dm");
Assert.IsTrue(i3dmfile != null);
var i3dm = I3dmReader.ReadI3dm(i3dmfile);
Assert.IsTrue(expectedMagicHeader == i3dm.I3dmHeader.Magic);
Assert.IsTrue(expectedVersionHeader == i3dm.I3dmHeader.Version);
Assert.IsTrue(i3dm.I3dmHeader.GltfFormat == 1);
Assert.IsTrue(i3dm.BatchTableJson.Length >= 0);
Assert.IsTrue(i3dm.GlbData.Length > 0);
Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
Assert.IsTrue(i3dm.BatchTableJson == "{\"Height\":[20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20]} ");
Assert.IsTrue(i3dm.FeatureTableJson == "{\"INSTANCES_LENGTH\":25,\"EAST_NORTH_UP\":true,\"POSITION\":{\"byteOffset\":0}}");
Assert.IsTrue(i3dm.FeatureTableBinary.Length == 304);
Assert.IsTrue(i3dm.FeatureTable.Positions[0].Equals(new Vector3(1214947.2f, -4736379f, 4081540.8f)));
var stream = new MemoryStream(i3dm.GlbData);
var glb = SharpGLTF.Schema2.ModelRoot.ReadGLB(stream);
Assert.IsTrue(glb.Asset.Version.Major == 2.0);
Assert.IsTrue(glb.Asset.Generator == "COLLADA2GLTF");
glb.SaveGLB(@"tree.glb");
```

## Dependencies

- System.Text.Json

## History

2020-06-22: Initial version - reading i3dm tiles


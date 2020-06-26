# i3dm-tile-cs

Reader/Writer for Instanced 3DTiles (i3dm)

i3dm specs:

https://github.com/CesiumGS/3d-tiles/tree/master/specification/TileFormats/Instanced3DModel

## Nuget

https://www.nuget.org/packages/i3dm.tile/

## Sample code

Reading i3dm. Supply the i3dm filename.

```
var i3dmfile = File.OpenRead(@"test.i3dm");
var i3dm = I3dmReader.Read(i3dmfile);
```

The i3dm object contains the glTF, FeatureTable (for instance Positions, NormalUps, NormalRights, ScaleNonUniforms) and 
BatchTable information.

Writing i3dm. Supply the GLB (as byte[]) and instance positions (as list of Vector3). Optionally 
instance NormalUps, NormalRights and ScaleNonUniforms can be provided.

```
var i3dm = new I3dm.Tile.I3dm(positions, glb);
I3dmWriter.Write("test.i3dm", i3dm);
```

## Dependencies

- System.Text.Json

## History

2020-06-25 - 0.2: Adding writing i3dm tiles

2020-06-22 - 0.1: Initial version - reading i3dm tiles


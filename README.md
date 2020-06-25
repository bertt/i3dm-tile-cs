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

Writing i3dm. Supply the GLB (as byte[]) and instance positions (as list of Vector3)

```
var i3dm = new I3dm.Tile.I3dm(positions, glb);
var result = @"test.i3dm";
I3dmWriter.Write(result, i3dm);
```

## Dependencies

- System.Text.Json

## History

2020-06-25 - 0.2: Adding writing i3dm tiles

2020-06-22 - 0.1: Initial version - reading i3dm tiles


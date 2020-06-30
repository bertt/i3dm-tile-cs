# i3dm-tile-cs

Reader/Writer for Instanced 3DTiles (i3dm)

i3dm specs:

https://github.com/CesiumGS/3d-tiles/tree/master/specification/TileFormats/Instanced3DModel

## NuGet

https://www.nuget.org/packages/i3dm.tile/

## Sample code

Reading i3dm. Supply the i3dm filename.

```
var i3dmfile = File.OpenRead(@"test.i3dm");
var i3dm = I3dmReader.Read(i3dmfile);
```

The i3dm object contains the glTF, FeatureTable (for instance Positions, NormalUps, NormalRights, ScaleNonUniforms, Scales) and 
BatchTable information.

Writing i3dm. Supply the GLB (as byte[]) and instance positions (as list of Vector3). Optionally 
instance NormalUps, NormalRights, Scales and ScaleNonUniforms can be provided.

```
var i3dm = new I3dm.Tile.I3dm(positions, glb);
I3dmWriter.Write("test.i3dm", i3dm);
```

Batch Id's are written in the FeatureTable by default as type 'UNSIGNED_SHORT', sample to use 'UNSIGNED_BYTE':

```
var batchIds = new List<int>() { 0, 1 };
var i3dm = new I3dm.Tile.I3dm(positions, glb);
i3dm.BatchIds = batchIds;
I3dmWriter.Write("test.i3dm", i3dm, "UNSIGNED_BYTE");
```

## Known limits

Not implemented (yet): 

- quantized and oct encoded properties

QUANTIZED_VOLUME_OFFSET, QUANTIZED_VOLUME_SCALE, POSITION_QUANTIZED, NORMAL_UP_OCT32P, NORMAL_RIGHT_OCT32P

- gltfFormat as Uri

## Dependencies

- System.Text.Json

## History

2020-06-29 - 0.3: adding FeatureTable batchIds formats (unsigned byte, unsigned short, unsigned int), scale and rtc_center

2020-06-25 - 0.2: Adding writing i3dm tiles

2020-06-22 - 0.1: Initial version - reading i3dm tiles


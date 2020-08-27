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

## Benchmarks

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.301
  [Host]     : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT


|    Method |       Mean |    Error |   StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------- |-----------:|---------:|---------:|---------:|---------:|---------:|----------:|
|  ReadI3dm |   163.4 us |  1.79 us |  1.59 us | 111.0840 | 111.0840 | 111.0840 | 359.43 KB |
| WriteI3dm | 2,208.6 us | 41.15 us | 36.48 us | 109.3750 | 109.3750 | 109.3750 | 418.31 KB |
```

## Known limits

Not implemented (yet): 

- quantized and oct encoded properties

QUANTIZED_VOLUME_OFFSET, QUANTIZED_VOLUME_SCALE, POSITION_QUANTIZED, NORMAL_UP_OCT32P, NORMAL_RIGHT_OCT32P

- gltfFormat as Uri

## Dependencies

- System.Text.Json

## History

2020-08-27 - 0.3.2: add support for rtc_center for high precision positions

2020-07-01 - 0.3.1: adding 8 byte padding requirements

2020-06-29 - 0.3: adding FeatureTable batchIds formats (unsigned byte, unsigned short, unsigned int), scale and rtc_center

2020-06-25 - 0.2: Adding writing i3dm tiles

2020-06-22 - 0.1: Initial version - reading i3dm tiles


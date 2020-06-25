using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace I3dm.Tile
{
    public static class I3dmReader
    {
        public static I3dm ReadI3dm(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var i3dmHeader = new I3dmHeader(reader);
                var featureTableJson = Encoding.UTF8.GetString(reader.ReadBytes(i3dmHeader.FeatureTableJsonByteLength));
                var featureTableBytes = reader.ReadBytes(i3dmHeader.FeatureTableBinaryByteLength);
                var batchTableJson = Encoding.UTF8.GetString(reader.ReadBytes(i3dmHeader.BatchTableJsonByteLength));
                var batchTableBytes = reader.ReadBytes(i3dmHeader.BatchTableBinaryByteLength);
                var glbLength = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                var glbBuffer = reader.ReadBytes(glbLength);
                var featureTable = JsonSerializer.Deserialize<FeatureTable>(featureTableJson);
                // todo: batch table ids'... complexity it can be any format like 'UNSIGNED_BYTE' or others

                var i3dm = new I3dm
                {
                    I3dmHeader = i3dmHeader,
                    GlbData = glbBuffer,
                    FeatureTableJson = featureTableJson,
                    FeatureTableBinary = featureTableBytes,
                    BatchTableJson = batchTableJson,
                    BatchTableBinary = batchTableBytes,
                    FeatureTable = featureTable
                };

                if (featureTable.PositionOffset != null)
                {
                    i3dm.Positions = GetVector3Collection(featureTable.InstancesLength, featureTable.PositionOffset.byteOffset, featureTableBytes);
                };
                if (featureTable.NormalUpOffset != null)
                {
                    i3dm.NormalUps = GetVector3Collection(featureTable.InstancesLength, featureTable.NormalUpOffset.byteOffset, featureTableBytes);
                }
                if (featureTable.NormalRightOffset != null)
                {
                    i3dm.NormalRights = GetVector3Collection(featureTable.InstancesLength, featureTable.NormalRightOffset.byteOffset, featureTableBytes);
                }
                if (featureTable.ScaleNonUniformOffset != null)
                {
                    i3dm.ScaleNonUniforms= GetVector3Collection(featureTable.InstancesLength, featureTable.ScaleNonUniformOffset.byteOffset, featureTableBytes);
                }

                return i3dm;
            }
        }

        private static List<Vector3> GetVector3Collection(int instances, int offset, byte[] featureTable)
        {
            var res = new List<Vector3>();
            for (var i = 0; i < instances; i++)
            {
                var x = BitConverter.ToSingle(featureTable, i * 12 + 0 + offset);
                var y = BitConverter.ToSingle(featureTable, i * 12 + 4 + offset);
                var z = BitConverter.ToSingle(featureTable, i * 12 + 8 + offset);
                var vector = new Vector3(x, y, z);
                res.Add(vector);
            }

            return res;
        }
    }
}

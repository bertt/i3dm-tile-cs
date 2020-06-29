using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace I3dm.Tile
{
    public struct I3dmReader
    {
        public static I3dm Read(Stream stream)
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

                var positions = GetVector3Collection(featureTable.InstancesLength, featureTable.PositionOffset.offset, featureTableBytes);

                var i3dm = new I3dm(positions,glbBuffer)
                {
                    I3dmHeader = i3dmHeader,
                    GlbData = glbBuffer,
                    FeatureTableJson = featureTableJson,
                    FeatureTableBinary = featureTableBytes,
                    BatchTableJson = batchTableJson,
                    BatchTableBinary = batchTableBytes,
                    FeatureTable = featureTable
                };

                if (featureTable.NormalUpOffset != null)
                {
                    i3dm.NormalUps = GetVector3Collection(featureTable.InstancesLength, featureTable.NormalUpOffset.offset, featureTableBytes);
                }
                if (featureTable.NormalRightOffset != null)
                {
                    i3dm.NormalRights = GetVector3Collection(featureTable.InstancesLength, featureTable.NormalRightOffset.offset, featureTableBytes);
                }
                if (featureTable.ScaleNonUniformOffset != null)
                {
                    i3dm.ScaleNonUniforms= GetVector3Collection(featureTable.InstancesLength, featureTable.ScaleNonUniformOffset.offset, featureTableBytes);
                }
                if (featureTable.BatchIdOffset != null)
                {
                    i3dm.BatchIds = GetBatchIdCollection(featureTable.InstancesLength, featureTable.BatchIdOffset.offset, featureTableBytes, featureTable.BatchIdOffset.componentType);
                }
                if (featureTable.ScaleOffset != null)
                {
                    i3dm.Scales = GetFloatCollection(featureTable.InstancesLength, featureTable.ScaleOffset.offset, featureTableBytes);
                }
                if (featureTable.RtcCenterOffset != null)
                {
                    i3dm.RtcCenter = GetVector3(featureTable.RtcCenterOffset.offset, featureTableBytes);
                }

                return i3dm;
            }
        }

        private static List<float> GetFloatCollection(int instances, int offset, byte[] featureTable)
        {
            var res = new List<float>();
            for (var i = 0; i < instances; i++)
            {
                res.Add(BitConverter.ToSingle(featureTable, i * 4 + offset));
            }

            return res;
        }

        private static List<int> GetBatchIdCollection(int instances, int offset, byte[] featureTable, string componentType)
        {
            var res = new List<int>();
            for (var i = 0; i < instances; i++)
            {
                int batchId=0;
                switch (componentType)
                {
                    case "UNSIGNED_BYTE":
                        batchId = featureTable[i + offset];
                        break;
                    case "UNSIGNED_SHORT":
                        batchId = BitConverter.ToUInt16(featureTable, i*2 + offset);
                        break;
                    case "UNSIGNED_INT":
                        batchId = (int)BitConverter.ToUInt32(featureTable, i*4 + offset);
                        break;
                }
                res.Add(batchId);
            }

            return res;
        }

        private static List<Vector3> GetVector3Collection(int instances, int offset, byte[] featureTable)
        {
            var res = new List<Vector3>();
            for (var i = 0; i < instances; i++)
            {
                Vector3 vector = GetVector3(offset, featureTable, i);
                res.Add(vector);
            }
            return res;
        }

        private static Vector3 GetVector3(int offset, byte[] featureTable, int i=0)
        {
            var x = BitConverter.ToSingle(featureTable, i * 12 + 0 + offset);
            var y = BitConverter.ToSingle(featureTable, i * 12 + 4 + offset);
            var z = BitConverter.ToSingle(featureTable, i * 12 + 8 + offset);
            var vector = new Vector3(x, y, z);
            return vector;
        }
    }
}

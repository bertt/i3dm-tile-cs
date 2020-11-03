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
        public static I3dm Read(BinaryReader reader)
        {
            var i3dmHeader = new I3dmHeader(reader);
            var featureTableJson = Encoding.UTF8.GetString(reader.ReadBytes(i3dmHeader.FeatureTableJsonByteLength));
            var featureTableBytes = reader.ReadBytes(i3dmHeader.FeatureTableBinaryByteLength);
            var batchTableJson = Encoding.UTF8.GetString(reader.ReadBytes(i3dmHeader.BatchTableJsonByteLength));
            var batchTableBytes = reader.ReadBytes(i3dmHeader.BatchTableBinaryByteLength);

            var glbLength = i3dmHeader.ByteLength - i3dmHeader.Length;
            var glbBuffer = reader.ReadBytes(glbLength);

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.IgnoreNullValues = true;
            serializeOptions.Converters.Add(new Vector3Converter());
            var featureTable = JsonSerializer.Deserialize<FeatureTable>(featureTableJson.TrimEnd(), serializeOptions);

            var positions = GetVector3Collection(featureTable.InstancesLength, featureTable.PositionOffset.offset, featureTableBytes);

            var i3dm = i3dmHeader.GltfFormat == 0 ?
                new I3dm(positions, Encoding.UTF8.GetString(glbBuffer)) : new I3dm(positions, glbBuffer);
            i3dm.I3dmHeader = i3dmHeader;
            i3dm.FeatureTableJson = featureTableJson;
            i3dm.FeatureTableBinary = featureTableBytes;
            i3dm.BatchTableJson = batchTableJson;
            i3dm.BatchTableBinary = batchTableBytes;
            i3dm.FeatureTable = featureTable;

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
            if (featureTable.RtcCenter != null)
            {
                i3dm.RtcCenter = featureTable.RtcCenter;
            }

            return i3dm;
        }

        public static I3dm Read(Stream stream)
        {
            var reader = new BinaryReader(stream);
            var i3dm = Read(reader);
            return i3dm;
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

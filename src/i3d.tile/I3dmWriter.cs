using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace I3dm.Tile
{
    public struct I3dmWriter
    {
        public static string Write(string path, I3dm i3dm, string batchIdSerializeType = "UNSIGNED_SHORT")
        {
            var batchIdBytes = new byte[0];
            if (i3dm.BatchIds != null)
            {
                batchIdBytes = GetBatchIdsBytes(i3dm.BatchIds, batchIdSerializeType);
            }

            i3dm.FeatureTableJson = BufferPadding.AddPadding(i3dm.GetFeatureTableJson(batchIdSerializeType, batchIdBytes.Length));

            var featureTableBinary = new List<byte>();
            featureTableBinary.AddRange(i3dm.Positions.ToBytes());
            if (i3dm.NormalUps != null)
            {
                featureTableBinary.AddRange(i3dm.NormalUps.ToBytes());
            }
            if (i3dm.NormalRights != null)
            {
                featureTableBinary.AddRange(i3dm.NormalRights.ToBytes());
            }
            if (i3dm.ScaleNonUniforms != null)
            {
                featureTableBinary.AddRange(i3dm.ScaleNonUniforms.ToBytes());
            }
            if (i3dm.Scales != null)
            {
                featureTableBinary.AddRange(i3dm.Scales.ToBytes());
            }
            if (i3dm.BatchIds != null)
            {
                featureTableBinary.AddRange(batchIdBytes);
            }
            if (i3dm.RtcCenter != null)
            {
                featureTableBinary.AddRange(((Vector3)i3dm.RtcCenter).ToBytes());
            }
            if (i3dm.BatchTableJson != string.Empty)
            {
                i3dm.BatchTableJson = BufferPadding.AddPadding(i3dm.BatchTableJson);
            }
            if (i3dm.BatchTableBinary != null)
            {
                i3dm.BatchTableBinary = BufferPadding.AddPadding(i3dm.BatchTableBinary);
            }

            i3dm.FeatureTableBinary = BufferPadding.AddPadding(featureTableBinary.ToArray());

            var header_length = 28;

            var glbLength = i3dm.I3dmHeader.GltfFormat == 0 ? i3dm.GlbUrl.Length : i3dm.GlbData.Length;

            i3dm.I3dmHeader.ByteLength =
                glbLength + header_length +
                i3dm.FeatureTableJson.Length +
                i3dm.BatchTableJson.Length +
                i3dm.BatchTableBinary.Length +
                i3dm.FeatureTableBinary.Length +
                BitConverter.GetBytes(i3dm.I3dmHeader.GltfFormat).Length;

            i3dm.I3dmHeader.FeatureTableJsonByteLength = i3dm.FeatureTableJson.Length;
            i3dm.I3dmHeader.BatchTableJsonByteLength = i3dm.BatchTableJson.Length;
            i3dm.I3dmHeader.FeatureTableBinaryByteLength = i3dm.FeatureTableBinary.Length;
            i3dm.I3dmHeader.BatchTableBinaryByteLength = i3dm.BatchTableBinary.Length;

            var fileStream = File.Open(path, FileMode.Create);
            var binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Write(i3dm.I3dmHeader.AsBinary());
            binaryWriter.Write(Encoding.UTF8.GetBytes(i3dm.FeatureTableJson));
            if (i3dm.FeatureTableBinary != null)
            {
                binaryWriter.Write(i3dm.FeatureTableBinary);
            }
            binaryWriter.Write(Encoding.UTF8.GetBytes(i3dm.BatchTableJson));

            if (i3dm.BatchTableBinary != null)
            {
                binaryWriter.Write(i3dm.BatchTableBinary);
            }

            if(i3dm.I3dmHeader.GltfFormat == 0)
            {
                binaryWriter.Write(Encoding.UTF8.GetBytes(i3dm.GlbUrl));
            }
            else
            {
                binaryWriter.Write(i3dm.GlbData);
            }
            binaryWriter.Flush();
            binaryWriter.Close();
            return fileStream.Name;
        }

        private static byte[] GetBatchIdsBytes(List<int> inputs, string batchIdSerializeType)
        {
            byte[] res = null;
            switch (batchIdSerializeType)
            {
                case "UNSIGNED_BYTE":
                    res = ByteConvertor.ToBytes<byte>(inputs);
                    break;
                case "UNSIGNED_SHORT":
                    res = ByteConvertor.ToBytes<ushort>(inputs);
                    break;
                case "UNSIGNED_INT":
                    res = ByteConvertor.ToBytes<uint>(inputs);
                    break;
            }

            return res;
        }
    }
}

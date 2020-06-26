using System.Collections.Generic;
using System.IO;
using System.Text;

namespace I3dm.Tile
{
    public struct I3dmWriter
    {
        public static string Write(string path, I3dm i3dm)
        {
            i3dm.FeatureTableJson = i3dm.GetFeatureTableJson();
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
            if (i3dm.BatchIdsBytes != null)
            {
                featureTableBinary.AddRange(i3dm.BatchIdsBytes.ToArray());
            }

            i3dm.FeatureTableBinary = featureTableBinary.ToArray();

            var header_length = 28;
            i3dm.I3dmHeader.ByteLength = 
                i3dm.GlbData.Length + header_length + 
                i3dm.FeatureTableJson.Length + 
                i3dm.BatchTableJson.Length + 
                i3dm.BatchTableBinary.Length + 
                i3dm.FeatureTableBinary.Length;
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
            binaryWriter.Write(i3dm.GlbData);
            binaryWriter.Flush();
            binaryWriter.Close();
            return fileStream.Name;
        }
    }
}

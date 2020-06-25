using System.IO;
using System.Text;

namespace I3dm.Tile
{
    public static class I3dmWriter
    {
        public static string WriteI3dm(string path, I3dm i3dm)
        {
            // create FeatureTableJson...
            // i3dm.FeatureTableJson = @"{""INSTANCES_LENGTH"":25,""EAST_NORTH_UP"":true,""POSITION"":{""byteOffset"":0}}";
            i3dm.FeatureTableJson = i3dm.GetFeatureTableJson();
            i3dm.FeatureTableBinary = i3dm.Positions.ToBytes();

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

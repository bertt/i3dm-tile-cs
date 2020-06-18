using System.IO;
using System.Text;

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

                var i3dm = new I3dm
                {
                    I3dmHeader = i3dmHeader,
                    GlbData = glbBuffer,
                    FeatureTableJson = featureTableJson,
                    FeatureTableBinary = featureTableBytes,
                    BatchTableJson = batchTableJson,
                    BatchTableBinary = batchTableBytes
                };
                return i3dm;
            }
        }
    }
}

using System.IO;

namespace I3dm.Tile
{
    public class I3dm
    {
        public I3dm()
        {
            I3dmHeader = new I3dmHeader();
            FeatureTableJson = string.Empty;
            BatchTableJson = string.Empty;
            FeatureTableJson = "{\"BATCH_LENGTH\":0}  ";
            FeatureTableBinary = new byte[0];
            BatchTableBinary = new byte[0];
        }

        public I3dm(byte[] glb) : this()
        {
            GlbData = glb;
        }


        public I3dmHeader I3dmHeader { get; set; }
        public string FeatureTableJson { get; set; }
        public byte[] FeatureTableBinary { get; set; }
        public string BatchTableJson { get; set; }
        public byte[] BatchTableBinary { get; set; }
        public byte[] GlbData { get; set; }

    }
}

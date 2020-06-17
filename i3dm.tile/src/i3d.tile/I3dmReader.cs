using System.IO;

namespace i3dm.tile
{
    public static class I3dmReader
    {
        public static I3dm ReadI3dm(Stream stream)
        {
            var i3dm = new I3dm();
            using (var reader = new BinaryReader(stream))
            {
            }
            return i3dm;
        }
    }
}

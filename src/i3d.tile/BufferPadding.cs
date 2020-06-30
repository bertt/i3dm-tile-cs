using System.Linq;
using System.Text;

namespace I3dm.Tile
{
    public static class BufferPadding
    {
        private static int boundary = 8;
        public static byte[] AddPadding(byte[] bytes)
        {
            var remainder = bytes.Length % boundary;
            var padding = (remainder == 0) ? 0 : boundary - remainder;
            var whitespace = string.Empty;
            for (var i = 0; i < padding; ++i)
            {
                whitespace += " ";
            }
            var addBytes = Encoding.UTF8.GetBytes(whitespace);
            var res = bytes.Concat(addBytes);
            return res.ToArray();
        }
        public static string AddPadding(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var paddedBytes = BufferPadding.AddPadding(bytes);
            var result = Encoding.UTF8.GetString(paddedBytes);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;

namespace I3dm.Tile
{
    public static class FloatExtensions
    {
        public static byte[] ToBytes(this IEnumerable<float> floats)
        {
            var bytes = new List<byte>();
            foreach (var f in floats)
            {
                bytes.AddRange(BitConverter.GetBytes(f));
            }
            return bytes.ToArray();
        }
    }
}

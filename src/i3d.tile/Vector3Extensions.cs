using System;
using System.Collections.Generic;
using System.Numerics;

namespace I3dm.Tile
{
    public static class Vector3Extensions
    {
        public static byte[] ToBytes(this Vector3 vector)
        {
            var res = new List<byte>();
            res.AddRange(BitConverter.GetBytes(vector.X));
            res.AddRange(BitConverter.GetBytes(vector.Y));
            res.AddRange(BitConverter.GetBytes(vector.Z));
            return res.ToArray();
        }

        public static byte[] ToBytes(this IEnumerable<Vector3> vectors)
        {
            var bytes = new List<byte>();
            foreach (var vector in vectors)
            {
                bytes.AddRange(vector.ToBytes());
            }
            return bytes.ToArray();
        }
    }
}

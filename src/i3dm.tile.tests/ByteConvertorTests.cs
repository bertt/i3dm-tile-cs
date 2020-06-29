using I3dm.Tile;
using NUnit.Framework;
using System.Collections.Generic;

namespace i3dm.tile.tests
{
    public class ByteConvertorTests
    {
        [Test]
        public void ByteConvertorToBytesTest()
        {
            var input = new List<int>() { 1, 2, 3 };

            var res = ByteConvertor.ToBytes<byte>(input);

            Assert.IsTrue(res.Length == 3);
        }

        [Test]
        public void ByteConvertorToShortsTest()
        {
            var input = new List<int>() { 1, 2, 3 };

            var res = ByteConvertor.ToBytes<ushort>(input);

            Assert.IsTrue(res.Length == 6);
        }

        [Test]
        public void ByteConvertorToIntsTest()
        {
            var input = new List<int>() { 1, 2, 3 };

            var res = ByteConvertor.ToBytes<uint>(input);

            Assert.IsTrue(res.Length == 12);
        }

    }
}

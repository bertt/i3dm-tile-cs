using NUnit.Framework;
using System.Collections.Generic;
using I3dm.Tile;

namespace i3dm.tile.tests
{
    public class FloatExtensionTests
    {
        [Test]
        public void TestToByteForFloat()
        {
            var f1 = 1.4f;
            var f2 = 1.8f;
            var floats = new List<float> { f1, f2 };

            var bytes = floats.ToBytes();
            Assert.IsTrue(bytes.Length == 8);
        }
    }
}

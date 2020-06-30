using I3dm.Tile;
using NUnit.Framework;

namespace i3dm.tile.tests
{
    public class BufferPaddingTests
    {
        [Test]
        public void Initial()
        {
            var featureTableJson = "{\"INSTANCES_LENGTH\":2,\"POSITION\":{\"byteOffset\":0},\"EAST_NORTH_UP\":false,\"RTC_CENTER\":{\"byteOffset\":24}}";
            var paddedJson = BufferPadding.AddPadding(featureTableJson);
            Assert.IsTrue(paddedJson == "{\"INSTANCES_LENGTH\":2,\"POSITION\":{\"byteOffset\":0},\"EAST_NORTH_UP\":false,\"RTC_CENTER\":{\"byteOffset\":24}} ");
        }
    }
}

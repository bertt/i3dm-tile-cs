using I3dm.Tile;
using NUnit.Framework;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;

namespace i3dm.tile.tests
{
    public class FeatureTableTests
    {

        [Test]
        public void FeatureTableWithRtcCenterTest()
        {
            // arrange
            var featureTableJson = @"{""INSTANCES_LENGTH"":25,""RTC_CENTER"":[1215013.8340490046,-4736316.75897742,4081608.4380407534],""EAST_NORTH_UP"":true,""POSITION"":{""byteOffset"":0}} ";

            // act
            var serializeOptions = new JsonSerializerOptions() { IgnoreNullValues = true };
            serializeOptions.Converters.Add(new Vector3Converter());
            var featureTable = JsonSerializer.Deserialize<FeatureTable>(featureTableJson,serializeOptions);

            // assert
            Assert.IsTrue(featureTable != null);
            Assert.IsTrue(featureTable.InstancesLength == 25);
            Assert.IsTrue(featureTable.RtcCenter.Equals(new Vector3(1215013.8340490046f, -4736316.75897742f, 4081608.4380407534f)));
        }

        [Test]
        public void SerializeFeatureTablejsonTest()
        {
            // arrange
            var featureTableJson = @"{""INSTANCES_LENGTH"":25,""EAST_NORTH_UP"":true,""POSITION"":{""byteOffset"":0}}";

            // act
            var featureTable = JsonSerializer.Deserialize<FeatureTable>(featureTableJson);

            // assert
            Assert.IsTrue(featureTable.InstancesLength == 25);
            Assert.IsTrue(featureTable.IsEastNorthUp);
            Assert.IsTrue(featureTable.PositionOffset.offset==0);
        }

        [Test]
        public void DeserializeFeatureTableToJsonTest()
        {
            // arrange
            var positions = new List<Vector3>();
            positions.Add(new Vector3(0, 0, 0));
            positions.Add(new Vector3(1, 1, 1));
            var i3dm = new I3dm.Tile.I3dm(positions, null);
            i3dm.Positions = positions;

            // act
            var featureTableJson = i3dm.GetFeatureTableJson();
            var serializeOptions = new JsonSerializerOptions() { IgnoreNullValues = true };
            serializeOptions.Converters.Add(new Vector3Converter());
            var featureTable = JsonSerializer.Deserialize<FeatureTable>(featureTableJson,serializeOptions);

            // assert
            Assert.IsTrue(featureTable.InstancesLength == 2);
            Assert.IsTrue(featureTable.IsEastNorthUp== true);
            Assert.IsTrue(featureTable.PositionOffset.offset== 0);
        }

        [Test]
        public void PositionsToByteArrayTest()
        {
            // arrange
            var positions = new List<Vector3>
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 1, 1)
            };

            // act
            var bytes = positions.ToBytes();

            // Assert
            // should be 4 bytes each value, 2 positions, 3 values (x, y, z) each
            Assert.IsTrue(bytes.Length == 2*3*4);
        }
    }
}

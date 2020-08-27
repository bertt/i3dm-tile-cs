using System.Numerics;
using System.Text.Json.Serialization;

namespace I3dm.Tile
{
    // todo: add QUANTIZED_VOLUME_OFFSET, QUANTIZED_VOLUME_SCALE
    // todo: add POSITION_QUANTIZED, NORMAL_UP_OCT32P, NORMAL_RIGHT_OCT32P
    public class FeatureTable
    {
        public FeatureTable()
        {
            IsEastNorthUp = true;
        }

        [JsonPropertyName("INSTANCES_LENGTH")]
        public int InstancesLength { get; set; }

        [JsonPropertyName("POSITION")]
        public ByteOffset PositionOffset { get; set; }

        [JsonPropertyName("NORMAL_UP")]
        public ByteOffset NormalUpOffset { get; set; }

        [JsonPropertyName("NORMAL_RIGHT")]
        public ByteOffset NormalRightOffset { get; set; }

        [JsonPropertyName("SCALE_NON_UNIFORM")]
        public ByteOffset ScaleNonUniformOffset { get; set; }

        [JsonPropertyName("SCALE")]
        public ByteOffset ScaleOffset { get; set; }

        [JsonPropertyName("BATCH_ID")]
        public ByteOffset BatchIdOffset { get; set; }

        [JsonPropertyName("EAST_NORTH_UP")]
        public bool IsEastNorthUp { get; set; }

        [JsonPropertyName("RTC_CENTER")]
        public Vector3? RtcCenter { get; set; }
    }
}
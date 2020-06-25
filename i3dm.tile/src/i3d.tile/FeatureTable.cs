using System.Text.Json.Serialization;

namespace I3dm.Tile
{
    public class FeatureTable
    {
        // global properties
        [JsonPropertyName("INSTANCES_LENGTH")]
        public int InstancesLength { get; set; }

        // todo: add RTC_CENTER, QUANTIZED_VOLUME_OFFSET, QUANTIZED_VOLUME_SCALE, EAST_NORTH_UP

        [JsonPropertyName("POSITION")]
        public ByteOffset PositionOffset { get; set; }

        [JsonPropertyName("NORMAL_UP")]
        public ByteOffset NormalUpOffset { get; set; }
        [JsonPropertyName("NORMAL_RIGHT")]
        public ByteOffset NormalRightOffset { get; set; }
        [JsonPropertyName("SCALE_NON_UNIFORM")]
        public ByteOffset ScaleNonUniformOffset { get; set; }
        [JsonPropertyName("BATCH_ID")]
        public ByteOffset BatchIdOffset { get; set; }
        [JsonPropertyName("EAST_NORTH_UP")]
        public bool IsEastNorthUp { get; set; }
        // todo: add POSITION_QUANTIZED, NORMAL_UP_OCT32P, NORMAL_RIGHT_OCT32P, SCALE
    }
}

using System.Collections.Generic;
using System.Numerics;
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
        public Position Position { get; set; }

        [JsonPropertyName("NORMAL_UP")]
        public NormalUp NormalUp { get; set; }
        [JsonPropertyName("NORMAL_RIGHT")]
        public NormalRight NormalRight { get; set; }
        [JsonPropertyName("SCALE_NON_UNIFORM")]
        public ScaleNonUniform ScaleNonUniForm { get; set; }
        [JsonPropertyName("BATCH_ID")]
        public BatchId BatchId { get; set; }

        // todo: add POSITION_QUANTIZED, NORMAL_UP_OCT32P, NORMAL_RIGHT_OCT32P, SCALE

        public List<Vector3> Positions {get;set;}
        public List<Vector3> NormalUps { get; set; }
        public List<Vector3> NormalRights { get; set; }
        public List<Vector3> ScaleNonUniforms { get; set; }
    }

    public class Position
    {
        public int byteOffset { get; set; }
    }

    public class BatchId
    {
        public int byteOffset { get; set; }
        public string componentType { get; set; }
    }

    public class NormalUp
    {
        public int byteOffset { get; set; }
    }

    public class NormalRight
    {
        public int byteOffset { get; set; }
    }

    public class ScaleNonUniform
    {
        public int byteOffset { get; set; }
    }
}

using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace I3dm.Tile
{
    public class FeatureTable
    {
        [JsonPropertyName("INSTANCES_LENGTH")]
        public int InstancesLength { get; set; }
        [JsonPropertyName("POSITION")]
        public Position Position { get; set; }
        [JsonPropertyName("BATCH_ID")]
        public BatchId BatchId { get; set; }
        [JsonPropertyName("NORMAL_UP")]
        public NormalUp NormalUp { get; set; }
        [JsonPropertyName("NORMAL_RIGHT")]
        public NormalRight NormalRight { get; set; }
        [JsonPropertyName("SCALE_NON_UNIFORM")]
        public ScaleNonUniform ScaleNonUniForm { get; set; }
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

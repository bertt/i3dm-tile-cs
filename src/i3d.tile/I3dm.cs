﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace I3dm.Tile
{
    public class I3dm
    {
        public I3dm()
        {
            I3dmHeader = new I3dmHeader();
            FeatureTable = new FeatureTable();
            FeatureTableJson = string.Empty;
            BatchTableJson = string.Empty;
            FeatureTableBinary = new byte[0];
            BatchTableBinary = new byte[0];
        }

        public I3dm(List<Vector3> positions, byte[] glb) : this()
        {
            Positions = positions;
            GlbData = glb;
        }

        public I3dmHeader I3dmHeader { get; set; }
        public string FeatureTableJson { get; set; }
        public byte[] FeatureTableBinary { get; set; }
        public string BatchTableJson { get; set; }
        public byte[] BatchTableBinary { get; set; }
        public byte[] GlbData { get; set; }

        public FeatureTable FeatureTable { get; set; }

        public List<Vector3> Positions { get; set; }
        public List<Vector3> NormalUps { get; set; }
        public List<Vector3> NormalRights { get; set; }
        public List<Vector3> ScaleNonUniforms { get; set; }
        public List<byte> BatchIdsBytes { get; set; }

        public string GetFeatureTableJson()
        {
            var offset = 0;
            FeatureTable.InstancesLength = Positions.Count;
            FeatureTable.PositionOffset = new ByteOffset() { byteOffset = offset };
            if (NormalUps != null)
            {
                offset += Positions.ToBytes().Count();
                FeatureTable.NormalUpOffset = new ByteOffset() { byteOffset = offset};
            }
            if (NormalRights != null)
            {
                offset += NormalRights.ToBytes().Count();
                FeatureTable.NormalRightOffset = new ByteOffset() { byteOffset = offset };
            }
            if (ScaleNonUniforms != null)
            {
                offset += ScaleNonUniforms.ToBytes().Count();
                FeatureTable.ScaleNonUniformOffset = new ByteOffset() { byteOffset = offset };
            }

            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            var featureTableJson = JsonSerializer.Serialize(FeatureTable, options);
            return featureTableJson;
        }
    }
}

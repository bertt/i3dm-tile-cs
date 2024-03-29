﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace I3dm.Tile
{
    public class I3dm
    {
        private I3dm()
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
            I3dmHeader.GltfFormat = 1;
            Positions = positions;
            GlbData = glb;
        }

        public I3dm(List<Vector3> positions, string glbUrl) : this()
        {
            I3dmHeader.GltfFormat = 0;
            Positions = positions;
            GlbUrl = glbUrl;
        }

        public I3dmHeader I3dmHeader { get; set; }
        public string FeatureTableJson { get; set; }
        public byte[] FeatureTableBinary { get; set; }
        public string BatchTableJson { get; set; }
        public byte[] BatchTableBinary { get; set; }
        public byte[] GlbData { get; set; }
        public string GlbUrl { get; set; }
        public FeatureTable FeatureTable { get; set; }
        public List<Vector3> Positions { get; set; }
        public List<Vector3> NormalUps { get; set; }
        public List<Vector3> NormalRights { get; set; }
        public List<Vector3> ScaleNonUniforms { get; set; }
        public List<float> Scales { get; set; }
        public List<int> BatchIds { get; set; }
        public Vector3? RtcCenter { get; set; }

        public string GetFeatureTableJson(string batchIdSerializeType = "UNSIGNED_SHORT", int batchIdBytesLength=0)
        {
            var offset = 0;
            FeatureTable.InstancesLength = Positions.Count;
            FeatureTable.PositionOffset = new ByteOffset() { offset = offset };
            offset += Positions.ToBytes().Count();
            if (NormalUps != null)
            {
                FeatureTable.NormalUpOffset = new ByteOffset() { offset = offset};
                offset += NormalUps.ToBytes().Count();
            }
            if (NormalRights != null)
            {
                FeatureTable.NormalRightOffset = new ByteOffset() { offset = offset };
                offset += NormalRights.ToBytes().Count();
            }
            if (ScaleNonUniforms != null)
            {
                FeatureTable.ScaleNonUniformOffset = new ByteOffset() { offset = offset };
                offset += ScaleNonUniforms.ToBytes().Count();
            }
            if (Scales != null)
            {
                FeatureTable.ScaleOffset = new ByteOffset() { offset = offset };
                offset += Scales.ToBytes().Count();
            }
            if(RtcCenter != null)
            {
                FeatureTable.RtcCenter = RtcCenter;
                offset += ((Vector3)RtcCenter).ToBytes().Count();
            }
            if (BatchIds != null)
            {
                FeatureTable.BatchIdOffset = new ByteOffset() { offset = offset, componentType = batchIdSerializeType };
                // not needed beacuse last one: offset += batchIdBytesLength;
            }

            var options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull};
            options.Converters.Add(new Vector3Converter());
            var featureTableJson = JsonSerializer.Serialize(FeatureTable, options);
            return featureTableJson;
        }
    }
}

using System;
using System.Text.Json.Serialization;

namespace ZR.Model.System
{
    public class AlarmMessage
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("task_id")]
        public string TaskId { get; set; }

        [JsonProperty("camera_code")]
        public string CameraCode { get; set; }

        [JsonProperty("algorithm_code")]
        public string AlgorithmCode { get; set; }

        [JsonProperty("rtsp_url")]
        public string RtspUrl { get; set; }

        [JsonProperty("detection")]
        public Detection Detection { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        // 添加构造函数，初始化嵌套对象
        public AlarmMessage()
        {
            Detection = new Detection(); // 避免 Detection 为 null
        }
    }

    public class Detection
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }

        [JsonProperty("bbox")]
        public int[] BoundingBox { get; set; }

        // 添加构造函数，初始化数组
        public Detection()
        {
            BoundingBox = Array.Empty<int>(); // 避免 BoundingBox 为 null
        }
    }
}

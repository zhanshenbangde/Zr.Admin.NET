using System;

namespace ZR.Model.System.Dto
{
    /// <summary>
    /// 摄像头查询对象
    /// </summary>
    public class SysCameraQueryDto : PagerInfo
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryInfo { get; set; }
    }

    /// <summary>
    /// 摄像头信息
    /// </summary>
    public class SysCameraDto
    {
        /// <summary>
        /// 摄像头ID
        /// </summary>
        public long CameraId { get; set; }

        /// <summary>
        /// 摄像头名称
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 摄像头编码
        /// </summary>
        public string CameraCode { get; set; }

        /// <summary>
        /// 摄像头流地址
        /// </summary>
        public string StreamUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_time { get; set; }
    }
}

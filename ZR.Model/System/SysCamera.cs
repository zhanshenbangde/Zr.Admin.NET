using System;
using SqlSugar;

namespace ZR.Model.System
{
    /// <summary>
    /// 摄像头信息表
    /// </summary>
    [SugarTable("sys_camera")]
    public class SysCamera
    {
        /// <summary>
        /// 摄像头ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
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
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime Create_time { get; set; } = DateTime.Now;
    }
}

using System;

namespace ZR.Model.System.Dto
{
    /// <summary>
    /// 报警记录查询对象
    /// </summary>
    public class SysAlarmRecordQueryDto : PagerInfo
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryInfo { get; set; }

        /// <summary>
        /// 算法编码
        /// </summary>
        public string AlgorithmCode { get; set; }

        /// <summary>
        /// 摄像头编码
        /// </summary>
        public string CameraCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 报警记录信息
    /// </summary>
    public class SysAlarmRecordDto
    {
        /// <summary>
        /// 报警ID
        /// </summary>
        public long AlarmId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 算法编码
        /// </summary>
        public string AlgorithmCode { get; set; }

        /// <summary>
        /// 算法名称
        /// </summary>
        public string AlgorithmName { get; set; }

        /// <summary>
        /// 报警图片
        /// </summary>
        public string AlarmImage { get; set; }

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

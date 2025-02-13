using System;
using SqlSugar;

namespace ZR.Model.System
{
    /// <summary>
    /// 报警记录表
    /// </summary>
    [SugarTable("sys_alarm_record")]
    public class SysAlarmRecord
    {
        /// <summary>
        /// 报警ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
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
        /// 报警时间
        /// </summary>
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 报警结果(JSON)
        /// </summary>
        public string AlarmResult { get; set; }

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

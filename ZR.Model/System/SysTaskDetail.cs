using System;
using SqlSugar;

namespace ZR.Model.System
{
    /// <summary>
    /// 任务组合信息表
    /// </summary>
    [SugarTable("sys_alg_task_detail")]
    public class SysTaskDetail
    {
        /// <summary>
        /// 组合ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long DetailId { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 摄像头编码
        /// </summary>
        public string CameraCode { get; set; }

        /// <summary>
        /// 摄像头名称
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 算法编码
        /// </summary>
        public string AlgorithmCode { get; set; }

        /// <summary>
        /// 算法名称
        /// </summary>
        public string AlgorithmName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime Create_time { get; set; } = DateTime.Now;

        /// <summary>
        /// 导航属性 - 任务信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(TaskId))]
        public SysTask Task { get; set; }
    }
}

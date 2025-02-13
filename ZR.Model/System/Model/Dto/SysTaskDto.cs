using System;
using System.Collections.Generic;

namespace ZR.Model.System.Dto
{
    /// <summary>
    /// 任务查询对象
    /// </summary>
    public class SysTaskQueryDto : PagerInfo
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

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
    }

    /// <summary>
    /// 任务组合信息
    /// </summary>
    public class TaskDetailDto
    {
        /// <summary>
        /// 组合ID
        /// </summary>
        public long DetailId { get; set; }

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
    }

    /// <summary>
    /// 任务信息
    /// </summary>
    public class SysTaskDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 任务组合列表
        /// </summary>
        public List<TaskDetailDto> Details { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_time { get; set; }
    }
}

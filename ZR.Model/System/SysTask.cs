using System;
using SqlSugar;

namespace ZR.Model.System
{
    /// <summary>
    /// 任务信息表
    /// </summary>
    [SugarTable("sys_alg_task")]
    public class SysTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        // 导航属性：任务详情
        [Navigate(NavigateType.OneToMany, nameof(SysTaskDetail.TaskId))]
        public List<SysTaskDetail> Details { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime Create_time { get; set; } = DateTime.Now;
    }
}

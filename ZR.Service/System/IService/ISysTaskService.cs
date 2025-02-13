using Infrastructure.Model;
using ZR.Model;
using ZR.Model.System;
using ZR.Model.System.Dto;

namespace ZR.Service.System.IService
{
    /// <summary>
    /// 任务管理Service接口
    /// </summary>
    public interface ISysTaskService : IBaseService<SysTask>
    {
        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <param name="task">任务查询对象</param>
        /// <returns></returns>
        //PagedInfo<SysTaskDto> GetList(SysTaskQueryDto task);

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns></returns>
        SysTaskDto GetInfo(long taskId);

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns></returns>
        ApiResult AddTask(SysTaskDto task);

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns></returns>
        ApiResult UpdateTask(SysTaskDto task);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskIds">任务ID数组</param>
        /// <returns></returns>
        int DeleteTaskByIds(long[] taskIds);

        /// <summary>
        /// 删除任务组合
        /// </summary>
        /// <param name="detailId">组合ID</param>
        /// <returns></returns>
        int DeleteTaskDetail(long detailId);
    }
}

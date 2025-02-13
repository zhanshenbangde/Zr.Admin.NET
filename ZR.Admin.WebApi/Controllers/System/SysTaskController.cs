using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;

namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 任务管理Controller
    /// </summary>
    [Verify]
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("system/task")]
    public class SysTaskController : BaseController
    {
        private readonly ISysTaskService _SysTaskService;

        public SysTaskController(ISysTaskService SysTaskService)
        {
            _SysTaskService = SysTaskService;
        }

        ///// <summary>
        ///// 查询任务列表
        ///// </summary>
        ///// <param name="task">任务查询对象</param>
        ///// <returns></returns>
        //[HttpGet("list")]
        //[ActionPermissionFilter(Permission = "system:task:list")]
        //public IActionResult GetList([FromQuery] SysTaskQueryDto task)
        //{
        //    return SUCCESS(_SysTaskService.GetList(task));
        //}

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns></returns>
        [HttpGet("{taskId}")]
        [ActionPermissionFilter(Permission = "system:task:query")]
        public IActionResult GetInfo(long taskId)
        {
            return SUCCESS(_SysTaskService.GetInfo(taskId));
        }

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "system:task:add")]
        [Log(Title = "任务管理", BusinessType = BusinessType.INSERT)]
        public IActionResult Add([FromBody] SysTaskDto task)
        {
            return SUCCESS(_SysTaskService.AddTask(task));
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "system:task:edit")]
        [Log(Title = "任务管理", BusinessType = BusinessType.UPDATE)]
        public IActionResult Edit([FromBody] SysTaskDto task)
        {
            return SUCCESS(_SysTaskService.UpdateTask(task));
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskIds">需要删除的任务ID数组</param>
        /// <returns></returns>
        [HttpDelete("{taskIds}")]
        [ActionPermissionFilter(Permission = "system:task:delete")]
        [Log(Title = "任务管理", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string taskIds)
        {
            long[] ids = Tools.SpitLongArrary(taskIds);
            return SUCCESS(_SysTaskService.DeleteTaskByIds(ids));
        }

        /// <summary>
        /// 删除任务组合
        /// </summary>
        /// <param name="detailId">组合ID</param>
        /// <returns></returns>
        [HttpDelete("detail/{detailId}")]
        [ActionPermissionFilter(Permission = "system:task:delete")]
        [Log(Title = "任务管理", BusinessType = BusinessType.DELETE)]
        public IActionResult RemoveDetail(long detailId)
        {
            return SUCCESS(_SysTaskService.DeleteTaskDetail(detailId));
        }
    }
}

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
    /// 报警记录Controller
    /// </summary>
    [Verify]
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("system/alarm")]
    public class SysAlarmRecordController : BaseController
    {
        private readonly ISysAlarmRecordService _SysAlarmRecordService;

        public SysAlarmRecordController(ISysAlarmRecordService SysAlarmRecordService)
        {
            _SysAlarmRecordService = SysAlarmRecordService;
        }

        /// <summary>
        /// 查询报警记录列表
        /// </summary>
        /// <param name="alarm">报警记录查询对象</param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "system:alarm:list")]
        public IActionResult GetList([FromQuery] SysAlarmRecordQueryDto alarm)
        {
            return SUCCESS(_SysAlarmRecordService.GetList(alarm));
        }

        /// <summary>
        /// 获取报警记录详情
        /// </summary>
        /// <param name="alarmId">报警记录ID</param>
        /// <returns></returns>
        [HttpGet("{alarmId}")]
        [ActionPermissionFilter(Permission = "system:alarm:query")]
        public IActionResult GetInfo(long alarmId)
        {
            return SUCCESS(_SysAlarmRecordService.GetInfo(alarmId));
        }

        /// <summary>
        /// 新增报警记录
        /// </summary>
        /// <param name="alarm">报警记录信息</param>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "system:alarm:add")]
        [Log(Title = "报警记录", BusinessType = BusinessType.INSERT)]
        public IActionResult Add([FromBody] SysAlarmRecordDto alarm)
        {
            return SUCCESS(_SysAlarmRecordService.AddAlarmRecord(alarm));
        }

        /// <summary>
        /// 修改报警记录
        /// </summary>
        /// <param name="alarm">报警记录信息</param>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "system:alarm:edit")]
        [Log(Title = "报警记录", BusinessType = BusinessType.UPDATE)]
        public IActionResult Edit([FromBody] SysAlarmRecordDto alarm)
        {
            return SUCCESS(_SysAlarmRecordService.UpdateAlarmRecord(alarm));
        }

        /// <summary>
        /// 删除报警记录
        /// </summary>
        /// <param name="alarmIds">需要删除的报警记录ID数组</param>
        /// <returns></returns>
        [HttpDelete("{alarmIds}")]
        [ActionPermissionFilter(Permission = "system:alarm:delete")]
        [Log(Title = "报警记录", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string alarmIds)
        {
            long[] ids = Tools.SpitLongArrary(alarmIds);
            return SUCCESS(_SysAlarmRecordService.DeleteAlarmRecordByIds(ids));
        }
    }
}

using ZR.Model;
using ZR.Model.Public;
using ZR.Model.System;
using ZR.Model.System.Dto;

namespace ZR.Service.System.IService
{
    /// <summary>
    /// 报警记录Service接口
    /// </summary>
    public interface ISysAlarmRecordService : IBaseService<SysAlarmRecord>
    {
        /// <summary>
        /// 查询报警记录列表
        /// </summary>
        /// <param name="alarm">报警记录信息</param>
        /// <returns></returns>
        public PagedInfo<SysAlarmRecordDto> GetList(SysAlarmRecordQueryDto alarm);

        /// <summary>
        /// 新增报警记录
        /// </summary>
        /// <param name="alarm">报警记录信息</param>
        /// <returns></returns>
        public long AddAlarmRecord(SysAlarmRecordDto alarm);

        /// <summary>
        /// 修改报警记录
        /// </summary>
        /// <param name="alarm">报警记录信息</param>
        /// <returns></returns>
        public int UpdateAlarmRecord(SysAlarmRecordDto alarm);

        /// <summary>
        /// 删除报警记录
        /// </summary>
        /// <param name="alarmIds">需要删除的报警记录ID数组</param>
        /// <returns></returns>
        public string DeleteAlarmRecordByIds(long[] alarmIds);

        /// <summary>
        /// 获取报警记录详情
        /// </summary>
        /// <param name="alarmId">报警记录ID</param>
        /// <returns></returns>
        public SysAlarmRecordDto GetInfo(long alarmId);
    }
}

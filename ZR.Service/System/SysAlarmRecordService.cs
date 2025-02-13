using Infrastructure.Attribute;
using Infrastructure.Model;
using Mapster;
using SqlSugar;
using ZR.Model;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Repository;
using ZR.Service.System.IService;

namespace ZR.Service.System
{
    /// <summary>
    /// 报警记录Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ISysAlarmRecordService), ServiceLifetime = LifeTime.Transient)]
    public class SysAlarmRecordService : BaseService<SysAlarmRecord>, ISysAlarmRecordService
    {
        private static Expressionable<SysAlarmRecord> QueryExp(SysAlarmRecordQueryDto parm)
        {
            var predicate = Expressionable.Create<SysAlarmRecord>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => 
                it.TaskName.Contains(parm.QueryInfo) ||
                it.AlgorithmName.Contains(parm.QueryInfo) ||
                it.CameraName.Contains(parm.QueryInfo));

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.AlgorithmCode), it => it.AlgorithmCode == parm.AlgorithmCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CameraCode), it => it.CameraCode == parm.CameraCode);
            predicate = predicate.AndIF(parm.BeginTime != null, it => it.Create_time >= parm.BeginTime);
            predicate = predicate.AndIF(parm.EndTime != null, it => it.Create_time <= parm.EndTime);

            return predicate;
        }

        public long AddAlarmRecord(SysAlarmRecordDto alarm)
        {
            var alarmInfo = alarm.Adapt<SysAlarmRecord>();
            return Insertable(alarmInfo).ExecuteReturnSnowflakeId();
        }

        public string DeleteAlarmRecordByIds(long[] alarmIds)
        {
            bool isDeleted = Delete(m => alarmIds.Contains(m.AlarmId));
            return isDeleted ? "1" : "0";
        }

        public SysAlarmRecordDto GetInfo(long alarmId)
        {
            var alarm = Queryable()
                          .Where(a => a.AlarmId == alarmId)
                          .First();

            if (alarm == null)
            {
                return null;
            }

            return alarm.Adapt<SysAlarmRecordDto>();
        }

        public PagedInfo<SysAlarmRecordDto> GetList(SysAlarmRecordQueryDto alarm)
        {
            var predicate = QueryExp(alarm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .OrderByDescending(x => x.Create_time)
                .ToPage<SysAlarmRecord, SysAlarmRecordDto>(alarm);

            return response;
        }

        public int UpdateAlarmRecord(SysAlarmRecordDto alarm)
        {
            var alarmEntity = alarm.Adapt<SysAlarmRecord>();
            return Update(alarmEntity, true);
        }
    }
}

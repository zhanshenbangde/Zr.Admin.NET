using Infrastructure.Attribute;
using Infrastructure.Model;
using Mapster;
using SqlSugar;
using System.Linq;
using ZR.Model;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Repository;
using ZR.Service.System.IService;

namespace ZR.Service.System
{
    /// <summary>
    /// 任务管理Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ISysTaskService), ServiceLifetime = LifeTime.Transient)]
    public class SysTaskService : BaseService<SysTask>, ISysTaskService
    {
        private readonly ISysAlgorithmService _algorithmService;
        private readonly ISysCameraService _cameraService;

        public SysTaskService(ISysAlgorithmService algorithmService, ISysCameraService cameraService)
        {
            _algorithmService = algorithmService;
            _cameraService = cameraService;
        }

        /// <summary>
        /// 检查组合是否已存在
        /// </summary>
        private (bool exists, string taskName) CheckCombinationExists(string cameraCode, string algorithmCode, long? excludeTaskId = null)
        {
            var detail = Context.Queryable<SysTaskDetail>()
                .LeftJoin<SysTask>((d, t) => d.TaskId == t.TaskId)
                .Where(d => d.CameraCode == cameraCode && d.AlgorithmCode == algorithmCode)
                .WhereIF(excludeTaskId.HasValue, (d, t) => d.TaskId != excludeTaskId.Value)
                .Select((d, t) => new { t.TaskName })
                .First();

            return detail != null ? (true, detail.TaskName) : (false, null);
        }

        public ApiResult AddTask(SysTaskDto task)
        {
            // 检查每个组合是否已存在
            foreach (var detail in task.Details)
            {
                var (exists, taskName) = CheckCombinationExists(detail.CameraCode, detail.AlgorithmCode);
                if (exists)
                {
                    return ApiResult.Error($"组合[摄像头:{detail.CameraCode}, 算法:{detail.AlgorithmCode}]已存在于任务[{taskName}]中");
                }
            }

            var taskEntity = task.Adapt<SysTask>();
            var taskId = Context.Insertable(taskEntity).ExecuteReturnSnowflakeId();

            // 补充组合信息并插入
            var details = task.Details.Select(d =>
            {
                var camera = _cameraService.Queryable().First(m=>m.CameraCode== d.CameraCode);
                var algorithm = _algorithmService.Queryable().First(m => m.AlgorithmCode == d.AlgorithmCode);

                return new SysTaskDetail
                {
                    TaskId = taskId,
                    CameraCode = d.CameraCode,
                    CameraName = camera?.CameraName,
                    AlgorithmCode = d.AlgorithmCode,
                    AlgorithmName = algorithm?.AlgorithmName
                };
            }).ToList();

            Context.Insertable(details).ExecuteCommand();

            return ApiResult.Success();
        }

        public int DeleteTaskByIds(long[] taskIds)
        {

            var result = UseTran(() =>
            {
                // 删除任务组合
                Context.Deleteable<SysTaskDetail>().Where(d => taskIds.Contains(d.TaskId)).ExecuteCommand();
                // 删除任务
                Context.Deleteable<SysTask>().Where(t => taskIds.Contains(t.TaskId)).ExecuteCommand();


            });

            return result.IsSuccess ? 1 : 0;
        }

        public int DeleteTaskDetail(long detailId)
        {
            return Context.Deleteable<SysTaskDetail>().Where(d => d.DetailId == detailId).ExecuteCommand();
        }

        public SysTaskDto GetInfo(long taskId)
        {
            var task = Context.Queryable<SysTask>()
                .Mapper(t => t.Details, t => t.TaskId)
                .Where(t => t.TaskId == taskId)
                .First()
                .Adapt<SysTaskDto>();

            return task;
        }

        //public PagedInfo<SysTaskDto> GetList(SysTaskQueryDto query)
        //{
        //    var expression = Expressionable.Create<SysTaskDetail>();

        //    expression = expression.AndIF(!string.IsNullOrEmpty(query.CameraCode), 
        //                               d => d.CameraCode.Contains(query.CameraCode));
        //    expression = expression.AndIF(!string.IsNullOrEmpty(query.CameraName), 
        //                               d => d.CameraName.Contains(query.CameraName));
        //    expression = expression.AndIF(!string.IsNullOrEmpty(query.AlgorithmCode), 
        //                               d => d.AlgorithmCode.Contains(query.AlgorithmCode));
        //    expression = expression.AndIF(!string.IsNullOrEmpty(query.AlgorithmName), 
        //                               d => d.AlgorithmName.Contains(query.AlgorithmName));

        //    var result = Context.Queryable<SysTaskDetail>()
        //        .LeftJoin<SysTask>((d, t) => d.TaskId == t.TaskId)
        //        .WhereIF(!string.IsNullOrEmpty(query.TaskName), (d, t) => t.TaskName.Contains(query.TaskName))
        //        .Where(expression.ToExpression())
        //        .OrderBy((d, t) => t.Create_time, OrderByType.Desc)
        //        .Select((d, t) => new SysTaskDto
        //        {
        //            TaskId = t.TaskId,
        //            TaskName = t.TaskName,
        //            Remark = t.Remark,
        //            Create_time = t.Create_time,
        //            Details = SqlFunc.Subqueryable<SysTaskDetail>()
        //                .Where(sd => sd.TaskId == t.TaskId)
        //                .ToList()
        //        })
        //        .ToPage(query);

        //    return result;
        //}

        public ApiResult UpdateTask(SysTaskDto task)
        {
            var oldDetails = Context.Queryable<SysTaskDetail>()
                .Where(d => d.TaskId == task.TaskId)
                .ToList();

            var newDetails = task.Details;

            // 找出需要新增的组合
            var detailsToAdd = newDetails.Where(n => 
                !oldDetails.Any(o => o.CameraCode == n.CameraCode && o.AlgorithmCode == n.AlgorithmCode))
                .ToList();

            // 检查新增的组合是否已存在于其他任务
            foreach (var detail in detailsToAdd)
            {
                var (exists, taskName) = CheckCombinationExists(detail.CameraCode, detail.AlgorithmCode, task.TaskId);
                if (exists)
                {
                    return ApiResult.Error($"组合[摄像头:{detail.CameraCode}, 算法:{detail.AlgorithmCode}]已存在于任务[{taskName}]中");
                }
            }

            // 找出需要删除的组合
            var detailsToDelete = oldDetails.Where(o => 
                !newDetails.Any(n => n.CameraCode == o.CameraCode && n.AlgorithmCode == o.AlgorithmCode))
                .ToList();

            UseTran(() =>
            {
                // 更新任务基本信息
                var taskEntity = task.Adapt<SysTask>();
                Context.Updateable(taskEntity)
                    .IgnoreColumns(t => new { t.Create_time })
                    .ExecuteCommand();

                // 删除不需要的组合
                if (detailsToDelete.Any())
                {
                    var deleteIds = detailsToDelete.Select(d => d.DetailId).ToArray();
                    Context.Deleteable<SysTaskDetail>().Where(d => deleteIds.Contains(d.DetailId)).ExecuteCommand();
                }

                // 添加新的组合
                if (detailsToAdd.Any())
                {
                    var details = detailsToAdd.Select(d =>
                    {
                        var camera = _cameraService.Queryable().First(m => m.CameraCode == d.CameraCode);
                        var algorithm = _algorithmService.Queryable().First(m => m.AlgorithmCode == d.AlgorithmCode);

                        return new SysTaskDetail
                        {
                            TaskId = task.TaskId,
                            CameraCode = d.CameraCode,
                            CameraName = camera?.CameraName,
                            AlgorithmCode = d.AlgorithmCode,
                            AlgorithmName = algorithm?.AlgorithmName
                        };
                    }).ToList();

                    Context.Insertable(details).ExecuteCommand();
                }
            });

            return ApiResult.Success();
        }
    }
}

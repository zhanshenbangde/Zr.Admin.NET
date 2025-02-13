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
    /// 摄像头管理Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ISysCameraService), ServiceLifetime = LifeTime.Transient)]
    public class SysCameraService : BaseService<SysCamera>, ISysCameraService
    {
        private static Expressionable<SysCamera> QueryExp(SysCameraQueryDto parm)
        {
            var predicate = Expressionable.Create<SysCamera>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => it.CameraCode.Contains(parm.QueryInfo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => it.CameraName.Contains(parm.QueryInfo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => it.Remark.Contains(parm.QueryInfo));

            return predicate;
        }

        public long AddCamera(SysCameraDto camera)
        {
            var cameraInfo = new SysCamera()
            {
                CameraName = camera.CameraName,
                CameraCode = camera.CameraCode,
                StreamUrl = camera.StreamUrl,
                Remark = camera.Remark,
            };

            return Insertable(cameraInfo).ExecuteReturnSnowflakeId();
        }

        public bool CheckCameraCodeUnique(SysCameraDto camera)
        {
            return Queryable()
                     .Where(c => c.CameraCode == camera.CameraCode)
                     .Any();
        }

        public string DeleteCameraByIds(long[] cameraIds)
        {
            bool isDeleted = Delete(m => cameraIds.Contains(m.CameraId));
            return isDeleted ? "1" : "0";
        }

        public SysCameraDto GetInfo(long cameraId)
        {
            var camera = Queryable()
                          .Where(c => c.CameraId == cameraId)
                          .First();

            if (camera == null)
            {
                return null;
            }

            return camera.Adapt<SysCameraDto>();
        }

        public PagedInfo<SysCameraDto> GetList(SysCameraQueryDto camera)
        {
            var predicate = QueryExp(camera);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<SysCamera, SysCameraDto>(camera);

            return response;
        }

        public int UpdateCamera(SysCameraDto camera)
        {
            var cameraEntity = camera.Adapt<SysCamera>();
            return Update(cameraEntity, true);
        }
    }
}

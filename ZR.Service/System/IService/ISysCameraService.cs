using ZR.Model;
using ZR.Model.System;
using ZR.Model.System.Dto;

namespace ZR.Service.System.IService
{
    /// <summary>
    /// 摄像头管理Service接口
    /// </summary>
    public interface ISysCameraService : IBaseService<SysCamera>
    {
        /// <summary>
        /// 查询摄像头管理列表
        /// </summary>
        /// <param name="camera">摄像头信息</param>
        /// <returns></returns>
        public PagedInfo<SysCameraDto> GetList(SysCameraQueryDto camera);

        /// <summary>
        /// 校验摄像头编码是否唯一
        /// </summary>
        /// <param name="camera">摄像头信息</param>
        /// <returns></returns>
        public bool CheckCameraCodeUnique(SysCameraDto camera);

        /// <summary>
        /// 新增摄像头
        /// </summary>
        /// <param name="camera">摄像头信息</param>
        /// <returns></returns>
        public long AddCamera(SysCameraDto camera);

        /// <summary>
        /// 修改摄像头
        /// </summary>
        /// <param name="camera">摄像头信息</param>
        /// <returns></returns>
        public int UpdateCamera(SysCameraDto camera);

        /// <summary>
        /// 删除摄像头
        /// </summary>
        /// <param name="cameraIds">需要删除的摄像头ID数组</param>
        /// <returns></returns>
        public string DeleteCameraByIds(long[] cameraIds);

        /// <summary>
        /// 获取摄像头详情
        /// </summary>
        /// <param name="cameraId">摄像头ID</param>
        /// <returns></returns>
        public SysCameraDto GetInfo(long cameraId);
    }
}

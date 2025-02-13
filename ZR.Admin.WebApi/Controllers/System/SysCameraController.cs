using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Filters;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;

namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 摄像头管理
    /// </summary>    
    [Verify]
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("system/camera")]
    public class SysCameraController : BaseController
    {
        private readonly ISysCameraService _SysCameraService;

        public SysCameraController(ISysCameraService SysCameraService)
        {
            _SysCameraService = SysCameraService;
        }

        /// <summary>
        /// 查询摄像头管理列表
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "system:camera:list")]
        public IActionResult QueryCamera([FromQuery] SysCameraQueryDto camera)
        {
            return SUCCESS(_SysCameraService.GetList(camera));
        }

        /// <summary>
        /// 获取摄像头管理详细信息
        /// </summary>
        /// <param name="CameraId"></param>
        /// <returns></returns>
        [HttpGet("{CameraId}")]
        [ActionPermissionFilter(Permission = "system:camera:query")]
        public IActionResult GetInfo(long CameraId)
        {
            return SUCCESS(_SysCameraService.GetInfo(CameraId));
        }

        /// <summary>
        /// 新增摄像头管理
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "system:camera:add")]
        [Log(Title = "摄像头管理", BusinessType = BusinessType.INSERT)]
        public IActionResult Add([FromBody] SysCameraDto camera)
        {
            if (_SysCameraService.CheckCameraCodeUnique(camera))
            {
                return ToResponse(ApiResult.Error($"新增摄像头'{camera.CameraName}'失败，摄像头编码已存在"));
            }

            return SUCCESS(_SysCameraService.AddCamera(camera));
        }

        /// <summary>
        /// 修改摄像头管理
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "system:camera:edit")]
        [Log(Title = "摄像头管理", BusinessType = BusinessType.UPDATE)]
        public IActionResult Edit([FromBody] SysCameraDto camera)
        {
            if (_SysCameraService.CheckCameraCodeUnique(camera))
            {
                return ToResponse(ApiResult.Error($"修改摄像头'{camera.CameraName}'失败，摄像头编码已存在"));
            }

            return SUCCESS(_SysCameraService.UpdateCamera(camera));
        }

        /// <summary>
        /// 删除摄像头管理
        /// </summary>
        /// <param name="cameraIds"></param>
        /// <returns></returns>
        [HttpDelete("{cameraIds}")]
        [ActionPermissionFilter(Permission = "system:camera:delete")]
        [Log(Title = "摄像头管理", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string cameraIds)
        {
            long[] ids = Tools.SpitLongArrary(cameraIds);
            return SUCCESS(_SysCameraService.DeleteCameraByIds(ids));
        }
    }
}

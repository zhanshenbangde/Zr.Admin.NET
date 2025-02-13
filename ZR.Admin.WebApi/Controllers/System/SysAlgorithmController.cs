using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;

namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 算法管理Controller
    /// </summary>
    [Verify]    
    [ApiExplorerSettings(GroupName = "sys")]
    [Route("system/algorithm")]
    public class SysAlgorithmController : BaseController
    {
        private readonly ISysAlgorithmService _SysAlgorithmService;

        public SysAlgorithmController(ISysAlgorithmService SysAlgorithmService)
        {
            _SysAlgorithmService = SysAlgorithmService;
        }

        /// <summary>
        /// 查询算法管理列表
        /// </summary>
        /// <param name="algorithm">算法查询对象</param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "system:algorithm:list")]
        public IActionResult GetList([FromQuery] SysAlgorithmQueryDto algorithm)
        {
     
            return SUCCESS(_SysAlgorithmService.GetList(algorithm));
        }

        /// <summary>
        /// 获取算法详情
        /// </summary>
        /// <param name="algorithmId">算法ID</param>
        /// <returns></returns>
        [HttpGet("{algorithmId}")]
        [ActionPermissionFilter(Permission = "system:algorithm:query")]
        public IActionResult GetInfo(long algorithmId)
        {
            return SUCCESS(_SysAlgorithmService.GetInfo(algorithmId));
        }

        /// <summary>
        /// 新增算法
        /// </summary>
        /// <param name="algorithm">算法信息</param>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "system:algorithm:add")]
        [Log(Title = "算法管理", BusinessType = BusinessType.INSERT)]
        public IActionResult Add([FromBody] SysAlgorithmDto algorithm)
        {
            if (_SysAlgorithmService.CheckAlgorithmCodeUnique(algorithm))
            {
                return ToResponse(ApiResult.Error($"新增算法'{algorithm.AlgorithmName}'失败，算法编码已存在"));
            }

            return ToResponse(_SysAlgorithmService.AddAlgorithm(algorithm));
        }

        /// <summary>
        /// 修改算法
        /// </summary>
        /// <param name="algorithm">算法信息</param>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "system:algorithm:edit")]
        [Log(Title = "算法管理", BusinessType = BusinessType.UPDATE)]
        public IActionResult Edit([FromBody] SysAlgorithmDto algorithm)
        {
            if (_SysAlgorithmService.CheckAlgorithmCodeUnique(algorithm))
            {
                return ToResponse(ApiResult.Error($"修改算法'{algorithm.AlgorithmName}'失败，算法编码已存在"));
            }

            return ToResponse(_SysAlgorithmService.UpdateAlgorithm(algorithm));
        }

        /// <summary>
        /// 删除算法
        /// </summary>
        /// <param name="algorithmIds">需要删除的算法ID数组</param>
        /// <returns></returns>
        [HttpDelete("{algorithmIds}")]
        [ActionPermissionFilter(Permission = "system:algorithm:remove")]
        [Log(Title = "算法管理", BusinessType = BusinessType.DELETE)]
        public IActionResult Remove(string algorithmIds)
        {
            long[] ids = Tools.SpitLongArrary(algorithmIds);

            
            return SUCCESS(_SysAlgorithmService.DeleteAlgorithmByIds(ids));
        }
    }
}

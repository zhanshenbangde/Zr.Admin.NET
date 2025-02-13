using ZR.Model.System;
using ZR.Model.System.Dto;

namespace ZR.Service.System.IService
{
    public interface ISysAlgorithmService : IBaseService<SysAlgorithm>
    {
        /// <summary>
        /// 查询算法管理列表
        /// </summary>
        /// <param name="algorithm">算法查询对象</param>
        /// <returns></returns>
        public PagedInfo<SysAlgorithmDto> GetList(SysAlgorithmQueryDto algorithm);

        /// <summary>
        /// 校验算法编码是否唯一
        /// </summary>
        /// <param name="algorithm">算法信息</param>
        /// <returns></returns>
        public bool CheckAlgorithmCodeUnique(SysAlgorithmDto algorithm);

        /// <summary>
        /// 新增算法
        /// </summary>
        /// <param name="algorithm">算法信息</param>
        /// <returns></returns>
        public long AddAlgorithm(SysAlgorithmDto algorithm);

        /// <summary>
        /// 修改算法
        /// </summary>
        /// <param name="algorithm">算法信息</param>
        /// <returns></returns>
        public int UpdateAlgorithm(SysAlgorithmDto algorithm);

        /// <summary>
        /// 删除算法
        /// </summary>
        /// <param name="algorithmIds">需要删除的算法ID数组</param>
        /// <returns></returns>
        public string DeleteAlgorithmByIds(long[] algorithmIds);

        /// <summary>
        /// 获取算法详情
        /// </summary>
        /// <param name="algorithmId">算法ID</param>
        /// <returns></returns>
        public SysAlgorithmDto GetInfo(long algorithmId);
    }
}

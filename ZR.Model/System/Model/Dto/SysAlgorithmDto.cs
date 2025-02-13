using System;
using ZR.Model;

namespace ZR.Model.System.Dto
{
    /// <summary>
    /// 算法管理查询对象
    /// </summary>
    public class SysAlgorithmQueryDto : PagerInfo
    {
        /// <summary>
        /// 算法名称
        /// </summary>
        public string QueryInfo { get; set; }       
    }

    /// <summary>
    /// 算法管理输出对象
    /// </summary>
    public class SysAlgorithmDto : SysBase
    {
        /// <summary>
        /// 算法ID
        /// </summary>
        public string AlgorithmId { get; set; }

        /// <summary>
        /// 算法名称
        /// </summary>
        public string AlgorithmName { get; set; }

        /// <summary>
        /// 算法编码
        /// </summary>
        public string AlgorithmCode { get; set; }

        /// <summary>
        /// 算法类型
        /// </summary>
        public string AlgorithmType { get; set; }

        /// <summary>
        /// 算法描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态（0正常 1停用）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

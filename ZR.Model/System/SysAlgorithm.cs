using System;
using SqlSugar;

namespace ZR.Model.System
{
    /// <summary>
    /// 算法管理表
    /// </summary>
    [SugarTable("sys_algorithm")]
    [Tenant("0")]
    public class SysAlgorithm : SysBase
    {
        /// <summary>
        /// 算法ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long AlgorithmId { get; set; }

        /// <summary>
        /// 算法名称
        /// </summary>
        public string AlgorithmName { get; set; }

        /// <summary>
        /// 算法编码
        /// </summary>
        public string AlgorithmCode { get; set; }

     
        /// <summary>
        /// 算法描述
        /// </summary>
        public string Description { get; set; }

      
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

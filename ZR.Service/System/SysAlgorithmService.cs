using Aliyun.OSS;
using Infrastructure.Attribute;
using Infrastructure.Model;
using Mapster;
using ZR.Model.Models;
using ZR.Model.Public.Dto;
using ZR.Model.Public;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;
using ZR.Repository;

namespace ZR.Service.System
{
    /// <summary>
    /// 算法管理Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ISysAlgorithmService), ServiceLifetime = LifeTime.Transient)]
    public class SysAlgorithmService : BaseService<SysAlgorithm>, ISysAlgorithmService
    {
        private static Expressionable<SysAlgorithm> QueryExp(SysAlgorithmQueryDto parm)
        {
            var predicate = Expressionable.Create<SysAlgorithm>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => it.AlgorithmCode.Contains(parm.QueryInfo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => it.AlgorithmName.Contains(parm.QueryInfo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.QueryInfo), it => it.Remark.Contains(parm.QueryInfo));            
            
            return predicate;
        }

        public long AddAlgorithm(SysAlgorithmDto algorithm)
        {
            var algInfo = new SysAlgorithm()
            {               
                AlgorithmName = algorithm.AlgorithmName,
                AlgorithmCode   = algorithm.AlgorithmCode,             
                Description =algorithm.Description,
                Remark = algorithm.Remark,                 
            };

             return Insertable(algInfo).ExecuteReturnSnowflakeId();
        }

        public bool CheckAlgorithmCodeUnique(SysAlgorithmDto algorithm)
        {
            // 使用 SQLSugar 的 Queryable 方法进行查询
            return Queryable()
                     .Where(a => a.AlgorithmCode == algorithm.AlgorithmCode)
                     .Any();           
        }

        public string DeleteAlgorithmByIds(long[] algorithmIds)
        {
            bool isDeleted = Delete(m => algorithmIds.Contains(m.AlgorithmId));

            return isDeleted ? "1" : "0";
        }   

        public SysAlgorithmDto GetInfo(long algorithmId)
        {
            // 查询数据
            var algorithm = Queryable()
                              .Where(a => a.AlgorithmId == algorithmId)
                              .First();

            if (algorithm == null)
            {
                return null; // 或者抛出异常
            }

            // 使用 Mapster 进行对象映射
            var algorithmDto = algorithm.Adapt<SysAlgorithmDto>();

            return algorithmDto;
        }

        public PagedInfo<SysAlgorithmDto> GetList(SysAlgorithmQueryDto algorithm)
        {
            var predicate = QueryExp(algorithm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<SysAlgorithm, SysAlgorithmDto>(algorithm);

            return response;
        }

        public int UpdateAlgorithm(SysAlgorithmDto algorithm)
        {
            // 将 DTO 对象映射为实体对象
            var algorithmEntity = algorithm.Adapt<SysAlgorithm>();

            // 使用 SQLSugar 的 Updateable 方法更新记录
            var result = Update(algorithmEntity,true);
                           

            return result;
        }
    }
}

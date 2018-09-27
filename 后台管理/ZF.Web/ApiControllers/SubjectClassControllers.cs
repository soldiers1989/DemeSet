using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SubjectClass 
    /// </summary>
    public class SubjectClassController : BaseController
    {
        private readonly SubjectClassAppService _subjectClassAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public SubjectClassController(SubjectClassAppService subjectClassAppService, OperatorLogAppService operatorLogAppService)
        {
            _subjectClassAppService = subjectClassAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectClass 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SubjectClassOutput> GetList(SubjectClassListInput input)
        {
            var count = 0;
            var list = _subjectClassAppService.GetList(input, out count);
            return new JqGridOutPut<SubjectClassOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 根据科目试卷结构id查询试题分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SubjectClassOutput> GetListByStructureId(string ProjectId)
        {
            var list = _subjectClassAppService.GetListByStructureId(ProjectId);
            return list;
        }


        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            var idList = array.Aggregate("", (current, item) => current + ("'" + item + "'" + ",")).TrimEnd(',');
            var count = _subjectClassAppService.GetCount(idList);
            if (count > 0)
            {
                return new MessagesOutPut { Id = 1, Message = "删除失败,所删除题型下存在有效试题!", Success = false };
            }
            foreach (var item in array)
            {
                var model = _subjectClassAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.SubjectClass,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除题型名称:" + model.ClassName
                    });
                }
                _subjectClassAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：SubjectClass
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SubjectClassInput input)
        {
            var data = _subjectClassAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public SubjectClass GetOne(IdInput input)
        {
            var model = _subjectClassAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public int? GetOneId(IdInput input)
        {
            var model = _subjectClassAppService.Get(input.Id);
            return model.BigType;
        }
    }
}


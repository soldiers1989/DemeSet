using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.MyCourseModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.WebApiAppService.MyCourseModule
{
    /// <summary>
    /// 数据表实体应用服务现实：我的足迹 
    /// </summary>
    public class MyFootprintAppService : BaseAppService<MyFootprint>
    {
        public string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly IMyFootprintRepository _iMyFootprintRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iMyFootprintRepository"></param>
        public MyFootprintAppService(IMyFootprintRepository iMyFootprintRepository) : base(iMyFootprintRepository)
        {
            _iMyFootprintRepository = iMyFootprintRepository;
        }

        /// <summary>
        /// 查询列表实体：我的足迹 
        /// </summary>
        public List<MyFootprintOutput> GetList(MyFootprintListInput input, out int count)
        {
            string sql = "SELECT  a.*,b.CourseName,'" + DefuleDomain + "/'+b.CourseIamge as CourseIamge,f.ProjectName ";
            var strSql = new StringBuilder(@" FROM    t_My_Footprint a
LEFT JOIN V_Course_Packcourse_Info b ON a.CourseId = b.Id
LEFT JOIN dbo.t_Base_Project f ON b.ProjectId=f.Id
WHERE   a.IsDelete = 0 ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.UserId))
            {
                strSql.Append(" and a.UserId=@UserId ");
                dynamicParameters.Add(":UserId", input.UserId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.query))
            {
                strSql.Append(" and b.CourseName like  @query ");
                dynamicParameters.Add(":query", '%' + input.query + '%', DbType.String);
            }
            switch (input.Type)
            {
                case 0:
                    break;
                case 1:
                    strSql.Append(" and a.BrowsingTime>@BrowsingTime");
                    dynamicParameters.Add(":BrowsingTime", DateTime.Now.AddDays(-7), DbType.DateTime);
                    break;
                case 2:
                    strSql.Append(" and a.BrowsingTime>@BrowsingTime");
                    dynamicParameters.Add(":BrowsingTime", DateTime.Now.AddDays(-30), DbType.DateTime);
                    break;
                case 3:
                    strSql.Append(" and a.BrowsingTime>@BrowsingTime");
                    dynamicParameters.Add(":BrowsingTime", DateTime.Now.AddDays(-365), DbType.DateTime);
                    break;
            }
            count = Db.QueryFirstOrDefault<int>(" select count(1) from (" + sql + strSql + ")  b", dynamicParameters);
            return Db.QueryList<MyFootprintOutput>(GetPageSql(sql+ strSql, input.Page, input.Rows, "BrowsingTime desc"), dynamicParameters);
        }

        /// <summary>
        /// 新增实体  我的足迹
        /// </summary>
        public MessagesOutPut AddOrEdit(MyFootprintInput input)
        {
            if (string.IsNullOrEmpty(input.UserId))
            {
                return new MessagesOutPut { Success = false };
            }
            string sql = @"select a.* from t_My_Footprint a WHERE   a.IsDelete = 0 ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.UserId))
            {
                sql += " and a.UserId=@UserId";
                dynamicParameters.Add(":UserId", input.UserId, DbType.String);
            }
            if ( !string.IsNullOrEmpty( input.CourseId ) ) {
                sql += " and a.CourseId=@CourseId ";
                dynamicParameters.Add(":CourseId",input.CourseId,DbType.String);
            }
            sql += " and a.CourseType=@CourseType";
            dynamicParameters.Add(":CourseType", input.CourseType, DbType.Int32);
            var model = Db.QueryFirstOrDefault<MyFootprint>(sql, dynamicParameters) ?? new MyFootprint();
            if (!string.IsNullOrEmpty(model.Id))
            {
                model.BrowsingTime = DateTime.Now;
                _iMyFootprintRepository.Update(model);
                return new MessagesOutPut { Success = true };
            }
            model = input.MapTo<MyFootprint>();
            model.Id = Guid.NewGuid().ToString();
            model.BrowsingTime = DateTime.Now;
            var keyId = _iMyFootprintRepository.InsertGetId(model);
            return new MessagesOutPut { Success = true };
        }
    }
}


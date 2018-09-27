
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.zTree;
using static Dapper.SqlMapper;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：CourseChapter 
    /// </summary>
    public class CourseChapterAppService : BaseAppService<CourseChapter>
    {
	   private readonly ICourseChapterRepository _iCourseChapterRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iCourseChapterRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public CourseChapterAppService(ICourseChapterRepository iCourseChapterRepository,OperatorLogAppService operatorLogAppService): base(iCourseChapterRepository)
	   {
			_iCourseChapterRepository = iCourseChapterRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：CourseChapter 
       /// 
       /// </summary>
	   public  List<CourseChapterOutput> GetList(CourseChapterListInput input)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Course_Chapter  a  where a.IsDelete=0  ");
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.CapterName))
          {
              strSql.Append( " and a.CapterName = @CapterName " );
              dynamicParameters.Add( ":CapterName", input.CapterName, DbType.String);
          }
            if ( !string.IsNullOrWhiteSpace( input.CourseId ) )
            {
                strSql.Append( " and a.CourseId = @CourseId " );
                dynamicParameters.Add( ":CourseId", input.CourseId, DbType.String );
            }
            if ( !string.IsNullOrWhiteSpace( input.ParentId ) )
            {
                strSql.Append( " and a.ParentId = @ParentId " );
                dynamicParameters.Add( ":ParentId", input.ParentId, DbType.String );
            }
            var list = Db.QueryList<CourseChapterOutput>(sql + strSql, dynamicParameters);
          return list;
	   }


        /// <summary>
        /// 获取章节树
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> GetChpaterTree( CourseChapterListInput input) {
             string strSql = @" SELECT  *
            FROM    ( SELECT    a.Id ,
                                a.CapterName AS Name ,
                                a.ParentId AS Pid,
                                a.CourseId,
                                a.CapterCode,
                                a.OrderNo
                      FROM      dbo.t_Course_Chapter a
                      WHERE a.IsDelete=0
                      UNION
                      SELECT    b.id,
                                b.CapterName AS Name ,
                                b.ParentId AS pId,
                                b.CourseId ,
                                b.CapterCode,
                                b.OrderNo
                      FROM      t_Course_Chapter b
                      WHERE b.IsDelete=0
                    ) c where 1=1 ";

            var dynamicParameters = new DynamicParameters( );
            if ( !string.IsNullOrWhiteSpace( input.CourseId ) )
            {
                strSql+=( " and c.CourseId = @CourseId " );
                dynamicParameters.Add( ":CourseId", input.CourseId, DbType.String );
            }
            if ( !string.IsNullOrWhiteSpace( input.ParentId ) )
            {
                strSql+=( " and c.ParentId = @ParentId " );
                dynamicParameters.Add( ":ParentId", input.ParentId, DbType.String );
            }
            strSql += " ORDER BY c.OrderNo asc ";
            var chapterList = Db.QueryList<Tree.zTree>( strSql,dynamicParameters);
            var row = new List<Tree.zTree>( );
            //var length = chapterList.Count;
            for ( int i = 0; i < chapterList.Count; i++ ) {
                var item = chapterList[i];
                if ( String.IsNullOrWhiteSpace( item.pId ) )
                {
                    row.Add( item );
                    Tree.SelfTree( item, chapterList );
                    //chapterList.Remove( item );
                }
            }
            return row;
        }


        /// <summary>
        /// 获取章节-视频节点树
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> GetChpaterVideoTree( CourseChapterListInput input )
        {
            string strSql = @" SELECT  c.Id,c.Name,c.Pid,c.CourseId,c.OrderNo,MAX(c.type) AS type
            FROM    ( SELECT    a.Id ,
                                a.CapterName AS Name ,
                                a.ParentId AS Pid,
                                a.CourseId,
                                a.OrderNo,
                                0 as Type
                      FROM      dbo.t_Course_Chapter a
                      WHERE a.IsDelete=0
                      UNION
                      SELECT    b.id,
                                b.CapterName AS Name ,
                                b.ParentId AS pId,
                                b.CourseId ,
                                b.OrderNo,
                                1 as Type
                      FROM      t_Course_Chapter b
                      WHERE b.IsDelete=0
                      union 
                      SELECT c.Id,c.VideoName AS Name,c.ChapterId AS Pid, a.CourseId,c.OrderNo,2 AS Type 
                      FROM  dbo.t_Course_Chapter a 
                      LEFT JOIN dbo.t_Course_Chapter b ON a.ParentId=b.Id 
                      LEFT JOIN dbo.t_Course_Video c ON a.Id=c.ChapterId
                        WHERE c.IsDelete=0
                    ) c where 1=1 ";

            var dynamicParameters = new DynamicParameters( );
            if ( !string.IsNullOrWhiteSpace( input.CourseId ) )
            {
                strSql += ( " and c.CourseId = @CourseId " );
                dynamicParameters.Add( ":CourseId", input.CourseId, DbType.String );
            }
            if ( !string.IsNullOrWhiteSpace( input.ParentId ) )
            {
                strSql += ( " and c.ParentId = @ParentId " );
                dynamicParameters.Add( ":ParentId", input.ParentId, DbType.String );
            }
            strSql += " GROUP BY c.Id,c.Name,c.Pid,c.CourseId,c.OrderNo  ORDER BY c.OrderNo asc ";
            var chapterList = Db.QueryList<Tree.zTree>( strSql, dynamicParameters );
            var row = new List<Tree.zTree>( );
            //var length = chapterList.Count;
            for ( int i = 0; i < chapterList.Count; i++ )
            {
                var item = chapterList[i];
                if ( String.IsNullOrWhiteSpace( item.pId ) )
                {
                    row.Add( item );
                    Tree.SelfTree( item, chapterList );
                    //chapterList.Remove( item );
                }
            }
            return row;
        }

        /// <summary>
        /// 新增实体  CourseChapter
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseChapterInput input)
        {
            CourseChapter model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseChapterRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                model.CapterName = input.CapterName;
                model.CapterCode = input.CapterCode;
                model.OrderNo = input.OrderNo;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iCourseChapterRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseChapter,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = string.Format( "修改课程章节:{0}-{1}", model.CourseId, model.CapterName )
                } );
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseChapter>();
			model.Id = Guid.NewGuid().ToString();
            model.CapterName = input.CapterName;
            model.ParentId = input.ParentId;
            model.CapterCode = input.CapterCode;
	       model.OrderNo = input.OrderNo;
            model.CourseId = input.CourseId;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iCourseChapterRepository.InsertGetId(model);
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = ( int )Model.CourseChapter,
                OperatorType = ( int )OperatorType.Add,
                Remark = string.Format( "新增课程章节:{0}-{1}",model.CourseId,model.CapterName)
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using OracleInternal.Secure.Network;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Md5;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 用户信息领域层
    /// </summary>
    public class UserAppService : BaseAppService<User>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IUserRepository _repository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly FileRelationshipAppService _fileRelationshipAppService;

        /// <summary>
        /// 文件关系仓储
        /// </summary>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        public UserAppService(IUserRepository repository, OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        /// 导入用户信息
        /// </summary>
        /// <param name="dvHouseBan"></param>
        /// <returns></returns>
        public MessagesOutPut ReptInsert(DataView dvHouseBan)
        {
            var list = new List<User>();
            for (int i = 0; i < dvHouseBan.Count; i++)
            {
                try
                {
                    var model = new User
                    {
                        UserName = dvHouseBan[i]["用户名"].ToString().Trim(),
                        PassWord = dvHouseBan[i]["密码"].ToString().Trim()
                    };
                    list.Add(model);
                }
                catch (Exception)
                {
                    return new MessagesOutPut { Success = false, Message = "导入失败!失败数据：第" + i + "条,失败原因，数据格式不正确!" };
                }
            }
            var remark = string.Empty;
            foreach (var item in list)
            {
                remark += item.UserName + ",";
                _repository.Insert(item);
            }
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = Guid.NewGuid().ToString(),
                ModuleId = (int)Model.User,
                OperatorType = (int)OperatorType.Import,
                Remark = "导入用户:" + remark
            });
            return new MessagesOutPut { Success = true, Message = "导入成功!" };
        }

        /// <summary>
        /// 新增or修改删除用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public MessagesOutPut AddOrEdit(UserEditInPut input)
        {
            User model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                if (input.UserName != model.UserName)
                {
                    var strSql = @" select count(1) from t_Base_User where 1=1 and LoginName= @LoginName ";
                    var dy = new DynamicParameters();
                    dy.Add(":LoginName", input.LoginName, DbType.String);
                    var count = Db.QueryFirstOrDefault<int>(strSql, dy);
                    if (count > 0)
                    {
                        return new MessagesOutPut { Success = false, Message = "修改失败,该登录名已经在系统内存在!" };
                    }
                }
                model.Id = input.Id;
                model.UserName = input.UserName;
                model.IsAdmin = input.IsAdmin;
                model.Phone = input.Phone;
                model.UpdateTime = DateTime.Now;
                model.UpdateUserId = UserObject.Id;
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.User,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改用户:" + model.UserName
                });
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = input.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            var strSql1 = @" select count(1) from t_Base_User where 1=1  and LoginName= @LoginName ";
            var dy1 = new DynamicParameters();
            dy1.Add(":LoginName", input.LoginName, DbType.String);
            var count1 = Db.QueryFirstOrDefault<int>(strSql1, dy1);
            if (count1 > 0)
            {
                return new MessagesOutPut { Success = false, Message = "新增失败,该登录名已经在系统内存在!" };
            }
            model = input.MapTo<User>();
            model.Id = Guid.NewGuid().ToString();
            model.AddTime = DateTime.Now;
            model.AddUserId = UserObject.Id;
            model.PassWord = Md5.GetMd5("123456");
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.User,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增用户:" + model.UserName
            });
            _fileRelationshipAppService.Add(new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = input.IdFilehiddenFile
            });
            return new MessagesOutPut { Success = true, Message = "新增成功,默认密码为123456!" };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public MessagesOutPut ModifyPassWordInput(ModifyPassWordInput input)
        {
            if (string.IsNullOrEmpty(input.PassWord))
            {
                return new MessagesOutPut { Success = false, Message = "修改失败,新密码不能为空!" };
            }
            if (string.IsNullOrEmpty(input.OldPassWord))
            {
                return new MessagesOutPut { Success = false, Message = "修改失败,原密码不能为空!" };
            }
            if (input.PassWord==input.OldPassWord)
            {
                return new MessagesOutPut { Success = false, Message = "修改失败,新密码不能和原密码一样!" };
            }
            User user = _repository.Get(UserObject.Id);
            if (user.PassWord != Md5.GetMd5(input.OldPassWord))
            {
                return new MessagesOutPut { Success = false, Message = "修改失败,原密码错误!" };
            }
            user.PassWord = Md5.GetMd5(input.PassWord);
            _repository.Update(user);
            return new MessagesOutPut { Success = true, Message = "修改成功!" };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UserListOutputDto> GetList(UserInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_User  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrEmpty(input.UserName))
            {
                strSql.Append("and UserName like @UserName");
                dynamicParameters.Add(":UserName", '%' + input.UserName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<UserListOutputDto>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 通过用户名称获取一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public UserListOutputDto GetLogin(LoginInput input)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_User  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.LoginName))
            {
                strSql.Append("and LoginName = @LoginName");
                dynamicParameters.Add(":LoginName", input.LoginName, DbType.String);
            }
            var model = Db.QueryFirstOrDefault<UserListOutputDto>(sql + strSql, dynamicParameters);
            return model;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.AlipayService.Model;
using ZF.Infrastructure.QiniuYun;
using ZF.Infrastructure.RedisCache;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：CourseVideo 
    /// </summary>
    public class CourseVideoController : BaseController
    {
        private readonly CourseVideoAppService _courseVideoAppService;

        private readonly CourseChapterAppService _courseChapterAppService;
        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly CourseVideoFileAppService _ourseVideoFileAppService;

        private readonly CourseVideoFileAppService _courseVideoFileAppService;

        public CourseVideoController(CourseVideoAppService courseVideoAppService, OperatorLogAppService operatorLogAppService, CourseVideoFileAppService ourseVideoFileAppService, CourseVideoFileAppService courseVideoFileAppService, CourseChapterAppService courseChapterAppService)
        {
            _courseVideoAppService = courseVideoAppService;
            _operatorLogAppService = operatorLogAppService;
            _ourseVideoFileAppService = ourseVideoFileAppService;
            _courseVideoFileAppService = courseVideoFileAppService;
            _courseChapterAppService = courseChapterAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseVideo 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<CourseVideoOutput> GetLists(CourseVideoListInput input)
        {
            var count = 0;
            var list = _courseVideoAppService.GetLists(input, out count);
            return new JqGridOutPut<CourseVideoOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }

        /// <summary>
        /// 查询列表实体：CourseVideo 
        /// </summary>
        [HttpPost]
        public List<CourseVideoOutput> GetList(CourseVideoListInput input)
        {
            var list = _courseVideoAppService.GetList(input);
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
            foreach (var item in array)
            {
                var model = _courseVideoAppService.Get(item);
                if (model != null)
                {
                    var courseChapter = _courseChapterAppService.Get(model.ChapterId);
                    RedisCacheHelper.Remove("GetCourseChapterList1_" + courseChapter.CourseId);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.CourseVideo,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = string.Format("删除课程视频：{0}", model.VideoName)
                    });
                }
                _courseVideoAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：CourseVideo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseVideoInput input)
        {
            var courseChapter = _courseChapterAppService.Get(input.ChapterId);
            RedisCacheHelper.Remove("GetCourseChapterList1_" + courseChapter.CourseId);
            if (!string.IsNullOrEmpty(input.VideoUrl))
            {
                var duration = _courseVideoFileAppService.Get(input.VideoUrl).Duration;
                if (duration != null)
                {
                    input.VideoLong = (int)duration;
                }
                else
                {
                    input.VideoLong = 0;
                }
                if (input.VideoLong <= input.TasteLongTime)
                {
                    return new MessagesOutPut { Id = -1, Message = "保存失败,试听时间不能超过视频总时长!", Success = false };
                }
                input.VideoLongTime = getFormatterTime(duration.ToString());
            }
            else
            {
                input.VideoLong = 0;
                input.VideoLongTime = "00:00:00";
            }
            var data = _courseVideoAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }


        /// <summary>
        /// 批量章节下视频的可试听时长
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut UpdateVideoTasteLongTime(CourseVideoInput input)
        {
            var data = _courseVideoAppService.UpdateVideoTasteLongTime(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 判断视频编号是否唯一
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut VideoIsOnlyOne(CourseVideoInput input)
        {
            return _courseVideoAppService.VideoIsOnlyOne(input);
        }


        /// <summary>
        /// 格式化视频时长 01：12:30  1小时12分30秒
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        private string getFormatterTime(string timeStr)
        {
            var longtime = int.Parse(timeStr.Split('.')[0]);
            var hour = longtime / 60 / 60;
            var minute = longtime / 60 - hour * 60;
            if (hour > 0)
            {
                return twolengthNumber(hour) + ":" + twolengthNumber(minute) + ":" + twolengthNumber(longtime % 3600 - minute * 60);
            }
            else if (minute > 0)
            {
                return "00:" + twolengthNumber(minute) + ":" + twolengthNumber(longtime % 60);
            }
            else
            {
                return "00:00:" + twolengthNumber(longtime % 60);
            }
        }


        private string twolengthNumber(int num)
        {
            return num > 9 ? "" + num : "0" + num;
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CourseVideoOutput GetOne(IdInput input)
        {
            var model = _courseVideoAppService.GetOne(input);
            return model;
        }

        /// <summary>
        /// 查看是否维护有视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool HasVideo(CourseVideoInput input)
        {
            return _courseVideoAppService.HasVideo(input);
        }

        [System.Web.Http.HttpPost]
        public UploadVideo CreateUploadVideo()
        {
            return Aliyun.CreateUploadVideo();
        }


        [System.Web.Http.HttpPost]
        public UploadVideo RefreshUploadVideo(string VideoId)
        {
            return Aliyun.RefreshUploadVideo(VideoId);
        }



        [System.Web.Http.HttpPost]
        public SecurityTokenModel GetSecurityToken()
        {
            return Aliyun.GetSecurityToken();
        }

        /// <summary>
        /// 添加视频到数据库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public bool AddVideo(CourseVideoFileInput input)
        {
            _ourseVideoFileAppService.AddOrEdit(new CourseVideoFileInput
            {
                VideoAlias = input.VideoAlias + "-" + input.Name,
                Name = input.Name,
                Type = input.Type,
                Id = input.Id
            });
            return true;
        }

        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut VideoDelete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            var ids = array.Aggregate("", (current, items) => current + ("'" + items + "',")).TrimEnd(',');
            if (_courseVideoAppService.Exsit(ids))
            {
                return new MessagesOutPut { Id = 1, Message = "删除失败,该视频资源已经被使用!", Success = false };
            }
            foreach (var item in array)
            {
                var model = _courseVideoFileAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.CourseVideo,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = string.Format("删除视频：{0}", model.Name)
                    });
                    Aliyun.DeleteVideoRequest(model.Id);
                    _courseVideoFileAppService.Delete(model);
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        [HttpPost]
        public JqGridOutPut<CourseVideoFileOutput> VideoGetList(CourseVideoFileListInput input)
        {
            var count = 0;
            var list = _courseVideoFileAppService.GetList(input, out count);
            return new JqGridOutPut<CourseVideoFileOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CourseVideoFile GetVideo(IdInput input)
        {
            var model = _courseVideoFileAppService.Get(input.Id);
            return model;
        }
    }
}


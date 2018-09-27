using ServiceStack.DataAnnotations;

namespace ZF.Infrastructure
{
    public enum Model
    {
        /// <summary>
        /// 用户表
        /// </summary>
        [Description("用户表")]
        User = 1,

        /// <summary>
        /// 菜单表
        /// </summary>
        [Description("菜单表")]
        Menu = 2,

        /// <summary>
        /// 模块表
        /// </summary>
        [Description("模块表")]
        Module = 3,

        /// <summary>
        /// 项目分类表
        /// </summary>
        [Description("项目分类表")]
        ProjectClass = 4,

        /// <summary>
        /// 项目表
        /// </summary>
        [Description("项目表")]
        Project = 5,

        /// <summary>
        /// 数据字典表
        /// </summary>
        [Description("数据字典表")]
        BaseData = 6,

        /// <summary>
        /// 科目表
        /// </summary>
        [Description("科目表")]
        Subject = 7,

        /// <summary>
        /// 字段分类
        /// </summary>
        [Description("字典分类")]
        DataType = 8,

        /// <summary>
        /// 科目知识点
        /// </summary>
        [Description("科目知识点")]
        SubjectKnowledgePoint = 9,

        /// <summary>
        /// 用户登陆日志
        /// </summary>
        [Description("用户登陆日志")]
        UserLoginLog = 10,

        /// <summary>
        /// 试题分类表
        /// </summary>
        [Description("试题分类表")]
        SubjectClass = 11,

        /// <summary>
        /// 试题大表
        /// </summary>
        [Description("试题大表")]
        SubjectBigQuestion = 12,

        /// <summary>
        /// 主讲讲师表
        /// </summary>
        [Description("主讲讲师表")]
        CourseOnTeachers = 13,


        /// <summary>
        /// 试题分类表
        /// </summary>
        [Description("试卷结构")]
        Structure = 14,
        /// <summary>
        /// 课程表
        /// </summary>
        [Description("课程表")]
        CourseInfo = 15,
        /// <summary>
        /// 试卷结构明细
        /// </summary>
        [Description("试卷结构明细")]
        PaperStructureDetail = 16,
        /// <summary>
        /// 试卷小题表
        /// </summary>
        [Description("试卷小题表")]
        SubjectSmallquestion = 17,

        [Description("试卷参数表")]
        PaperPaperParam = 18,

        [Description("参数明细表")]
        PaperParamDetail = 19,

        /// <summary>
        /// 课程资源
        /// </summary>
        [Description("课程资源")]
        CourseResource = 20,
        /// <summary>
        /// 课程章节
        /// </summary>
        [Description("课程章节")]
        CourseChapter = 21,
        /// <summary>
        /// 课程章节
        /// </summary>
        [Description("课程评价")]
        CourseAppraise = 22,
        /// <summary>
        /// 课程章节
        /// </summary>
        [Description("课程视频")]
        CourseVideo = 23,
        /// <summary>
        /// 课程章节
        /// </summary>
        [Description("课程试题")]
        CourseSubject = 24,


        /// <summary>
        /// 资讯,帮助管理表
        /// </summary>
        [Description("资讯,帮助管理表")]
        AfficheHelp = 25,

        /// <summary>
        /// 网站相关配置表
        /// </summary>
        [Description("网站相关配置表")]
        SysSet = 26,

        /// <summary>
        /// 文本配置表
        /// </summary>
        [Description("文本配置表")]
        Danye = 27,

        /// <summary>
        /// 使用指南
        /// </summary>
        [Description("使用指南")]
        UseDescription = 28,

        [Description("试卷表")]
        PaperInfo = 29,

        [Description("订单明细表")]
        OrderDetail = 30,

        [Description("订单表")]
        OrderSheet = 31,

        [Description("试卷明细表")]
        PaperDetatail = 32,
        [Description("套餐课程表")]
        CoursePackcourseInfo = 33,
        CourseSuitDetail = 34,
        [Description("短信发送记录表")]
        SmsendLog = 33,
        [Description("课程收藏表")]
        MyCollection = 34,
        [Description("购物车表")]
        OrderCart = 35,
        [Description("订单明细表")]
        OrderCartDetail = 36,
        [Description("课程试题表")]
        CoursePaper = 37,
        [Description("我的课程")]
        MyCourse = 38,

        [Description("幻灯片设置")]
        SlideSetting = 39,
        [Description("标签管理")]
        BaseTag = 40,
        [Description("实体标签管理")]
        BaseTagMiddle = 41,


        [Description("面试班")]
        CourseFaceToFace = 42,

        [Description("讲义我的地址维护")]
        DeliveryAddress = 43,

        [Description("推广员表")]
        PromotePromoters = 44,
        [Description("推广公司")]
        PromoteCompany = 45,
        [Description("快递公司")]
        ExpressCompany = 46,

        Discount_Card = 47,
        [Description("学习卡")]
        My_StudyCard = 48,
        PurchaseDiscount = 49,

        [Description("防伪码管理")]
        CourseSecurityCode =50,

        [Description("科目关联书籍管理")]
        SubjectBook = 51,
        [Description("备考指南")]
        ExamInfo = 52,

        [Description("试卷组")]
        PaperGroup = 53,

        [Description("试卷组试卷关系表")]
        PaperGroupRelation = 54,

        [Description("下单机构表")]
        OrderInstitutions = 55,
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// 添加
        /// </summary>
        [Description("添加")]
        Add = 1,

        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Edit = 2,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 3,

        /// <summary>
        /// 导出
        /// </summary>
        [Description("导出")]
        Export = 4,

        /// <summary>
        /// 导入
        /// </summary>
        [Description("导入")]
        Import = 5,
    }

    /// <summary>
    /// 题型类型
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// 单选
        /// </summary>
        [Description("单选")]
        One = 1,

        /// <summary>
        /// 多选
        /// </summary>
        [Description("多选")]
        Two = 2,

        /// <summary>
        /// 判断
        /// </summary>
        [Description("判断")]
        Three = 3,

        /// <summary>
        /// 案例分析
        /// </summary>
        [Description("案例分析")]
        Seven = 7
    }

    /// <summary>
    /// 难度等级
    /// </summary>
    public enum DifficultLevel
    {
        /// <summary>
        /// 简单
        /// </summary>
        [Description("易")]
        Simple = 1,

        /// <summary>
        /// 容易
        /// </summary>
        [Description("中")]
        Easily = 2,

        /// <summary>
        /// 困难
        /// </summary>
        [Description("难")]
        Difficulty = 3,
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum QuestionState
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 0,

        /// <summary>
        /// 失效
        /// </summary>
        [Description("失效")]
        Failure = 1
    }

    /// <summary>
    /// 订单下单方式
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// web
        /// </summary>
        [Description("web")]
        Web = 0,

        /// <summary>
        /// App
        /// </summary>
        [Description("App")]
        App = 1,

        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeiXin = 2,

        /// <summary>
        /// 后台
        /// </summary>
        [Description("后台")]
        Artificial = 3
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        PaymentHasBeen = 0,

        /// <summary>
        /// 代付款
        /// </summary>
        [Description("待付款")]
        GenerationPayment = 1,

        /// <summary>
        /// 付款失败
        /// </summary>
        [Description("付款失败")]
        PaymentFailure = 2,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled = 3,

        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款")]
        HaveARefund = 4,

        /// <summary>
        /// 已撤销
        /// </summary>
        [Description("已撤销")]
        Invalid = 5,
    }

    /// <summary>
    /// 订单显示消息
    /// </summary>
    public enum OrderMessage
    {
        /// <summary>
        /// 交易成功
        /// </summary>
        [Description("交易成功")]
        TradingSuccess = 0,
        /// <summary>
        /// 交易失败
        /// </summary>
        [Description("交易失败")]
        TradingFailure = 1,
    }

    public enum Msg
    {
        [Description("发送成功")]
        Success = 200,
        [Description("发送失败")]
        Failed = 500
    }
}

$(function () {
    var urlList = [
        "api/ExamInfo/GetList", //查询
        "ExamInfo/AddOrEdit",//新增或修改
        "api/ExamInfo/Delete"//删除
    ];
    var grid = $("#tblData");
    grid.jgridInit({
        url: urlList[0],
        sortname: "AddTime",
        sortorder: "desc",
        colNames: ['编码', '描述', '内容', '考试报名', '成绩管理', '教材', '考试开始时间', '考试结束时间', '状态', '维护时间'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 100, align: "center", hidden: true },
            { name: 'Description', index: 'Description', width: 100, align: "center" },
            { name: 'Content', index: 'Content', width: 100, align: "center" },
            { name: 'SignUp', index: 'SignUp', width: 100, align: "center" },
            { name: 'ScoreManage', index: 'SignUp', width: 100, align: "center" },
            { name: 'TextBox', index: 'TextBox', width: 100, align: "center" },
            { name: 'BeginTime', index: 'TextBox', width: 100, align: "center", formatter: topevery.dataTimeView },
            { name: 'EndTime', index: 'TextBox', width: 100, align: "center", formatter: topevery.dataTimeView },
            {
                name: 'IfUse', index: 'IfUse', width: 100, align: "center", formatter: function (value) {
                    return value == 1 ? "启用" : "停用";
                }
            },
            { name: 'AddTime', index: 'TextBox', width: 100, align: "center", hidden: true },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(urlList[1], 600, 500);
    $(".edit_btn").bindEditBtn(urlList[1], grid, 600, 500);
    $(".del_btn").bindDelBtn(urlList[2], grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: urlList[0], page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    $('.use_btn').on('click', function () {
        layer.confirm("确认启用?", {
            title: "确认"
        }, function (index) {
            var rowIndexs = $(grid).jqGrid('getGridParam', 'selarrrow');
            if (rowIndexs.length == 1) {
                var rowData = $(grid).jqGrid('getRowData', rowIndexs);
                $.topevery.ajax({
                    url: 'api/ExamInfo/UpdateState',
                    data: JSON.stringify({
                        "Id": rowData.Id
                    })
                }, function (data) {
                    var message = "操作成功";
                    var icon = 1;
                    if (data.Result) {
                        icon = 2;
                        $(grid).trigger("reloadGrid");
                    }
                    layer.msg(message, {
                        icon: icon,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 2
                    });
                });
            } else {
                layer.alert("请选择一条且只选择一条记录!");
            }
        });
    })
});




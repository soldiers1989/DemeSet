$(function () {
    var getPostDataUrl = "api/CoursePaper/GetListAdd";
    var addCoursePaper = "api/CoursePaper/AddOrEdit";
    var grid = $("#paperDataAdd");
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect: true,
        colNames: ['编码', '试卷名称','试卷类别'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, hidden: true, align: "center" },
            { name: 'PaperInfoName', width: 50, align: "center" },
            {
                name: 'Type', width: 50, align: "center", formatter: function (value) {
                    return value == 0 ? "历年真题" : "模拟试卷";
                }
            },
        ],
        postData: topevery.form2Json("paperSelectFrom"),
        pager: '#paperPagerAdd',
        height: '300'
    });
    $("#addPaperAdd").bind("click", function () {
        var data = $("#paperDataAdd").jqGrid('getGridParam', 'selarrrow');
        var ids = [];
        if (data.length > 0) {
            //遍历访问这个集合  
            $(data).each(function (index, id) {
                //由id获得对应数据行  
                var rowData = $("#paperDataAdd").jqGrid('getRowData', id);
                ids.push(rowData.Id);
            });
            topevery.ajax({
                url: addCoursePaper,
                data: JSON.stringify({
                    "PaperInfoId": ids.join(),
                    CourseId: $("#AddCourseId").val()
                })
            }, function (data) {
                var icon = 1;
                var message = "保存失败";
                if (data.Success) {
                    icon = data.Result.Success === true ? 1 : 2;
                    if (data.Result.Success) {
                        $("#paperDataAdd").trigger("reloadGrid");
                        $("#paperData").trigger("reloadGrid");
                    }
                    message = data.Result.Message;
                } else {
                    message = data.Error;
                }
                layer.msg(message, {
                    icon: icon,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            });
        } else {
            layer.alert("请选择一条记录!");
        }
    });
});



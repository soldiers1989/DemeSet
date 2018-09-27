$(function () {
    var getPostDataUrl = "api/CourseAppraise/GetList";
    var deleteUrl = "api/CourseAppraise/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['编码', '课程名称', '评价内容', '评价级别', '回复内容', 'IP地址', '评价时间', '操作',''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, hidden: true, align: "center" },
            { name: 'CourseName', width: 60, align: "left" },
            { name: 'AppraiseCotent', width: 100, align: "left" },
            { name: 'AppraiseLevel', index: 'AppraiseLevel', width: 20, align: "center", formatter: formatterLevel },
            { name: 'ReplyContent', width: 50, align: "center" },
            { name: 'AppraiseIp', width: 40, align: "center" },
            { name: 'AppraiseTime', width: 45, align: "center", formatter: topevery.dataTimeFormatTT },
            {
                name: '', width: 40, align: 'center', formatter: function (cellValue, options, rowdata, action) {
                    var html = rowdata.AuditStatus == 1 ? "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-down\" data-state=\"0\" title=\"取消审核\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>取消审核</a> " : "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-up\" title=\"审核\" data-state=\"1\" data-id='" + rowdata.Id + "' onclick='operateState(this)'> 审核 </a> ";
                    var htm1 = $.trim(rowdata.ReplyContent) == "" ? "<input onclick=\"javascript:reply(this)\" data-id=\"" + rowdata.Id + "\" class=\"btn btn-info edit_btn\" type=\"button\" value=\"回复\">" : "<input onclick=\"javascript:reply(this)\" disabled=\"disabled\" data-id=\"" + rowdata.Id + "\" class=\"btn btn-info edit_btn\" type=\"button\" value=\"回复\">";
                    return html + htm1;
                             ;
                }
            },
              { name: '', width: 50 },
        ],
        postData: topevery.form2Json("selectFrom")
    });


    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    $('#QueryCourseType').change(function () {
        var courseType = $(this).children('option:selected').val();
        topevery.BindSelect("QueryCourseId", "Common/AllCourseList?courseType=" + courseType, "--请选择--");
    });

    topevery.BindSelect("QueryCourseId", "Common/AllCourseList?courseType=-1", "--请选择--");

    function formatterLength(cellValue) {
        console.log(cellValue);
        return cellValue.substr(0, 20) + '...';
    }

    function formatterLevel(appraiseLevel) {
        switch (appraiseLevel) {
            case 0:
                return '很差';
            case 1:
                return '较差';
            case 2:
                return '一般';
            case 3:
                return '良好';
            case 4:
                return '推荐';
            case 5:
                return '极佳';
            default:
                '';
        }
    }
    $('.upper_btn').on('click', function () {
        layer.confirm("确认批量审核?", {
            title: "确认"
        }, function (index) {
            var rowIndexs = $(grid).jqGrid('getGridParam', 'selarrrow');
            if (rowIndexs.length > 0) {
                var ids = [];
                for (var i = 0; i < rowIndexs.length; i++) {
                    var rowData = $(grid).jqGrid('getRowData', rowIndexs[i]);
                    ids.push(rowData.Id);
                }
                $.topevery.ajax({
                    url: 'api/CourseAppraise/BatchUpper',
                    data: JSON.stringify({
                        "Ids": ids.join(',')
                    })
                }, function (data) {
                    var message = "操作失败";
                    var icon = 1;
                    if (data.Success) {
                        icon = data.Result.Success === true ? 1 : 2;
                        if (data.Result.Success) {
                            $(grid).trigger("reloadGrid");
                        }
                        message = data.Result.Message;
                    }
                    layer.msg(data.Result.Message, {
                        icon: icon,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 2
                    });
                });
            } else {
                layer.alert("请选择数据!");
            }
        });
    })


    $('.lower_btn').on('click', function () {
        layer.confirm("确认批量取消审核?", {
            title: "确认"
        }, function (index) {
            var rowIndexs = $(grid).jqGrid('getGridParam', 'selarrrow');
            if (rowIndexs.length > 0) {
                var ids = [];
                for (var i = 0; i < rowIndexs.length; i++) {
                    var rowData = $(grid).jqGrid('getRowData', rowIndexs[i]);
                    ids.push(rowData.Id);
                }
                $.topevery.ajax({
                    url: 'api/CourseAppraise/BatchLower',
                    data: JSON.stringify({
                        "Ids": ids.join(',')
                    })
                }, function (data) {
                    var message = "操作失败";
                    var icon = 1;
                    if (data.Success) {
                        icon = data.Result.Success === true ? 1 : 2;
                        if (data.Result.Success) {
                            $(grid).trigger("reloadGrid");
                        }
                        message = data.Result.Message;
                    }
                    layer.msg(data.Result.Message, {
                        icon: icon,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 2
                    });
                });
            } else {
                layer.alert("请选择数据!");
            }

        });
    })
});

function reply(e) {
    var id = $(e).data('id');
    topevery.ajax({
        type: 'get',
        url: 'CourseAppraise/Reply?Id=' + id,
        dataType: 'html'
    }, function (data) {
        layer.open({
            type: 1,
            title: "回复",
            skin: 'layui-layer-rim', //加上边框
            area: [800 + 'px', 400 + 'px'], //宽高
            content: data
        });
    }, true);
}



function operateState(e) {
    var id = $(e).data('id');
    var state = $(e).data('state');
    topevery.ajax({
        url: 'api/CourseAppraise/UpdateState',
        data: JSON.stringify({
            Id: id, AuditStatus: state
        })
    }, function (data) {
        layer.msg('操作成功', {
            time: 2000
        });
        $(".query_btn").click();
    });
}


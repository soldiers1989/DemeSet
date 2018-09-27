$(function () {
    //初始化教师select2
    topevery.ajax({
        url: "Common/SelectTeacherList",
        data: JSON.stringify({}),
        async: false
    }, function (data) {
        $("#QueryTeacherId").select2({
            data: data,
            placeholder: { id: '-1', text: '请选择' },
            dropdownParent: $(".append"),
            allowClear: true
        })
        $('#QueryTeacherId').val('-1').trigger('change');
    });

    var getPostDataUrl = "api/CourseFaceToFace/GetList";
    var addUrl = "FaceToFace/AddOrEdit";
    var deleteUrl = "api/CourseFaceToFace/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['编码', '名称', '上课地点', '线上报名', '价格(元)',  '操作',''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "left", hidden: true },
            { name: 'ClassName', width: 100, align: "left" },
            { name: 'Address', width: 100, align: "center" },
            { name: 'Count', width: 30, align: "center" },
            { name: 'FavourablePrice', width: 30, align: "center" },
            {
                name: '', width: 60, align: "center", formatter: function (cellValue, options, rowdata, action) {
                    return rowdata.State == 1 ? "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-down\" data-state=\"0\" title=\"下架\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>下架</a> " : "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-up\" title=\"上架\" data-state=\"1\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>上架</a> ";
                }
            },
           { name: '', width: 100, align: "center" },

        ],
        postData: topevery.form2Json("selectFrom")
    });

    //绑定编辑按钮
    $(".edit_btn").click(function () {
        var rowIndex = $(grid).jqGrid('getGridParam', 'selarrrow');
        if (rowIndex.length === 1) {
            var rowData = $(grid).jqGrid('getRowData', rowIndex);
            topevery.ajax({
                type: "get",
                url: addUrl + "?Id=" + rowData.Id,
                dataType: "html"
            }, function (data) {
                $('.content-wrapper').html(data);
            }, true);
        } else {
            layer.alert("请选择一条且只选择一条记录!");
        }
    });

    //绑定删除按钮
    $(".del_btn").click(function () {
        layer.confirm("确认要删除吗，删除后不能恢复", {
            title: "删除确认"
        }, function (index) {
            var ids = [];
            var names = []
            var data = $(grid).jqGrid('getGridParam', 'selarrrow');
            if (data.length > 0) {
                $(data).each(function (index, id) {
                    //由id获得对应数据行  
                    var rowData = $(grid).jqGrid('getRowData', id);
                    //是否存在在訂單中
                    topevery.ajax({
                        url: 'api/CourseInfo/ExistInOrderOrCart', data: JSON.stringify({ "Id": rowData.Id }), async: false
                    }, function (data) {
                        if (data.Result) {
                            names.push(rowData.CourseName);
                        } else {
                            ids.push(rowData.Id);
                        }
                    });
                });
                if (ids.length > 0) {
                    topevery.ajax({
                        url: deleteUrl,
                        data: JSON.stringify({
                            "Ids": ids.join()
                        })
                    }, function (data) {
                        var message = "";
                        if (data.Success) {
                            if (data.Result.Success) {
                                $(grid).trigger("reloadGrid");
                            }
                            message = data.Result.Message;
                            if (names.length > 0) {
                                message = names.join().trimRight(',') + " 已经在购物车或订单中存在，不能删除！";
                            }
                        } else {
                            message = data.Error;
                        }
                        layer.msg(message);
                    })
                } else {
                    if (names.length > 0) {
                        layer.msg(names.join().trimRight(',') + " 已经在购物车或订单中存在，不能删除！");
                    }
                }
            } else {
                layer.alert("请选择一条或多条记录!");
            }
        });
    });

    //绑定上架按钮
    $('.upper_btn').on('click', function () {
        layer.confirm("确认批量上架?", {
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
                    url: 'api/CourseFaceToFace/BatchUpper',
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
                        anim: 5
                    });
                });
            } else {
                layer.alert("请选择数据!");
            }
        });
    })

    //绑定下架按钮
    $('.lower_btn').on('click', function () {
        layer.confirm("确认批量下架?", {
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
                    url: 'api/CourseFaceToFace/BatchLower',
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
                        anim: 5
                    });
                });
            } else {
                layer.alert("请选择数据!");
            }

        });
    })

    //绑定查询按钮
    $(".query_btn").on("click", function () {
        var postData = { ClassName: $('#QueryClassName').val(), TeacherId: $('#QueryTeacherId').val(), State: $('#QueryState').val() };
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: postData
        }).trigger("reloadGrid");
    });

    //绑定添加按钮
    $(".add_btn").on("click", function () {
        topevery.ajax({
            type: 'get', url: addUrl, dataType: 'html'
        }, function (data) {
            $('.content-wrapper').html(data);
        }, true);
    });
});


//上架or下架操作
function operateState(e) {
    var id = $(e).data('id');
    var state = $(e).data('state');
    topevery.ajax({
        url: 'api/CourseFaceToFace/UpdateState',
        data: JSON.stringify({
            Id: id, State: state
        })
    }, function (data) {
        layer.msg('操作成功', {
            time: 2000
        });
        $("#tblData").jqGrid('setGridParam', {
            url: "api/CourseFaceToFace/GetList", page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
}


$("#myTab a").click(function () {
    var url = $(this).attr("url");
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) { $(".content-wrapper").html(data); }, true);
});
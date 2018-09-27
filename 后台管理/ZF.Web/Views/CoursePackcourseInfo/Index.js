$(function () {
    var CourseInfo = {
        formatterState: function (state) {
            return state == 1 ? "已上架" : "未上架"
        },
        formatterPrice: function (price) {
            return '' + price + '￥';
        },
    }
    var dataType = false;
    var getPostDataUrl = "api/CoursePackcourseInfo/GetList";
    var addUrl = "CoursePackcourseInfo/AddOrEdit";
    var deleteUrl = "api/CoursePackcourseInfo/Delete";
    var DetailUrl = "CoursePackcourseInfo/CourseDetail"
    var grid = $("#packCourseData");
    grid.jgridInit({
        url: getPostDataUrl,
        //multiselect: false,
        colNames: ['编码', '课程名称', '价格(元)', '课程数', '上下架状态', '报班人数','排序', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'CourseName', width: 80, align: "left" },
            { name: 'FavourablePrice', width: 25, align: "center" },
            { name: 'CourseCount', width: 20, align: "center" },
            { name: 'State', width: 30, align: "center", hidden: true },
            { name: 'LearnCount', width: 20, align: "center" },
            { name: 'OrderNo', width: 20, align: "center" },
            { name: '', width: 60, align: "center", formatter: contentLink },
            { name: '', width: 150, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    //$(".edit_btn").bindEditBtnEx(addUrl, grid, 950, 680);

    $(".edit_btn").click(function () {
        var rowIndex = $(grid).jqGrid('getGridParam', 'selarrrow');
        if (rowIndex.length === 1) {
            var rowData = $(grid).jqGrid('getRowData', rowIndex);
            topevery.ajax({
                type: "get", url: addUrl + "?Id=" + rowData.Id, dataType: "html"
            }, function (data) {
                $('.content-wrapper').html(data);
            }, true);
        } else {
            layer.alert("请选择一条且只选择一条记录!");
        }
    })



    //$(".del_btn").bindDelBtnDx(deleteUrl, grid);
    $('.del_btn').click(function () {
        layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
            var rowIndex = $(grid).jqGrid('getGridParam', 'selarrrow');
            var ids = [];//要删除的课程ID
            var names = [];//已经存在购物车或订单的课程名称
            if (rowIndex.length > 0) {
                $(rowIndex).each(function (index, id) {
                    var rowData = $(grid).jqGrid('getRowData', id);
                    //判断是否在购物车或订单中
                    topevery.ajax({
                        url: 'api/CoursePackcourseInfo/ExistInOrderOrCart',
                        data: JSON.stringify({ "Id": rowData.Id }),
                        async: false
                    }, function (data) {
                        if (data.Result) {
                            names.push(rowData.CourseName);
                        } else {
                            ids.push(rowData.Id);
                        }
                    })
                })
                if (ids.length > 0) {
                    topevery.ajax({
                        url: deleteUrl,
                        data: JSON.stringify({ "Ids": ids.join() })
                    }, function (data) {
                        if (data.Success) {
                            icon = data.Result.Success === true ? 1 : 2;
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
                    });
                } else {
                    layer.msg(names.join().trimRight(',') + " 已经在购物车或订单中存在，不能删除！");
                }
            } else {
                layer.alert("请选择一条或多条记录!");
            }
        })
    });




    $('.upper_btn').on('click', function () {
        layer.confirm("确认批量上架?", { title: "确认" }, function (index) {
            var rowIndexs = $(grid).jqGrid('getGridParam', 'selarrrow');
            if (rowIndexs.length > 0) {
                var ids = [];
                for (var i = 0; i < rowIndexs.length; i++) {
                    var rowData = $(grid).jqGrid('getRowData', rowIndexs[i]);
                    ids.push(rowData.Id);
                }
                $.topevery.ajax({
                    url: 'api/CoursePackcourseInfo/BatchUpper',
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
        layer.confirm("确认批量下架?", { title: "确认" }, function (index) {
            var rowIndexs = $(grid).jqGrid('getGridParam', 'selarrrow');
            if (rowIndexs.length > 0) {
                var ids = [];
                for (var i = 0; i < rowIndexs.length; i++) {
                    var rowData = $(grid).jqGrid('getRowData', rowIndexs[i]);
                    ids.push(rowData.Id);
                }
                $.topevery.ajax({
                    url: 'api/CoursePackcourseInfo/BatchLower',
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





    //function beforeOperate(obj) {
    //    console.log('info',obj);
    //    if (obj.State == "1") {//已上架
    //        layer.msg('该课程已上架，请下架后再进行操作', { time: 2000 });
    //        return false;
    //    } 
    //    return true;
    //}

    $(".query_btn").on("click", function () {
        var postData = { CourseName: $('#QueryCourseName').val(), TeacherId: $('#QueryTeacherId').val(), State: $('#state').val() };
        $("#packCourseData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: postData
        }).trigger("reloadGrid");
    });

    $(".add_btn").on("click", function () {
        //topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
        //    layer.open({
        //        type: 1,
        //        title: "新增",
        //        skin: 'layui-layer-rim', //加上边框
        //        area: [1000 + 'px', 680 + 'px'], //宽高
        //        content: data
        //    });
        //}, true);
        topevery.ajax({
            type: 'get', url: addUrl, dataType: 'html'
        }, function (data) {
            $('.content-wrapper').html(data);
        });
    });
    //topevery.BindSelect("QueryTeacherId", "Common/TeacherList", "---全部---");
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
});

var courseDetailUrl = "CourseInfo/CourseInfoDetail"

function contentLink(cellValue, options, rowdata, action) {
    return "<span class='glyphicon glyphicon-pencil' style='color:#00c0ef;cursor:pointer;' title=\"明细\" data-id='" + rowdata.Id + "'  onclick='checkCourseDetail(this)'>明细&nbsp;</span> "
           + "<span class='glyphicon glyphicon-stats' style='color:#00c0ef;cursor:pointer;' title=\"维护子课程\" data-id='" + rowdata.Id + "'  onclick='addSubCourse(this)'>维护子课程&nbsp;</span>"
           + (rowdata.State == 1 ? "<a style=\"cursor:pointer;color:red;\" class=\"glyphicon glyphicon-arrow-down\" data-state=\"0\" title=\"下架\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>下架</a> " : "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-up\" title=\"上架\" data-state=\"1\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>上架</a> ");

}

function checkCourseDetail(event) {
    var id = $(event).data("id");
    var url = "CoursePackcourseInfo/CourseDetail?packCourseId=" + id;
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: '查看套餐明细',
            skin: 'layui-layer-rim', //加上边框
            area: [900 + 'px', 700 + 'px'], //宽高
            content: data
        });
    }, true);
}

function addSubCourse(event) {
    var id = $(event).data('id');//套餐课程id
    var url = "CoursePackcourseInfo/AddSubCourse?packCourseId=" + id;
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: '添加子课程',
            skin: 'layui-layer-rim', //加上边框
            area: [1200 + 'px', 800 + 'px'], //宽高
            content: data
        });
    }, true);

}

function operateState(e) {
    var id = $(e).data('id');
    var state = $(e).data('state');
    //判断套餐课程中是否含有子课程
    var flag = false;
    if (state == 0) {//已经上架，下架不需要判断是否包含是否有子课程
        topevery.ajax({
            url: 'api/CoursePackcourseInfo/UpdateState',
            data: JSON.stringify({ Id: id, State: state })
        }, function (data) {
            layer.msg('操作成功', { time: 2000 });
            $("#packCourseData").jqGrid('setGridParam', {
                url: "api/CoursePackcourseInfo/GetList", page: 1, postData: topevery.form2Json("selectFrom")
            }).trigger("reloadGrid");
        });
    } else {
        topevery.ajax({
            url: 'api/CourseSuitDetail/IsContainsSubCourse',
            data: JSON.stringify({ PackCourseId: id })
        }, function (data) {
            flag = data.Result;
            console.log(data.Result);
            if (!data.Result) {
                layer.msg("该套餐课程尚未维护子课程,请勿上架！", { time: 2000 });
            } else {
                topevery.ajax({
                    url: 'api/CoursePackcourseInfo/UpdateState',
                    data: JSON.stringify({ Id: id, State: state })
                }, function (data) {
                    layer.msg('操作成功', { time: 2000 });
                    $("#packCourseData").jqGrid('setGridParam', {
                        url: "api/CoursePackcourseInfo/GetList", page: 1, postData: topevery.form2Json("selectFrom")
                    }).trigger("reloadGrid");
                });
            }
        })
    }
}

$("#myTab a").click(function () {
    var url = $(this).attr("url");
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) { $(".content-wrapper").html(data); }, true);
});
$(function () {

    var CourseInfo = {
        formatterState: function (state) {
            return state == 1 ? "已上架" : "未上架"
        },
        formatterPrice: function (price) {
            return '' + price + '￥';
        },
    }
    var header = $(".content-wrapper").outerHeight(true)
                    - $(".nav-header").outerHeight(true) - 3
                    - $(".content-header").outerHeight(true)
                    - $(this).parent().parent().prev().find(".ui-jqgrid-hbox").outerHeight(true)
                    - 31;
    $(".KnowledgePointTree").height(header);

    topevery.ajax({
        url: "Common/SelectTeacherList",
        data: JSON.stringify({
        })
    }, function (data) {
        $("#QueryTeacherId").select2({
            data: data,
            placeholder: {
                id: '-1',
                text: '请选择'
            },
            dropdownParent: $(".col-xs-10"),
            allowClear: true
        })
        $('#QueryTeacherId').val('-1').trigger('change');
    });


    var dataType = false;
    var getPostDataUrl = "api/CourseInfo/GetList";
    var addUrl = "CourseInfo/AddOrEdit";
    var deleteUrl = "api/CourseInfo/Delete";
    var DetailUrl = "CourseInfo/CourseDetail"
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        //multiselect: false,
        colNames: ['编码', '课程名称', '课程所属科目', '科目ID', '项目ID', '价格(元)', '浏览量', '状态', '排序号', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "left", hidden: true },
            { name: 'CourseName', width: 120, align: "left" },
            { name: 'SubjectName', width: 60, align: "center" },
            { name: 'SubjectId', width: 20, align: 'center', hidden: true },
            { name: 'ProjectId', width: 20, align: 'center', hidden: true },
            { name: 'FavourablePrice', width: 40, align: "center" },
            { name: 'ViewCount', width: 20, align: "center" },
            { name: 'State', width: 40, align: "center", hidden: true },
             { name: 'OrderNo', width: 40, align: "center"},
            {
                name: '', width: 40, align: "center", formatter: function (cellValue, options, rowdata, action) {
                    return rowdata.State == 1 ? "<a style=\"cursor:pointer;color:red;\" class=\"glyphicon glyphicon-arrow-down\" data-state=\"0\" title=\"下架\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>下架</a> " : "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-up\" title=\"上架\" data-state=\"1\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>上架</a> ";
                }
            },
             { name: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom"),

    });





    //$(".edit_btn").bindEditBtnEx(addUrl, grid, 950, 680);
    $(".edit_btn").click(function () {
        var rowIndex = $(grid).jqGrid('getGridParam', 'selarrrow');
        if (rowIndex.length === 1) {
            var rowData = $(grid).jqGrid('getRowData', rowIndex);
            topevery.ajax({
                type: "get", url: addUrl + "?id=" + rowData.Id , dataType: "html"
            }, function (data) {
                $('.content-wrapper').html(data);
            }, true);
        } else {
            layer.alert("请选择一条且只选择一条记录!");
        }
    })

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
                        console.log(data);
                        if (data.Result) {
                            names.push(rowData.CourseName);
                        } else {
                            ids.push(rowData.Id);
                        }
                    });
                });
                console.log(names, ids);

                if (ids.length > 0) {
                    topevery.ajax({
                        url: deleteUrl,
                        data: JSON.stringify({
                            "Ids": ids.join()
                        })
                    }, function (data) {
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
                    url: 'api/CourseInfo/BatchUpper',
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
                    url: 'api/CourseInfo/BatchLower',
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



    $(document).keydown(function (event) {
        switch (event.keyCode) {
            case 13:
                $('.query_btn').click();
            return false;
        }
    });

    $(".query_btn").on("click", function () {
        var postData = {
            CourseName: $('#QueryCourseName').val(), State: $('#QueryState').val(),Type:$('#QueryType').val(), TeacherId: $('#QueryTeacherId').val(), ProjectClassId: $('#QueryProjectClassId').val(), ProjectId: $('#QueryProjectId').val(), SubjectId: $('#QuerySubjectId').val()
        };
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: postData
        }).trigger("reloadGrid");
    });

    $(".add_btn").on("click", function () {
        if (dataType) {
            //topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
            //    layer.open({
            //        type: 1,
            //        title: "新增",
            //        skin: 'layui-layer-rim', //加上边框
            //        area: [950 + 'px', 680 + 'px'], //宽高
            //        content: data
            //    });
            //}, true);

            topevery.ajax({
                type: 'get', url: addUrl+"?id=&"+ "&subjectId=" + $("#QuerySubjectId").val(), dataType: 'html'
            }, function (data) {
                $('.content-wrapper').html(data);
            });

        } else {
            layer.alert("请先选择树形菜单上的科目节点再进行课程新增!");
        }
    });


    //topevery.BindSelect("QueryTeacherId", "Common/TeacherList", "--请选择--");

    topevery.BindTree("treeDemo1", "Common/CourseInfoPointTree", beforeClick, "");

    topevery.BindSelect("QuerySubjectId", "Common/SubjectList", "--请选择--");

    function beforeClick(data) {
        if (data.level == 0) {
            $('#QueryProjectClassId').val(data.id);
            $('#QueryProjectId').val('');
            $("#QuerySubjectId").val('');
            dataType = false;
        }
        if (data.level == 1) {
            $('#QueryProjectClassId').val('');
            $('#QueryProjectId').val(data.id);
            $("#QuerySubjectId").val('');
            dataType = false;
        }
        if (data.level === 2) {
            $('#QueryProjectClassId').val('');
            $("#QueryProjectId").val("");
            $("#QuerySubjectId").val(data.id);
            dataType = true;
            //addUrl = "CourseInfo/AddOrEdit?SubjectId=" + $("#QuerySubjectId").val();

        }
        $(".query_btn").click();

        //console.log("科目", $('#QuerySubjectId').val());
        //console.log('height',window.screen.availWidth - 20);
        //$('.col-xs-2').attr('style', 'height:' + (window.screen.availWidth - 1120) + 'px;overflow-y:scroll;');
    }

    //function beforeEdit(obj) {
    //    if (obj.State == "1") {//已上架
    //        layer.msg('该课程已上架，请下架后再进行操作', { time: 2000 });
    //        return false;
    //    }
    //    return true;
    //}

    //function beforeDelete(obj) {
    //    if (obj.State == "1") {//已上架
    //        layer.msg('该课程已上架，请下架后再进行操作', { time: 2000 });
    //        return false;
    //    }
    //    topevery.ajax({
    //        url: 'api/CourseSuitDetail/ExistInCoursepack',
    //        data: JSON.stringify({ CouresId: obj.Id }),
    //        async:false
    //    }, function (data) {
    //        if (data.Result) {
    //            layer.msg("该课程已维护到套餐课程中，不能删除！", { time: 2000 });
    //            return false;
    //        }
    //    })

    //    return true;
    //}
});

var courseDetailUrl = "CourseInfo/CourseInfoDetail"

//function contentLink(cellValue, options, rowdata, action) {
//    return rowdata.State == 1 ? "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-down\" data-state=\"0\" title=\"下架\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>下架</a> " : "<a style=\"cursor:pointer;\" class=\"glyphicon glyphicon-arrow-up\" title=\"上架\" data-state=\"1\" data-id='" + rowdata.Id + "' onclick='operateState(this)'>上架</a> "
//}



function operateState(e) {
    var id = $(e).data('id');
    var state = $(e).data('state');
    topevery.ajax({
        url: 'api/CourseInfo/UpdateState',
        data: JSON.stringify({
            Id: id, State: state
        })
    }, function (data) {
        layer.msg('操作成功', {
            time: 2000
        });
        $("#tblData").jqGrid('setGridParam', {
            url: "api/CourseInfo/GetList", page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
}







//$(function () {
//    var getPostDataUrl = "api/BaseData/GetList";
//    var addUrl = "BaseData/AddOrEdit";
//    var deleteUrl = "api/BaseData/Delete";
//    var grid = $("#tblData");
//    grid.jgridInit({
//        url: getPostDataUrl,
//        sortname: "Sort",
//        sortorder: "asc",
//        colNames: ['编码', '数据类型代码', '数据名称', '分类名称', '排序号', ''], //列头
//        colModel: [
//            { name: 'Id', index: 'Id', width: 80, align: "center",hidden:true },
//            { name: 'Code', index: 'Code', width: 80, align: "center" },
//            { name: 'Name', index: 'Name', width: 80, align: "center" },
//            { name: 'DataTypeName', index: 'DataTypeName', width: 80, align: "center" },
//            { name: 'Sort', index: 'Sort', width: 50, align: "center" },
//           { name: '',  width: 180, align: "center" },
//        ],
//        postData: topevery.form2Json("selectFrom")
//    });
//    $(".add_btn").bindAddBtn(addUrl, 500, 600);
//    $(".edit_btn").bindEditBtn(addUrl, grid, 500, 600);
//    $(".del_btn").bindDelBtn(deleteUrl, grid);
//    $(".query_btn").on("click", function () {

//        $("#tblData").jqGrid('setGridParam', {
//            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
//        }).trigger("reloadGrid");
//    });

//    topevery.BindSelect("QueryDataTypeId", "Common/DataTypeList", "--请选择--");
//});


$(function () {
    var app = new Vue({
        el: "#app",
        data: {
            width: 500,
            height: 600,
            getPostDataUrl: "api/BaseData/GetList",
            addUrl: "BaseData/AddOrEdit",
            deleteUrl: "api/BaseData/Delete",
        },
        methods: {
            init: function () {
                $("#tblData").jgridInit({
                    url: this.getPostDataUrl,
                    sortname: "Sort",
                    sortorder: "asc",
                    colNames: ['编码', '数据类型代码', '数据名称', '分类名称', '排序号', ''], //列头
                    colModel: [
                        { name: 'Id', index: 'Id', width: 80, align: "center", hidden: true },
                        { name: 'Code', index: 'Code', width: 80, align: "center" },
                        { name: 'Name', index: 'Name', width: 80, align: "center" },
                        { name: 'DataTypeName', index: 'DataTypeName', width: 80, align: "center" },
                        { name: 'Sort', index: 'Sort', width: 50, align: "center" },
                       { name: '', width: 180, align: "center" },
                    ],
                    postData: topevery.form2Json("selectFrom")
                });
                topevery.BindSelect("QueryDataTypeId", "Common/DataTypeList", "--请选择--");
            },
            add: function () {
                topevery.ajax({ type: "get", url: this.addUrl, dataType: "html" }, function (data) {
                    layer.open({
                        type: 1,
                        title: "新增",
                        skin: 'layui-layer-rim', //加上边框
                        area: [this.width + 'px', this.height + 'px'], //宽高
                        content: data
                    });
                }, true);
            },
            edit: function () {
                var rowIndex = $("#tblData").jqGrid('getGridParam', 'selarrrow');
                if (rowIndex.length === 1) {
                    var rowData = $("#tblData").jqGrid('getRowData', rowIndex);
                    /*layer弹出一个html页面或者html片段*/
                    topevery.ajax({ type: "get", url: this.addUrl + "?Id=" + encodeURI(rowData.Id), dataType: "html" }, function (data) {
                        layer.open({
                            type: 1,
                            title: "修改",
                            skin: 'layui-layer-rim', //加上边框
                            area: [this.width + 'px', this.height + 'px'], //宽高
                            content: data
                        });
                    }, true);

                } else {
                    layer.alert("请选择一条且只选择一条记录!");
                }
            },
            del: function () {
                layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
                    var ids = [];
                    var data = $("#tblData").jqGrid('getGridParam', 'selarrrow');
                    if (data.length > 0) {
                        //遍历访问这个集合  
                        $(data).each(function (index, id) {
                            //由id获得对应数据行  
                            var rowData = $(grid).jqGrid('getRowData', id);
                            ids.push(rowData.Id);
                        });
                        topevery.ajax({
                            url: this.deleteUrl,
                            data: JSON.stringify({
                                "Ids": ids.join()
                            })
                        }, function (data) {
                            var icon = 1;
                            var message = "删除失败";
                            if (data.Success) {
                                icon = data.Result.Success === true ? 1 : 2;
                                if (data.Result.Success) {
                                    $(grid).trigger("reloadGrid");
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
            },
            query: function () {
                $("#tblData").jqGrid('setGridParam', {
                    url: this.getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
                }).trigger("reloadGrid");
            }
        },
        //mounted: function () {
        //    this.init();
        //}
    });
    app.init();
})
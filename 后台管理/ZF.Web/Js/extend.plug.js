(function ($) {
    /**
     * 设置jgrid的样式
     */
    $.jgrid.defaults.styleUI = "Bootstrap";
    $.fn.jgridInit = function (options) {
        $.fn.jgridInit.defaults = {
            datatype: "json",
            mtype: "POST",
            postData: options.postData,
            autowidth: true,
            height: options.height == null ? "100%" : options.height,
            rowNum: options.rowNum ? options.rowNum : 15,
            rowList: [10, 15,30, 50, 100],
            viewrecords: true,
            multiselect: options.multiselect ? options.multiselect : true,//复选框     
            rownumbers: true,
            repeatitems: false,
            shrinkToFit: options.shrinkToFit ? options.shrinkToFit : true,
            autoScroll: options.autoScroll ? options.autoScroll : true,
            altRows: true,
            altclass: 'someClass',
            jsonReader: {
                root: function (obj) {
                    return obj.Result.Rows;
                },
                page: function (obj) {
                    return obj.Result.Page;
                },
                total: function (obj) {
                    return obj.Result.Total;
                },
                records: function (obj) {
                    return obj.Result.Records;
                },
                repeatitems: false
            },
            caption: options.caption ? options.caption : false,
            sortname: options.sortname ? options.sortname : 'AddTime',
            sortorder: options.sortorder ? options.sortorder : 'desc',
            pager: options.pager == null ? '#pager' : options.pager,
            pginput: true,
            pgbuttons: true,
            gridComplete: function () {
                //$(this).setGridHeight($(".content-wrapper").outerHeight(true) - $(".nav-header").outerHeight(true) - 3 - $(".content-header").outerHeight(true) - 31 - 36 - 28 - 20);
                var header = $(".content-wrapper").outerHeight(true)
                        - $(".nav-header").outerHeight(true) - 3
                        - $(".content-header").outerHeight(true)
                        - $(".nav-tabs").outerHeight(true)
                        - $(this).parent().parent().prev().find(".ui-jqgrid-hbox").outerHeight(true)
                        - 31 - 28 - 20;
                if (!options.height) {
                    $(this).setGridHeight(header);
                } else {
                    $(this).setGridHeight(options.height);
                }

            }
        };
        var array = $.extend({}, $.fn.jgridInit.defaults, options);
        $(this).jqGrid(array);
    }

    /**
    * 绑定导入按钮
    * @param {} url  
    * @param {} width 
    * @param {} height 
    * @param {} title 
    * @returns {} 
    */
    $.fn.btnIfram = function (url) {
        $(this).on("click", function () {
            layer.open({
                type: 2,
                title: "导入",
                closeBtn: 1, //不显示关闭按钮
                area: [600 + 'px', 420 + 'px'],
                anim: 2,
                content: [url, 'no'] //iframe的url，no代表不显示滚动条
            });
        });
    },


    /**
     * 绑定添加按钮
     * @param {} url  
     * @param {} width 
     * @param {} height 
     * @param {} title 
     * @returns {} 
     */
    $.fn.bindAddBtn = function (url, width, height, title) {
        $(this).on("click", function () {
            topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: title || "新增",
                    skin: 'layui-layer-rim', //加上边框
                    area: [width + 'px', height + 'px'], //宽高
                    content: data
                });
            }, true);
        });
    }
    /**
     * 绑定编辑
     * @param {} url 
     * @param {} obj 
     * @param {} width 
     * @param {} height 
     * @param {} title 
     * @returns {} 
     */
    $.fn.bindEditBtn = function (url, obj, width, height, title, extension) {
        $(this).on("click", function () {
            var rowIndex = $(obj).jqGrid('getGridParam', 'selarrrow');
            if (rowIndex.length === 1) {
                var rowData = $(obj).jqGrid('getRowData', rowIndex);
                var extensionUrl = "";
                if (extension) {
                    extensionUrl = extension;
                }
                /*layer弹出一个html页面或者html片段*/
                topevery.ajax({ type: "get", url: url + "?Id=" + encodeURI(rowData.Id + extensionUrl), dataType: "html" }, function (data) {
                    layer.open({
                        type: 1,
                        title: title || "修改",
                        skin: 'layui-layer-rim', //加上边框
                        area: [width + 'px', height + 'px'], //宽高
                        content: data
                    });
                }, true);

            } else {
                layer.alert("请选择一条且只选择一条记录!");
            }
        });
    },
        $.fn.bindEditBtnEx = function (url, obj, width, height, title, extension, beforeClick) {
           
            $(this).on("click", function () {
                var rowIndex = $(obj).jqGrid('getGridParam', 'selarrrow');
                if (rowIndex.length === 1) {
                    var flag = true;
                    var rowData = $(obj).jqGrid('getRowData', rowIndex);
                    var extensionUrl = "";
                    if (extension) {
                        extensionUrl = extension;
                    }
                    if (beforeClick != undefined && beforeClick !== "") {
                       flag= beforeClick(rowData);
                    }

                    if (flag) {
                        /*layer弹出一个html页面或者html片段*/
                        topevery.ajax({ type: "get", url: url + "?Id=" + rowData.Id + extensionUrl, dataType: "html" }, function (data) {
                            layer.open({
                                type: 1,
                                title: title || "修改",
                                skin: 'layui-layer-rim', //加上边框
                                area: [width + 'px', height + 'px'], //宽高
                                content: data
                            });
                        }, true);
                    }
                } else {
                    layer.alert("请选择一条且只选择一条记录!");
                }
            });
        },

    /**
    * 绑定编辑  单选
    * @param {} url 
    * @param {} obj 
    * @param {} width 
    * @param {} height 
    * @param {} title 
    * @returns {} 
    */
    $.fn.bindEditBtnDx = function (url, obj, width, height, title, extension) {
        $(this).on("click", function () {
            var rowIndex = $(obj).jqGrid('getGridParam', 'selrow');
            if (rowIndex != null) {
                var rowData = $(obj).jqGrid('getRowData', rowIndex);
                var extensionUrl = "";
                if (extension) {
                    extensionUrl = extension;
                }
                /*layer弹出一个html页面或者html片段*/
                topevery.ajax({ type: "get", url: url + "?Id=" + rowData.Id + extensionUrl, dataType: "html" }, function (data) {
                    layer.open({
                        type: 1,
                        title: title || "修改",
                        skin: 'layui-layer-rim', //加上边框
                        area: [width + 'px', height + 'px'], //宽高
                        content: data
                    });
                }, true);

            } else {
                layer.alert("请选择一条记录!");
            }
        });
    },
    /**
     * 绑定导出
     * @param {} url 
     * @param {} obj 
     * @returns {} 
     */
     $.fn.bindExportBtn = function (url) {
         $(this).on("click", function () {
             window.location.href = url + topevery.GetUrlByform2Json();
         });
     }

    /**
    * 绑定新增页面跳转
    * @param {} url 
    * @returns {} 
    */
    $.fn.bindAddSkip = function (url) {
        $(this).on("click", function () {
            topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
                $(".content-wrapper").html(data);
            }, true);
        });
    }


    /**
   * 绑定修改页面跳转
   * @param {} url 
   * @param {} obj 
   * @param {} extension 
   * @returns {} 
   */
    $.fn.bindEditSkip = function (url, obj, extension) {
        $(this).on("click", function () {
            var rowIndex = $(obj).jqGrid('getGridParam', 'selarrrow');
            if (rowIndex.length === 1) {
                var rowData = $(obj).jqGrid('getRowData', rowIndex);
                var extensionUrl = "";
                if (extension) {
                    extensionUrl = extension;
                }
                topevery.ajax({ type: "get", url: url + "?Id=" + rowData.Id + extensionUrl, dataType: "html" }, function (data) {
                    $(".content-wrapper").html(data);
                }, true);
            } else {
                layer.alert("请选择一条且只选择一条记录!");
            }
        });
    }



    /**
   * 删除事件  单选
   * @param {} url 
   * @param {} obj 
   * @returns {} 
   */
    $.fn.bindDelBtnDx = function (url, obj, beforeClick) {
        $(this).on("click", function () {
            layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
                var rowIndex = $(obj).jqGrid('getGridParam', 'selrow');
                if (rowIndex) {
                    var rowData = $(obj).jqGrid('getRowData', rowIndex);
                    var flag = true;
                    if (beforeClick != undefined && beforeClick !== "") {
                       flag = beforeClick(rowData);
                    }
                    if (flag) {
                    $.topevery.ajax({
                        url: url,
                        data: JSON.stringify({
                            "Ids": rowData.Id
                        })
                    }, function (data) {
                        var message = "删除失败";
                        var icon = 1;
                        if (data.Success) {
                            icon = data.Result.Success === true ? 1 : 2;
                            if (data.Result.Success) {
                                $(obj).trigger("reloadGrid");
                            }
                            message = data.Result.Message;
                        }
                        layer.msg(message, {
                            icon: icon,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 3000, //10秒后自动关闭
                            anim: 5
                        });
                    });
                    }
                } else {
                    layer.alert("请选择一条记录!");
                }
            });
        });
    }



    /**
     * 删除事件
     * @param {} url 
     * @param {} obj 
     * @returns {} 
     */
    $.fn.bindDelBtn = function (url, obj, callback) {
        $(this).on("click", function () {
            layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
                var ids = [];
                var data = $(obj).jqGrid('getGridParam', 'selarrrow');
                if (data.length > 0) {
                    //遍历访问这个集合  
                    $(data).each(function (index, id) {
                        //由id获得对应数据行  
                        var rowData = $(obj).jqGrid('getRowData', id);
                        ids.push(rowData.Id);
                    });
                    topevery.ajax({
                        url: url,
                        data: JSON.stringify({
                            "Ids": ids.join()
                        })
                    }, function (data) {
                        var icon = 1;
                        var message = "删除失败";
                        if (data.Success) {
                            icon = data.Result.Success === true ? 1 : 2;
                            if (data.Result.Success) {
                                $(obj).trigger("reloadGrid");
                            }
                            if (callback != undefined && callback !== "") {
                                callback();
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
    }
    /**
     * 绑定验证属性以及提交按钮
     * @param {} options 
     * @returns {} 
     */
    $.fn.bootstrapValidatorAndSumbit = function (url, options, validateForm, objectData, callback, sumbitFormId) {
        if (validateForm == undefined || validateForm === "") {
            validateForm = function () {
                return true;
            };
        }
        if (sumbitFormId == undefined || sumbitFormId === "") {
            sumbitFormId = "sumbitForm";
        }
        $(this).bootstrapValidator({
            message: '输入的值无效',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: options
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var $form = $(e.target);
            var bv = $form.data('bootstrapValidator');
            if (validateForm() && bv.isValid()) {
                var objectData1 = new Object();
                $.extend(true, objectData1, objectData);
                if (objectData == undefined || objectData === "") {
                    objectData = {};
                } else {
                    try {
                        $.each(objectData, function (i, n) {
                            objectData1[i] = eval(n);
                        });
                    } catch (e) {

                    }
                }
                topevery.ajax({
                    url: url,
                    data: topevery.extend(objectData1, topevery.serializeObject($("#" + sumbitFormId)))
                }, function (data) {
                    var message = "新增失败";
                    var icon = 1;
                    if (data.Success) {
                        message = data.Result.Message;
                        icon = data.Result.Success === true ? 1 : 2;
                        if (data.Result.Success) {
                            if (callback != undefined && callback !== "") {
                                bv.resetForm();
                                callback();
                            } else {
                                $(".layui-layer-close").click();
                                $(".query_btn").click();
                                $(".return").click();
                                layer.msg(message, {
                                    icon: icon,
                                    title: false, //不显示标题
                                    offset: 'auto',
                                    time: 3000, //10秒后自动关闭
                                    anim: 5
                                });
                            }
                        } else {
                            layer.msg(message, {
                                icon: icon,
                                title: false, //不显示标题
                                offset: 'auto',
                                time: 3000, //10秒后自动关闭
                                anim: 5
                            });
                        }
                    }
                }
                );
            }

        });
    }




    ///**
    // * 绑定验证属性以及提交按钮  测试用
    // * @param {} options 
    // * @returns {} 
    // */
    //$.fn.bootstrapValidatorAndSumbit1 = function (url, options, validateForm, objectData, callback) {
    //    if (validateForm == undefined || validateForm === "") {
    //        validateForm = function () {
    //            return true;
    //        };
    //    }
    //    $(this).bootstrapValidator({
    //        message: '输入的值无效',
    //        feedbackIcons: {
    //            valid: 'glyphicon glyphicon-ok',
    //            invalid: 'glyphicon glyphicon-remove',
    //            validating: 'glyphicon glyphicon-refresh'
    //        },
    //        fields: options
    //    }).on('success.form.bv', function (e) {
    //        e.preventDefault();
    //        var $form = $(e.target);
    //        var bv = $form.data('bootstrapValidator');
    //        if (objectData == undefined || objectData === "") {
    //            objectData = {};
    //        } else {
    //            $.each(objectData, function (i, n) {
    //                objectData[i] = eval(n);
    //            });
    //        }
    //        if (validateForm() && bv.isValid()) {
    //            topevery.ajax({
    //                url: url,
    //                data: topevery.extend(objectData, topevery.serializeObject($("#sumbitForm")))
    //            }, function (data) {
    //                var message = "新增失败";
    //                var icon = 1;
    //                if (data.Success) {
    //                    message = data.Result.Message;
    //                    icon = data.Result.Success === true ? 1 : 2;
    //                    if (data.Result.Success) {
    //                        if (callback != undefined && callback !== "") {
    //                            callback();
    //                        } else {
    //                            $(".layui-layer-close").click();
    //                            $(".query_btn").click();
    //                            layer.msg(message, {
    //                                icon: icon,
    //                                title: false, //不显示标题
    //                                offset: 'rb',
    //                                time: 3000, //10秒后自动关闭
    //                                anim: 2
    //                            });
    //                        }
    //                    } else {
    //                        layer.msg(message, {
    //                            icon: icon,
    //                            title: false, //不显示标题
    //                            offset: 'rb',
    //                            time: 3000, //10秒后自动关闭
    //                            anim: 2
    //                        });
    //                    }
    //                }
    //            }
    //            );
    //        }

    //    });
    //}



})(jQuery);




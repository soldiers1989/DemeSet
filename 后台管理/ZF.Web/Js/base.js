(function ($) {
    var dataQ = 2014;
    $.topevery = {
        /**
        * 遮罩层加载
        * @returns {} 
        */
        ajaxLoading: function () {
            $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
            $("<div class=\"datagrid-mask-msg\"></div>").html("正在处理，请稍候...").appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
        },
        /**
        * 遮罩层关闭
        * @returns {} 
        */
        ajaxLoadEnd: function () {
            $(".datagrid-mask").remove();
            $(".datagrid-mask-msg").remove();
        },
        /**
        * 
        * @param {} args ajax参数
        * @param {} callback 回调函数
        * @param {} isShowLoading 是否需要加载动态图片
        * @returns {} 
        */
        ajax: function (args, callback, isShowLoading) {
            if (isShowLoading) {
                $.topevery.ajaxLoading();
            }
            args = $.extend({}, { type: "POST", dataType: "json", contentType: "application/json" }, args);
            $.ajax(args).done(function (data) {
                if (isShowLoading) {
                    $.topevery.ajaxLoadEnd();
                }
                if (callback) {
                    callback(data);
                }
            }).fail(function () {
                if (isShowLoading) {
                    $.topevery.ajaxLoadEnd();
                }
            });
        },
        /**
 * 上传控件的初始化
 * @param {} moduleId 模块Id
 * @param {} keyId    申请主体Id
 * @param {} activityInstanceId 环节实例Id
 * @param {} target 目标
 * @returns {} 
 */
        setUploadFile: function (keyId, moduleId, target) {
            target = $("#" + target);
            $.topevery.ajax({
                type: "POST",
                url: "api/Attchfile/GetFileRDtoList",
                contentType: "application/json",
                data: JSON.stringify({ Id: keyId, MzObjectId: moduleId })
            }, function (row) {
                if (row.success) {
                    var data = row.result;
                    var hdFileData = "";
                    for (var i = 0; i < data.length; i++) {
                        var li = target.next().next().find("ul").clone().html();
                        li = li.replace("{imgUrl}", data[i].fileurl).replace("{imgName}", data[i].filename).replace("{imgName}", data[i].filename)
                        .replace("{fileId}", data[i].id);
                        target.next().append(li);
                        if (hdFileData === "") {
                            hdFileData = data[i].id + "," + data[i].filename;
                        } else {
                            hdFileData += ";" + data[i].id + "," + data[i].filename;
                        }
                    }
                    //回发时还原hiddenfiled的保持数据
                    target.next().next().find("input").val(hdFileData);
                }
            });
        },
        setViewUploadFile: function (moduleId, attachmentId, width, height) {
            if (width) {
                width = 100;
            };
            if (!height) {
                height = 100;
            };
            $.topevery.ajax({
                type: "POST",
                url: "api/Attchfile/GetFileRDtoList",
                contentType: "application/json",
                data: JSON.stringify({ MzObjectId: moduleId })
            }, function (row) {
                if (row.success) {
                    var data = row.result;
                    var html = "<ul class='photo-list'>";
                    for (var i = 0; i < data.length; i++) {
                        var suffix = data[i].filename.split('.')[1];
                        html += "<li><a title=\"" + data[i].filename + "\" href=\"" + data[i].fileurl + "\" target=\"_blank\"><img width='" + width + "' height='" + height + "' href=\"" + data[i].fileDownUrl + "\" src=\"" + data[i].fileurl + "\"> <img/></a><a href=\"" + data[i].fileDownUrl + "\">" + data[i].filename + "</a></li>";
                    }
                    html += "</ul>";
                    $("#" + attachmentId + "").append(html);
                } else {
                    error();
                }
            }, true);
        },
        /**
        * 日期格式
        * @param {} tm 
        * @returns {} 
        */
        dateFormat: function (tm) {
            if (tm != null && tm !== 'undefined') {
                tm = tm.replace("/Date(", "").replace(")/", "");
                return new Date(parseInt(tm)).toLocaleString().replace(/:\d{1,2}$/, ' ');
            }
            return "";
        },

        /**
         * 时间格式装换  
         * @param {} datatime 
         * @returns {} 00:00
         */
        dataTimeFormat1: function (datatime) {
            if (datatime !== null) {
                var data = datatime.split("T")[1].substring(0, 5);
                return data;
            } else {
                return "";
            }
        },
        /**
         * 时间格式装换  
         * @param {} datatime 
         * @returns {} 2015-12-11
         */
        dataTime: function (datatime) {
            if (datatime !== null) {
                var data = datatime.split("T")[0].replace('-', '/').replace('-', '/');
                return data;
            } else {
                return "";
            }
        },
        /**
         * 时间格式装换  
         * @param {} datatime 
         * @returns {} 2015-12-11
         */
        dataTimeView: function (datatime) {
            if (datatime !== null) {
                var data = datatime.split("T")[0];
                return data;
            } else {
                return "";
            }
        },
        /**
         * 
         * @param {} datatime 
         * @returns {} 2015-12-11 11:11:00
         */
        dataTimeFormatTT: function (datatime) {
            if (datatime !== null) {
                var data = datatime.replace("T", " ");
                return data;
            } else {
                return "";
            }
        },
        /**
         * 
         * @param {} form 
         * @returns {} 
         */
        serializeObject: function (form) {
            var o = {};
            $.each(form.serializeArray(), function (index) {
                if (o[this['name']]) {
                    o[this['name']] = o[this['name']] + "," + $.trim(this['value']);
                } else {
                    o[this['name']] = $.trim(this['value']);
                }
            });
            return o;
        },
        userSate: function (tm) {
            debugger
            if (tm === 0) {
                return "启用";
            } else if (tm === 1) {
                return "禁用";
            }
            return "";
        },
        isYesOrNo: function (tm) {
            if (tm === 0) {
                return "否";
            } else if (tm === 1) {
                return "是";
            }
            return "";
        },
        stu: function (tm) {
            if (tm === 0) {
                return "在职";
            } else if (tm === 1) {
                return "离岗";
            }
            return "";
        },
        isToView: function (tm) {
            if (tm === 0) {
                return "已查看";
            } else if (tm === 1) {
                return "未查看";
            }
            return "";
        },
        changtype: function (tm) {
            if (tm === 0) {
                return "登记";
            } else if (tm === 1) {
                return "离职";
            }
            return "";
        },
        outboundinventory: function (tm) {
            if (tm === 0) {
                return "入库";
            } else if (tm === 1) {
                return "出库";
            }
            else if (tm === 2) {
                return "报废";
            }
            return "";
        },
        sex: function (tm) {
            if (tm === 0) {
                return "男";
            } else if (tm === 1) {
                return "女";
            }
            return "";
        },
        fabu: function (tm) {
            if (tm === 0) {
                return "已发布";
            } else if (tm === 1) {
                return "未发布";
            }
            return "";
        },
        spzt: function (tm) {
            if (tm === 0) {
                return "申请";
            } else if (tm === 1) {
                return "待审批";
            }
            else if (tm === 2) {
                return "通过";
            }
            else if (tm === 3) {
                return "未通过";
            }
            return "";
        },
        sqlx: function (tm) {
            if (tm === 0) {
                return "调拨";
            } else if (tm === 1) {
                return "报废";
            }
            return "";
        },
        wzxqGet: function (count, data, rows) {
            return "<a onclick=\"$.topevery.WzxqVIew('" + rows.id + "')\">查看物资详情</a>";
        },
        getAurl: function (count, data, rows) {
            if (count)
                return "<a onclick=\"$.topevery.fujianVIew('" + rows.id + "')\">" + count + "</a>";
            return 0;
        },
        getDownload: function (count, data, rows) {
            return "<a onclick=\"$.topevery.fujianVIew('" + rows.id + "')\">查看</a>";
        },
        getRecipient: function (count, data, rows) {
            return "<a onclick=\"$.topevery.getRecipientView('" + rows.id + "')\">查看</a>";
        },
        getToView: function (count) {
            if (count)
                return "<a onclick=\"$.topevery.fujianVIew('" + count + "')\">查看</a>";
            return 0;
        },

        color: function (count, data, rows) {
            if (rows.inventoryNum <= rows.minnumber) {
                return "<a style=\"color:red\">" + count + "</a>";
            } else {
                return "<a style=\"color:#33ae13\">" + count + "</a>";
            }
        },
        /**
               * 附件数量转换成链接  
               * @param {} datatime 
               * @returns {} 
               */
        //function getAurl(count, data, rows) {
        //    return "<a onclick=\"fujianVIew('" + rows.id + "')\">" + count + "</a>";
        //}
        getRecipientView: function (parameters) {
            $.topevery.bindAddBtn(parameters, 'Announcements/RecipientView', 620, 450, '查看通知情况');
        },


        fujianVIew: function (parameters) {
            $.topevery.bindAddBtn(parameters, 'File/FileIndex', 600, 650);
        },

        WzxqVIew: function (parameters) {
            $.topevery.bindAddBtn(parameters, 'ReserveSupplies/MaterialView', 600, 420, '物资详情');
        },

        /**
             * 绑定添加按钮
             * @param {} url 
             * @param {} width 
             * @param {} height 
             * @param {} title 
             * @returns {} 
             */
        bindAddBtn: function (id, url, width, height, title) {
            $.topevery.ajax({ type: "get", url: url + "?Id=" + id, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: title || "附件展示",
                    skin: 'layui-layer-rim', //加上边框
                    area: [width + 'px', height + 'px'], //宽高
                    content: data
                });
            }, true);
        },
        age: function (tm, data, rows) {
            if (rows.cardno) {
                var temp;
                var year;
                var myDate = new Date();
                var str = rows.cardno;
                if (rows.cardno.length === 18) {
                    temp = str.substring(6, 14);
                    year = temp.substring(0, 4);
                    return myDate.getFullYear() - year;

                } else if (rows.cardno.length === 16) {
                    temp = str.substring(6, 14);
                    year = 19 + temp.substring(0, 2);
                    return myDate.getFullYear() - year;
                }
            }
            return "";
        },
        /* 设置json变量到查看表单 */
        setParmByLookForm: function (row) {
            var objs = $("#sumbitForm input,select,textarea");
            for (var i = 0; i < objs.length; i++) {
                var o = objs[i];
                try {
                    var name = o.name;
                    var tagName = o.tagName.toLocaleLowerCase();
                    var type = o.type; o.tagName.toLocaleLowerCase();
                    var dysetMes = "";
                    var jqObject = name.substring(0, 1).toLowerCase() + name.substring(1, name.length);
                    row[jqObject] = row[jqObject] == null ? "" : row[jqObject];
                    if (tagName === "input" || tagName === "select") {
                        if (type === "radio") {
                            dysetMes = "$('input[name='" + name + "'][value='" + row[jqObject] + "']').attr('checked', true)";
                        } else {
                            dysetMes = "$('#" + name + "').val('" + row[jqObject] + "');";
                        }
                    }
                    if (tagName === "textarea") {
                        dysetMes = "$('#" + name + "').text('" + row[jqObject] + "');";
                    }
                    try {
                        eval(dysetMes);
                    } catch (e) {

                    }
                } catch (e) {

                }
            }
        },
        ///两个对象合并成一个
        extend: function (obj1, obj2) {
            if (obj1 !== null && obj2 !== null) {
                for (var key in obj2) {
                    if (obj1.hasOwnProperty(key)) continue; //有相同的属性则略过 
                    obj1[key] = obj2[key];
                }
                return JSON.stringify(obj1);
            } else if (obj1 !== null) {
                return JSON.stringify(obj1);
            } else if (obj2 !== null) {
                return JSON.stringify(obj2);
            } else {
                return null;
            }
        },
        datagrid: function (options, queryData, load) {
            $('#' + options.tableId).datagrid({
                idField: options.idField !== undefined ? options.idField : "Id",
                striped: options.striped !== undefined ? options.striped : true,
                fitColumns: options.fitColumns !== undefined ? options.fitColumns : false,
                fit: options.fit !== undefined ? options.fit : false,
                singleSelect: options.singleSelect !== undefined ? options.singleSelect : true,
                nowrap: options.nowrap !== undefined ? options.nowrap : true,
                height: options.height !== undefined ? options.height : 410,
                rownumbers: options.rownumbers !== undefined ? options.rownumbers : true,
                pagination: options.pagination !== undefined ? options.pagination : true,
                pageSize: options.pageSize !== undefined ? options.pageSize : 10,
                pageList: options.pageList !== undefined ? options.pageList : [10, 50, 100, 500, 1000, 2000, 10000],
                showFooter: options.showFooter !== undefined ? options.showFooter : true,
                onLoadSuccess: function (data) {
                    $(this).datagrid('doCellTip', { 'max-width': '400px', 'delay': 500 });
                    $(this).datagrid("clearSelections").datagrid("clearChecked");
                },
                columns: options.columns
            });
        }
    };

    $.topevery.um = {
        /**
         * 绑定数据字典
         * @param {} nameId 
         * @param {} obj
         * @returns {} 
         * @param {} defaultValue  默认值传入参数，下拉框默认选中此值
         */
        bindDictionary: function (nameId, obj, callback, defaultValue) {
            $.topevery.ajax({
                url: "api/BasicData/GetDictionarysByNameSpaceId?guid=" + nameId
            }, function (data) {
                if (data.success) {
                    var row = data.result;
                    var html = '<option value="">--请选择--</option>';
                    $(row).each(function () {
                        if (defaultValue && this.key.toLowerCase() === defaultValue.toLowerCase()) {
                            html += '<option value="' + this.key + '" selected="selected">' + this.value + '</option>';
                        } else {
                            html += '<option value="' + this.key + '">' + this.value + '</option>';
                        }
                    });
                    $(obj).html(html);
                    if (typeof callback === "function") {
                        callback();
                    }
                }
            });
        },
        /**
         * 绑定下拉数据
         * @param {} nameId 
         * @param {} obj
         * @returns {} 
         * @param {} defaultValue  默认值传入参数，下拉框默认选中此值
         */
        bindReportArea: function (url, obj, callback, defaultValue) {
            $.topevery.ajax({
                url: url
            }, function (data) {
                if (data.success) {
                    var row = data.result;
                    var html = '<option value="">--请选择--</option>';
                    $(row).each(function () {
                        if (defaultValue && this.key.toLowerCase() === defaultValue.toLowerCase()) {
                            html += '<option value="' + this.key + '" selected="selected">' + this.value + '</option>';
                        } else {
                            html += '<option value="' + this.key + '">' + this.value + '</option>';
                        }
                    });
                    $(obj).html(html);
                    if (typeof callback === "function") {
                        callback();
                    }
                }
            });
        }
    }
    $.topevery.rept = {
        initialize: function (data, url, callback) {
            $.topevery.ajax({
                type: "POST",
                url: url,
                contentType: "application/json",
                data: JSON.stringify(data)
            }, function (row1) {
                if (row1.success) {
                    var data = row1.result;
                    callback(data);
                } else {
                    error();
                }
            }, true);
        },
        btnBindClick: function () {
            var date = new Date();
            var html = "";
            for (var i = date.getFullYear() ; i > dataQ; i--) {
                if (i === date.getFullYear()) {
                    html += '<li class="text-center"><p class="menu-list-title" value="' + i + '">' + i + '年<i class="glyphicon glyphicon-triangle-top pull-right"></i></p><ul class="menu-list-second">';
                    for (var h = date.getMonth()+1; h >= 1; h--) {
                        html += '<li><a href="javascript:void(0);" value="' + h + '">' + h + '月</a></li>';
                    }
                } else {
                    html += '<li class="text-center"><p class="menu-list-title" value="' + i + '">' + i + '年<i class="glyphicon glyphicon-triangle-top pull-right"></i></p><ul class="menu-list-second"  style = "display: none;">';
                    for (var j = 12; j >= 1; j--) {
                        html += '<li><a href="javascript:void(0);" value="' + j + '">' + j + '月</a></li>';
                    }
                }
                html += '</ul></li>';
            }
            $("#menu-list").html(html);
            $('.content-box-left li:has(ul)').addClass('parent_li');
            $('.content-box-left li.parent_li > p').on('click', function (e) {
                var children = $(this).parent('li.parent_li').find(' > ul');
                if (children.is(":visible")) {
                    children.slideUp(500);
                    $(this).find(' > i').addClass('glyphicon-triangle-top').removeClass('glyphicon-triangle-bottom');
                } else {
                    children.slideDown(500);
                    $(this).find(' > i').addClass('glyphicon-triangle-bottom').removeClass('glyphicon-triangle-top');
                }
                e.stopPropagation();
            });
            
            //图表表格页面高度设置
            $('.content-box-left').height($(window).height() - 142);
            $('.tab-cons').height($(window).height() - 202);
            $('.tab-list').height($('.tab-cons').height());     //图表部分
            $('.table-box').height($('.tab-list').height() - 55);
            $(window).resize(function () {
                $('.content-box-left').height($(window).height() - 142);
                $('.tab-cons').height($(window).height() - 202);
                $('.tab-list').height($('.tab-cons').height());     //图表部分
                $('.table-box').height($('.tab-list').height() - 55);
            });

            //tab切换
            $('.tab-btns a').on('click', function () {
                $(this).addClass('tab-active').siblings().removeClass('tab-active');
                $('.tab-cons .tab-list').eq($(this).index()).show().siblings().hide();
            });

        },
        btnAddorEdit: function (option) {
            $.topevery.ajax({ type: "get", url: option.addUrl, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: option.title ? option.title : "新增",
                    skin: 'layui-layer-rim', //加上边框
                    area: [option.width + 'px', option.height + 'px'], //宽高
                    content: data
                });
            }, true);
        },
        btnIfram: function (option) {
            layer.open({
                type: 2,
                title: "导入",
                closeBtn: 1, //不显示关闭按钮
                area: [600 + 'px', 400 + 'px'],
                anim: 2,
                content: [option.reptUrl, 'no'] //iframe的url，no代表不显示滚动条
            });
        },
        btnDelete: function (option) {
            $.topevery.ajax({
                url: option.deleteUrl,
                data: JSON.stringify({
                    "Id": option.id
                })
            }, function (data) {
                var message = "删除失败";
                if (data.success) {
                    if (data.result.success) {
                        option.callback(option.year, option.month);
                    }
                    message = data.result.message;
                }
                layer.msg(message, {
                    icon: icon,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            });
        },
        clickFirst:function() {
            $('.menu-list-second>li a')[0].click();
        }
    }
})(jQuery);

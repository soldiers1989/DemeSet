var topevery = {
    index: 0,
    webapipath: "",
    /**
     * 遮罩层加载
     * @returns {} 
     */
    ajaxLoading: function () {
        topevery.index = layer.load(0, { shade: [0.8, '#393D49'] });
    },
    /**
     * 遮罩层关闭
     * @returns {}   
     */
    ajaxLoadEnd: function () {
        layer.close(topevery.index);
    },
    /**
     * 
     * @param {} args ajax参数
     * @param {} callback 回调函数
     * @param {} isShowLoading 是否需要加载动态图片
     * @returns {} 
     */
    ajax: function (args, callback, isShowLoading) {
        //采用jquery easyui loading css效果

        if (isShowLoading) {
            topevery.ajaxLoading();
        }
        args.url = topevery.webapipath + args.url;
        args = $.extend({}, { type: "POST", dataType: "json", contentType: "application/json" }, args);
        $.ajax(args).done(function (data) {
            if (isShowLoading) {
                topevery.ajaxLoadEnd();
            }
            if (callback) {
                callback(data);
            }
        }).fail(function (data) {
            if (isShowLoading) {
                topevery.ajaxLoadEnd();
            }

            layer.msg(data.responseJSON.Error, {
                icon: 2,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        });
    },

    SetCookie: function (cname, cvalue, exdays) {
        if (!exdays)
            exdays = 1;
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 12 * 60 * 60 * 1000));
        document.cookie = cname + '=' + cvalue + ';expires=' + d.toUTCString() + ';path=/';
    },
    GetCookie: function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
        }
        return "";
    },

    /**
    * 弹出提示框
    * @param {} message 
    * @param {} icon 
    * @returns {} 
    */
    msg: function (message, icon) {
        layer.msg(message, {
            icon: icon ? icon : 1,
            time: 3000
        });
    },
    ///json转换成对象
    form2Json: function (id) {
        var arr = $("#" + id).serializeArray();
        var jsonStr = "";
        jsonStr += '{';
        for (var i = 0; i < arr.length; i++) {
            jsonStr += '"' + arr[i].name + '":"' + $.trim(arr[i].value) + '",';
        }
        jsonStr = jsonStr.substring(0, (jsonStr.length - 1));
        jsonStr += '}';
        var json = JSON.parse(jsonStr);
        return json;
    },
    /* 设置json变量到查看表单 */
    setParmByLookForm: function (row, fromId) {
        var objs = $("#sumbitForm input,select,textarea");
        if (fromId !== undefined && fromId !== "") {
            objs = $("#" + fromId + " input,select,textarea");
        }
        for (var i = 0; i < objs.length; i++) {
            var o = objs[i];
            try {
                var name = o.name;
                var tagName = o.tagName.toLocaleLowerCase();
                var type = o.type; o.tagName.toLocaleLowerCase();
                var dysetMes = "";
                var jqObject = name.substring(0, 1).toUpperCase() + name.substring(1, name.length);
                row[jqObject] = row[jqObject] == null ? "" : row[jqObject];
                if (tagName === "input" || tagName === "select") {
                    if (type === "radio") {
                        dysetMes = "$('input[name=\"" + name + "\"][value=\"" + row[jqObject] + "\"]').attr('checked', true)";
                    } else {
                        dysetMes = "$('#" + name + "').val('" + row[jqObject] + "');";
                    }
                }
                if (tagName === "textarea") {
                    dysetMes = "$('#" + name + "').val('" + row[jqObject] + "');";
                }
                try {
                    eval(dysetMes);
                } catch (e) {

                }
            } catch (e) {

            }
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
    /**
    * 上传控件的初始化
    * @param {} keyId    申请主体Id
    * @param {} activityInstanceId 环节实例Id
    * @param {} target 目标
    * @returns {} 
    */
    setUploadFile: function (keyId, target, type) {
        if (type == undefined || type === "") {
            type = 0;
        }
        if (keyId) {
            target = $("#" + target);
            $.topevery.ajax({
                type: "POST",
                url: "File/GetFileRDtoList",
                contentType: "application/json",
                data: JSON.stringify({ ModuleId: keyId, Type: type })
            }, function (row) {
                if (row.Success) {
                    var data = row.Result;
                    var hdFileData = "";
                    for (var i = 0; i < data.length; i++) {
                        var li = target.next().next().find("ul").clone().html();
                        li = li.replace("{imgUrl}", data[i].FileUrl).replace("{imgName}", data[i].QlyName).replace("{imgName}", data[i].QlyName)
                        .replace("{fileId}", data[i].QlyName).replace("{href}", data[i].DownloadUrl).replace("{href}", data[i].DownloadUrl);
                        target.next().append(li);
                        if (hdFileData === "") {
                            hdFileData = data[i].QlyName;
                        } else {
                            hdFileData += "," + data[i].QlyName;
                        }
                    }
                    //回发时还原hiddenfiled的保持数据
                    target.next().next().find("input").val(hdFileData);
                }
            });
        }
    },
    //根据form2Json对象转换成get链接
    GetUrlByform2Json: function () {
        var arr = topevery.form2Json("selectFrom");
        var url = new Array("?1=1");
        for (var i in arr) {
            url.push(i + "=" + arr[i]);
        }
        return url.join("&");
    },
    ///两个对象合并成一个  前面对象合并后面对象
    extend: function (obj1, obj2) {
        var obj3 = new Object();
        $.extend(true, obj3, obj1);
        if (obj1 !== null && obj2 !== null) {
            for (var key in obj2) {
                if (obj1.hasOwnProperty(key)) continue; //有相同的属性则略过 
                obj3[key] = obj2[key];
            }
            return JSON.stringify(obj3);
        } else if (obj1 !== null) {
            return JSON.stringify(obj1);
        } else if (obj2 !== null) {
            return JSON.stringify(obj2);
        } else {
            return null;
        }
    },
    IsEconomicBase: function (tm) {
        if (tm === 0) {
            return "否";
        } else if (tm === 1) {
            return "是";
        }
        return "";
    },
    isYesOrNo: function (tm) {
        if (tm === 0) {
            return "是";
        } else if (tm === 1) {
            return "否";
        }
        return "";
    },
    State: function (tm) {
        if (tm === 0) {
            return "启用";
        } else if (tm === 1) {
            return "失效";
        }
        return "";
    },

    SubjectType: function (tm) {
        if (tm === 1) {
            return "单选题";
        } else if (tm === 2) {
            return "多选题";
        } else if (tm === 3) {
            return "判断题";
        } else if (tm === 7) {
            return "案例分析";
        }
        return "";
    },
    DifficultLevel: function (tm) {
        if (tm === 1) {
            return "易";
        } else if (tm === 2) {
            return "中";
        } else if (tm === 3) {
            return "难";
        }
        return "";
    },

    RightAnswer: function (tm, row, data) {
        var html = "";
        switch (data.SubjectType) {
            case 1:
            case 2:
                var rightAnswerData = tm.split(',');
                for (var i = 0; i < rightAnswerData.length; i++) {
                    html += "选项" + rightAnswerData[i] + ",";
                }
                break;;
            case 3:
                if (tm === "0") {
                    html = "正确";
                } else {
                    html = "错误";
                }
                break;;
            case 7:
                break;;
            default:
        }
        return html.substring(0, html.lastIndexOf(','));
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
     * @returns {} 2015-12-11
     */
    dataTimeView: function (datatime) {
        if (datatime !== null) {
            var data = datatime.split(" ")[0];
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
    *绑定下拉列表,允许传递对象参数,实现级联'
     * @param {} btnid  绑定Id
     * @param {} url  url
     * @param {} obj 参数对象
     * @param {} defaultText  默认值
     * @param {} callback 回调函数
     * @returns {} 
    */
    BindLevelSelect: function (btnid, url, obj, defaultText, callback) {
        topevery.ajax({
            url: url,
            data: JSON.stringify(obj)
        }, function (data) {
            var html = "";
            html = "<option value=''>" + defaultText + "</option>";
            if (defaultText == undefined || defaultText === "") {
                html = "";
            }
            for (var i = 0; i < data.length; i++) {
                html += "<option value=" + data[i].Key + ">" + data[i].Value + "</option>";
            }
            $("#" + btnid).html(html);
            if (callback != undefined && defaultText !== "") {
                callback();
            }
        });
    },


    /**
     * 绑定下拉列表
     * @param {} btnid  绑定Id
     * @param {} url  url
     * @param {} defaultText  默认值
     * @param {} callback 回调函数
     * @returns {} 
     */
    BindSelect: function (btnid, url, defaultText, callback) {
        topevery.ajax({
            url: url,
            data: JSON.stringify({})
        }, function (data) {
            var html = "";
            html = "<option value=''>" + defaultText + "</option>";
            if (defaultText == undefined || defaultText === "") {
                html = "";
            }
            for (var i = 0; i < data.length; i++) {
                html += "<option value=" + data[i].Key + ">" + data[i].Value + "</option>";
            }
            $("#" + btnid).html(html);
            if (callback != undefined && callback !== "") {
                callback();
            }
        });
    },
    /**
     * 绑定树
     * @param {} btnid  绑定Id
     * @param {} url  url
     * @param {} defaultText  默认值
     * @param {} callback 回调函数
     * @returns {} 
     */
    BindTree: function (btnid, url, callback, onClick, expandNode, callback1, l, nodecheck, checkCallback) {
        $("#" + btnid).hide();
        if (onClick == undefined || onClick === "") {
            onClick = function () {

            };
        }
        if (l == undefined || l === "") {
            l = 1;
        }
        if (expandNode == undefined || expandNode === "") {
            expandNode = false;
        }
        //节点字符过长，截取字符长度
        var strLengSub;
        if (nodecheck) {
            strLengSub = addDiyDomWithCheck;
        } else {
            strLengSub = null;
        }
        topevery.ajax({
            url: url,
            data: JSON.stringify({})
        }, function (data) {
            var setting = {
                showLine: true,
                checkable: true,
                sonSign: true,
                isSimpleData: true,
                enable: true,
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: 0
                },
                callback: {
                    beforeClick: function (event, treeId, treeNode) {
                        if (callback != undefined) {
                            callback(treeId);
                        }
                    },
                    onClick: onClick,
                    onCheck: checkCallback
                },
                check: {
                    enable: nodecheck,
                    chkboxType: { "Y": "s", "N": "s" }
                },
                view: {
                    selectedMulti: false,
                    txtSelectedEnable: true,
                    showLine: true,
                    addDiyDom: function () {
                        try {
                            addDiyDomWithCheck();
                        } catch (e) {
                        }
                    }
                }

            };

            $.fn.zTree.init($("#" + btnid), setting, data);
            var treeObj = $.fn.zTree.getZTreeObj("" + btnid + "");
            if (nodecheck) {
                var nodes = treeObj.getNodesByParam("type", "6", null);
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].icon = "../Img/2.jpg";
                }
            }

            if (expandNode) {
                treeObj.expandAll(true);
            } else {
                topevery.showztreemenuNum(treeObj, true, "", l);
            }

            var nodeList = treeObj.getNodesByParam("type", "1", null);
            if (nodeList) {
                for (var i = 0; i < nodeList.length; i++) {
                    treeObj.expandNode(nodeList[i], false, false, true);
                }
            }
            if (callback1 != undefined && callback1 !== "") {
                callback1();
            }
            setTimeout(function () { $("#" + btnid).show() }, 200);
        });
    },
    showztreemenuNum: function (zTreeObj, b, childnodes, l) {
        if (b) {
            var rootnodes = zTreeObj.getNodes();
            topevery.showztreemenuNum(zTreeObj, false, rootnodes, l);//递归  
        } else {
            var len = -1;
            if (childnodes && (len = childnodes.length) && len > 0) {
                if (l < childnodes[0].level) {
                    return;
                }
                for (var i = 0; i < len; i++) {
                    zTreeObj.expandNode(childnodes[i], true, false, false, true);
                    var child = childnodes[i].children;
                    topevery.showztreemenuNum(zTreeObj, false, child, l);//递归  
                }
            }
        }
    },

    BindReport: function (xdata, apiurl, yname, event, lendata) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(event);
        // 指定图表的配置项和数据
        //myChart.hideLoading();
        var seriesData = [];
        var datas = [];
        var index = 0;
        topevery.ajax({
            type: "GET",
            url: apiurl,
            data: JSON.stringify({})
        }, function (data) {
            $.each(data.Result, function (i, item) {
                datas = [];
                for (var i = 0; i < item.RegiesterCount.split(',').length; i++) {
                    datas.push(item.RegiesterCount.split(',')[i])
                }
                if (index == 0) { yname = item.RegisYear + yname; }
                index++;
                seriesData.push({
                    name: item.RegiesterType,
                    type: 'bar',
                    data: datas,
                    label: {
                        normal: {
                            show: true,            //显示数字
                            position: 'top'        //这里可以自己选择位置
                        }
                    }
                });
            });
        });
        // 使用刚指定的配置项和数据显示图表。
        setTimeout(function () {
            var option = {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'cross',
                        crossStyle: {
                            color: '#999'
                        }
                    }
                },
                toolbox: {
                    feature: {
                        dataView: { show: true, readOnly: false },
                        magicType: { show: true, type: ['line', 'bar'] },
                        restore: { show: true },
                        saveAsImage: { show: true, title: "下载" }
                    }
                },
                legend: {
                    data: lendata
                },
                xAxis: [
                    {
                        type: 'category',
                        data: xdata,
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: yname,//'注册数',
                        axisLabel: {
                            formatter: '{value}'
                        }
                    }
                ],
                series: seriesData
            };
            myChart.setOption(option)
        }, 200);
    },
    getQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var reg_rewrite = new RegExp("(^|/)" + name + "/([^/]*)(/|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        var q = window.location.pathname.substr(1).match(reg_rewrite);
        if (r != null) {
            return unescape(r[2]);
        } else if (q != null) {
            return unescape(q[2]);
        } else {
            return null;
        }
    },
    urlParam: function (paramName) {
        var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
        return sValue ? sValue[1] : sValue;
        //},

        ///**
        // * 获取下拉列表
        // * @param {} btnid  绑定Id
        // * @param {} url  url
        // * @param {} defaultText  默认值
        // * @param {} callback 回调函数
        // * @returns {} 
        // */
        //GetSelectHtml: function (url, defaultText) {
        //    topevery.ajax({
        //        url: url,
        //        data: JSON.stringify({})
        //    }, function (data) {
        //        var html = "<option value=''>" + defaultText + "</option>";
        //        for (var i = 0; i < data.length; i++) {
        //            html += "<option value=" + data[i].Key + ">" + data[i].Value + "</option>";
        //        }
        //        return html;
        //    });
        //}
    }
}

//StringBuilder
function StringBuilder(str) {
    //字符串数组  
    this.arrstr = (str === undefined ? new Array() : new Array(str.toString()));

    //字符串长度（任何影响newstr字符串长度的方法都必须同时重置length）  
    this.length = (str === undefined ? 0 : str.length);

    // 字符串模板转换，类似于C#中的StringBuilder.Append方法  
    this.append = StringBuilder_append;

    // 字符串模板转换，类似于C#中的StringBuilder.AppendFormat方法  
    this.appendFormat = StringBuilder_appendFormat;

    //重写toString()方法  
    this.toString = StringBuilder_toString;

    //重写replace()方法  
    this.replace = StringBuilder_replace;

    //添加remove()方法  
    this.remove = StringBuilder_remove;

    //从当前实例中移除所有字符  
    this.clear = StringBuilder_clear;
}

// 字符串模板转换，类似于C#中的StringBuilder.Append方法  
function StringBuilder_append(f) {
    if (f === undefined || f === null) {
        return this;
    }

    this.arrstr.push(f);
    this.length += f.length;
    return this;
}

// 字符串模板转换，类似于C#中的StringBuilder.AppendFormat方法  
function StringBuilder_appendFormat(f) {
    if (f === undefined || f === null) {
        return this;
    }

    //存储参数，避免replace回调函数中无法获取调用该方法传过来的参数  
    var params = arguments;

    var newstr = f.toString().replace(/\{(\d+)\}/g,
        function (i, h) { return params[parseInt(h, 10) + 1]; });
    this.arrstr.push(newstr);
    this.length += newstr.length;
    return this;
}

//重写toString()方法  
function StringBuilder_toString() {
    return this.arrstr.join('');
}

//重新replace()方法  
function StringBuilder_replace() {
    if (arguments.length >= 1) {
        var newstr = this.arrstr.join('').replace(arguments[0], arguments[1]);
        this.arrstr.length = 0;
        this.length = newstr.length;
        this.arrstr.push(newstr);
    }
    return this;
}

//添加remove()方法  
function StringBuilder_remove() {
    if (arguments.length > 0) {
        var oldstr = this.arrstr.join('');
        var substr = (arguments.length >= 2 ?
            oldstr.substring(arguments[0], arguments[1]) : oldstr.substring(arguments[0]));
        var newstr = oldstr.replace(substr, "");
        this.arrstr.length = 0;
        this.length = newstr.length;
        this.arrstr.push(newstr);
    }
    return this;
}

//从当前实例中移除所有字符  
function StringBuilder_clear() {
    this.arrstr.length = 0;
    this.length = 0;
    return this;
}

//获取 URL参数
function GetUrlParam(paramName) {
    var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
    return sValue ? sValue[1] : sValue;
}

function AddToHistory(url) {
    if (!!(window.history && history.pushState)) {
        // 支持History API
        history.pushState(null,null,url);
    } else {

        // 不支持

    }
}
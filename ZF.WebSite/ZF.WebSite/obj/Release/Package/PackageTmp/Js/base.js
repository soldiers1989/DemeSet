var topevery = {
    webapipath: "http://localhost/ZF.API/",

    domain: "www.test.com",
    /**
     * 
     * @param {} args ajax参数
     * @param {} callback 回调函数
     * @param {} isShowLoading 是否需要加载动态图片
     * @returns {} 
     */
    ajax: function (args, callback) {
        //采用jquery easyui loading css效果
        args.url = topevery.webapipath + args.url;
        args = $.extend({}, {
            type: "POST", dataType: "json", contentType: "application/json",
            beforeSend: function (XHR) {
                //发送ajax请求之前向http的head里面加入验证信息
                var tick = $.cookie("userToken") == undefined ? "" : $.cookie("userToken");
                XHR.setRequestHeader('Authorization', 'BasicAuth ' + tick);
            },
        }, args);
        $.ajax(args).done(function (data) {
            if (callback) {
                callback(data);
            }
        }).fail(function (data) {
            if (data.status === 401) {
                parent.layer.open({
                    type: 2,
                    title: '用户登录',
                    shadeClose: true,
                    maxmin: false, //开启最大化最小化按钮
                    area: ['650px', '430px'],
                    shade: [0.7, '#BEBEBE'], //0.7透明度的白色
                    content: '/Login/UserLogin?RefUrl=' + parent.location.href,
                    end: function () {
                        //location.reload();
                    }
                });
                //location.href = "/Login/Index?RefUrl=" + location.href;
            }
                //else if (data.status === 500) {
                //        location.href = "/Home/Index";
                //}
            else {
                try {
                    layer.alert(data.responseJSON.Result.MessageDetail);
                } catch (e) {
                    layer.alert(data.responseJSON.Message);
                }
            }
        });
    },
    messageShow: function (showtype, message) {
        new $.zui.Messager(message, {
            type: showtype,
            placement: 'center' // 定义显示位置
        }).show();
    },

    Trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
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
    * @saveDay      cookie保存天数
    * @path         cookie可用的页面
    * @cookieKey    cookie键
    * @cookieVal    cookie值 
    */
    SetCookie: function (cname, cvalue, exdays) {
        if (!exdays)
            exdays = 1;
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
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
    DelCookie: function (name) {
        topevery.SetCookie(name, "", -1);
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
    cookieHelper: function (name, value, options) {
        if (typeof value != 'undefined') { // name and value given, set cookie
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            var path = options.path ? '; path=' + options.path : '';
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        } else { // only name given, get cookie
            var cookieValue = null;
            if (document.cookie && document.cookie != '') {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var cookie = jQuery.trim(cookies[i]);
                    // Does this cookie string begin with the name we want?
                    if (cookie.substring(0, name.length + 1) == (name + '=')) {
                        cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                        break;
                    }
                }
            }
            return cookieValue;
        }
    },
    ajaxToThis: function (args, callback, isShowLoading) {
        //采用jquery easyui loading css效果

        if (isShowLoading) {
            topevery.ajaxLoading();
        }
        args.url = args.url;
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
                offset: 'rb',
                time: 3000, //10秒后自动关闭
                anim: 2
            });
        });
    },
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
    getQueryStringHtlm: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var reg_rewrite = new RegExp("(^|/)" + name + "/([^/]*)(/|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        var q = window.location.pathname.substr(1).match(reg_rewrite);
        if (r != null) {
            return r[2];
        } else if (q != null) {
            return q[2];
        } else {
            return null;
        }
    },
    myopen: function (url) {
        window.open(url, 'XX', ' left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars=yes,status=no,titlebar=no,resizable=no,toolbar=no,location=no,menubar=no,fullscreen=yes');
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
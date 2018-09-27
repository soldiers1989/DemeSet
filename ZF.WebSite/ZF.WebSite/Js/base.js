var topevery = {
    preOnlineDate: '2018年7月31日',
    onlineDate: '2018年8月31日',
    index: 0,
    wait: 60,
    IsValueAddedWebApp: false,
    webapipath: "http://localhost/ZF.API/",
    wxPcLoginUrl: function () {
        return "https://open.weixin.qq.com/connect/qrconnect?appid=wx9727a70792e03a0d&redirect_uri=http%3A%2F%2F205c8u6006.imwork.net%2FHome%2FIndex&response_type=code&scope=snsapi_login&state=" + topevery.guid() + "#wechat_redirect";
    },
    bindWikiUrl: function () {
        return "https://open.weixin.qq.com/connect/qrconnect?appid=wx9727a70792e03a0d&redirect_uri=http%3A%2F%2F205c8u6006.imwork.net%2FHome%2FIndex&response_type=code&scope=snsapi_login&state=bindwiki#wechat_redirect";
    },
    wxPcLogin: function () {
        parent.layer.open({
            type: 2,
            title: '微信登录',
            shadeClose: true,
            shade: [0.7, '#000'], //0.7透明度的白色
            maxmin: true, //开启最大化最小化按钮
            area: ['600px', '540px'],
            content: topevery.wxPcLoginUrl(),
            btn: ['手机登录'],
            skin: 'demo-class',
            yes: function (index, layero) {
                setTimeout(function () {
                    parent.layer.open({
                        type: 2,
                        title: '手机登录',
                        shadeClose: true,
                        maxmin: false, //开启最大化最小化按钮
                        area: ['500px', '430px'],
                        shade: [0.7, '#000'], //0.7透明度的白色
                        content: '/Login/UserLogin?RefUrl=' + location.href,
                        end: function () {
                        }
                    });
                }, 200);
                layer.close(index);
                parent.layer.close(index);
            }
        });
    },
    bindWiki: function () {
        parent.layer.open({
            type: 2,
            title: '扫码绑定微信,如跳过绑定则与公众平台数据不能同步',
            shadeClose: true,
            shade: [0.7, '#000'], //0.7透明度的白色
            maxmin: true, //开启最大化最小化按钮
            area: ['600px', '540px'],
            content: topevery.bindWikiUrl(),
            skin: 'demo-class',
            end: function () {
                parent.location.href = "/Home/Index";
            }
        });
    },
    delHtmlTag: function (str) {
        return str.replace(/<[^>]+>/g, "");//去掉所有的html标记  
    },
    timeCode: function (event) {
        if (topevery.wait == 0) {
            $(event).removeAttr("disabled");
            $(event).css({ "background-color": "red" });
            $(event).val("重新获取校验码");
            topevery.wait = 30;
        } else {
            $(event).attr("disabled", "disabled");
            $(event).css({ "background-color": "black" });
            topevery.wait--;
            $(event).val(topevery.wait + "秒后重新获取");
            setTimeout(function () { topevery.timeCode(event) }, 1000);
        }
    },
    openClick: function (href, type) {
        if (type) {
            var a = $("<a href='" + href + "' target='_blank'></a>").get(0);
            var e = document.createEvent('MouseEvents');
            e.initEvent('click', true, true);
            a.dispatchEvent(e);
        } else {
            parent.location.href = href;
        }
    },
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
        topevery.ajaxLoading();
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
                topevery.ajaxLoadEnd();
                callback(data);
            }
        }).fail(function (data) {
            topevery.ajaxLoadEnd();
            if (data.status === 401) {
                if (!$.cookie("userToken")) {
                    topevery.wxPcLogin();
                } else {
                    layer.confirm('此帐号已在别处登录，被迫下线？', {
                        btn: ['确定'] //按钮
                    }, function () {
                        topevery.DelCookie("userToken");
                        location.href = '/Home/Index';
                    });
                }
            } else {
                try {
                    layer.alert(data.responseJSON.Result.MessageDetail);
                } catch (e) {
                    layer.alert(data.responseJSON.Message);
                }
            }
        });
    },

    toForm: function (url, args) {
        var form = $("<form method='post' target='_blank'></form>"),
            input;
        $(document.body).append(form);
        //document.body.appendChild(form);
        form.attr({ "action": url });
        $.each(args, function (key, value) {
            input = $("<input type='hidden'>");
            input.attr({ "name": key });
            input.val(value);
            form.append(input);
        });
        form.submit();
    },
    /**
     * 
     * @param {} args ajax参数
     * @param {} callback 回调函数
     * @param {} isShowLoading 是否需要加载动态图片
     * @returns {} 
     */
    ajaxwx: function (args, callback) {
        topevery.ajaxLoading();
        //采用jquery easyui loading css效果
        topevery.ajaxLoading();
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
                topevery.ajaxLoadEnd();
                callback(data);
                topevery.ajaxLoadEnd();
            }
        }).fail(function (data) {
            topevery.ajaxLoadEnd();
            if (data.status === 401) {
                //topevery.SetCookie("userToken", "");
                //location.href = "/jjs/Home/Index";
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
    guid: function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
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

        args.url = args.url;
        args = $.extend({}, { type: "POST", dataType: "json", contentType: "application/json" }, args);
        $.ajax(args).done(function (data) {

            if (callback) {
                callback(data);
            }
        }).fail(function (data) {

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
        topevery.index = parent.layer.load(1);
    },
    /**
     * 遮罩层关闭
     * @returns {}   
     */
    ajaxLoadEnd: function () {
        parent.layer.close(topevery.index);
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
    },
    getUrlParam: function (paramName) {//获取 URL参数
        var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
        return sValue ? sValue[1] : sValue;
    },
    shoping: function (options) {
        var defaults = {
            endElement: ".gwc_sp",
            iconCSS: "",
            iconImg: "/Images/cart.png",
            endFunction: function (element) {
                return false;
            }
        };
        var self = options.that,
            $options = $.extend(defaults, options);
        if ($options.endElement == "" || $options.endElement == null) throw new Error("结束节点为必填字段");
        var $endElement = parent.$($options.endElement);
        var $target = $(self),
            x = $target.offset().left + 30,
            y = $target.offset().top + 10,
            X = $endElement.offset().left - 20,
            Y = $endElement.offset().top;
        if (!($(document).find("#cartIcon").length > 0)) {
            $('body').append(topevery.addIcon($options));
            var $obj = $('#cartIcon');
            if (!$obj.is(':animated')) {
                $obj.css({ 'left': x, 'top': y }).animate({ 'left': X, 'top': Y + 70 }, 500, function () {
                    $obj.stop(false, false).animate({ 'top': Y - 20, 'opacity': 0 }, 500, function () {
                        $obj.fadeOut(300, function () {
                            $obj.remove();
                            $target.data('click', false);
                            $options.endFunction($(this));
                        });
                    });
                });
            };
        }

    },
    addIcon: function ($options) {
        if ($options.iconImg == "" || $options.iconImg == null) {
            throw new Error("样式图片必须填上");
        }
        var icon = '<div id="cartIcon" style="width:30px;height:30px;padding:2px; border:solid 0px #e54144;overflow:hidden;position:absolute;z-index:99999999999;' + $options.iconCSS + '"><img src="' + $options.iconImg + '" width="30" height="30" /></div>';
        return icon;
    },
    verifyPhone: function (phone) {
        var pattern_chin = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(15[0-3]{1})|(15[5-9]{1})|(18[0-9]{1}))+\d{8})$/;
        var matchResult = pattern_chin.test(phone);
        if (!matchResult) {
            return false;
        } else {
            return true;
        }
    },
    verifyEmail: function (email) {
        var pattern_chin = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$/;
        var matchResult = pattern_chin.test(email);
        if (!matchResult) {
            return false;
        } else {
            return true;
        }
    }
    ,
    verifyPwdLength: function (pwd) {
        var pattern_chin = /^(?![^a-zA-Z]+$)(?!\D+$).{8,16}$/;
        var matchResult = pattern_chin.test(pwd);
        if (!matchResult) {
            return false;
        } else {
            return true;
        }
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

function formatterTasteLongTime(longtime) {
    var value = parseInt(longtime);
    if (value > 0 && value < 1 * 60 * 60) {
        var minute = Math.floor(value / 60);
        var second = value % 60;
        if (minute > 0) {
            if (second > 0) {
                return minute + "分" + second + "秒";
            } else {
                return minute + "分钟"
            }
        } else {
            if (second > 0) {
                return second + "秒";
            }
            return 0;
        }
    }
    return 0;
}
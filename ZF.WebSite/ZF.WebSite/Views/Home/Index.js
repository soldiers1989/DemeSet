if (topevery.getQueryString("PromotionCode")) {
    topevery.SetCookie("PromotionCode", topevery.getQueryString("PromotionCode"));
}

$(function () {
    var parament = $("#hiparament").val();
    if (parament) {
        var bindwiki = $("#bindwiki").val();
        var tourl = bindwiki == "bindwiki" ? "api/Register/BindWiki" : "api/Account/WikiLogin";
        var promotionCode = topevery.GetCookie("PromotionCode");
        var obj = JSON.parse(parament);
        obj.Code = promotionCode;
        obj.regiestertype = "Web";
        topevery.ajax({
            url: tourl,
            data: JSON.stringify(obj),
            async: false
        }, function (data) {
            if (data.Success) {
                var info = data.Result;
                if (!info.Success) {
                    if (bindwiki == "bindwiki") {
                        parent.layer.msg(info.Message)
                    } else {
                        $("#divmessage").html(info.Message);
                    }
                } else {
                    $(".layui-layer-close1").click()
                    topevery.SetCookie("userToken", info.Ticket);
                    var obj = info.data.split("@");
                    if (obj.length > 1) {
                        if (obj[0] && obj[1]) {
                            topevery.SetCookie("SubjectId", obj[1]);
                            topevery.SetCookie("subjectName", obj[0]);
                        }
                    }
                    parent.setUserInfo();
                    //判断在登录之前是否有加入购物车的操作
                    var cookieCart = topevery.GetCookie("cartInfoList");
                    if (cookieCart) {
                        //有加入购物车的情况下写入购物车
                        var obj = new Object()
                        obj.CourseId = cookieCart;
                        if (topevery.GetCookie("userToken")) {
                            topevery.ajax({
                                url: 'api/Cart/CartAdd',
                                data: JSON.stringify(obj),
                                async: false
                            }, function (data) {
                                if (data.Success) {
                                    if (data.Result.Success) {
                                        topevery.DelCookie("cartInfoList"); // 删除 cookie
                                        parent.setNum();
                                        //购物车加入成功后跳转支付页面
                                        var isToOrder = topevery.GetCookie("cartToSubmitOrder");
                                        if (isToOrder) {
                                            var obj = JSON.parse(isToOrder);
                                            if (data.Result.Message) {
                                                obj.Id = data.Result.Message;
                                                obj.cookieSubmit = "yes";
                                                parent.window.location.href = "/Cart/IframSubmitOrder?parment=" + encodeURIComponent(JSON.stringify(obj));
                                                //$.StandardPost('/Cart/IframSubmitOrder', obj);
                                            }
                                            //window.location.href = "/Cart/IframSubmitOrder?parment=" + obj;
                                        }
                                    }
                                } else {

                                }
                            });
                        }
                    }
                }
            }
        })
    }

})

$.extend({
    StandardPost: function (url, args) {
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
    }
});

topevery.ajax({
    url: "api/BaseDanye/GetSlideSettingList",
    data: JSON.stringify({}),
    async: false
}, function (data) {
    if (data.Success) {
        $("#fsD1").html(template("pic_html", data));
        Qfast.add('widgets', { path: "../../Js/terminator2.2.min.js", type: "js", requires: ['fx'] });
        Qfast(false, 'widgets', function () {
            K.tabs({
                id: 'fsD1',   //焦点图包裹id
                conId: "D1pic1",  //** 大图域包裹id
                tabId: "D1fBt",
                tabTn: "a",
                conCn: '.fcon', //** 大图域配置class
                auto: 1,   //自动播放 1或0
                effect: 'fade',   //效果配置
                eType: 'click', //** 鼠标事件
                pageBt: true,//是否有按钮切换页码
                bns: ['.prev', '.next'],//** 前后按钮配置class
                interval: 5000  //** 停顿时间
            })
        })
    }
});


GetConfigInfo();
//加载课程信息
function GetConfigInfo() {
    $.getJSON("/HomeConfig.json", function (data) {
        //考试指南
        $('#examGuide').html(template('examGuide_html', data.ExamGuide));
        //友情链接
        $('#relatedLink').html(template('relatedLink_html', data));
        //学员心得
        $('#studentResult').html(template('studentResult_html', data));
        ////初级课程
        //$('#cjCourse').html(template('cjCourse_html', data.CourseSetCJ));
        //console.log(template('cjCourse_html', data.CourseSetCJ));
        ////中级课程
        //$('#zjCourse').html(template('zjCourse_html', data.CourseSetZJ));
        $('.kc_tab').html(template('course_html', data));

        $('input:checkbox').click(function () {
            if ($(this).is(':checked')) {
                var that = this;
                var url = $(this).attr("url");
                var courseType = 0;
                try {
                    courseType = url.match(new RegExp("[\?\&]" + "CourseType" + "=([^\&]*)(\&?)", "i"))[1];
                } catch (e) {
                    courseType = 1;
                }
                var courseId = url.match(new RegExp("[\?\&]" + "CourseId" + "=([^\&]*)(\&?)", "i"))[1];
                if (topevery.GetCookie("userToken")) {
                    topevery.ajax({
                        url: 'api/Cart/Add',
                        data: JSON.stringify({
                            CourseType: courseType,
                            CourseId: courseId
                        })
                    }, function (data) {
                        if (data.Success) {
                            if (data.Result.Success) {
                                topevery.shoping({ that: that });
                                parent.setNum();
                                parent.layer.msg(data.Result.Message);
                            } else {
                                parent.layer.msg(data.Result.Message);
                            }
                        }
                    });
                } else {
                    //没登录的情况下写入cookie
                    var cookieCart = topevery.GetCookie("cartInfoList");
                    var cartInfoList = courseId + "@" + courseType;
                    if (cookieCart) {
                        var cartSplit = cookieCart.split(",");
                        if ($.inArray(cartInfoList, cartSplit) > -1) {
                            parent.layer.msg("该课程已加入购物车!");
                            return;
                        } else {
                            topevery.DelCookie("cartInfoList"); // 删除 cookie
                            cartInfoList = cookieCart + "," + cartInfoList;
                        }
                    }
                    topevery.SetCookie("cartInfoList", cartInfoList);
                    topevery.shoping({ that: that });
                    parent.setNum();
                    parent.layer.msg("加入成功!");
                }
            }
        });
    })
}

var Rows = 9;
AfficheHelp();
//资讯
function AfficheHelp() {
    //绑定资讯
    topevery.ajax({
        url: "api/BaseDanye/GetAfficheHelp",
        data: JSON.stringify({ IsIndex: 0, Type: 0, Page: 1, Rows: Rows })
    }, function (data) {
        if (data.Success) {
            var Result = data.Result;
            for (var i = 0; i < Result.Rows.length; i++) {
                Result.Rows[i].Content = topevery.delHtmlTag(Result.Rows[i].Content);
                Result.Rows[i].AddTime = topevery.dataTimeView(Result.Rows[i].AddTime);
            }
            $("#Affiche").html(template("Affiche_html", Result));
        }
    });
}



//名师团队
GetTeacherList();
function GetTeacherList() {
    topevery.ajax({
        url: 'api/CourseOnTeacher/GetList',
        data: JSON.stringify({ Page: 1, Rows: 50, IsFamous: 1 })
    }, function (data) {
        if (data.Success) {
            var Result = data.Result;
            for (var i = 0; i < Result.Rows.length; i++) {
                Result.Rows[i].Synopsis = topevery.delHtmlTag(Result.Rows[i].Synopsis);
            }
            $('#scrollDiv').html(template('teacherinfo_html', data.Result));
            $("#scrollDiv").Scroll({ line: 1, speed: 500, timer: 4000, up: "but_up", down: "but_down" });
            jQuery(".slideTxtBox").slide();
        }
    })
}

template.helper('formatterDeteline', function (date) {
    var stime = Date.parse(new Date());
    var etime = Date.parse(new Date(date));
    if (etime >= stime) {
        var usedTime = etime - stime;  //两个时间戳相差的毫秒数  
        var days = Math.floor(usedTime / (24 * 3600 * 1000));
        return days;
    } else {
        return 0;
    }


})

//教材
GetBooks();
function GetBooks() {
    topevery.ajax({
        url: 'api/CourseInfo/GetBooks',
        data: JSON.stringify({ Page: 1, Rows: 5 })
    }, function (data) {
        if (data.Result) {
            $('#booklist').html(template('books_html', data.Result));
        }
    })
}


//页面初始化
var hiTotalPrice = 0;
$(function () {
    dataInfo();


    //全选事件
    $("#checkall").on("click", function () {
        var ischecked = false;

        if ($(this).prop("checked")) {
            ischecked = true;
        } else {
            ischecked = false;
        }

        $("#datas").find("div[class='gwc_form']").each(function () {
            $(this).find("div[class='gwc_f_l']>input[type='checkbox']").prop("checked", ischecked);
        });

        $(".gwc_djzf").find("ul>li").each(function () {
            $(this).find("input[type='checkbox']").prop("checked", ischecked);
        });

        //重新计算价格
        CalculationPrice();
    })

})
//提交订单事件
function toPay() {
    var parment = new Object();
    var idList = "";
    var infoList = "";
    var rowcount = 0;
    $("#datas").find("div[class='gwc_form']").each(function () {
        if ($(this).find("div[class='gwc_f_l']>input[type='checkbox']").prop("checked")) {
            var num = parseInt($(this).find("div[class='gwc_new_r']").find("dl>dd").find("div>input").val());
            rowcount++;
            if (idList == "") {
                idList = "'" + $(this).attr("name") + "'";
                infoList = $(this).attr("name") + "+" + num;
            } else {
                idList += ",'" + $(this).attr("name") + "'";
                infoList += "," + $(this).attr("name") + "+" + num;
            }
        }
    });
    var cartcount = parseInt($(".CartCount").html()) - rowcount;
    parment.Id = idList;
    //订单金额
    parment.OrderAmount = hiTotalPrice;
    //下单ip
    parment.OrderIp = $("#userIp").val();
    //下单终端
    parment.OrderType = "0";
    parment.CarDetailIdList = infoList;
    parment.CartCount = cartcount;
    //学习卡数组
    var discourseCard = "";
    var arrUnique = selectedCard.unique();
    for (var i = 0; i < arrUnique.length; i++) {
        discourseCard += arrUnique[i] + ",";
    }
    setTimeout(function () { parent.window.close(); }, 200);
    parment.DiscountCard = discourseCard.trimRight(',');


    //写入订单
    topevery.ajaxwx({
        url: "api/Sheet/SheetAddToListByWiki",
        data: JSON.stringify(parment)
    }, function (data) {
        if (data.Success) {
            var amount, orderno, emailnotes, handeout, deliveryaddressid;
            $.each(data.Result, function (i, item) {
                orderno = item.OrderNo;
            });
            parment = new Object();
            parment.OrderNo = orderno;
            parment.openid = $("#openid").val();
            parment.OrderAmount = hiTotalPrice;//hiTotalPrice;
            $.StandardPost('MyPay', parment);
        }
    });
}

$.extend({
    StandardPost: function (url, args) {
        var form = $("<form method='post'></form>"),
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

//页面初始化
function dataInfo() {
    topevery.ajaxwx({
        url: "api/Cart/CartInfoList"
    }, function (data) {
        if (data.Success) {
            if (data.Result.length > 0) {
                arr = [];
                cardList = "";
                var subHtml = new StringBuilder();
                $.each(data.Result, function (i, item) {
                    subHtml.appendFormat(" <div class=\"gwc_form\" name=\"{0}\">", item.Id);
                    subHtml.append(" <div class=\"gwc_f_l\">");
                    subHtml.appendFormat(" <input type=\"checkbox\" name=\"One{0}\" checked='checked' id=\"One{0}\" onclick=\"thisclick(this)\" lay-skin=\"primary\" lay-filter=\"sub\">", item.Id);
                    subHtml.append(" </div>");
                    subHtml.append(" <div class=\"gwc_fo_new\"> ");
                    subHtml.append(" <div class=\"gwc_new_img\"> ");
                    subHtml.appendFormat("  <img src=\"{0}\" />", item.CourseIamge);
                    subHtml.append(" </div>");
                    subHtml.append(" <div class=\"gwc_new_r\">");
                    subHtml.appendFormat(" <h4>{0}</h4> ", item.CourseName);
                    subHtml.append(" <dl>");
                    subHtml.appendFormat(" <dt>{0}元</dt>", item.FavourablePrice)
                    subHtml.append(" <dd><div class=\"num\"> ");
                    //subHtml.appendFormat(" <a href=\"javascript:;\" onclick='CarNumEndit(this,1)' name='{0}' >-</a> ", item.Num);
                    subHtml.appendFormat(" <input type=\"text\" class=\"layui-input\" readonly=\"readonly\" onblur=\"numKeUp(this)\" value=\"{0}\" /> ", item.Num);
                    //subHtml.appendFormat(" <a href=\"javascript:;\" onclick='CarNumEndit(this,2)'  name='{0}' >+</a>", item.Num);
                    subHtml.append(" </div> </dd> <div class=\"clear\"></div> </dl>  </div>");
                    subHtml.append(" <div class=\"clear\"></div>");
                    subHtml.append(" </div>");
                    subHtml.append(" <div class=\"clear\"></div>");
                    subHtml.append(" <div class=\"gwc_xj\">");
                    subHtml.appendFormat(" <dt>小计：<span class=\"red_gwc\" name=\"{0}\">{0}元</span></dt> ", item.Amount);
                    subHtml.appendFormat(" <dd><a href=\"javascript:;\" name=\"{0}\"  onclick=\"DelCartInfo(this)\" >删除</a></dd>", item.Id);
                    subHtml.append(" <div class=\"clear\"></div>");
                    subHtml.append(" </div>");
                    subHtml.append(" </div>");
                    GetDiscountCard(item.CourseId);
                });
                ShowDiscountCard(arr);
                $("#datas").html(subHtml.toString());
                $('.gwc_djzf').html(cardList);
                CalculationPrice();
            } else {
                var subHtml = new StringBuilder();
                subHtml.append(" <div class=\"gwc_normal\">");
                subHtml.append(" <p>购物车内没有课程</p>");
                subHtml.append(" <a href=\"../Subject/Course\" class=\"link\">去添加课程>></a>");
                subHtml.append(" </div>");
                $(".gwc_box").html(subHtml.toString());
                $("#totalPrice").text("");
                $("#checkoutToPayParent").attr({ "class": "gwc-header-navds" })
                $("#checkoutToPay").attr("onclick", "return false;");
                hiTotalPrice = 0;
            }
       
        }
    });
}

//删除购物车信息
function DelCartInfo(event) {
    layer.confirm("确认要删除吗，删除后不能恢复", {
        title: "删除确认"
    }, function (index) {
        var obj = new Object();
        obj.Ids = $(event).attr("name");
        topevery.ajaxwx({
            url: "api/Cart/DelCartDetail",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (data.Success) {
                layer.msg(info.Message, {
                    time: 1000, icon: 1
                });
                dataInfo();
                CalculationPrice();
            } else {
                layer.msg(info.Message, {
                    time: 1000, icon: 2
                });
            }
        });
    })
}


///获取可用学习卡
var cardList = "";
var arr = [];
//已选择学习卡
var selectedCard = [];
function GetDiscountCard(courseid) {
    topevery.ajaxwx({
        url: 'api/UserDiscountCard/GetUseCard?courseId=' + courseid,
        async: false
    }, function (data) {
        if (data.Success && data.Result) {
            var list = data.Result;
            for (var i = 0; i < list.length; i++) {
                arr.push(list[i])
            }
        }
    })
}
function ShowDiscountCard(arr) {
    var uniqueArr = arr.unique();
    cardList = "  <ul>";
    cardList += " <li>订单总额：<span id=\"sumPrice\">0元</span></li> ";
    cardList += " <li>活动优惠：<span id=\"preferential\">0元</span></li>";
    cardList += " <li>优惠券抵扣：<span id=\"discount\">0元</span></li>";
    for (var j = 0; j < uniqueArr.length; j++) {
        cardList += "<li><input  checked=\"checked\" lay-skin=\"primary\" lay-filter=\"sub\" type=\"checkbox\" data=\"" + uniqueArr[j].Amount + "\"\
                     onclick=\"checkCard(this)\" cardCode=\"" + uniqueArr[j].CardCode + "\">&nbsp;" + uniqueArr[j].CardName + "：\
                     <span>" + uniqueArr[j].Amount + "元</span></li>";
    }
    cardList += "  </ul>";
}

Array.prototype.unique = function () {
    var res = [];
    var json = {};
    for (var i = 0; i < this.length; i++) {
        if (typeof this[i] == 'object') {
            if (!json[JSON.stringify(this[i])]) {
                res.push(this[i]);
                json[JSON.stringify(this[i])] = 1;
            }
        } else {
            if (!json[this[i]]) {
                res.push(this[i]);
                json[this[i]] = 1;
            }
        }

    }
    return res;
}

function numKeUp(event) {
    if (!isPositiveInteger($(event).val())) {
        $(event).val("1");
    } else {
        CalculationPrice();
    }
}

function isPositiveInteger(s) {//是否为正整数
    var reg = /(^[1-9]+\d*$)|(^0$)/;
    return reg.test(s);
}

//选择优惠券事件
function checkCard(event) {
    CalculationPrice();
}

//选择商品
function thisclick(event) {
    CalculationPrice();
}


var discountNum = 0.0;
//计算金额
function CalculationPrice() {
    var priceSum = 0.0;
    var orderSum = 0.0;
    var checkedIsTrue = false;
    selectedCard = [];
    $("#datas").find("div[class='gwc_form']").each(function () {
        if ($(this).find("div[class='gwc_f_l']>input[type='checkbox']").prop("checked")) {
            var price = parseFloat($(this).find("div[class='gwc_new_r']").find("dl>dt").html()).toFixed(2);
            var num = parseInt($(this).find("div[class='gwc_new_r']").find("dl>dd").find("div>input").val());
            checkedIsTrue = true;
            priceSum = parseFloat(parseFloat(priceSum) + price * num).toFixed(2);
            orderSum = priceSum;
        }
    });
    GetBestDiscountNum(priceSum);

    //选择学习卡后计算价格
    //discount
    if (priceSum > 0) {
        var discount = 0.0;
        $(".gwc_djzf").find("ul>li").each(function () {
            if ($(this).find("input[type='checkbox']").prop("checked")) {
                selectedCard.push($(this).find("input[type='checkbox']").attr("cardCode"));
                var value = $(this).find("input[type='checkbox']").attr("data");
                discount = discount + parseFloat(value);
                priceSum = (priceSum - parseFloat(value)).toFixed(2);
                hiTotalPrice = priceSum;
            }
        })
        $("#discount").html(discount + "元");
    }
    var payPrice = 0;
    $("#sumPrice").text(orderSum + "元");
    if (checkedIsTrue) {
        payPrice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;
        $("#totalPrice").html("<span style='color: #888888;' >原价" + $("#sumPrice").text() + "</span> <span style='margin-left: 1em;'>  结算金额" + payPrice + "元 </span>");
        hiTotalPrice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;
        $("#checkoutToPayParent").attr({ "class": "gwc-header-navd" })
        $("#checkoutToPay").attr("onclick", "toPay()");
    } else {
        payPrice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;
        $("#checkoutToPayParent").attr({ "class": "gwc-header-navds" })
        $("#checkoutToPay").attr("onclick", "return false;");
        $("#totalPrice").text("");
        hiTotalPrice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;
        $("#checkall").prop("checked", false);
    }
   
}

//获取最优满减
function GetBestDiscountNum(priceSum) {
    $('#preferential').html("0元");
    discountNum = 0.0;
    topevery.ajaxwx({
        url: 'api/PurchaseDiscount/GetGetBestDiscountNum?priceSum=' + priceSum,
        async: false
    }, function (data) {
        if (data.Success && data.Result) {
            discountNum = data.Result.MinusNum;
            $('#preferential').html("(满" + data.Result.TopNum + "立减" + data.Result.MinusNum + ")&nbsp;&nbsp;" + discountNum + '元');
        }
    });
}

//购物数量发生改变事件
function CarNumEndit(event, type) {
    var Num;
    var htmlInput;
    switch (type) {
        case 1:
            htmlInput = parseInt($(event).parent().parent().find("input[type='text']").val());
            if (htmlInput <= 1) {
                $(event).parent().parent().find("input[type='text']").val(1);
            } else {
                $(event).parent().parent().find("input[type='text']").val(htmlInput - 1);
            }

            break;
        case 2:
            htmlInput = parseInt($(event).parent().parent().find("input[type='text']").val());
            $(event).parent().parent().find("input[type='text']").val(htmlInput + 1);
            break;
    }
    $("#datas").find("div[class='gwc_form']").each(function () {
        var price = parseFloat($(this).find("div[class='gwc_new_r']").find("dl>dt").html()).toFixed(2);
        var num = parseInt($(this).find("div[class='gwc_new_r']").find("dl>dd").find("div>input").val());
        var priceSum = parseFloat(price * num).toFixed(2);

        $(this).find("div[class='gwc_xj']").find("dt>span").attr("name", priceSum);
        $(this).find("div[class='gwc_xj']").find("dt>span").html((priceSum).toString() + "元");
    });
    CalculationPrice();
}
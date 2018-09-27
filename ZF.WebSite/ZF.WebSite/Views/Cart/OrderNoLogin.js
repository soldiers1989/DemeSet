//页面初始化
var hiTotalPrice = 0;
$(function () {
    dataInfo();
    //提交订单事件
    $("#checkoutToPay").on("click", function () {
        var parment = new Object();
        var idList = "";
        var infoList = "";
        var rowcount = 0;
        $("tbody").find("tr").each(function () {
            if ($(this).find("td>input[type='checkbox']").prop("checked")) {
                var num = $(this).find("td:eq(3)").find("div").find("input[type='text']").val();
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
        var cartcount = rowcount;
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
        //var discourseCard = "";
        //var arrUnique = selectedCard.unique();
        //for (var i = 0; i < arrUnique.length; i++) {
        //    discourseCard += arrUnique[i] + ",";
        //}
        //setTimeout(function () { parent.window.close(); }, 200);
        //parment.DiscountCard = discourseCard.trimRight(',');

        if (topevery.GetCookie("PromotionCode")) {
            parment.PromotionCode = topevery.GetCookie("PromotionCode");
        }

        if (topevery.GetCookie("cartToSubmitOrder")) {
            topevery.DelCookie("cartToSubmitOrder"); // 删除 cookie
        }
        topevery.SetCookie("cartToSubmitOrder", JSON.stringify(parment));
        //登录
        topevery.wxPcLogin();
    });
});

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

//页面初始化
function dataInfo() {
    var cookieCart = topevery.GetCookie("cartInfoList");
    var obj = new Object();
    obj.Ids = cookieCart;
    if (cookieCart) {
        topevery.ajax({
            url: "api/CartNoLogin/CartList",
            data:JSON.stringify(obj)
        }, function (data) {
            if (data.Success) {
                if (data.Result.length > 0) {
                    var subHtml = new StringBuilder();
                    $.each(data.Result, function (i, item) {
                        subHtml.appendFormat("<tr name=\"{0}\">", item.Id);
                        subHtml.append("<td >");
                        subHtml.appendFormat(" <input  class='regular-checkbox' type=\"checkbox\" name=\"One{0}\" id=\"One{0}\" onclick=\"thisclick(this)\" /><label style=\"margin-left:20px;margin-top: 9px;\" for=\"One{0}\"></label>", item.Id);
                        subHtml.append("</td>");
                        subHtml.append("<td>");
                        subHtml.appendFormat(" <img src=\"{0}\" width=\"40\" height=\"80\" />", item.CourseIamge);
                        if (item.CourseType === 0) {
                            subHtml.appendFormat(" <a name=\"{0}\" href=\"/CourseInfo/CourseInfo?courseId={0}\"  target=\"_blank\" >{1}</a>", item.CourseId, item.CourseName);
                        } else {
                            subHtml.appendFormat(" <a name=\"{0}\" href=\"/CourseInfo/CoursePackInfo?courseId={0}\"  target=\"_blank\" >{1}</a>", item.CourseId, item.CourseName);
                        }
                        subHtml.append("</td>");
                        subHtml.appendFormat("<td name=\"{0}\">{0}元</td>", item.FavourablePrice);
                        subHtml.appendFormat("<td name=\"{0}\"><div class='adddiv'>", item.Num);
                        subHtml.appendFormat("<input class=\"inp\" type=\"text\" readonly=\"readonly\" onblur=\"numKeUp(this)\" value=\"{0}\" />", item.Num);
                        subHtml.append("</div></td>");
                        subHtml.appendFormat("<td name=\"{0}\" style=\"color:red;\" >{0}元</td>", item.Amount);
                        subHtml.appendFormat(" <td><img src=\"../Images/cancel.png\" name=\"{0}\" width=\"20\" height=\"20\" onclick=\"DelCartInfo(this)\" /></td>", item.Id);
                        subHtml.append("</tr>");
                    });
                    $("#datas").html(subHtml.toString());
                    $("#eInvoiceTip").html("共<font class=\"red\">" + data.Result.length + "</font>件商品");
                    $("#fHtml").show();
                } else {
                    topevery.DelCookie("cartInfoList"); // 删除 cookie
                    $("#fHtml").show();
                    $("#fHtml").html('<div class="normal"><p>购物车内没有课程</p><a onclick=\"goHome();\"" class="link">去添加课程&gt;&gt;</a></div>');
                    $("#eInvoiceTip").text("共0件商品");
                }
            }
        });
    } else {
        $("#fHtml").show();
        $("#fHtml").html('<div class="normal"><p>购物车内没有课程</p><a onclick=\"goHome();\"" class="link">去添加课程&gt;&gt;</a></div>');
        $("#eInvoiceTip").text("共0件商品");
    }
}


function goHome() {
    parent.location.href = "/Home/Index";
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

function CarNumEndit(event, type) {
    var Num;
    var htmlInput;
    switch (type) {
        case 1:
            htmlInput = parseInt($(event).parent().parent().find("input[type='text']").val());
            if (htmlInput <= 1) {
                $(event).parent().parent().find("input[type='text']").val(1);
                parent.parent.setNumBydel(1, 3);
            } else {
                htmlInput = htmlInput - 1;
                $(event).parent().parent().find("input[type='text']").val(htmlInput);
            }

            break;
        case 2:
            htmlInput = parseInt($(event).parent().parent().find("input[type='text']").val());
            htmlInput = htmlInput + 1;
            $(event).parent().parent().find("input[type='text']").val(htmlInput);
            break;
    }
    $("tbody").find("tr").each(function () {
        var price = parseFloat($(this).find("td:eq(2)").attr("name")).toFixed(2);
        var num = parseInt($(this).find("td:eq(3)").find("div").find("input[type='text']").val());
        var priceSum = parseFloat(price * num).toFixed(2);
        $(this).find("td:eq(4)").attr("name", priceSum);
        $(this).find("td:eq(4)").html((priceSum).toString() + "元");
    });
    CalculationPrice();
}

function checkClick(event) {
    if ($(event).find("input[type='checkbox']").prop("checked")) {
        $(event).find("input[type='checkbox']").prop("checked", false);
    } else {
        $(event).find("input[type='checkbox']").prop("checked", true);
    }
    CalculationPrice();
}

function thisclick(event) {
    CalculationPrice();
}
//全选
function checkAll(event) {
    if ($(event).prop("checked")) {
        $("tbody").find("tr").each(function () {
            $(this).find("td>input[type='checkbox']").prop("checked", true);
        })
    } else {
        $("tbody").find("tr").each(function () {
            $(this).find("td>input[type='checkbox']").prop("checked", false);
        })
    }
    CalculationPrice();
}

var discountNum = 0.0;
//计算金额
function CalculationPrice() {
    var priceSum = 0.0;
    var orderSum = 0.0;
    var checkedIsTrue = false;
    selectedCard = [];
    $("tbody").find("tr").each(function () {
        if ($(this).find("td>input[type='checkbox']").prop("checked")) {
            var price = parseFloat($(this).find("td:eq(2)").attr("name")).toFixed(2);
            var num = parseInt($(this).find("td:eq(3)").find("div").find("input[type='text']").val());
            checkedIsTrue = true;
            priceSum = parseFloat(parseFloat(priceSum) + price * num).toFixed(2);
            orderSum = priceSum;
        }
    });

    if (checkedIsTrue) {
        $("#totalPrice").text(priceSum - discountNum < 0 ? 0 : priceSum - discountNum);
        hiTotalPrice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;
        $("#checkoutToPay").removeAttr("disabled")
    } else {
        $("#checkoutToPay").attr("disabled", "disabled");
        $("#totalPrice").text(priceSum - discountNum < 0 ? 0 : priceSum - discountNum);
        hiTotalPrice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;
        $("#checkall").prop("checked", false);
    }
    $("#sumPrice").text(orderSum + "元");
}

//删除购物车信息
function DelCartInfo(event) {
    var id = $(event).attr("name");
    var cookieCart = topevery.GetCookie("cartInfoList");
    if (cookieCart) {
        var cartSplit = cookieCart.split(",");
        var newCart = [];
        for (var i = 0; i < cartSplit.length; i++) {
            var infoSplit = cartSplit[i].split("@")[0];
            if (id != infoSplit) {
                newCart.push(cartSplit[i]);
            }
        }
        topevery.DelCookie("cartInfoList"); // 删除 cookie
        if (newCart.length > 0) {
            topevery.SetCookie("cartInfoList", newCart.join(","));
        }
    }
    dataInfo();
}
parent.setIframeHeight();
$(function () {
    /**
     * 菜单页面回调
     * @param {} data 
     * @returns {} 
     */
    function loadHtml(data, dataName, modelName) {
        $(".content-wrapper").html(data);
        $($(".box-header a")[1]).html(dataName);
        $($(".box-header a")[0]).html(modelName);
    }

    $(".treeview-menu > li").click(function () {
        var dataName = $(this).find("a").attr("dataName");
        var modelName = $(this).parent().parent().attr("modelName");
        var args = $.extend({}, { type: "get", dataType: "html", contentType: "application/json", url: $(this).attr("dataUrl") });
        topevery.ajaxLoading();
        $.ajax(args).done(function (data) {
            loadHtml(data, dataName, modelName);
            topevery.ajaxLoadEnd();
        });
        //topevery.ajax({ type: "get", url: $(this).attr("dataUrl"), dataType: "html" }, loadHtml, true);
        $(this).siblings().removeClass("active");
        $(this).addClass("active");
    });
    $(".treeview a").first().click();
    $(".treeview").first().find("ul > li").first().click();
    /**
     * 延迟加载选择样式类
     */
    setTimeout(function () {
        $(".treeview").first().find("ul > li").first().addClass("active");
    }, 1000);

    $(".logout-sys").on("click", function () {
        topevery.ajax({
            url: "/Home/Logout",
        }, function (row) {
            if (row.Success) {
                topevery.msg(row.Message, 1);
                location.href = "/Login/Index";
            } else {
                topevery.msg(row.Message, 2);
            }
        });
        // self.location =  "api/Home/Logout";
    });
    $("#Modify").bindAddBtn("/Login/Modify", 500, 300, "修改密码");
});
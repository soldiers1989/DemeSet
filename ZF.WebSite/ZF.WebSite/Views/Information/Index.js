var data = {
    id: "",
    InitializeDataType: function () {
        //绑定资讯大类
        topevery.ajax({
            url: "api/BaseDanye/GetDataType?code=zxgl",
        }, function (data) {
            if (data.Success) {
                $("#GetDataType").html(template("GetDataType_html", data));
            }
        });
    },
    GetList: function (id) {
        if (id) {
            $("#PageIndex").val("1");
            $(".typeList li").removeClass("cur");
            $("." + id).parent().addClass("cur");
            data.id = id;
        }
        //绑定资讯
        topevery.ajax({
            url: "api/BaseDanye/GetAfficheHelp",
            data: JSON.stringify({ Type: 0, BigClassId: data.id, page: $('#PageIndex').val(), rows: $('#Rows').val() })
        }, function (data1) {
            if (data1.Success) {
                for (var i = 0; i < data1.Result.Rows.length; i++) {
                    data1.Result.Rows[i].Content = topevery.delHtmlTag(data1.Result.Rows[i].Content);
                    data1.Result.Rows[i].AddTime = topevery.dataTimeView(data1.Result.Rows[i].AddTime);
                }
                $("#Affiche").html(template("Affiche_html", data1.Result));
                if (data1.Result.Records > 0) {
                    $('.M-box1').show();
                    $('.M-box1').pagination({
                        pageCount: Math.ceil(data1.Result.Records / parseInt($("#Rows").val())),
                        jump: true,
                        coping: true,
                        homePage: '首页',
                        endPage: '末页',
                        prevContent: '上页',
                        nextContent: '下页',
                        callback: function (api) {
                            $("#PageIndex").val(api.getCurrent());
                            data.loading1();
                        }
                    });
                } else {
                    $('.M-box1').hide();
                }
            }
        });
    },
    loading1: function () {
        //绑定资讯
        topevery.ajax({
            url: "api/BaseDanye/GetAfficheHelp",
            data: JSON.stringify({ Type: 0, BigClassId: data.id, page: $('#PageIndex').val(), rows: $('#Rows').val() })
        }, function (data1) {
            if (data1.Success) {
                for (var i = 0; i < data1.Result.Rows.length; i++) {
                    data1.Result.Rows[i].Content = topevery.delHtmlTag(data1.Result.Rows[i].Content);
                    data1.Result.Rows[i].AddTime = topevery.dataTimeView(data1.Result.Rows[i].AddTime);
                }
                $("#Affiche").html(template("Affiche_html", data1.Result));
            }
        });
    },
};
$(function () {
    data.InitializeDataType();
    data.GetList();
});


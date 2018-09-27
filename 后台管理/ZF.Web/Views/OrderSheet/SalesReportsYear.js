$(function () {
    var nowDate = new Date();
    var year = nowDate.getFullYear();
    var dateStr = year;

    $("#EndTime").val(dateStr);

    var getPostDataUrl = "api/OrderSheet/GetSalesReportsYearList";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '年份', '笔数', '金额（元）', ''],//列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Date', index: 'Date', width: 100, align: "center" },
            { name: 'TradeCount', index: 'TradeCount', width: 100, align: "center" },
             { name: 'TotalMoney', index: 'TotalMoney', width: 100, align: "center" },
             { name: '', index: '', width: 300, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".query_table_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
        $("#table_2").hide();
        $("#table_1").show();
    });
    $(".query_report_btn").on("click", function () {
        $("#table_1").hide();
        $("#table_2").show();
        GraphicReport();
    });
    //图形报表
    function GraphicReport() {
        topevery.ajax({
            type: "Post",
            url: "api/OrderSheet/GetSalesReportsYearList",
            data: JSON.stringify(topevery.form2Json("selectFrom"))
        }, function (data) {
            if (data.Success) {
                var data1 = new Array();
                var data2 = new Array();
                var data3 = new Array();
                $.each(data.Result.Rows, function (i, item) {
                    data1[i] = item.Date;
                    data2[i] = item.TotalMoney;
                    data3[i] = item.TradeCount;
                });
                echartsFnInit1(data1, data2, data3)
            }
        });

    }
    //
    function echartsFnInit1(data, data1, data2) {
        //月份税收收入完成情况
        (function chartGszsssrqk() {
            var gpcbzj1 = echarts.init(document.getElementById('main'));
            var option = {
                title: {
                    text: '销售记录统计',
                    textStyle: {
                        fontSize: 22
                    },
                    x: 'center'
                },
                grid: {
                    bottom: 40,
                    left: 20,
                    containLabel: true,
                },
                tooltip: {
                    trigger: 'item',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                toolbox: {
                },
                legend:
                    [
                        {
                            x: 'right',
                            data: ['金额', '笔数'],
                        },
                    ],
                xAxis: [
                    {
                        type: 'category',
                        axisLabel: {
                            interval: 0,
                            margin: 10,
                        },
                        axisTick: {
                            show: false
                        },
                        data: data
                    }
                ],
                yAxis: [
                {
                    show: true,
                    type: 'value',
                    splitLine: {
                        show: true,
                    },
                    axisLine: {
                        show: true
                    },
                    axisTick: {
                        show: true
                    }
                },
                ],
                series: [
                    //国税本期
                    {
                        name: '金额',
                        type: 'line',
                        data: data1,
                        itemStyle: {
                            "normal": {
                                "barBorderRadius": 0,
                                "color": "#2bd79c",
                                "label": {
                                    "show": true,
                                    "textStyle": {
                                        "color": "rgba(0,0,0,1)"
                                    },
                                }
                            }
                        }

                    },
                    //国税同期
                    {
                        name: '笔数',
                        type: 'line',
                        data: data2,
                        itemStyle: {
                            "normal": {
                                "barBorderRadius": 0,
                                "color": "#4c79e0",
                                "label": {
                                    "show": true,
                                    "textStyle": {
                                        "color": "rgba(0,0,0,1)"
                                    },
                                }
                            }
                        }
                    },
                ]
            };
            gpcbzj1.setOption(option);
            $(window).resize(function () {
                gpcbzj1.resize();
            });
        })();
    }

    $("#myTab a").click(function () {
       var url= $(this).attr("url");
       topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) { $(".content-wrapper").html(data); }, true);
    });
    $(".import_btn").bindExportBtn("OrderSheet/ExportYear");
});
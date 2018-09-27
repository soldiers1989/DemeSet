$(function () {
    var getPostDataUrl = "api/UserLoginLog/GetList";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "LoginTime",
        colNames: ['日志编码', '昵称', '手机号', '登录时间', '登录IP', '登录方式', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
              { name: 'NickNamw', index: 'NickNamw', width: 100, align: "center" },
            { name: 'TelphoneNum', index: 'TelphoneNum', width: 100, align: "center" },
            { name: 'LoginTime', index: 'LoginTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: 'LoginIp',  width: 100, align: "center" },
            { name: 'LoginType', index: 'LoginType', width: 100, align: "center", formatter: loginTypes },
              { name: '', width: 200, align: "center" },
        ],
        multiselect: false,
        postData: topevery.form2Json("selectFrom")
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });


    var myChart1 = echarts.init(document.getElementById('SmoothLine'));

    var yearSelector = $('#YearSelector');
    var currentYear =parseInt( new Date().getFullYear());
    var selectOptions = '';
    for (var i = currentYear - 20; i < currentYear + 20; i++) {
        selectOptions+="<option value=\""+i+"\">"+i+"年</option>";
    }
    $(selectOptions).appendTo(yearSelector);
    $(yearSelector).val(currentYear);

   
    $('#myTab a').click(function (e) {
        e.preventDefault();
        QueryLineData(currentYear);
    })


    $(yearSelector).change(function () {
        var year = $(this).children('option:selected').val();
        QueryLineData(year);
    });

    //查询统计数据
    function QueryLineData(year) {
        topevery.ajax({
            url: "Common/GetStatics",
            data: JSON.stringify({ yearPart: year })
        }, function (data) {
            console.log(data);
            option1 = {
                xAxis: {
                    type: 'category',
                    data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                },
                yAxis: {
                    name: '登录次数',
                    type: 'value'
                },
                series: [{
                    data: [data.Month1, data.Month2, data.Month3, data.Month4, data.Month5, data.Month6, data.Month7, data.Month8, data.Month9, data.Month10, data.Month11, data.Month12, ],
                    type: 'line',
                    smooth: true
                }]
            };
            myChart1.setOption(option1);
            $(this).tab('show');
        })
        topevery.ajax({
            url: "Common/GetClassStatics",
            data:JSON.stringify({yearPart:year})
        },function(data){
            console.log(data);
            var myChart2 = echarts.init(document.getElementById('StackBar'));
            option2 = {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                        type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                    }
                },
                legend: {
                    data: ['PC端', '手机端']
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: [
                    {
                        type: 'category',
                        data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                    }
                ],
                yAxis: [
                    {
                        name: '登录次数',
                        type: 'value',

                    }
                ],
                series: [
                    {
                        name: 'PC端',
                        type: 'bar',
                        stack: '登录方式',
                        data: [320, 332, 301, 334, 390, 330, 320, 301, 334, 390, 330, 320]
                    },
                    {
                        name: '手机端',
                        type: 'bar',
                        stack: '登录方式',
                        data: [120, 132, 101, 134, 90, 230, 210, 101, 134, 90, 230, 210]
                    }
                ]
            };

            myChart2.setOption(option2);
        })
    }
   


});

function loginTypes(tm) {
    debugger
    if (tm === "0") {
        return "web";
    } else if (tm === "1") {
        return "App";
    }
    else if (tm === "2") {
        return "微信";
    }
    else if (tm === "3") {
        return "后台";
    }
    return "";
}
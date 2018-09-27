$(function () {
    var getPostDataUrl = "api/OrderSheet/GetSaleReportByCourse";
    var getPostDataByPageUrl = "api/OrderSheet/GetSaleReportByCourseWithPage";
    var echartShow = false;

    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataByPageUrl,
        colNames: ['课程名称', '购买人数', ''],//列头
        colModel: [
            { name: 'CourseName', index: 'CourseName', width: 100, align: "center" },
            { name: 'CourseCount', index: 'CourseCount', width: 100, align: "center" },
            { name: '', index: '', width: 300, align: "center" },
        ],
        postData: {rows:150, BeginDate: $('#BeginDate').val(), EndDate: $('#EndDate').val(), CourseType: $('#CourseType').val() }
    });

    $('#myTab a ').click(function () {
        var courseType = parseInt($(this).attr('data'));
        $('#CourseType').val(courseType);
        if (echartShow) {
            GraphicReport();
        } else {
            grid.jqGrid('setGridParam', {
                url: getPostDataByPageUrl,
                postData: { BeginDate: $('#BeginDate').val(), EndDate: $('#EndDate').val(), CourseType: courseType }
            }).trigger("reloadGrid");
            echartShow = false;
        }
    })

    $('.query_table_btn').click(function () {
        echartShow = false;
        if (parseInt($('#CourseType').val()) == 0) {
            $('#myTab a')[0].click();
        } else if (parseInt($('#CourseType').val()) == 1) {
            $('#myTab a')[1].click();
        }
        $('#table_1').attr('style', 'display:block;');
        $('#table_2').attr('style', 'display:none;');
    })

    $('.query_report_btn').click(function () {
        GraphicReport();
        $('#table_1').attr('style', 'display:none;');
        $('#table_2').attr('style', 'display:block;');
        echartShow = true;
    })


    function GraphicReport() {
        topevery.ajax({
            type: 'Post',
            url: getPostDataUrl,
            data: JSON.stringify({ BeginDate: $('#BeginDate').val(), EndDate: $('#EndDate').val(), CourseType: $('#CourseType').val() })
        }, function (data) {
            if (data.Success) {
                var data1 = new Array();
                var data2 = new Array();
                $.each(data.Result.Rows, function (i, item) {
                    data1[i] = item.CourseName;
                    data2[i] = item.CourseCount;
                });
                echartInite(data1,data2);
            }
        });
    }

    function echartInite(data1,data2){
        var obj = echarts.init(document.getElementById('main'));
        option = {
            title: {
                text: '课程销量统计',
                textStyle: {
                    fontSize: 22
                },
                x: 'center'
            },
            color: ['#3398DB'],
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                {
                    name:'课程',
                    type: 'category',
                    data: data1,
                    axisTick: {
                        alignWithLabel: true
                    },
                    axisLabel:{
                        interval:0,//横轴信息全部显示
                        formatter: function (value) {
                            return value.split("").join("\n");
                        }
                    }
                }
            ],
            yAxis: [
                {
                    name:'购买数',
                    type: 'value'
                }
            ],
            series: [
                {
                    name:'购买数',
                    type: 'bar',
                    barWidth: '40%',
                    data: data2
                }
            ]
        };
        obj.setOption(option);
        $(window).resize(function () {
            obj.resize();
        });
    }

    $(".import_btn").bindExportBtn("OrderSheet/ExportCourseSale");
});
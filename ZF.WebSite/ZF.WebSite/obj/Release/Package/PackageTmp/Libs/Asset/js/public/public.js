/* 
* @Author: anchen
* @Date:   2016-07-23 16:08:28
* @Last Modified by:   anchen
* @Last Modified time: 2016-08-02 18:27:43
*/

$(function(){
    
    var logo;/*logo链接*/
    var arr;/*导航链接*/
    var subItem;/*右侧真题链接*/
    var href=location.href;
    var current=0;
    logo="/index.html";
    arr=["/index.html","/look/Examination/ExaminationItem.html","/look/mocks/mocks.html","/look/downpage/downpage.html"];
    subItem=["/look/subject/Economist/20160723170501.html",
             "/look/subject/Economist/20160723170502.html",
             "/look/subject/Economist/20160723180603.html",
             "/look/subject/Economist/20160723193180.html",
             "/look/subject/Economist/20160723193181.html",
             "/look/subject/Economist/20160723193182.html"];


    var footer=$("<div class='main city'><p><a href='http://www.mohrss.gov.cn/' target='_blank'>人力资源和社会保障部</a><a href='http://www.cpta.com.cn/' target='_blank'>中国人事考试网</a> <a href='http://rsks.class.com.cn/' target='_blank'>中国人事考试图书网</a></p><a href='http://www.bjrbj.gov.cn/bjpta/' target='_blank'>北京</a><a href='http://www.spta.gov.cn/index.html' target='_blank'>上海</a><a href='http://www.tjkpzx.com/' target='_blank'>天津</a><a href='http://www.cqhrss.gov.cn/' target='_blank'>重庆</a><a href='http://www.hebpta.com.cn/' target='_blank'>河北</a><a href='http://www.sxpta.com/' target='_blank'>山西</a><a href='http://www.impta.com/' target='_blank'>内蒙</a><a href='http://www.lnrsks.com/' target='_blank'>辽宁</a><a href='http://www.jlzkb.com/' target='_blank'>吉林</a><a href='http://www.hljrsks.org.cn/' target='_blank'>黑龙江</a><a href='http://rsks.jshrss.gov.cn/' target='_blank'>江苏</a><a href='http://www.zjks.com/' target='_blank'>浙江</a><a href='http://www.apta.gov.cn/' target='_blank'>安徽</a><a href='http://www.fjpta.com/' target='_blank'>福建</a><a href='http://www.jxpta.com/' target='_blank'>江西</a><a href='http://www.rsks.sdhrss.gov.cn/' target='_blank'>山东</a><a href='http://www.hnrsks.com/' target='_blank'>河南</a><a href='http://www.hbsrsksy.cn/' target='_blank'>湖北</a><a href='http://www.hunanpta.com/' target='_blank'>湖南</a><a href='http://www.gdkszx.com.cn/' target='_blank'>广东</a><a href='http://www.gxpta.com.cn/' target='_blank'>广西</a><a href='http://www.hnjy.gov.cn/' target='_blank'>海南</a><a href='http://www.scpta.gov.cn/' target='_blank'>四川</a><a href='http://www.gzpta.gov.cn/' target='_blank'>贵州</a><a href='http://www.ynrsksw.cn/' target='_blank'>云南</a><a href='http://www.xz.hrss.gov.cn/' target='_blank'>西藏</a><a href='http://www.sxrsks.cn/'>陕西</a><a href='http://www.rst.gansu.gov.cn/' target='_blank'>甘肃</a><a href='http://www.qhpta.com/html/default.html' target='_blank'>青海</a><a href='http://www.nxpta.gov.cn/' target='_blank'>宁夏</a><a href='http://www.xjrsks.com.cn/' target='_blank'>新疆</a><p class='one'>技术支持：<a href='http://www.zhuofan.net' target='_blank'>深圳卓帆科技有限公司</a><span>服务热线：0755-25884776</span></p></div>");
    $(".footer").html("");
    $(".footer").append(footer);

    // $(".Nav .main").html("");
    // $(".Nav .main").html("<ul><li><a href='javascript:;'>首   页</a></li><li><a href='javascript:;'>服务资讯</a></li><li><a href='javascript:;'>模拟作答系统</a></li><li><a href='javascript:;'>下载专区</a></li></ul>");


    if(href.indexOf("index")>-1){
        current=0;
    }else if(href.indexOf("Examination")>-1 || href.indexOf("Review")>-1 || href.indexOf("subject")>-1 || href.indexOf("news")>-1 ){
        current=1;
    }else if(href.indexOf("downpage")>-1){
        current=3;
    }else if(href.indexOf("mocks")>-1){
        current=2;
    }

    $(".Nav li a").each(function(index, el) {
        console.log(arr[index]);
        $(this).attr("href",arr[index]);
    });
    $(".Nav li").click(function(event) {
            $(this).addClass('current').siblings().removeClass('current');
        });
    $("h1 a").attr("href",logo);
    $(".Nav li").eq(current).addClass('current').siblings("li").removeClass('current');
    if(!(href.indexOf("Item")>-1)){
        $(".long a").each(function(index, el) {
            console.log(subItem[index]);
            $(this).attr("href",subItem[index]);
        });
    }
    $("h1 a").text("电子化考试考生服务专区");
    $(".crumbs a").eq(0).text("电子化考试考生服务专区");
});

















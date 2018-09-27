$(function(){
    /*两栏显示*/
    $(document).on("click",".row",function(){
        if($(".main").hasClass('two')){
            $(".main").removeClass('two');
            $(this).removeClass('heng');
            $(".timu").css("height","auto");
            chooseHeight();
        }else{
            $(".main").addClass("two");
            $(this).addClass('heng');
            timu();
            
            $(window).resize(function(event) {
                 timu();
            });
        }
    });
    /*点击屏幕设置框隐藏*/
    $(document).on("click",function(){
        $(".dropContents").hide();
    });
    
});
function two(){
    if($(".skill").css("display")=="none"){
        $(".main").removeClass("two")
        $(".chooseItem").css({"width":"auto"});
        
     }
     chooseHeight();
}
function chooseHeight(){
    var heights;
        if($(".skill").css("display")=="block"){
            heights=$(window).height()-$(".header").outerHeight()-$(".btn").outerHeight()-$(".part").outerHeight()-$(".skill").outerHeight();
        }else{

            heights=$(window).height()-$(".header").outerHeight()-$(".btn").outerHeight()-$(".part").outerHeight()-10;
        }

        $(".tiComtent").css("height",heights);
        $(".chooseItem").css({"height":heights-$(".timu").outerHeight(),"width":"auto"});
        $(".mainLeft").css("height",$(window).height()-$(".header").outerHeight()-54);
}
 function timu(){
    var cun=$(document).width()-$(".mainLeft").outerWidth()-$(".timu").outerWidth()-6;
    var cun2=$(window).height()-$(".header").outerHeight()-$(".btn").outerHeight()-$(".part").outerHeight()-18;
    $(".chooseItem").css({"width":cun+"px","height":cun2+"px"});
    $(".timu").css("height",cun2+"px");
}
/*改变字体大小*/
function font(){
    var asize=$("#fontsize").val();
    if(asize>69){
        var size=16*asize/100;
        $(".chooseItem label,.chooseItem p").css("font-size",size+"px");
    }
    return;
}
 /*强调显示*/
function setColor(color){
    if(color==1){
        color="#ffff00";
    }else{
        color="#f5fff0";
    }
    if(navigator.userAgent.indexOf("MSIE") > -1){
        var tr = document.selection.createRange();
        if(tr.parentElement().className  == "editor" || tr.parentElement().parentNode.nodeName == "P"){
            tr.execCommand("BackColor", false, color);
            return;
        }
    }else{
       /* ////("360 谷歌 火狐");*/
        var tr = window.getSelection().getRangeAt(0);
        if(tr.commonAncestorContainer.parentNode.className == "editor" || tr.commonAncestorContainer.parentNode.nodeName == "FONT"){
            var span = document.createElement("font");
            span.style.cssText = "background-color:"+color;
            tr.surroundContents(span);
            return;
        }
    }
}
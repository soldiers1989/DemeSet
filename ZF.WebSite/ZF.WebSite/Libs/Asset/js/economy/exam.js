$(function(){
	//点击放大字体
	$(document).on("click",".changeBig",function(){
        $("#fontsize").val($(this).attr("type"));
        font();
    }); 
    //点击：设置-切换=显示隐藏左边
    $(document).on("click",".changeFull",function(){
    	
    	if($(this).hasClass("Cur")){
    		$(".main").removeClass("screen");
    		$(this).removeClass("Cur");
    	}else{

    		$(".main").addClass("screen");
    		$(this).addClass("Cur");
    	}
    	return;
    })
	
});
/*改变字体大小*/
function font(){
    var asize=$("#fontsize").val();
    if(asize>69){
        var size=16*asize/100;
        $(".chooseItem label,.chooseItem p").css("font-size",size+"px");
    }
    return;
}
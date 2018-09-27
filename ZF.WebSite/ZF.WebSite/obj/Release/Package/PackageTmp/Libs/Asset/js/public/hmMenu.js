/* 
* @Author: anchen
* @Date:   2017-07-06 10:31:02
* @Last Modified by:   anchen
* @Last Modified time: 2017-07-06 10:30:56
* 百度统计+禁止右键
*/

document.oncontextmenu = function (event){
    if(window.event){
        event = window.event;
    }try{
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")){
            return false;
        }
        return true;
    }catch (e){
        return false; 
    } 
}
//百度统计
var _hmt = _hmt || [];
(function() {
  var hm = document.createElement("script");
  hm.src = "//hm.baidu.com/hm.js?567ac26ed09f3821d327b6776f5c6bcb";
  var s = document.getElementsByTagName("script")[0]; 
  s.parentNode.insertBefore(hm, s);
})();
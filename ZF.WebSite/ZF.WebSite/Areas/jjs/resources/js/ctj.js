/**
 * 
 * @authors Your Name (you@example.org)
 * @date    2018-06-09 21:46:20
 * @version $Id$
 */

$(document).ready(function() {
	$(".btn1").click(function(event) {
		$(this).hide();
		$(this).siblings('.btn2').show();
		$(this).parents(".ctj_list").find('.ctj-list').show();
		$(this).parents(".ctj_list").find(".ctj_xx").show();
	});
	$(".btn2").click(function(event) {
		$(this).hide();
		$(this).siblings(".btn1").show();
		$(this).parents(".ctj_list").find(".ctj-list").hide();
		$(this).parents(".ctj_list").find(".ctj_xx").hide();
	});

	$(".qtkc").click(function(event) {
		$(".jjsc-tclb").toggle();
	});
});
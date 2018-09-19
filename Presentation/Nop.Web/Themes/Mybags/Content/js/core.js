$(window).scroll(function(){
	if($(".site-header").offset().top > 100){
		$(".navbar-fixed-top").addClass("header-collapse");
	} else {
		$(".navbar-fixed-top").removeClass("header-collapse");
	}
});

$(window).load(function(){
	var topHeader = $(".site-header").outerHeight();
	$("body").css('padding-top',topHeader);
});

$(function(){
	//header height
	var topHeader = $(".site-header").outerHeight();
	$("body").css('padding-top',topHeader);
	
	//banner height
	if($(window).width() > 1100){
		var windowHeight = $(window).height();
		var bannerHeight = windowHeight - topHeader;
	}
	//$(".site-banner li img").css('height',bannerHeight);
	//toggle search header
	$("#search-toggle").click(function(){
		$(".toggle-search").toggleClass("open");
		return false;
	});
	$(".main-footer .footer-box").matchHeight();
	$(".video-thumb li img").matchHeight();
});

$(window).resize(function(){
	//header height
	var topHeader = $(".site-header").outerHeight();
	$("body").css('padding-top',topHeader);
	//banner height
	if($(window).width() > 1100){
		var windowHeight = $(window).height();
		var bannerHeight = windowHeight - topHeader;
	}
});
//$(function () {

//    if (document.readyState === 'ready' || document.readyState === 'complete') {
//        $("#spinner").addClass("hidden");
//    } else {
//        document.onreadystatechange = function () {
//            if (document.readyState == "complete") {
//                $("#spinner").addClass("hidden");
//            }
//        }
//    }
//})

$(window).bind("load", function () {    
    $('.loading').fadeOut(1000);
});
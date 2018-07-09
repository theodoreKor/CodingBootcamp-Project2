$(document).ready(function () {

    $('.pre-footer').hover(function () {
        showPreFooter();
    }, function () {
        hidePreFooter();
    })

});

function showPreFooter() {
    var preFooter = $('.pre-footer');
    var preFooterHeight = preFooter.innerHeight();

    preFooter.stop().animate({ top: "-" + preFooterHeight + 'px' }, 500, 'swing');
}
function hidePreFooter() {
    var preFooter = $('.pre-footer');
    preFooter.stop().animate({ top: "-40px" }, 500, 'swing');
}
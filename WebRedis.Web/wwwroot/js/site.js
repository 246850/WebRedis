(function (win, $) {
    /* 获取页面最大的z-index值 */
    function getMaxZIndex() {
        var maxZ = Math.max.apply(null,
            $.map($('body *'), function (e, n) {
                if ($(e).css('position') != 'static')
                    return parseInt($(e).css('z-index')) || 1;
            }));
        return maxZ;
    }
    $.extend({
        //弹窗蒙层
        alert: function (msg) {
            $('#sys-alert').css("z-index", getMaxZIndex() + 1).find(".modal-body > p").html(msg).end().modal().on("hidden.bs.modal", function () {
                $(this).find(".modal-content").removeAttr("style");
            });
        }
    })
})(window, jQuery);
// ajax tree 插件
(function ($) {
    function buildTree(data, callback, loop) {
        var $ul = $("<ul/>").addClass("treeview-menu");
        for (var i = 0, len = data.length; i < len; i++) {
            var item = data[i];
            if (item["children"] && item["children"].length > 0) {
                var $li2 = $("<li/>").addClass("treeview"),
                    $a2 = $("<a/>").attr("href", "javascript:void(0);"),
                    $i2 = $("<i/>").addClass(item["iconClass"]),
                    $span2 = $("<span>").html(item["title"]),
                    $span3 = $("<span/>").addClass("pull-right-container"),
                    $i3 = $("<i/>").addClass("fa fa-angle-left pull-right");
                $li2.append($a2.append($i2).append($span2).append($span3.append($i3)));
                $li2.append(buildTree(item["children"], callback, ++loop));
                $ul.append($li2);
                loop = 0;
            } else {
                var $li1 = $("<li/>"),
                    $a1 = $("<a/>").attr("href", "javascript:void(0);"),
                    $i1 = $("<i/>").addClass(item["iconClass"]),
                    $span1 = $("<span>").html(item["title"]);
                (function (node) {
                    $a1.on("click", function () {
                        if (callback) callback(node, arguments);
                    });
                })(item);
                for (var j = 0; j < loop; j++) {
                    $a1.append($("<span/>").addClass("empty-span"));
                }
                $li1.append($a1.append($i1).append($span1));
                $ul.append($li1);
            }
        }
        return $ul;
    }
    $.fn._liteTree = function (url, callback) {
        var $treeDom = $(this);
        $.ajax({
            url: url,
            dataType: "json",
            type: "post",
            beforeSend: function () {
                $treeDom.next(".treemenu-loading").show();
            },
            success: function (res) {
                if (res.code !== 0) {
                    alert("服务异常");
                    return;
                }
                var data = res.body;
                if (data && data.length > 0) {
                    for (var i = 0, len = data.length; i < len; i++) {
                        var item = data[i];
                        var $li = $("<li/>").addClass("treeview"),
                            $a = $("<a/>").attr("href", "javascript:void(0);"),
                            $i1 = $("<i/>").addClass(item["iconClass"]),
                            $span1 = $("<span>").html(item["title"]),
                            $span2 = $("<span/>").addClass("pull-right-container"),
                            $i2 = $("<i/>").addClass("fa fa-angle-double-left pull-right");
                        $li.append($a.append($i1).append($span1).append($span2.append($i2)));
                        if (item["children"] && item["children"].length > 0) {
                            $li.append(buildTree(item["children"], callback, 0));
                        }
                        $treeDom.append($li);
                    }
                }
                //  初始化
                var $tree = $treeDom.tree();
                // 展开第一个
                $tree.children("li").eq(0).children("a").eq(0).trigger("click");
                //// 菜单折叠
                //setTimeout(function() {
                //    $("a[data-toggle=push-menu]").trigger("click");
                //}, 2000);
                
            },
            complete: function () {
                $treeDom.next(".treemenu-loading").fadeOut();
            }
        });
    };
})(jQuery);

// 初始化
$(function () {
    //  提示框拖动
    $("#sys-alert .modal-content").draggable({ cursor: "move", containment: document.body });

    //  布局
    function layout() {
        try {
            var contentHeight = $(window).height() - $(".main-header").outerHeight(),
                navHeight = $(".nav-tabs-custom .nav-tabs").outerHeight(),
                panelContentHeight = contentHeight - navHeight - 2;
            $(".nav-tabs-custom .tab-content").height(panelContentHeight);
        } catch (ex) {
            console.dir(ex);
        }
    }
    layout();
    $(window).on("resize", function () {
        layout();
    });
    $(document).on("selectstart", function () { return false; });

    //  移除tab - 关闭
    function removeTab(current) {
        var $parent = $(current).parent("a"),
            $li = $parent.parent("li"),
            $next = $li.next("li").children("a"),
            $prev = $li.prev("li").children("a"),
            $content = $($parent.attr("href"));
        $li.remove();
        if ($content) {
            $content.remove();
        }
        if ($li.hasClass("active")) {
            if ($next.length === 1) {
                $next.trigger("click");
            } else {
                $prev.trigger("click");
            }
        }
    }
    // 调整tab数量
    function checkTab(li) {
        var tabWith = $(".nav-tabs-custom .nav-tabs").outerWidth(),
            liWith = li.outerWidth() + 5;
        $(".nav-tabs-custom .nav-tabs li").each(function () {
            liWith += $(this).outerWidth() + 5;
        });
        liWith += 300;
        if (tabWith <= liWith) {
            $(".nav-tabs-custom .nav-tabs li:eq(2) .copter-menu-close").trigger("click");
        }
    }
    if (window._liteTreeUrl) {
        $(".sidebar-menu")._liteTree(window._liteTreeUrl, function (node, param) {
            var id = "page_" + node["id"],
                tabId = "tab_" + node["id"];
            if ($("#" + tabId).length === 1) {
                $("#" + tabId).trigger("click");
                return;
            }
            var $li = $("<li/>").attr("title", node["title"]),
                $a = $("<a/>").attr("data-toggle", "tab").attr("href", "#" + id).attr("id", tabId),
                $i1 = $("<i/>").addClass(node["iconClass"]).addClass("copter-menu-icon"),
                $i2 = $("<i/>").addClass("fa fa-close copter-menu-close").attr("role", "button"),
                $span = $("<span/>").text(node["title"]);
            $li = $li.append($a.append($i1).append($span).append($i2));
            // 检查宽度
            checkTab($li);
            $(".nav-tabs-custom .nav-tabs").append($li);

            var $div = $("<div/>").addClass("tab-pane").attr("id", id),
                $frame = $("<iframe/>").attr("src", node["href"]).addClass("tab-pane-iframe");
            $(".nav-tabs-custom .tab-content").append($div.append($frame));
            $("#" + tabId).trigger("click");
            $i2.on("click", function () {
                // 关闭
                removeTab(this);
            });
        });
    }
    // 当打开左侧菜单 检查是否要调整tab数量
    $(document).on('expanded.pushMenu', function () {
        checkTab($("<li/>"));
    });
    // tab刷新按钮
    $("#btn-reload").on("click", function () {
        var current = $(".nav-tabs-custom .nav-tabs li.active:eq(0) > a").prop("href").split("#"),
            selector = "";
        if (current.length === 2) {
            selector = current[1];
        }
        var $frame = $("#" + selector).children(".tab-pane-iframe:eq(0)");
        $frame.get(0).contentWindow.location.reload(true);
        //$frame.prop("src", $frame.prop("src"));
    });
});
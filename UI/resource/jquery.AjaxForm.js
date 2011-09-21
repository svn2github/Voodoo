var formsubmit = function (formid, bgimg) {
    $("#" + formid).live('submit', function () {
        try { if (jQuery('#' + formid).validationEngine('validate') == false) { return false; } } catch (e) { }
        Mask.load(bgimg);
        $.post($(this).attr("action") + "?ajax=1",
                    $(this).serialize(),
                    function (data) {
                        var j = eval("(" + data + ")").data;
                        for (var i = 0; i < j.length; i++) {
                            if (j[i]["attr"] == "value") {
                                $("#" + j[i]["name"]).val(j[i]["val"]);
                            }
                            if (j[i]["attr"] == "href" || j[i]["attr"] == "disabled" || j[i]["attr"] == "readonly" || j[i]["attr"] == "checked") {
                                $("#" + j[i]["name"]).prop(j[i]["attr"], j[i]["val"]);
                            }
                            else if (j[i]["attr"] == "text") {
                                $("#" + j[i]["name"]).text(j[i]["val"]);
                            }
                            else if (j[i]["attr"] == "checkindex" && j[i]["val"].length > 0) {
                                var indexs = j[i]["val"].split(",");
                                $("#" + j[i]["name"] + " checkbox").prop("checked", false);
                                for (var im = 0; im < indexs.length; im++) {
                                    $("#" + j[i]["name"] + " :checkbox").eq(parseInt(indexs[im])).prop("checked", true);
                                }
                            }
                            else if (j[i]["attr"] == "selectindex" && j[i]["val"].length > 0) {
                                $("#" + j[i]["name"] + " radio").eq(parseInt(j[i]["val"])).prop("checked", true);
                            }
                            else {
                                //
                            }
                        }
                        //end ajax request
                        Mask.unload();
                        if (eval("(" + data + ")").result.length > 0) {
                            alert(eval("(" + data + ")").result);
                        }
                        var url = eval("(" + data + ")").url;
                        if (url.length > 0) {
                            location.href = url;
                        }
                    })
        return false;
    });
}


var Mask = function () {
    function load(bgimg) {
        var jq = _render(bgimg);
    };
    function unload() {
        $("#_mask").remove();
    };
    function _render(bgimg) {
        var _div = $("<div id='_mask' style='padding : 0px ; margin : 0px ;  background : #555 ;  position : absolute ;  background-image: url(" + bgimg + "); background-repeat: no-repeat;background-position: center 40%; '></div>")
                  .appendTo("body");
        var _css = _getCss();
        _div
                        .css(_css)
                        .fadeIn();
        return _div;
    };
    function _getCss() {
        var css = {
            display: "none",
            top: 0 + "px",
            left: 0 + "px",
            width: document.documentElement.clientWidth + "px",
            height: document.documentElement.clientHeight + "px",
            zIndex: 9999,
            opacity: 0.6
        };
        return css;
    };
    return {
        load: load,
        unload: unload
    };
} ();
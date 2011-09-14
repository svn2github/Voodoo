$(function () {
    $("#form1").submit(function () {
        $.post($(this).attr("action") + "?ajax=1",
                    $(this).serialize(),
                    function (data) {
                        var j = eval(data);
                        for (var i = 0; i < j.length; i++) {
                            alert(j[i]["name"])
                            $("#" + j[i]["name"]).val(j[i]["value"]);
                        }
                    })
        return false;
    });
})
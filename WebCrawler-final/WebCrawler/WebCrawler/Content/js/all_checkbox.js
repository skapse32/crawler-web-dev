$(document).ready(function () {

    // add multiple select / deselect functionality
    $("#selectall").click(function () {
        $('.second').attr('checked', this.checked);
    });

    // if all checkbox are selected, check the selectall checkbox
    // and viceversa
    $(".second").click(function () {

        if ($(".second").length == $(".second:checked").length) {
            $("#selectall").attr("checked", "checked");
        } else {
            $("#selectall").removeAttr("checked");
        }

    });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

        // add multiple select / deselect functionality
        $("#selectall").click(function () {
            $('.second').attr('checked', this.checked);
        });

        // if all checkbox are selected, check the selectall checkbox
        // and viceversa
        $(".second").click(function () {

            if ($(".second").length == $(".second:checked").length) {
                $("#selectall").attr("checked", "checked");
            } else {
                $("#selectall").removeAttr("checked");
            }

        });
    });
});

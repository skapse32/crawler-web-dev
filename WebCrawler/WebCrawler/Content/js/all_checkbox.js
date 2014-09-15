$(document).ready(function () {
    // Below code is used to remove all check property if,
    // User select/unselect options with name=option2 options.
    $("input[name=option2]").click(function () {
        $("#selectall").prop("checked", false);
    });
    /////////////////////////////////////////////////////////////
    // JS for Check/Uncheck all CheckBoxes by Checkbox //
    /////////////////////////////////////////////////////////////
    $("#selectall").click(function () {
        $(".second").prop("checked", $("#selectall").prop("checked"));
    });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        // Below code is used to remove all check property if,
        // User select/unselect options with name=option2 options.
        $("input[name=option2]").click(function() {
            $("#selectall").prop("checked", false);
        });
        /////////////////////////////////////////////////////////////
        // JS for Check/Uncheck all CheckBoxes by Checkbox //
        /////////////////////////////////////////////////////////////
        $("#selectall").click(function() {
            $(".second").prop("checked", $("#selectall").prop("checked"));
        });
    });
});

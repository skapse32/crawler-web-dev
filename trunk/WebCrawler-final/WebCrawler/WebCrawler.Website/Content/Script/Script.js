function Modules(hdModuleName) {
    $(document).ready(function () {
        $(".txtModuleName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../../../Service.asmx/GetModules",
                    type: 'POST',
                    data: "{ 'pattern': '" + request.term + "' }",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function(item) {
                            return {
                                label: "<span>" + item.ModuleName +
                                        "</span><br/><span style='font-size:10px; color:red; font-weight:bold'>Office: "
                                        + item.OfficeName + "</span>",
                                value: item.ModuleName,
                                id: item.ModuleId
                            }
                        }));
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                var a = document.getElementById(hdModuleName);
                a.value = ui.item.id;
                $('#txtModuleName').val(ui.item.value);
            }
        });
    });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        $(document).ready(function () {
            $(".txtModuleName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../Service.asmx/GetModules",
                        type: 'POST',
                        data: "{ 'pattern': '" + request.term + "' }",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function(item) {
                                return {
                                    label: "<span>" + item.ModuleName +
                                        "</span><br/><span style='font-size:10px; color:red; font-weight:bold'>Office: "
                                        + item.OfficeName + "</span>",
                                    value: item.ModuleName,
                                    id: item.ModuleId
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var a = document.getElementById(hdModuleName);
                    a.value = ui.item.id;
                    $('#txtModuleName').val(ui.item.value);
                }
            });
        });
    });
}
    function Username(hdUsername) {
    $(document).ready(function () {
        $(".txtUsername").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../../../Service.asmx/GetUsers",
                    type: 'POST',
                    data: "{ 'pattern': '" + request.term + "' }",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: "<span>" + item.username +
                                       "</span><br/><span style='font-size:10px; color:red; font-weight:bold'>Full name:"
                                       + item.fullname + "</span>",
                                value: item.username,
                                id: item.id
                            }
                        }));
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                var a = document.getElementById(hdUsername);
                a.value = ui.item.id;
                $('#txtUsername').val(ui.item.value);
            }
        });
    });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        $(document).ready(function () {
            $(".txtUsername").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../Service.asmx/GetUsers",
                        type: 'POST',
                        data: "{ 'pattern': '" + request.term + "' }",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: "<span>" + item.username +
                                       "</span><br/><span style='font-size:10px; color:red; font-weight:bold'>Full name:"
                                       + item.fullname + "</span>",
                                    value: item.username,
                                    id: item.id
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var a = document.getElementById(hdUsername);
                    a.value = ui.item.id;
                    $('#txtUsername').val(ui.item.value);
                }
            });
        });
    });
}
    function Bold(name, pattern) {
        var result = "";
        var index = name.toLowerCase().indexOf(pattern.toLowerCase());
        var substring1 = "<strong>" + name.substring(index, pattern.length) + "</strong>";
        var substring2 = name.substring(pattern.length, name.length);
        return substring1 + substring2;
    }
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebCrawler.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/style-sub.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/form.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/checkboxstyle.css" rel="stylesheet" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <script type="text/javascript" src="js/jquery.min.js"></script>

    <script src="js/all_checkbox.js"></script>
    <style>
        
    </style>
</head>
<body>
    <div id="header">
        Sample Crawler Website
    </div>
    <div class="wrap">
        <div class="rsidebar span_1_of_left">
            <section class="sky-form">
                <h4>Dashbard</h4>
                <div class="row row1 scroll-pane">
                    <div class="col col-4">
                        <label class="checkbox">
                            <input type="checkbox" name="checkbox" checked=""><i></i>Dưới 500.000</label>
                    </div>
                    <div class="col col-4">
                        <label class="checkbox">
                            <input type="checkbox" name="checkbox"><i></i>Từ 500.000 -> 1.000.000</label>
                        <label class="checkbox">
                            <input type="checkbox" name="checkbox"><i></i>Từ 1.000.000 -> 3.000.000</label>
                        <label class="checkbox">
                            <input type="checkbox" name="checkbox"><i></i>Từ 3.000.000 trở lên</label>
                    </div>
                </div>
            </section>
        </div>
        <form id="form1" runat="server">
            <div class="cont span_2_of_3">
                <div class="col_1_of_single1 span_1_of_single1"></div>
                <asp:TextBox ID="TextBox1" runat="server" Text="http://www.cars.com/vehicledetail/detail/610397293/photo"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" CssClass="button1" Text="Lấy ảnh" OnClick="Button1_OnClick" />
                <input class="second" id="selectall" name="check" type="checkbox">
                <label class="label2" for="selectall">Check/Uncheck All</label>
            </div>
            <div class="cont span_2_of_3">
                <div class="check">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </div>
            </div>

        </form>
    </div>
    <script>
        function getValueUsingClass() {
            var chkArray = [];
            $(".second:checked").each(function () {
                chkArray.push($(this).val());
            });

            var selected;
            selected = chkArray.join(',') + ",";

            /* check if there is selected checkboxes, by default the length is 1 as it contains one single comma */
            if (selected.length > 1) {
                alert("You have selected " + selected);
            } else {
                alert("Please at least one of the checkbox");
            }
        }

    </script>
</body>
</html>

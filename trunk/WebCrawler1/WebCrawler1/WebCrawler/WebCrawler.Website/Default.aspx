<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebCrawler.Website.Default" %>

<%@ Import Namespace="WebCrawler.Core.Info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title>Admin | Image Crop</title>
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport' />

    <!-- font Awesome -->
    <link href="Content/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Ionicons -->
    <link href="Content/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <!-- Theme style -->
    <link href="Content/css/AdminLTE.css" rel="stylesheet" type="text/css" />

    <link href="Content/Css/jQueryUI/jquery-ui-1.10.3.custom.css" rel="stylesheet" />

    <link href="Content/css/style.css" rel="stylesheet" />

    <link href="Content/css/style-sub.css" rel="stylesheet" />

    <link href="Content/css/jcrop.css" rel="stylesheet" />

    <link href="Content/css/checkboxstyle.css" rel="stylesheet" />

    <script src="Content/Script/jquery-1.7.1.js"></script>

    <script src="Content/Script/jquery.Jcrop.js"></script>
    <!-- bootstrap 3.0.2 -->
    <link href="Content/css/bootstrap.css" rel="stylesheet" />

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view thFwwe page via file:// -->
    <!--[if lt IE 9]>
          <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
          <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
        <![endif]-->
</head>
<body class="skin-blue">
    <%
        UserInfo aLoginUserInfo = new UserInfo();
        if (Session["UserInfo"] != null)
        {
            aLoginUserInfo = (UserInfo)Session["UserInfo"];
        }                                                            
    %>

    <!-- header logo: style can be found in header.less -->
    <header class="header">
        <a href="Default.aspx" class="logo">
            <!-- Add the class icon to your logo image or logo icon to add the margining -->
            Image croping tool
        </a>
        <!-- Header Navbar: style can be found in header.less -->
        <nav class="navbar navbar-static-top" role="navigation">
            <!-- Sidebar toggle button-->
            <a href="#" class="navbar-btn sidebar-toggle" data-toggle="offcanvas" role="button">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </a>
            <div class="navbar-right">
                <ul class="nav navbar-nav">
                    <!-- User Account: style can be found in dropdown.less -->
                    <li class="dropdown user user-menu">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                            <i class="glyphicon glyphicon-user"></i>
                            <span><% Response.Write(aLoginUserInfo.UserName); %> <i class="caret"></i></span>
                        </a>
                        <ul class="dropdown-menu">
                            <!-- User image -->
                            <li class="user-header bg-light-blue">
                                <img src="Content/images/avatar3.png" class="img-circle" alt="User Image" />
                                <p>
                                    <% Response.Write(aLoginUserInfo.UserName ?? "Unknow"); %>
                                    <small>Member login <% Response.Write(DateTime.Now.ToShortDateString()); %></small>
                                </p>
                            </li>
                            <!-- Menu Footer-->
                            <li class="user-footer">
                                <div class="pull-left">
                                    <a href="#" class="btn btn-default btn-flat">Profile</a>
                                </div>
                                <div class="pull-right">
                                    <a href="#" class="btn btn-default btn-flat">Sign out</a>
                                </div>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
    <div class="wrapper row-offcanvas row-offcanvas-left">
        <!-- Left side column. contains the logo and sidebar -->
        <aside class="left-side sidebar-offcanvas">
            <!-- sidebar: style can be found in sidebar.less -->
            <section class="sidebar">
                <!-- Sidebar user panel -->
                <div class="user-panel">
                    <div class="pull-left image">
                        <img src="Content/images/avatar3.png" class="img-circle" alt="User Image" />
                    </div>
                    <div class="pull-left info">
                        <p>Hello, <% Response.Write(aLoginUserInfo.UserName); %></p>

                        <a href="#"><i class="fa fa-circle text-success"></i>Online</a>
                    </div>
                </div>
                <!-- sidebar menu: : style can be found in sidebar.less -->
                <ul class="sidebar-menu">
                    <li>
                        <a href="Default.aspx">
                            <i class="fa fa-dashboard"></i><span>Dashboard</span>
                        </a>
                    </li>

                    <li class="treeview">
                        <a href="#">
                            <i class="fa fa-bar-chart-o"></i>
                            <span>Template Created</span>
                            <i class="fa fa-angle-left pull-right"></i>
                        </a>
                        <ul class="treeview-menu">
                            <li><a href="Default.aspx"><i class="fa fa-angle-double-right"></i>Create new gallery</a></li>
                            <li><a href="Upload.aspx"><i class="fa fa-angle-double-right"></i>Upload sample template</a></li>
                        </ul>
                    </li>
                    <li class="treeview">
                        <a href="#">
                            <i class="fa fa-laptop"></i>
                            <span>Account</span>
                            <i class="fa fa-angle-left pull-right"></i>
                        </a>
                        <ul class="treeview-menu">
                            <li><a href="Password.aspx"><i class="fa fa-angle-double-right"></i>Change password</a></li>
                        </ul>
                    </li>
                </ul>
            </section>
            <!-- /.sidebar -->
        </aside>
        <!-- Right side column. Contains the navbar and content of the page -->
        <aside class="right-side">
            <section class="content">
                <form id="form1" runat="server">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgressStep1" runat="server">
                                <ProgressTemplate>
                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <%--                            <div class="row" id="">
                                <asp:DataList ID="dtlListTemplate" Width="100%" runat="server" RepeatColumns="3">
                                    <ItemTemplate>
                                        <div class="box1">
                                            <div class='col_1_of_single1 span_1_of_single1'>
                                                <div class='view1 view-fifth1'>
                                                    <div class='top_box' style="padding: 0 !important">
                                                        <div class='m_2'>
                                                            <a href='<%# Eval("FolderName") %>'>View</a>
                                                            <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_OnClick" Text="Delete template"/>
                                                            <asp:HiddenField runat="server" ID="hdtemplateId" Value='<%# Eval("FolderId") %>'/>
                                                        </div>
                                                        <div class='grid_img'>
                                                            <div class='css3'>
                                                            
                                                                <asp:Image ID="imgImage" CssClass="test" ImageUrl='<%# Eval("FolderImage") %>' runat="server" /><br />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>--%>

                            <div class="row" runat="server" id="page1">
                                <div class="box box-primary">
                                    <div class="box-header">
                                        <h3 class="box-title">Step 1 - Getting start</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                        </div>
                                        <div class="form-group">
                                            <asp:Button runat="server" Width="100%" CssClass="btn btn-warning dropdown-toggle" ID="btnNextStep2" OnClick="btnNextStep2_OnClick" Text="NEXT STEP 2" />
                                        </div>
                                        <div class="form-group">
                                            <label for="txturl">Your link:</label>
                                            <input type="text" id="txturl" runat="server" class="form-control" placeholder="Enter your link" />
                                            <label for="txturl">Example: http://www.cars.com/vehicledetail/detail/610397293/photo </label>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button runat="server" Width="100%" CssClass="btn btn-danger"
                                               OnClick="Button5_OnClick" ID="Button5" Text="View Existing Galleries" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" runat="server" id="page2">
                                <div class="box box-primary">
                                    <div class="box-header">
                                        <h3 class="box-title">Step 2 - Choose image</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-xs-5">
                                                    <div class="form-group">
                                                        <label for="txturl">Gallery name:</label>
                                                        <input type="text" id="txtGalleryName" runat="server" class="form-control" placeholder="Enter gallery name" />
                                                    </div>
                                                </div>
                                                <div class="col-xs-7">
                                                    <div class="form-group">
                                                        <label>Select</label>
                                                        <asp:DropDownList ID="cboTemplate" runat="server" class="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="txturl">Gallery description:</label>
                                            <input type="text" id="txtGalleryDescripton" runat="server" class="form-control" placeholder="Enter gallery descripton" />
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-4">
                                                <div class="form-group">
                                                    <asp:Button runat="server" Width="100%" CssClass="btn btn-info btn-flat" ID="btnSelectAllImage"
                                                        OnClick="btnSelectAllImage_OnClick" Text="Select All(Select/remove selection)" />
                                                </div>
                                            </div>
                                            <div class="col-xs-4">
                                                <div class="form-group">
                                                    <asp:Button runat="server" Width="100%" CssClass="btn btn-info btn-flat" ID="btnCropallImage"
                                                        OnClick="btnCropallImage_OnClick" Text="Crop All(Select/remove selection)" />
                                                </div>
                                            </div>
                                            <div class="col-xs-4">
                                                <div class="form-group">
                                                    <asp:Button runat="server" Width="100%" CssClass="btn btn-warning dropdown-toggle" ID="btnNextStep3"
                                                        OnClick="btnNextStep3_OnClick" Text="NEXT STEP 3" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <asp:DataList ID="dtlListImage" Width="100%" runat="server" RepeatColumns="3">
                                                <ItemTemplate>
                                                    <div class="box1">
                                                        <div class='col_1_of_single1 span_1_of_single1'>
                                                            <div class='view1 view-fifth1'>
                                                                <div class='top_box'>
                                                                    <div class='m_2'>
                                                                        <asp:CheckBox runat="server" ID="chkChooser" class="second" Text="Select" value='<%# Eval("link") %>' AutoPostBack="True" OnCheckedChanged="chkChooser_OnCheckedChanged" />
                                                                        <asp:CheckBox runat="server" ID="chkChooserCrop" class="second" Text="Crop" value='<%# Eval("link") %>' AutoPostBack="True" OnCheckedChanged="chkChooserCrop_OnCheckedChanged" />
                                                                    </div>
                                                                    <div class='grid_img'>
                                                                        <div class='css3'>
                                                                            <asp:Image ID="imgImage" ImageUrl='<%# Eval("link") %>' runat="server" /><br />
                                                                            <asp:TextBox Text='<%# Eval("link") %>' ID="txtImageUrl" runat="server" Style="width: 80%; height: 40px; margin-top: 5px;" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </div>
                                    <div class="box-footer">
                                        <asp:Button runat="server" ID="btnNextStep32" Width="100%" Text="NEXT STEP 3"
                                            OnClick="btnNextStep3_OnClick" CssClass="btn btn-warning dropdown-toggle" />
                                    </div>
                                </div>
                            </div>

                            <div class="row" runat="server" id="page3">
                                <div class="box box-primary">
                                    <div class="box-header">
                                        <h3 class="box-title">Step 3 -  Crop</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="form-group">
                                                        <asp:Button runat="server" ID="btnNextStep4" OnClick="btnNextStep4_OnClick" Width="100%" Text="NEXT STEP 4" CssClass="btn btn-warning dropdown-toggle" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-xs-4">
                                                    <div class="form-group">
                                                        <asp:Button runat="server" ID="btnCrop" Width="100%" OnClick="btnCrop_OnClick" Text="CROP" CssClass="btn btn-danger" />
                                                    </div>
                                                </div>
                                                <div class="col-xs-4">
                                                    <div class="form-group">
                                                        <asp:Button runat="server" ID="btnCropNext" OnClick="btnCropNext_OnClick" Width="100%" Text="CROP NEXT" CssClass="btn btn-danger" />
                                                    </div>
                                                </div>
                                                <div class="col-xs-4">
                                                    <div class="form-group">
                                                        <asp:Button runat="server" ID="btnCropBack" Width="100%" OnClick="btnCropBack_OnClick" Text="CROP BACK" CssClass="btn btn-danger" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label>Additional text to image</label>
                                            <input type="text" placeholder="Enter Additional text" id="txtAdditionalText" class="form-control" runat="server" />
                                        </div>
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-xs-12" id="imgContainer" style="overflow: auto; border: solid 1px black; padding: 10px; margin-bottom: 5px; position: relative;">
                                                    <img runat="server" title="That's my auto :)" id="originalImage" />
                                                </div>
                                                <div style="display: none;">
                                                    <input type="text" size="4" id="x1" name="x1" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                                    <input type="text" size="4" id="y1" name="y1" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                                    <input type="text" size="4" id="x2" name="x2" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                                    <input type="text" size="4" id="y2" name="y2" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                                    <input type="text" size="4" id="w" name="w" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                                    <input type="text" size="4" id="h" name="h" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box-footer">
                                        <asp:Button runat="server" ID="Button2" Text="Upload" />
                                    </div>
                                </div>
                            </div>

                            <div class="row" runat="server" id="page4">
                                <div class="box box-primary">
                                    <div class="box-header">
                                        <h3 class="box-title">Step 4 - Preview</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                             <asp:Button runat="server" ID="btnPublish" Width="100%" OnClick="btnPublish_OnClick" Text="PUBLISH" CssClass="btn btn-danger" />
                                        </div>
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-xs-12" id="drag-drop">
                                                    <div class="column left first" id="sourceDiv">
                                                        <div class="sortable-list" id="sourceItem">
                                                            <asp:Literal runat="server" ID="ltrSourceItem"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearer">&nbsp;</div>
                                        <input type="hidden" runat="server" id="targetImage" />
                                        <input type="hidden" runat="server" id="targetTitle" />
                                    </div>
                                </div>
                            </div>

                            <div class="row" runat="server" id="page5">
                                <div class="box box-primary">
                                    <div class="box-header">
                                        <h3 class="box-title">Step 4 - Preview</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <label for="txtUrlPublish">Your gallery was published. Galleries url is:</label>
                                            <input type="text" id="txtUrlPublish" class="form-control" runat="server" />
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-4">
                                                <div class="form-group">
                                                    <asp:Button runat="server" ID="btnCopy" Width="100%" Text="Copy" CssClass="btn btn-danger" />
                                                </div>
                                            </div>
                                            <div class="col-xs-4">
                                                <div class="form-group">
                                                    <asp:Button runat="server" ID="btnNew"
                                                       OnClick="btnNew_OnClick" Width="100%" Text="New Gallery" CssClass="btn btn-danger" />
                                                </div>
                                            </div>
                                            <div class="col-xs-4">
                                                <div class="form-group">
                                                    <asp:Button runat="server" ID="btnIndex" Width="100%" Text="Galleries Index" 
                                                       OnClick="btnIndex_OnClick" CssClass="btn btn-danger" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box-footer">
                                        
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </form>
            </section>
        </aside>
    </div>

    <!-- Bootstrap -->
    <script src="<%: ResolveUrl("~/Content/Script/bootstrap.min.js") %>" type="text/javascript"></script>
    <!-- AdminLTE App -->
    <script src="<%: ResolveUrl("~/Content/script/AdminLTE/app.js") %>" type="text/javascript"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="<%: ResolveUrl("~/Content/script/AdminLTE/demo.js") %>" type="text/javascript"></script>

    <script src="Content/Script/jquery-ui-1.8.custom.min.js"></script>
    <script src="Content/Script/jquery.clipboard.js"></script>
    <script>
        function showMessage(message) {
            alert(message);
        }

        var a = "";
        var HiddenField;
        var HiddenField1;
        var chkArray = [];
        var sourceArray = [];
        var targetArray = [];
        var indeximage = 0;
        var current_fs, next_fs, previous_fs; //fieldsets
        var left, opacity, scale; //fieldset properties which we will animate
        var animating; //flag to prevent quick multi-click glitches

        document.addEventListener("keydown", function (evt) {
            if (evt.keyCode == 37 || evt.keyCode == 40) {
                $('#<%= btnCropNext.ClientID %>').trigger("click");
            }
            if (evt.keyCode == 39 || evt.keyCode == 38) {
                $('#<%= btnCropBack.ClientID %>').trigger("click");
            }
        }, false);

        $(function () {
            $('#originalImage').Jcrop({
                onChange: showCoords,
                onSelect: showCoords
            });
            // Patch for IE to force "overflow: auto;"
            document.getElementById("imgContainer").style["position"] = "relative";
        });

        $(document).ready(function () {
            //$('#drag-drop .sortable-list').sortable({
            //    connectWith: '#drag-drop .sortable-list',
            //    update: function () {
            //        //iterate through the textboxes and place their values in the correct hidden field
            //        $("#sourceItem").find("img").each(function () {
            //            HiddenField = document.getElementById("targetImage");
            //            a = a + $(this).attr('src') + "|";
            //            HiddenField.value = a;
            //        });
            //        a = "";
            //        $("#sourceItem").find("input").each(function () {
            //            HiddenField1 = document.getElementById("targetTitle");
            //            a = a + $(this).val() + "|";
            //            HiddenField1.value = a;
            //        });
            //        a = "";
            //    }
            //}).disableSelection();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $('#originalImage').Jcrop({
                        onChange: showCoords,
                        onSelect: showCoords
                    });
                    $('#drag-drop .sortable-list').sortable({
                        connectWith: '#drag-drop .sortable-list',
                        update: function () {
                            //iterate through the textboxes and place their values in the correct hidden field
                            $("#sourceItem").find("img").each(function () {
                                HiddenField = document.getElementById("targetImage");
                                a = a + $(this).attr('src') + "|";
                                HiddenField.value = a;
                            });
                            a = "";
                            $("#sourceItem").find("input").each(function () {
                                HiddenField1 = document.getElementById("targetTitle");
                                a = a + $(this).val() + "|";
                                HiddenField1.value = a;
                            });
                            a = "";
                        }
                    }).disableSelection();
                    $('#<%= btnCopy.ClientID %>').clipboard({
                        path: 'Content/Script/jquery.clipboard.swf',
                        copy: function () {
                            return $('#txtUrlPublish').val();
                        }
                    });
                }
            });
        };
        function showCoords(c) {
            $('#x1').val(c.x);
            $('#y1').val(c.y);
            $('#x2').val(c.x2);
            $('#y2').val(c.y2);
            $('#w').val(c.w);
            $('#h').val(c.h);
        };

        function Del(e) {
            $("#sourceItem").find("img").each(function () {
                HiddenField = document.getElementById("targetImage");
                a = a + $(this).attr('src') + "|";
                HiddenField.value = a;
            });
            a = "";
            $("#sourceItem").find("input").each(function () {
                HiddenField1 = document.getElementById("targetTitle");
                a = a + $(this).val() + "|";
                HiddenField1.value = a;
            });
            a = "";
            $('#sourceItem').remove(b);
        }

        function changr() {
            $("#sourceItem").find("img").each(function () {
                HiddenField = document.getElementById("targetImage");
                a = a + $(this).attr('src') + "|";
                HiddenField.value = a;
            });
            a = "";
            $("#sourceItem").find("input").each(function () {
                HiddenField1 = document.getElementById("targetTitle");
                a = a + $(this).val() + "|";
                HiddenField1.value = a;
            });
            a = "";
        }
        $(document).ready(function () {
            $('#<%= btnCopy.ClientID %>').clipboard({
                path: 'Content/Script/jquery.clipboard.swf',
                copy: function() {
                    return $('#txtUrlPublish').val();
                }
            });
        });
    </script>
</body>
</html>

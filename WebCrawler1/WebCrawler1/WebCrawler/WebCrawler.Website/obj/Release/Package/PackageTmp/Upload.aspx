<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="WebCrawler.Website.Upload" %>

<%@ Import Namespace="WebCrawler.Core.Info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title>Admin | Image Crop</title>
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport' />
    <!-- bootstrap 3.0.2 -->
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- font Awesome -->
    <link href="Content/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Ionicons -->
    <link href="Content/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <!-- Theme style -->
    <link href="Content/css/AdminLTE.css" rel="stylesheet" type="text/css" />

    <link href="Content/Css/jQueryUI/jquery-ui-1.10.3.custom.css" rel="stylesheet" />

    <link href="Content/css/style-sub.css" rel="stylesheet" />

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.0.2/jquery.min.js"></script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script>

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
                    <div class="row">
                        <!-- left column -->
                        <div class="col-md-6">
                            <!-- general form elements -->
                            <div class="box box-primary">
                                <div class="box-header">
                                    <h3 class="box-title">Upload sample template</h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->

                                <div class="box-body">
                                    <div class="form-group">
                                        <label for="<%= txtName %>">Template Name:</label>
                                        <asp:TextBox runat="server" class="form-control" ID="txtName" />
                                    </div>
                                    <div class="form-group">
                                        <label for="<%= fileUpload.ClientID %>">Template file (* html file)</label>
                                        <asp:FileUpload runat="server" class="form-control" ID="fileUpload" />
                                    </div>
                                    <div class="form-group">
                                        <label for="<%= fileUpload1.ClientID %>">cover file (* image file)</label>
                                        <asp:FileUpload runat="server" class="form-control" ID="fileUpload1" />
                                    </div>
                                </div>
                                <!-- /.box-body -->

                                <div class="box-footer">
                                    <asp:Button runat="server" ID="btnUpload" OnClick="btnUpload_OnClick" Text="Upload" />
                                </div>

                            </div>
                            <!-- /.box -->
                        </div>
                    </div>
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
</body>
</html>

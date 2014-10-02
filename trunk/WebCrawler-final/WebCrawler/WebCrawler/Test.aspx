<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WebCrawler.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/css/jcrop.css" rel="stylesheet" />
    <link href="Content/css/style-sub.css" rel="stylesheet" />
    <link href="Content/css/checkboxstyle.css" rel="stylesheet" />
    <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/jquery.Jcrop.js"></script>
    <script src="Content/js/jquery.easing.min.js"></script>

    <script src="Content/js/all_checkbox.js"></script>
    <style>
        .ns-box {
            padding: 5px;
            line-height: 1.4;
            z-index: 1000;
            pointer-events: none;
            color: rgba(250,251,255,0.95);
            font-size: 90%;
            font-family: 'Helvetica Neue', 'Segoe UI', Helvetica, Arial, sans-serif;
            position: relative;
        }

        .ns-effect-thumbslider .ns-box-inner {
            overflow: hidden;
        }

        .ns-effect-thumbslider.ns-hide .ns-thumb {
            -webkit-animation-direction: reverse;
            animation-direction: reverse;
            -webkit-animation-delay: 0.3s;
            animation-delay: 0.3s;
        }

        .ns-effect-thumbslider.ns-show .ns-thumb, .ns-effect-thumbslider.ns-hide .ns-thumb {
            -webkit-animation-name: animJelly;
            animation-name: animJelly;
            -webkit-animation-duration: 1s;
            animation-duration: 1s;
            -webkit-animation-timing-function: linear;
            animation-timing-function: linear;
            -webkit-animation-fill-mode: both;
            animation-fill-mode: both;
        }

        .ns-effect-thumbslider .ns-thumb {
            position: absolute;
            z-index: 100;
            overflow: hidden;
        }

        .ns-effect-thumbslider.ns-hide .ns-content {
            -webkit-animation-direction: reverse;
            animation-direction: reverse;
            -webkit-animation-delay: 0.3s;
            animation-delay: 0.3s;
        }

        .ns-effect-thumbslider.ns-show .ns-content, .ns-effect-thumbslider.ns-hide .ns-content {
            -webkit-animation-name: animSlide;
            animation-name: animSlide;
            -webkit-animation-duration: 0.4s;
            animation-duration: 0.4s;
            -webkit-animation-fill-mode: both;
            animation-fill-mode: both;
            -webkit-animation-timing-function: cubic-bezier(0.7,0,0.3,1);
            animation-timing-function: cubic-bezier(0.7,0,0.3,1);
        }

        .ns-effect-thumbslider .ns-content {
            background: #fff;
            color: #727275;
            font-weight: bold;
            padding: 0 40px 0 80px;
            height: 64px;
            line-height: 60px;
        }

        .ns-thumb img {
            width: 90px;
            height: 64px;
        }

        .ns-effect-thumbslider.ns-hide .ns-close, .ns-effect-thumbslider.ns-hide .ns-content p {
            -webkit-animation-direction: reverse;
            animation-direction: reverse;
            padding: 5px;
        }

        .ns-effect-thumbslider.ns-show .ns-close, .ns-effect-thumbslider.ns-hide .ns-close, .ns-effect-thumbslider.ns-show .ns-content p, .ns-effect-thumbslider.ns-hide .ns-content p {
            -webkit-animation-name: animFade;
            animation-name: animFade;
            -webkit-animation-duration: 0.3s;
            animation-duration: 0.3s;
            -webkit-animation-fill-mode: forwards;
            animation-fill-mode: forwards;
        }

        .ns-box a {
            color: inherit;
            opacity: 0.7;
            font-weight: 700;
        }

        .ns-close {
            width: 20px;
            height: 20px;
            right: 4px;
            top: 4px;
            overflow: hidden;
            text-indent: 100%;
            cursor: pointer;
            -webkit-backface-visibility: hidden;
            backface-visibility: hidden;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ListView ID="ListView1" runat="server">
                <ItemTemplate>
                    <div class='col_1_of_single1 span_1_of_single1'>
                        <div class='view1 view-fifth1'>
                            <div class='top_box'>
                                <div class='m_2'>
                                    <input class='second' name='option2' type='checkbox' id='<%# Eval("link") %>' value='<%# Eval("link") %>' />
                                    <label class='label2' for='<%# Eval("link") %>'>Choose image</label>
                                </div>
                                <div class='grid_img'>
                                    <div class='css3'>
                                        <img src='<%# Eval("link") %>' alt='' /><br />
                                        <input type='text' value='<%# Eval("link") %>' />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>

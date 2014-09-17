<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateBuilder.aspx.cs" Inherits="WebCrawler.TemplateBuilder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/css/jcrop.css" rel="stylesheet" />
    <link href="Content/css/style-sub.css" rel="stylesheet" />
    <link href="Content/css/checkboxstyle.css" rel="stylesheet" />
    <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/jquery.Jcrop.js"></script>
    <script src="Content/js/jquery.easing.min.js"></script>
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
<body onload="Init();">
    <!-- multistep form -->
    <form id="msform" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <!-- progressbar -->
        <ul id="progressbar">
            <li class="active">Getting image</li>
            <li>Crop and Additional text</li>
            <li>Template</li>
            <li>Adding image</li>
            <li>Completed</li>
        </ul>
        <!-- fieldsets -->
        <fieldset>
            <h2 class="fs-title">Getting images</h2>
            <input id="first" name="next" class="next action-button" value="Next" />
            <asp:UpdatePanel runat="server" ID="UpdatePanelStep1">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgressStep1" runat="server">
                        <ProgressTemplate>
                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <label for="txturl">Your link:</label>
                    <input type="text" id="txturl" runat="server" placeholder="Enter your link" />
                    <label for="txturl">Example: http://www.cars.com/vehicledetail/detail/610397293/photo </label>
                    <br />
                    <asp:Button type="button" class="action-button" ID="btnGetImage" OnClick="btnGetImage_OnClick" Text="Get Images" runat="server" />
                    <div class="clear"></div>
                    <div class="check">
                        <asp:Literal ID="lblResult" runat="server"></asp:Literal>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </fieldset>
        <fieldset>
            <h2 class="fs-title">Crop and Additional text</h2>
            <input type="button" name="previous" class="previous action-button" value="Previous" id="frevfirst" />
            <input type="button" name="next" class="next action-button" value="Next" />
            <asp:UpdatePanel runat="server" ID="UpdatePanelStep2">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgressStep2" runat="server">
                        <ProgressTemplate>
                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table>
                        <tr>
                            <asp:Button runat="server" ID="btnBegin" OnClick="btnBegin_OnClick" CssClass="action-button" Text="Begin Crop Image" Width="215" />
                            <asp:Button OnClientClick="previewImage();" Style="width: 128px; height: 37px; padding: 2px;" class="action-button" Enabled="False" ID="btnPrevButton" OnClick="btnPrevButton_OnClick" Text="Preview Image" runat="server" />
                            <asp:Button OnClientClick="nextImage();" Style="width: 128px; height: 37px; padding: 2px; margin-left: 5px" Enabled="False" class="action-button" ID="btnNextButton" OnClick="btnNextButton_OnClick" Text="Next Image" runat="server" />
                        </tr>
                        <tr>
                            <td>Original image
                                <div style="width: 510px; height: 400px; overflow: auto; border: solid 1px black; padding: 10px; margin-bottom: 5px; position: relative;"
                                    id="imgContainer">
                                    <img runat="server" alt="My auto" title="That's my auto :)" id="originalImage" />
                                </div>
                                <div>
                                    X1
                                    <input type="text" size="4" id="x1" name="x1" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                    Y1
                                    <input type="text" size="4" id="y1" name="y1" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                    X2
                                    <input type="text" size="4" id="x2" name="x2" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                    Y2
                                    <input type="text" size="4" id="y2" name="y2" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                    W
                                    <input type="text" size="4" id="w" name="w" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                    H
                                    <input type="text" size="4" id="h" name="h" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                                </div>
                                <div>
                                    <asp:Button runat="server" ID="btnCrop" OnClick="btnCrop_OnClick" Text="Crop" OnClientClick="return ValidateSelected();" />
                                </div>
                            </td>
                            <td style="vertical-align: top;">Cropped image:
                                <div style="width: 400px; height: 200px; overflow: auto; border: solid 1px black; padding: 10px; margin-bottom: 5px;">
                                    <asp:Literal runat="server" ID="lblCroppedImage"></asp:Literal>
                                </div>
                                <div style="width: 400px; height: 200px; overflow: auto; border: solid 1px black; padding: 10px; margin-bottom: 5px;">
                                    <asp:FileUpload runat="server" ID="fileUpload" />
                                    <input type="text" placeholder="Enter Additional text" id="txtAdditionalText" runat="server" />
                                    <input type="text" placeholder="Enter title text" id="txtTitle" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clearfix"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Select Template</h2>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
            <input type="button" name="next" class="next action-button" value="Next" onclick="setImageComplatedToDragAndDrop()" />
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Adding image</h2>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
            <input type="button" name="next" class="next action-button" value="Next" />
            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                        <ProgressTemplate>
                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div id="div1" ondrop="drop(event)" ondragover="allowDrop(event)">
                        
                    </div>

                    <div id="div2" ondrop="drop(event)" ondragover="allowDrop(event)"></div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Completed</h2>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
            <input type="submit" name="submit" class="submit action-button" value="Submit" />
            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                        <ProgressTemplate>
                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <input type="hidden" id="imagelink" runat="server" />
        <input type="hidden" id="imdex" runat="server" />
        <input type="hidden" id="imageCompleted" runat="server" />
    </form>


    <script>
        function allowDrop(ev) {
            ev.preventDefault();
        }

        function drag(ev) {
            ev.dataTransfer.setData("text/html", ev.target.id);
        }

        function drop(ev) {
            ev.preventDefault();
            var data = ev.dataTransfer.getData("text/html");
            ev.target.appendChild(document.getElementById(data));
        }
        function Init() {
            $(function () {
                $('#originalImage').Jcrop({
                    onChange: showCoords,
                    onSelect: showCoords
                });
            });
            // Patch for IE to force "overflow: auto;"
            document.getElementById("imgContainer").style["position"] = "relative";
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                $(function () {
                    $('#originalImage').Jcrop({
                        onChange: showCoords,
                        onSelect: showCoords
                    });
                });
                // Patch for IE to force "overflow: auto;"
                document.getElementById("imgContainer").style["position"] = "relative";
            });
        }

        var chkArray = [];
        var indeximage = 0;
        $(document).ready(function () {
            //jQuery time
            var current_fs, next_fs, previous_fs; //fieldsets
            var left, opacity, scale; //fieldset properties which we will animate
            var animating; //flag to prevent quick multi-click glitches

            $(".next").click(function () {
                if ($(this).attr('id') == 'first')
                    getValueUsingClass();
                if (animating) return false;
                animating = true;

                current_fs = $(this).parent();
                next_fs = $(this).parent().next();

                //activate next step on progressbar using the index of next_fs
                $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

                //show the next fieldset
                next_fs.show();
                //hide the current fieldset with style
                current_fs.animate({ opacity: 0 }, {
                    step: function (now, mx) {
                        //as the opacity of current_fs reduces to 0 - stored in "now"
                        //1. scale current_fs down to 80%
                        scale = 1 - (1 - now) * 0.2;
                        //2. bring next_fs from the right(50%)
                        left = (now * 50) + "%";
                        //3. increase opacity of next_fs to 1 as it moves in
                        opacity = 1 - now;
                        current_fs.css({ 'transform': 'scale(' + scale + ')' });
                        next_fs.css({ 'left': left, 'opacity': opacity });
                    },
                    duration: 800,
                    complete: function () {
                        current_fs.hide();
                        animating = false;
                    },
                    //this comes from the custom easing plugin
                    easing: 'easeInOutBack'
                });
            });

            $(".previous").click(function () {
                if ($(this).attr("id") == 'frevfirst')
                    setDefault();
                if (animating) return false;
                animating = true;

                current_fs = $(this).parent();
                previous_fs = $(this).parent().prev();

                //de-activate current step on progressbar
                $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

                //show the previous fieldset
                previous_fs.show();
                //hide the current fieldset with style
                current_fs.animate({ opacity: 0 }, {
                    step: function (now, mx) {
                        //as the opacity of current_fs reduces to 0 - stored in "now"
                        //1. scale previous_fs from 80% to 100%
                        scale = 0.8 + (1 - now) * 0.2;
                        //2. take current_fs to the right(50%) - from 0%
                        left = ((1 - now) * 50) + "%";
                        //3. increase opacity of previous_fs to 1 as it moves in
                        opacity = 1 - now;
                        current_fs.css({ 'left': left });
                        previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
                    },
                    duration: 800,
                    complete: function () {
                        current_fs.hide();
                        animating = false;
                    },
                    //this comes from the custom easing plugin
                    easing: 'easeInOutBack'
                });
            });

            $(".submit").click(function () {
                return false;
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

                $(".next").click(function () {
                    if (animating) return false;
                    animating = true;

                    current_fs = $(this).parent();
                    next_fs = $(this).parent().next();

                    //activate next step on progressbar using the index of next_fs
                    $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

                    //show the next fieldset
                    next_fs.show();
                    //hide the current fieldset with style
                    current_fs.animate({ opacity: 0 }, {
                        step: function (now, mx) {
                            //as the opacity of current_fs reduces to 0 - stored in "now"
                            //1. scale current_fs down to 80%
                            scale = 1 - (1 - now) * 0.2;
                            //2. bring next_fs from the right(50%)
                            left = (now * 50) + "%";
                            //3. increase opacity of next_fs to 1 as it moves in
                            opacity = 1 - now;
                            current_fs.css({ 'transform': 'scale(' + scale + ')' });
                            next_fs.css({ 'left': left, 'opacity': opacity });
                        },
                        duration: 800,
                        complete: function () {
                            current_fs.hide();
                            animating = false;
                        },
                        //this comes from the custom easing plugin
                        easing: 'easeInOutBack'
                    });
                });

                $(".previous").click(function () {
                    if (animating) return false;
                    animating = true;

                    current_fs = $(this).parent();
                    previous_fs = $(this).parent().prev();

                    //de-activate current step on progressbar
                    $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

                    //show the previous fieldset
                    previous_fs.show();
                    //hide the current fieldset with style
                    current_fs.animate({ opacity: 0 }, {
                        step: function (now, mx) {
                            //as the opacity of current_fs reduces to 0 - stored in "now"
                            //1. scale previous_fs from 80% to 100%
                            scale = 0.8 + (1 - now) * 0.2;
                            //2. take current_fs to the right(50%) - from 0%
                            left = ((1 - now) * 50) + "%";
                            //3. increase opacity of previous_fs to 1 as it moves in
                            opacity = 1 - now;
                            current_fs.css({ 'left': left });
                            previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
                        },
                        duration: 800,
                        complete: function () {
                            current_fs.hide();
                            animating = false;
                        },
                        //this comes from the custom easing plugin
                        easing: 'easeInOutBack'
                    });
                });

                $(".submit").click(function () {
                    return false;
                });
            });
        });
        function getValueUsingClass() {
            chkArray = [];
            $(".second:checked").each(function () {
                chkArray.push($(this).val());
            });
            var selected;
            selected = chkArray.join("|");
            $("#imagelink").val(selected);
            $("#imdex").val("0");
            $("#originalImage").attr("src", chkArray[0]);

        }

        function setDefault() {
            indeximage = 0;
            $("#imdex").val(indeximage);
        }

        function nextImage() {
            if (indeximage < chkArray.length - 1) {
                indeximage++;
            } else {
                indeximage = 0;
            }
            $("#imdex").val(indeximage);
            $("#originalImage").attr("src", chkArray[indeximage]);
        }

        function previewImage() {
            if (indeximage > 0) {
                indeximage--;
            } else {
                indeximage = chkArray.length - 1;
            }
            $("#imdex").val(indeximage);
            $("#originalImage").attr("src", chkArray[indeximage]);
        }

        function showCoords(c) {
            $('#x1').val(c.x);
            $('#y1').val(c.y);
            $('#x2').val(c.x2);
            $('#y2').val(c.y2);
            $('#w').val(c.w);
            $('#h').val(c.h);
        };

        function ValidateSelected() {
            if (document.getElementById("w").value == "" || document.getElementById("w").value == "0"
                || document.getElementById("h").value == "" || document.getElementById("h").value == "0") {
                alert("No area to crop was selected");
                return false;
            }
        }

        function setImageComplatedToDragAndDrop() {
            var myLink = [];
            myLink = $('#imageCompleted').val().split('|');

            var myDiv = $('#div1');
            var tmp = "";
            for (var i = 0; i < myLink.length; i++) {
                tmp += "<img  width='88' height='31' src='content/images/02.jpg' draggable='true' ondragstart='drag(event)' id='drag1'>";
                
                myDiv.append(tmp);
                tmp = "";
            }
        }

        function allowDrop(ev) {
            ev.preventDefault();
        }

        function drag(ev) {
            ev.dataTransfer.setData("text/html", ev.target.id);
        }

        function drop(ev) {
            ev.preventDefault();
            var data = ev.dataTransfer.getData("text/html");
            ev.target.appendChild(document.getElementById(data));
        }
    </script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateBuilder.aspx.cs" Inherits="WebCrawler.TemplateBuilder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <link href="Content/css/style-sub.css" rel="stylesheet" />
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/css/checkboxstyle.css" rel="stylesheet" />
    <link href="Content/css/jquery.Jcrop.css" rel="stylesheet" />
       <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/jquery.Jcrop.js"></script>
    <script src="Content/js/jquery.easing.min.js"></script>
     
    <script src="Content/js/all_checkbox.js"></script>
   
</head>
<body>
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
            <input type="button" name="next" class="next action-button" value="Next" />
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
                    <br />
                    <div style="float: left;">
                        <input class="second" id="selectall" name="check" type="checkbox">
                        <label class="label2" for="selectall">Select all</label>
                    </div>
                    <div class="clear"></div>

                    <div class="check">
                        <asp:Literal ID="lblResult" runat="server"></asp:Literal>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Crop and Additional text</h2>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
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
                    <div class="float-lt show-image ">
                        <div class="image-content">
                            <asp:Image runat="server" ID="imgContent" ImageUrl="http://images.autotrader.com/scaler/620/420/cms/images/cars/dodge/durango/2012/12-durango/228936.jpg" />
                        </div>
                        <div>
                            <asp:Button runat="server" Text="Crop and Additional Text" CssClass="btn action-button" Width="180px" ID="btnCropAndSave" OnClick="btnCropAndSave_OnClick" />
                            <input type="button" class="action-button" value="Next image" />
                        </div>
                    </div>
                    <div class="float-lt options-image">
                        <div class="options-top">
                            <asp:Literal runat="server" ID="imgResult"></asp:Literal>
                        </div>
                        <div class="options-bottom">
                            <div class="btn_form">
                                <span>Choose logo:</span>
                                <asp:FileUpload runat="server" ID="fileUpload" CssClass="btn btn-primary" />
                            </div>
                            <div class="additianalText">
                                <label for="txtText" style="display: inline-block">Additional text:</label>
                                <input type="text" id="txtText" placeholder="Enter additional text" runat="server" />
                            </div>
                            <div class="title">
                                <label for="txtTitle" style="display: inline-block">Title text:</label>
                                <input type="text" id="txtTitle" placeholder="Enter Title" runat="server" />
                            </div>
                        </div>
                    </div>
                    <%-- Hidden field for store cror area --%>
                    <div class="hidden">
                        <asp:HiddenField ID="X" runat="server" />
                        <asp:HiddenField ID="Y" runat="server" />
                        <asp:HiddenField ID="W" runat="server" />
                        <asp:HiddenField ID="H" runat="server" />
                    </div>
                    <div class="clearfix"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Personal Details</h2>
            <h3 class="fs-subtitle">We will never sell it</h3>
            <input type="text" name="fname" placeholder="First Name" />
            <input type="text" name="lname" placeholder="Last Name" />
            <input type="text" name="phone" placeholder="Phone" />
            <textarea name="address" placeholder="Address"></textarea>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
            <input type="button" name="next" class="next action-button" value="Next" />
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Personal Details</h2>
            <h3 class="fs-subtitle">We will never sell it</h3>
            <input type="text" name="fname" placeholder="First Name" />
            <input type="text" name="lname" placeholder="Last Name" />
            <input type="text" name="phone" placeholder="Phone" />
            <textarea name="address" placeholder="Address"></textarea>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
            <input type="button" name="next" class="next action-button" value="Next" />
        </fieldset>
        <fieldset>
            <h2 class="fs-title">Personal Details</h2>
            <h3 class="fs-subtitle">We will never sell it</h3>
            <input type="text" name="fname" placeholder="First Name" />
            <input type="text" name="lname" placeholder="Last Name" />
            <input type="text" name="phone" placeholder="Phone" />
            <textarea name="address" placeholder="Address"></textarea>
            <input type="button" name="previous" class="previous action-button" value="Previous" />
            <input type="submit" name="submit" class="submit action-button" value="Submit" />
        </fieldset>
    </form>

 
    <script>
        var chkArray = [];
        var indeximage = 0;
        $(document).ready(function () {
            //jQuery time
            var current_fs, next_fs, previous_fs; //fieldsets
            var left, opacity, scale; //fieldset properties which we will animate
            var animating; //flag to prevent quick multi-click glitches

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
            
            $(".second:checked").each(function () {
                chkArray.push($(this).val());
            });
        }
        function SelectCropArea(c) {
            $('#<%=X.ClientID%>').val(parseInt(c.x));
            $('#<%=Y.ClientID%>').val(parseInt(c.y));
            $('#<%=W.ClientID%>').val(parseInt(c.w));
            $('#<%=H.ClientID%>').val(parseInt(c.h));
        }
        $(document).ready(function () {
            $('#<%= imgContent.ClientID%>').Jcrop({
                onSelect: SelectCropArea
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                $('#<%= imgContent.ClientID%>').Jcrop({
                    onSelect: SelectCropArea
                });
            });
        });

    </script>
</body>
</html>

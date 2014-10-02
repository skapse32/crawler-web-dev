<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateBuider.aspx.cs" Inherits="WebCrawler.Website.TemplateBuider" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/css/jcrop.css" rel="stylesheet" />
    <link href="Content/css/style-sub.css" rel="stylesheet" />
    <link href="Content/css/checkboxstyle.css" rel="stylesheet" />
    <link href="Content/css/reveal.css" rel="stylesheet" />
    <script src="http://code.jquery.com/jquery-1.6.min.js"></script>
    <script src="Content/js/jquery.Jcrop.js"></script>
    <script src="Content/js/jquery.easing.min.js"></script>
    <script src="Content/js/all_checkbox.js"></script>
</head>
<body>
    <form id="msform" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <ul id="progressbar">
            <li class="active" id="step1">Getting image</li>
            <li id="step2">Crop and Additional text</li>
            <li id="step3">Template</li>
            <li id="step4">Adding image</li>
            <li id="step5">Completed</li>
        </ul>
        <!-- fieldsets -->
        <asp:UpdatePanel runat="server" ID="UpdatePanelStep1">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgressStep1" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Content/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <fieldset runat="server" id="fieldStep1">
                    <h2 class="fs-title">Getting images</h2>
                    <asp:Button runat="server" ID="btnFirstStepNext" Style="width: 128px; height: 37px; padding: 2px;" Enabled="False" OnClick="btnFirstStepNext_OnClick" name="next" class="next action-button" Text="Next" OnClientClick="NextStep(2)" />
                    <br/>
                    <label for="txturl">Your link:</label>
                    <input type="text" id="txturl" runat="server" placeholder="Enter your link" />
                    <label for="txturl">Example: http://www.cars.com/vehicledetail/detail/610397293/photo </label>
                    <br />
                    <div style="float: left;">
                        <asp:Button type="button" class="action-button" ID="btnGetImage" Text="Get Images" runat="server" OnClick="btnGetImage_OnClick" />
                        <br />
                        <asp:CheckBox runat="server" ID="chkSelectAll" CssClass="second" AutoPostBack="True" OnCheckedChanged="chkSelectAll_OnCheckedChanged" Text="Check all" />
                    </div>
                    <div class="clear"></div>
                    <div class="check">
                        <asp:DataList ID="dtlListImage" Width="100%" runat="server" RepeatColumns="3">
                            <ItemTemplate>
                                <div class="box1">
                                    <div class='col_1_of_single1 span_1_of_single1'>
                                        <div class='view1 view-fifth1'>
                                            <div class='top_box'>
                                                <div class='m_2'>
                                                    <asp:CheckBox runat="server" ID="chkChooser" class="second" value='<%# Eval("link") %>' Text="Choose" AutoPostBack="True" OnCheckedChanged="chkChooser_OnCheckedChanged" />
                                                </div>
                                                <div class='grid_img'>
                                                    <div class='css3'>
                                                        <asp:Image ID="imgImage" CssClass="test" ImageUrl='<%# Eval("link") %>' runat="server" /><br />
                                                        <asp:TextBox Text='<%# Eval("link") %>' ID="txtImageUrl" runat="server" />
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
                </fieldset>
                <fieldset runat="server" id="fieldStep2">
                    <h2 class="fs-title">Crop and Additional text</h2>
                    <asp:Button runat="server" ID="btnSecondPrevious" OnClick="btnSecondPrevious_OnClick" Text="Previous" CssClass="previous action-button" OnClientClick="PreviousStep(2)" />
                    <asp:Button runat="server" ID="btnSecondNext" OnClick="btnSecondNext_OnClick" Text="Next" CssClass="previous action-button" OnClientClick="NextStep(3)" />
                    <br />
                    <table>
                        <tr>
                            <a href="#" class="btn btn-primary btn-large" id="selectImageForCrop" data-reveal-id="myModal">Choose image for cropping</a>
                            <br />
                            <asp:Button runat="server" Style="width: 150px; height: 37px; padding: 2px;" ID="PreviousImage" Enabled="False" OnClick="btnPreviousImage_Click" class="previous action-button" Text="Previous image" Width="150px" />
                            <asp:Button runat="server" Style="width: 150px; height: 37px; padding: 2px; margin-left: 5px" ID="btnNextImage" Enabled="False" OnClick="btnNextImage_OnClick" class="next action-button" Text="Next image" Width="150px" />
                            <asp:Button runat="server" Style="width: 128px; height: 37px; padding: 2px; margin-left: 5px" ID="btnCrop" Enabled="False" OnClick="btnCrop_OnClick" Text="Crop" OnClientClick="return ValidateSelected();" class="action-button" />
                        </tr>
                        <tr>
                            <td>Original image
                                <div style="width: 510px; height: 430px; overflow: auto; border: solid 1px black; padding: 10px; margin-bottom: 5px; position: relative;"
                                    id="imgContainer">
                                    <img runat="server" title="That's my auto :)" id="originalImage" />
                                </div>
                                <div style="display: none;">
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
                </fieldset>
                <fieldset runat="server" id="fieldStep3">
                    <h2 class="fs-title">Select Template</h2>
                    <asp:Button runat="server" ID="btnThirdPrevious" CssClass="previous action-button" OnClick="btnThirdPrevious_OnClick" Text="Previous" OnClientClick="PreviousStep(3)" />
                    <asp:Button runat="server" ID="btnThirdNext" CssClass="previous action-button" Text="Next" OnClick="btnThirdNext_OnClick" OnClientClick="NextStep(4)" />
                    <div class="check" style="overflow: auto">
                        <asp:DataList ID="dtlTemplate" Width="100%" runat="server" RepeatColumns="3">
                            <ItemTemplate>
                                <div class="box1">
                                    <div class='col_1_of_single1 span_1_of_single1'>
                                        <div class='view1 view-fifth1'>
                                            <div class='top_box'>
                                                <div class='m_2'>
                                                    <asp:CheckBox runat="server" CssClass="second" ID="chkTemplate" Text="Choose" />
                                                </div>
                                                <div class='grid_img'>
                                                    <div class='css3'>
                                                        <asp:Image ID="imgImage" ImageUrl='<%# Eval("templateImageCover") %>' runat="server" />
                                                        <asp:HiddenField runat="server" ID="hdIdTemplate" Value='<%# Eval("TemplateId") %>' />
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
                </fieldset>
                <fieldset runat="server" id="fieldStep4">
                    <h2 class="fs-title">Adding image</h2>
                    <asp:Button runat="server" ID="btnFourPrevious" CssClass="previous action-button" OnClick="btnFourPrevious_OnClick" Text="Previous" OnClientClick="PreviousStep(4)" />
                    <asp:Button runat="server" ID="btnFourNext" CssClass="next action-button" OnClick="btnFourNext_OnClick" Text="Next" OnClientClick="NextStep(5)" />
                    <br />
                        <asp:Button ID="btnPreview" OnClick="btnPreview_OnClick" class="action-button" Width="250px" Text="Creat Template" runat="server" />
                    <a href="View.aspx" target="_blank" class="btn btn-primary btn-large">Preview</a>
                    <div id="drag-drop">

                        <div class="column left first" id="sourceDiv">

                            <div class="sortable-list" id="sourceItem">
                                <asp:Literal runat="server" ID="ltrSourceItem"></asp:Literal>
                            </div>

                        </div>

                        <div class="column left" id="targetDiv">
                            <div class="sortable-list" id="targetItem">
                                <asp:Literal runat="server" ID="ltrTargetItem"></asp:Literal>
                            </div>
                        </div>
                        <div class="clearer">&nbsp;</div>
                        <input type="hidden" runat="server" id="targetImage" />
                    </div>
                    <div class="clearer">&nbsp;</div>
                </fieldset>
                <fieldset runat="server" id="fieldStep5">
                    <h2 class="fs-title">Completed</h2>
                    <asp:Button runat="server" ID="btnFive" CssClass="previous action-button" OnClick="btnFive_OnClick" Text="Previous" OnClientClick="PreviousStep(5)" />
                    <asp:Button runat="server" ID="btnSubmit" CssClass="previous action-button" OnClick="btnSubmit_OnClick" Text="Completed" />
                    <br/>
                    <div>
                        <label for="txtFolderName">Name:</label>
                        <input type="text" runat="server" id="txtFolderName" placeholder="Enter name your template"/>
                        <label for="txtDescription">Description:</label>
                        <input type="text" runat="server" id="txtDescription" placeholder="Enter name description"/>
                    </div>
                </fieldset>
                <div id="myModal" class="reveal-modal">
                    <h1>Choose image for cropping</h1>
                    <asp:Button runat="server" ID="btnBeginCropImage" OnClick="btnBeginCropImage_OnClick" Text="Begin crop" CssClass="submit action-button close-reveal-modal" />
                    <div style="overflow: auto; height: 400px;">
                        <asp:DataList ID="dtlImageCanCut" Width="100%" runat="server" RepeatColumns="3">
                            <ItemTemplate>
                                <div class="box1">
                                    <div class='col_1_of_single1 span_1_of_single1'>
                                        <div class='view1 view-fifth1'>
                                            <div class='top_box'>
                                                <div class='m_2'>
                                                    <asp:CheckBox runat="server" ID="chkChooserCancut" class="second" value='<%# Eval("ImagesLink") %>'
                                                        Text="Choose" />
                                                </div>
                                                <div class='grid_img'>
                                                    <div class='css3'>
                                                        <asp:Image ID="imgImageCanCut" CssClass="test" ImageUrl='<%# Eval("ImagesLink") %>' runat="server" /><br />
                                                        <asp:TextBox Text='<%# Eval("ImagesLink") %>' ID="txtImageUrl" runat="server" />
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
                <triggers>
                    <asp:PostBackTrigger ControlID="btnPreview" />
                </triggers>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField runat="server" ID="HdImageLinkChoosed" />
    </form>
    <script src="Content/js/jquery-ui-1.8.custom.min.js"></script>
    <script>
        var a = "";
        var HiddenField;
        var chkArray = [];
        var sourceArray = [];
        var targetArray = [];
        var indeximage = 0;
        var current_fs, next_fs, previous_fs; //fieldsets
        var left, opacity, scale; //fieldset properties which we will animate
        var animating; //flag to prevent quick multi-click glitches
        $(function () {
            $('#originalImage').Jcrop({
                onChange: showCoords,
                onSelect: showCoords
            });
            // Patch for IE to force "overflow: auto;"
            document.getElementById("imgContainer").style["position"] = "relative";
        });

        $(function () {
            $('a[data-reveal-id]').live('click', function (e) {
                //e.preventDefault();
                var modalLocation = $(this).attr('data-reveal-id');
                $('#' + modalLocation).reveal($(this).data());
            });

            /*---------------------------
             Extend and Execute
            ----------------------------*/

            $.fn.reveal = function (options) {


                var defaults = {
                    animation: 'fadeAndPop', //fade, fadeAndPop, none
                    animationspeed: 300, //how fast animtions are
                    closeonbackgroundclick: true, //if you click background will modal close?
                    dismissmodalclass: 'close-reveal-modal' //the class of a button or element that will close an open modal
                };

                //Extend dem' options
                var options = $.extend({}, defaults, options);

                return this.each(function () {

                    /*---------------------------
                     Global Variables
                    ----------------------------*/
                    var modal = $(this),
                        topMeasure = parseInt(modal.css('top')),
                        topOffset = modal.height() + topMeasure,
                        locked = false,
                        modalBG = $('.reveal-modal-bg');

                    /*---------------------------
                     Create Modal BG
                    ----------------------------*/
                    if (modalBG.length == 0) {
                        modalBG = $('<div class="reveal-modal-bg" />').insertAfter(modal);
                    }

                    /*---------------------------
                     Open & Close Animations
                    ----------------------------*/
                    //Entrance Animations
                    modal.bind('reveal:open', function () {
                        modalBG.unbind('click.modalEvent');
                        $('.' + options.dismissmodalclass).unbind('click.modalEvent');
                        if (!locked) {
                            lockModal();
                            if (options.animation == "fadeAndPop") {
                                modal.css({ 'top': $(document).scrollTop() - topOffset, 'opacity': 0, 'visibility': 'visible' });
                                modalBG.fadeIn(options.animationspeed / 2);
                                modal.delay(options.animationspeed / 2).animate({
                                    "top": $(document).scrollTop() + topMeasure + 'px',
                                    "opacity": 1
                                }, options.animationspeed, unlockModal());
                            }
                            if (options.animation == "fade") {
                                modal.css({ 'opacity': 0, 'visibility': 'visible', 'top': $(document).scrollTop() + topMeasure });
                                modalBG.fadeIn(options.animationspeed / 2);
                                modal.delay(options.animationspeed / 2).animate({
                                    "opacity": 1
                                }, options.animationspeed, unlockModal());
                            }
                            if (options.animation == "none") {
                                modal.css({ 'visibility': 'visible', 'top': $(document).scrollTop() + topMeasure });
                                modalBG.css({ "display": "block" });
                                unlockModal()
                            }
                        }
                        modal.unbind('reveal:open');
                    });

                    //Closing Animation
                    modal.bind('reveal:close', function () {
                        if (!locked) {
                            lockModal();
                            if (options.animation == "fadeAndPop") {
                                modalBG.delay(options.animationspeed).fadeOut(options.animationspeed);
                                modal.animate({
                                    "top": $(document).scrollTop() - topOffset + 'px',
                                    "opacity": 0
                                }, options.animationspeed / 2, function () {
                                    modal.css({ 'top': topMeasure, 'opacity': 1, 'visibility': 'hidden' });
                                    unlockModal();
                                });
                            }
                            if (options.animation == "fade") {
                                modalBG.delay(options.animationspeed).fadeOut(options.animationspeed);
                                modal.animate({
                                    "opacity": 0
                                }, options.animationspeed, function () {
                                    modal.css({ 'opacity': 1, 'visibility': 'hidden', 'top': topMeasure });
                                    unlockModal();
                                });
                            }
                            if (options.animation == "none") {
                                modal.css({ 'visibility': 'hidden', 'top': topMeasure });
                                modalBG.css({ 'display': 'none' });
                            }
                        }
                        modal.unbind('reveal:close');
                    });

                    /*---------------------------
                     Open and add Closing Listeners
                    ----------------------------*/
                    //Open Modal Immediately
                    modal.trigger('reveal:open')

                    //Close Modal Listeners
                    var closeButton = $('.' + options.dismissmodalclass).bind('click.modalEvent', function () {
                        modal.trigger('reveal:close')
                    });

                    if (options.closeonbackgroundclick) {
                        modalBG.css({ "cursor": "pointer" })
                        modalBG.bind('click.modalEvent', function () {
                            modal.trigger('reveal:close')
                        });
                    }
                    $('body').keyup(function (e) {
                        if (e.which === 27) { modal.trigger('reveal:close'); } // 27 is the keycode for the Escape key
                    });


                    /*---------------------------
                     Animations Locks
                    ----------------------------*/
                    function unlockModal() {
                        locked = false;
                    }
                    function lockModal() {
                        locked = true;
                    }

                });//each call
            }//orbit plugin call
        });

        $(document).ready(function () {
            var a = "";
            var HiddenField;
            $('#drag-drop .sortable-list').sortable({
                connectWith: '#drag-drop .sortable-list',
                update: function () {
                    //iterate through the textboxes and place their values in the correct hidden field
                    $("#targetItem").find("img").each(function () {
                        HiddenField = document.getElementById("targetImage");
                        a = a + $(this).attr('src') + "|";
                        HiddenField.value = a;
                    });
                    a = "";
                }
            }).disableSelection();
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
                            $("#targetItem").find("img").each(function () {
                                HiddenField = document.getElementById("targetImage");
                                a = a + $(this).attr('src') + "|";
                                HiddenField.value = a;
                            });
                            a = "";
                        }
                    }).disableSelection();

                }
            });
        };

        function NextStep(e) {
            $('#step' + e).addClass("active");
        }

        function PreviousStep(e) {
            $('#step' + e).removeClass("active");
        }

        function setDefault() {
            indeximage = 0;
            $("#imdex").val(indeximage);
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
            sourceArray = $('#imageCompleted').val().split('|');
            sourceArray.splice(0, 1);
            var myDiv = $('#sourceItem');
            myDiv.empty();
            var myDiv1 = $('#targetItem');
            myDiv1.empty();
            var tmp = "";
            for (var i = 0; i < sourceArray.length; i++) {
                tmp = "<img class='sortable-item'  width='400' height='150' src='" + sourceArray[i] + "' />";
                myDiv.append(tmp);
            }
        }


    </script>
</body>
</html>

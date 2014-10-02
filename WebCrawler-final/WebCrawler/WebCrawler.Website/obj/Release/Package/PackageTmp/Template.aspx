<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="WebCrawler.Website.Template" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <link href="Content/css/style-sub.css" rel="stylesheet" />
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/css/jcrop.css" rel="stylesheet" />
    <link href="Content/css/checkboxstyle.css" rel="stylesheet" />
    <script src="Content/Script/jquery-1.7.1.js"></script>
    <script src="Content/js/jquery.Jcrop.js"></script>
    <link href="Content/css/bootstrap.css" rel="stylesheet" />
    <script src="Content/Script/bootstrap.min.js"></script>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".btn").click(function () {
                $("#myModal").modal('show');
            });
        });
    </script>
</head>
<body>
    <form id="msform" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <ul id="progressbar">
            <li class="active" id="step1">Getting image</li>
            <li id="step2">Selecting images</li>
            <li id="step3">Croping images</li>
            <li id="step4">Adding image</li>
            <li id="step5">Completed</li>
        </ul>
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
                    <asp:Button runat="server" ID="btnFirstStepNext" Style="width: 150px" OnClick="btnFirstStepNext_OnClick" name="next" class="next action-button" Text="Get Image & Nex" OnClientClick="NextStep(2)" />
                    <br />
                    <label for="txturl">Your link:</label>
                    <input type="text" id="txturl" runat="server" placeholder="Enter your link" />
                    <label for="txturl">Example: http://www.cars.com/vehicledetail/detail/610397293/photo </label>

                </fieldset>
                <fieldset runat="server" id="fieldStep2">
                    <h2 class="fs-title">Crop and Additional text</h2>
                    <asp:Button runat="server" ID="btnSecondPrevious" OnClick="btnSecondPrevious_OnClick" Text="Previous" CssClass="previous action-button" OnClientClick="PreviousStep(2)" />
                    <asp:Button runat="server" ID="btnSecondNext" OnClick="btnSecondNext_OnClick" Text="Next" CssClass="previous action-button" OnClientClick="NextStep(3)" />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:CheckBox runat="server" ID="chkSelectAll" CssClass="second" AutoPostBack="True" OnCheckedChanged="chkSelectAll_OnCheckedChanged" Text="Check all" />
                        </div>
                        <div class="col-md-12">
                            <div class="row">
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
                        </div>
                    </div>
                </fieldset>
                <fieldset runat="server" id="fieldStep3">
                    <h2 class="fs-title">Croping image</h2>
                    <asp:Button runat="server" ID="btnThirdPrevious" CssClass="previous action-button" OnClick="btnThirdPrevious_OnClick" Text="Previous" OnClientClick="PreviousStep(3)" />
                    <asp:Button runat="server" ID="btnThirdNext" CssClass="previous action-button" Text="Next" OnClick="btnThirdNext_OnClick" OnClientClick="NextStep(4)" />
                    <br />
                    <div class="row">
                        <div class="form-group">
                            <label>Using:</label>
                            <label>1. Button Crop to croping image after choose area on image</label>
                            <br />
                            <label>1. Keyup (or keyleft in keybroad) to previous image</label>
                            <br />
                            <label>1. Keydown (or keyright in keybroad) to next image</label>
                        </div>
                        <div>
                            <div style="display: none">
                                <asp:Button runat="server" Style="width: 150px; height: 37px; padding: 2px;" ID="btnPreviousImage" OnClick="btnPreviousImage_OnClick" class="previous action-button" Text="Previous image" Width="150px" />
                                <asp:Button runat="server" Style="width: 150px; height: 37px; padding: 2px; margin-left: 5px" ID="btnNextImage" OnClick="btnNextImage_OnClick" class="next action-button" Text="Next image" Width="150px" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="box box-warning">
                                <div class="box-header">
                                    <h3 class="box-title">Options</h3>
                                </div>
                                <!-- /.box-header -->
                                <div class="box-body">
                                    <div class="form-group">
                                        <label>Additional text to image</label>
                                        <input type="text" placeholder="Enter Additional text" id="txtAdditionalText" runat="server" />
                                        <asp:Button runat="server" Style="width: 128px; height: 37px; padding: 2px; margin-left: 5px" ID="btnCrop" OnClick="btnCrop_OnClick" Text="Crop" OnClientClick="return ValidateSelected();" class="action-button btn-primary" />
                                        <button class="btn btn-primary" id="btnShow1" data-toggle="modal" data-target="#ChooserImage">Choose image for croping</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-9" style="width: 100%; height: 500px; overflow: auto; border: solid 1px black; padding: 10px; margin-bottom: 5px; position: relative;">
                            <img runat="server" title="That's my auto :)" id="originalImage" />
                        </div>

                    </div>
                    <div style="display: none;">
                        <input type="text" size="4" id="x1" name="x1" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                        <input type="text" size="4" id="y1" name="y1" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                        <input type="text" size="4" id="x2" name="x2" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                        <input type="text" size="4" id="y2" name="y2" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                        <input type="text" size="4" id="w" name="w" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                        <input type="text" size="4" id="h" name="h" class="coor" style="width: 20px; height: 30px" readonly="readonly" />
                    </div>
                </fieldset>
                <fieldset runat="server" id="fieldStep4">
                    <h2 class="fs-title">Select template & Adding image</h2>
                    <asp:Button runat="server" ID="btnFourPrevious" CssClass="previous action-button" OnClick="btnFourPrevious_OnClick" Text="Previous" OnClientClick="PreviousStep(4)" />
                    <asp:Button runat="server" ID="btnFourNext" CssClass="next action-button" OnClick="btnFourNext_OnClick" Text="Next" OnClientClick="NextStep(5)" />
                    <br />
                    <asp:Button runat="server" CssClass="action-button" ID="btnPreview" OnClick="btnPreview_OnClick" Text="Preview" />
                    <button class="btn btn-primary" id="btnShow" data-toggle="modal" data-target="#largeModal">Choose template for adding image</button>
                    <label for="drag-drop">Eg: Drag image from left to right. Drag to sortable images</label>
                    <div id="drag-drop">
                        <label for="sourceDiv">Remove image</label>
                        <div class="column left" id="targetDiv" style="height: 240px; overflow-y: hidden; max-width: 960px; margin-left: 0px">
                            <div class="sortable-list" id="targetItem" style="width: 999999999999px;">
                                <asp:Literal runat="server" ID="ltrTargetItem"></asp:Literal>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <label for="sourceDiv">Use image</label>
                        <div class="column left first" id="sourceDiv" style="overflow-x: hidden; width:960px; margin-left: 0px">
                            <div class="sortable-list" id="sourceItem">
                                <asp:Literal runat="server" ID="ltrSourceItem"></asp:Literal>
                            </div>

                        </div>
                        
                        
                        <div class="clearer">&nbsp;</div>
                        <input type="hidden" runat="server" id="targetImage" />
                        <input type="hidden" runat="server" id="targetTitle" />
                    </div>
                    <div class="clearer">&nbsp;</div>
                </fieldset>
                <fieldset runat="server" id="fieldStep5">
                    <h2 class="fs-title">Completed</h2>
                    <asp:Button runat="server" ID="btnFive" CssClass="previous action-button" OnClick="btnFive_OnClick" Text="Previous" OnClientClick="PreviousStep(5)" />
                    <asp:Button runat="server" ID="btnSubmit" CssClass="previous action-button" OnClick="btnSubmit_OnClick" Text="Completed" />
                    <br />
                    <div>
                        <label for="txtFolderName">Name:</label>
                        <input type="text" runat="server" id="txtFolderName" placeholder="Enter name your template" />
                        <label for="txtDescription">Description:</label>
                        <input type="text" runat="server" id="txtDescription" placeholder="Enter name description" />
                    </div>
                </fieldset>
                <!-- Large modal -->
                <button class="btn btn-primary" id="btnShowPopup" data-toggle="modal" data-target="#largeModal">Choose template for adding image</button>
                <div id="largeModal" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-lg" style="width: 90%; height: 90%; position: absolute; left: 5%; top: 5%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Template gallery</h4>
                            </div>
                            <div class="modal-body" style="overflow: auto">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:DataList ID="dtlTemplate" Width="100%" runat="server" RepeatColumns="3">
                                            <ItemTemplate>
                                                <div class="box1">
                                                    <div class='col_1_of_single1 span_1_of_single1'>
                                                        <div class='view1 view-fifth1'>
                                                            <div class='top_box'>
                                                                <div class='m_2'>
                                                                    <asp:RadioButton GroupName="country" runat="server" ID="rboChooser" AutoPostBack="True" class="second" Text="Choose" OnCheckedChanged="rboChooser_OnCheckedChanged" />
                                                                </div>
                                                                <div class='grid_img'>
                                                                    <div class='css3'>
                                                                        <asp:Image ID="imgImageTemplate" ImageUrl='<%# Eval("templateImageCover") %>' runat="server" />
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
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="remove()" class="btn btn-default" data-dismiss="modal">OK</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Div1" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Template gallery</h4>
                            </div>
                            <div class="modal-body">
                                <p>The default you choose all image to crop in next step. If you click "<span style="color: red; font-weight: bold">continue</span>", you will have all image to crop in next step. 
                                    If you click "<span style="color: red; font-weight: bold">Yes</span>", you can choose one or another image to crop in next step</p>
                                <p>Are you choose image for crop in next step?</p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="ShowModalCrop()" class="btn btn-default" data-dismiss="modal">Yes</button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Continue</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="ChooserImage" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-lg" style="width: 90%; height: 90%; position: absolute; left: 5%; top: 5%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Template gallery</h4>
                            </div>
                            <div class="modal-body" style="overflow: auto">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:DataList ID="dtlListImageCrop" Width="100%" runat="server" RepeatColumns="3">
                                            <ItemTemplate>
                                                <div class="box1">
                                                    <div class='col_1_of_single1 span_1_of_single1'>
                                                        <div class='view1 view-fifth1'>
                                                            <div class='top_box'>
                                                                <div class='m_2'>
                                                                    <asp:CheckBox runat="server" ID="rboChooserCrop" class="second" Text="Choose" />
                                                                </div>
                                                                <div class='grid_img'>
                                                                    <div class='css3'>
                                                                        <asp:Image ID="imgImageTemplateCrop" ImageUrl='<%# Eval("ImagesLink") %>' runat="server" />
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
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnOK" OnClick="btnOK_OnClick" CssClass="btn btn-default action-button" Text="OK"/>
                                 <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
    <script src="Content/js/jquery-ui-1.8.custom.min.js"></script>
    <script>
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

        function remove() {
            var a = $('.modal-backdrop');
            if (a != null)
                a.removeClass("modal-backdrop");
        }

        function addTextbox() {
            $('.sortable-item').append("<input type='text' placeholder='Add title text here' >");
        }

        document.addEventListener("keydown", function (evt) {
            if (evt.keyCode == 37 || evt.keyCode == 40) {
                $('#<%= btnPreviousImage.ClientID %>').trigger("click");
            }
            if (evt.keyCode == 39 || evt.keyCode == 38) {
                $('#<%= btnPreviousImage.ClientID %>').trigger("click");
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
                            $("#targetItem").find("input").each(function () {
                                HiddenField1 = document.getElementById("targetTitle");
                                a = a + $(this).val() + "|";
                                HiddenField1.value = a;
                            });
                            a = "";
                        }
                    }).disableSelection();
                    $("#btnShow").click(function () {
                        $('#largeModal').modal('show');
                    });
                }
            });
        };

        function NextStep(e) {
            $('#step' + e).addClass("active");
        }

        function PreviousStep(e) {
            $('#step' + e).removeClass("active");
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

        function ShowModalCrop() {
            $('#ChooserImage').modal('show');
        }

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
            var b = $(e).parent('.sortable-item');
            $('#targetItem').append(b);
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
    </script>
</body>
</html>

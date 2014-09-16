<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cropping with jquery & ASP.NET - By Miron Abramson</title>
    <link rel="stylesheet" href="style/jcrop.css" type="text/css" />

    <script src="js/jquery.min.js" type="text/javascript"></script>

    <script src="js/jquery.Jcrop.js" type="text/javascript"></script>

    <script src="js/crop.js" type="text/javascript"></script>

</head>
<body onload="Init();">
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>Original image
                <div style="width: 400px; height: 400px; overflow: auto; border: solid 1px black;
                    padding: 10px; margin-bottom: 5px;" id="imgContainer">
                    <img src="images/02.jpg" alt="My auto" title="That's my auto :)" id="originalImage" />
                </div>
                <div>
                    X1
                    <input type="text" size="4" id="x1" name="x1" class="coor" readonly="readonly" />
                    Y1
                    <input type="text" size="4" id="y1" name="y1" class="coor" readonly="readonly" />
                    X2
                    <input type="text" size="4" id="x2" name="x2" class="coor" readonly="readonly" />
                    Y2
                    <input type="text" size="4" id="y2" name="y2" class="coor" readonly="readonly" />
                    W
                    <input type="text" size="4" id="w" name="w" class="coor" readonly="readonly" />
                    H
                    <input type="text" size="4" id="h" name="h" class="coor" readonly="readonly" />
                </div>
                <div>
                    <asp:Button runat="server" ID="btnCrop" OnClick="btnCrop_Click" Text="Crop" OnClientClick="return ValidateSelected();" />
                </div>
            </td>
            <td style="vertical-align:top;">
                Cropped image:
                <div style="width: 400px; height: 400px; overflow: auto; border: solid 1px black;
                    padding: 10px; margin-bottom: 5px;">
                    <asp:Literal runat="server" ID="lblCroppedImage"></asp:Literal>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

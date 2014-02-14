<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ImageGallery.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html lang="sv" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bildgalleri</title>
    <script src="Scripts/ImageGallery.js"></script>
    <link href="~/styles/style.css" rel="stylesheet" />
    <meta charset="UTF-8">
</head>
<body>
    <div id="wrap">
        <div id="header">
            <h1>Bildgalleri</h1>
        </div>
        <div id="content">
            <form id="form1" runat="server">
                <div id="PreviewImageContainer">
                    <asp:Image ID="PreviewImage" runat="server" Visible="false" />
                </div>
                <div id="ImageListContainer">
                    <asp:Repeater ID="ImageRepeater" runat="server" ItemType="System.String" SelectMethod="ImageRepeater_GetData">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "?i=" + Item.ToString() %>'>
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# @"~/Content/Images/thumbs/" + Item.ToString() %>' />
                                </asp:HyperLink>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div id="UploadFormContainer">
                    <asp:PlaceHolder ID="phSuccess" runat="server" Visible="false">
                        <div id="UploadSuccessContainer">
                            <asp:Label ID="lblUploadSuccess" runat="server"></asp:Label>
                            <span class="closebutton">X</span>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phFail" runat="server" Visible="false">
                        <div id="UploadFailContainer">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                        </div>
                    </asp:PlaceHolder>

                    <asp:FileUpload ID="fiUploadGalleryImage" runat="server" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ogiltig filtyp." Text="*" ControlToValidate="fiUploadGalleryImage" ValidationExpression="(?i:^.*\.(gif|jpg|png)$)"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Måste välja en fil att ladda upp" Text="*" Display="Dynamic" ControlToValidate="fiUploadGalleryImage"></asp:RequiredFieldValidator>
                    <asp:Button ID="btnUploadImage" runat="server" Text="Ladda upp bild" OnClick="btnUploadImage_Click" />
                </div>
            </form>
        </div>
        <div id="footer">
            <p>ASP.NET WebForms - lnu</p>
            <p>Laboration 2.1: Bildgalleri</p>
            <p>Andreas Fridlund - afrxx09</p>
        </div>
    </div>
</body>
</html>

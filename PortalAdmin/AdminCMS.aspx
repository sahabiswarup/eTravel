<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master" AutoEventWireup="true" CodeBehind="AdminCMS.aspx.cs" Inherits="e_Travel.PortalAdmin.AdminCMS" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            <asp:Label ID="CMSHeaderLabel" runat="server" ></asp:Label>
        </div>
        <div class="BoxBody">
            <div class="BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>
                                <%--<asp:Label ID="BusinessNameLabel" runat="server"></asp:Label>--%>
                            </legend>
                            <div class="row">
                                <asp:Label id = "ContentLevel" runat="server" Font-Bold="True" ></asp:Label>
                                <CKEditor:CKEditorControl ID="ContentCKEditor" BasePath="~/Scripts/CkEditor" runat="server"
                                    Height="300"></CKEditor:CKEditorControl>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="SaveButton" runat="server" Text=" Save / Approve " CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="110px" OnClick="SaveButton_Click" />
                                <asp:Button ID="RejectButton" runat="server" Text=" Reject Change Request " CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="150px" OnClick="RejectButton_Click" />
                            </div>
                        </fieldset>
                        <fieldset id = "ExistingContentFieldSet" runat = "server">
                            <legend><span id = "ExistingContentSpan" runat = "server"></span></legend>
                            <div style = "width: 100%; float: left; text-align: justify;">
                                <asp:Label ID="ExistingContentLabel" runat="server"></asp:Label>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

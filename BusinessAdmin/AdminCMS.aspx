<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master" AutoEventWireup="true" CodeBehind="AdminCMS.aspx.cs" Inherits="e_Travel.BusinessAdmin.AdminCMS" %>
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
                            <div class="row">
                                <asp:Label id = "ContentLevel" runat="server" Font-Bold="True"></asp:Label>
                                <CKEditor:CKEditorControl ID="ContentCKEditor" BasePath="~/Scripts/CkEditor" runat="server"
                                    Height="200"></CKEditor:CKEditorControl>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="SaveButton" runat="server" Text=" Submit Content Change Request " CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="230px" OnClick="SaveButton_Click" />
                            </div>
                        </fieldset>
                        <fieldset id = "ExistingContentFieldSet" runat = "server">
                            <legend><span id = "ExistingContentSpan" runat = "server"></span></legend>
                            <div class="row">
                                <asp:Label ID="ExistingContentLabel" runat="server"></asp:Label>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master"
    AutoEventWireup="true" CodeBehind="BusinessTestimonails.aspx.cs" Inherits="e_Travel.PortalAdmin.BusinessTestimonails" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            <asp:Label ID="CMSHeaderLabel" runat="server"></asp:Label>
        </div>
        <div class="BoxBody">
            <div class="BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
     
                        <fieldset>
                            <legend>
                                <asp:Label ID="BusinessNameLabel" runat="server"></asp:Label>
                            </legend>
                            <div class="row">
                                <label for="ClientMessageCKEditor">
                                    Client Name</label>
                                <asp:TextBox ID="ClientNameTextBox" runat="server" Width="670px" EnableViewState="true"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="ClientMessageCKEditor">
                                    Client Message</label>
                                <div style="clear: both;">
                                </div>
                                <CKEditor:CKEditorControl ID="ClientMessageCKEditor" BasePath="~/Scripts/CkEditor"
                                    runat="server" Height="200"></CKEditor:CKEditorControl>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="AddButton" runat="server" Text=" Add " CssClass="FromBodyButton"
                                    Width="80px" OnClick="AddButton_Click" />
                                <asp:Button ID="EditButton" runat="server" Text=" Edit " CssClass="FromBodyButton"
                                    Width="80px" OnClick="EditButton_Click" />
                                <asp:Button ID="SaveButton" runat="server" Text=" Save " CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="80px" OnClick="SaveButton_Click" />
                                <asp:Button ID="ApproveButton" runat="server" Text="Approved" CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="80px" OnClick="ApproveButton_Click" />
                                <asp:Button ID="RejectButton" runat="server" Text="Reject" CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="92px" OnClick="RejectButton_Click" />
                                <asp:Button ID="ClearButton" runat="server" Text=" Clear " CssClass="FromBodyButton"
                                    Width="80px" OnClick="ClearButton_Click" />
                            </div>
                        </fieldset>
                        <%--   </ContentTemplate>
                </asp:UpdatePanel>--%>
                        <fieldset>
                           <div style="display: inline-block; width: 300px; font-weight: bold; margin-bottom:10px;">
                            <asp:LinkButton ID="AllLinkButton" runat="server" ForeColor="Red" CssClass="FromBodyButton" Text="All" OnClick="AllLinkButton_Click"></asp:LinkButton>
                            <asp:LinkButton ID="ReqLinkButton" runat="server" ForeColor="Red" CssClass="FromBodyButton" Text="Requested" OnClick="ReqLinkButton_Click"></asp:LinkButton>
                            <asp:LinkButton ID="AppLinkButton" runat="server" Text="Approved" CssClass="FromBodyButton" ForeColor="Red" OnClick="AppLinkButton_Click"></asp:LinkButton>
                            <asp:LinkButton ID="RejLinkButton" runat="server" Text="Rejected" CssClass="FromBodyButton" ForeColor="Red"  OnClick="RejLinkButton_Click"></asp:LinkButton>
                            </div>
                            
                            <gridUC:CustomGridView ID="TestimonialsDtlsCustomGridView" runat="server" Width="100%" />
                        </fieldset>
                        <asp:HiddenField ID="LinkStatusHiddenField" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master" AutoEventWireup="true" CodeBehind="ReqTestimonials.aspx.cs" Inherits="e_Travel.BusinessAdmin.ReqTestimonials" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class = "FromBody">
        <div class = "BoxHeader">Testimonials  </div>
        <div class = "BoxBody">
            <div class = "BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Testimonails</legend>
                            <div class="row">
                                <label for="ClientNameTextBox">Client Name </label>
                                <asp:TextBox ID="ClientNameTextBox" runat="server" TextMode="SingleLine" MaxLength = "128" width = "660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="ClientMessageCKEditor">Client Message</label>
                                 <div style="clear:both;"></div>
                                <CKEditor:CKEditorControl ID="ClientMessageCKEditor" BasePath="~/Scripts/CkEditor"
                                    runat="server" Height="200" ></CKEditor:CKEditorControl>
                            </div>
                            
                        </fieldset>
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="AddButton" runat="server" Text=" Add " 
                                    CssClass="FromBodyButton"  Width = "80px" 
                                    onclick="AddButton_Click" />
                                <asp:Button ID="EditButton" runat="server" Text=" Edit " 
                                    CssClass="FromBodyButton"  Width = "80px" 
                                    onclick="EditButton_Click" />
                                <asp:Button ID="SaveButton" runat="server" Text=" Save " 
                                    CssClass="FromBodyButton"  ValidationGroup="ValidateData" Width = "80px" 
                                    onclick="SaveButton_Click" />
                                <asp:Button ID="ClearButton" runat="server" Text=" Clear " 
                                    CssClass="FromBodyButton"  Width = "80px" 
                                    onclick="ClearButton_Click" />
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <fieldset>
                    <gridUC:CustomGridView ID="TestimonailsCustomGridView" runat="server" Width = "100%"/>
                </fieldset>

            </div>
        </div>
    </div>
</asp:Content>

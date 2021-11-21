<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SchoolAdmin.Master" AutoEventWireup="true"
    CodeBehind="RegisterBulkUser.aspx.cs" Inherits="SchoolWebSite.Account.RegisterBulkUser" Theme="AdminTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contentHead">
        District Master</div>
    <div class="pageContentWrapper">
        <asp:Panel runat="server" ID="Panel1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <fieldset style="width: 90%; margin-bottom: 10px; height: 120px">
                        <legend>Register Bul User..</legend>
                        <div style="width: 100%; margin-bottom: 10px; clear: both;">
                        </div>
                        <div class="divLeft">
                            <div class="divLabelArea">
                                Role </div>
                            <div class="divText">
                               <asp:DropDownList ID="ddlNodeUserRole" runat="server" Height="21px" Width="230px"
                                    AppendDataBoundItems="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                    </fieldset>
                    <div class="divLeft">
                    </div>
                    <div class="divLeft">
                    </div>
                    <div style="padding-top: 100px; width: 100%; padding-bottom: 10px">
                        <asp:Button ID="btnCreateUser" runat="server" SkinID="buttonSkin" Text="Add" Width="87px"
                            OnClick="btnCreateUser_Click" />
                    </div>
                    <hr />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>

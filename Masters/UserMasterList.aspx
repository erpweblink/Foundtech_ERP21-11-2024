﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/AdminMaster.master" CodeFile="UserMasterList.aspx.cs" Inherits="Masters_UserMasterList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Content/css/Griddiv.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.all.min.js"></script>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.min.css' />
    <script>
        function HideLabelerror(msg) {
            Swal.fire({
                icon: 'error',
                text: msg,

            })
        };
        function HideLabel(msg) {

            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 5000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "UserMaster.aspx";
            })
        };
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid px-4">
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="row">
                        <div class="col-9 col-md-10">
                            <h4 class="mt-4">&nbsp <b>USER LIST</b></h4>

                        </div>
                        <div class="col-3 col-md-2 mt-4">
                            <asp:Button ID="btnCreate" CssClass="form-control btn btn-warning" OnClick="btnCreate_Click" runat="server" Text="Create" />
                        </div>
                    </div>
                    <br />
                    <div>
                        <div class="row">
                            <div class="table ">
                                <asp:GridView ID="GVUser" runat="server" CellPadding="4" DataKeyNames="id" PageSize="10" AllowPaging="true" Width="100%"
                                    CssClass="display table table-striped table-hover" AutoGenerateColumns="false" OnRowCommand="GVUser_RowCommand" OnPageIndexChanging="GVUser_PageIndexChanging" OnRowDataBound="GVUser_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Code" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="usercode" runat="server" Text='<%#Eval("UserCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="username" runat="server" Text='<%#Eval("Username")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Password" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="Password" runat="server" Text='<%#Eval("Password")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No." HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="Mobileno" runat="server" Text='<%#Eval("Mobileno")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email ID" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="Email" runat="server" Text='<%#Eval("EmailID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Role" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="role" runat="server" Text='<%#Eval("Role")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="Status" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTION" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" Height="27px" CausesValidation="false" CommandName="RowEdit" CommandArgument='<%#Eval("ID")%>'><i class='fas fa-edit' style='font-size:24px;color: #212529;'></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" Height="27px" ToolTip="Delete" CausesValidation="false" CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%#Eval("ID")%>'><i class='fas fa-trash' style='font-size:24px;color: red;'></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>

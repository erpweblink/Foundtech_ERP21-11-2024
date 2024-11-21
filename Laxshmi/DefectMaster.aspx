﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/LaxmiMaster.master" CodeFile="DefectMaster.aspx.cs" Inherits="Laxshmi_DefectMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .LblStyle {
            font-weight: 500;
            color: black;
        }

        .spncls {
            color: red;
        }
    </style>
    <!---Number--->
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.all.min.js"></script>
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
                timer: 2500,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "Admin/UserMasterList.aspx";
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
                    <div class="container-fluid px-3">
                        <div class="row">
                            <div class="col-md-10">
                                <h4 class="mt-4">&nbsp <b>DEFECT MASTER</b></h4>                             
                            </div>
                            <div class="col-md-2 mt-4">
                                <asp:LinkButton ID="Button1" CssClass="form-control btn btn-warning" Font-Bold="true" CausesValidation="false" runat="server" OnClick="Button1_Click">
    <i class="fas fa-file-alt"></i>&nbsp List
                                </asp:LinkButton>
                            </div>
                        </div>
                        <hr />
                        <div class="card mb-4">

                            <div class="card-body ">
                                <div class="row">
                                    <div class="col-md-6 col-12 mb-3">
                                        <label for="lblDefectname" class="form-label LblStyle">Defect Name : <span class="spncls">*</span></label>
                                        <asp:TextBox ID="txtDefectname" CssClass="form-control" placeholder="Enter Defect name" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Defect name" ControlToValidate="txtDefectname" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 col-12 mb-3">
                                        <label for="lblDefectcode" class="form-label LblStyle">Defect Code :</label>
                                        <asp:TextBox ID="txtDefectode" CssClass="form-control" ReadOnly="true" ForeColor="red" placeholder="Enter Defect Code" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <br />
                                <div class="row">
                                    <div class="col-md-4"></div>
                                    <div class="col-6 col-md-2">
                                        <asp:Button ID="btnsave" OnClick="btnsave_Click" CssClass="form-control btn btn-outline-primary m-2" runat="server" Text="Save" />
                                    </div>
                                    <div class="col-6 col-md-2">
                                        <asp:Button ID="btncancel" OnClick="btncancel_Click" CssClass="form-control btn btn-outline-danger m-2" runat="server" Text="Cancel" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hhd" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/LaxmiMaster.master" CodeFile="LaxshmiDashboard.aspx.cs" Inherits="Laxshmi_LaxshmiDashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <%--  <script src="../Content/assets/js/plugin/sweetalert/sweetalert.min.js"></script>
    <script>
        jQuery(document).ready(function () {
            SweetAlert2Demo.init();
        });
        var SweetAlert2Demo = (function () {
            //== Demos
            var initDemos = function () {
                $("#alert_demo_3_3").click(function (e) {
                    swal("Good job!", "You clicked the button!", {
                        icon: "success",
                        buttons: {
                            confirm: {
                                className: "btn btn-success",
                            },
                        },
                    });
                });
            }
        });
    </script>--%>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <br />
    <form runat="server">


        <div class="row">
            <div class="col-sm-6 col-md-3">
                <div class="card card-stats card-round">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div
                                    class="icon-big text-center icon-success bubble-shadow-small">
                                    <i class="fas fa-luggage-cart"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Customers</p>
                                    <h4 class="card-title">
                                        <asp:Label ID="lblcustomers" runat="server"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-3">
                <div class="card card-stats card-round">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div
                                    class="icon-big text-center icon-primary bubble-shadow-small">
                                    <i class="fas fa-cubes"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Material</p>
                                    <h4 class="card-title">
                                        <asp:Label ID="lblMaterial" runat="server"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-3">
                <div class="card card-stats card-round">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div
                                    class="icon-big text-center icon-warning bubble-shadow-small">
                                    <i class="fas fa-arrow-alt-circle-down"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Inward Quantity</p>
                                    <h4 class="card-title">
                                        <asp:Label ID="lblInwardQuantity" runat="server"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-3">
                <div class="card card-stats card-round">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div
                                    class="icon-big text-center icon-success bubble-shadow-small">
                                    <i class="fas fa-arrow-alt-circle-up"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Ouward Quantity</p>
                                    <h4 class="card-title">
                                        <asp:Label ID="lblOuwardQuantity" runat="server"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row form-control">
            <div class="col-md-11">
                <asp:DropDownList ID="ddlType" Width="120px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" Font-Bold="true" CssClass="form-select form-control-lg">
                    <asp:ListItem Text="ToDay" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Month" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Year" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </div>
<%--       <asp:Button ID="alert_demo_3_3" runat="server" text="save" />--%>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="card card-stats card-round form-control">
                    <div class="card-body">
                        <asp:GridView ID="GVCustomerList" runat="server" CellPadding="4" Width="100%"
                            CssClass="table table-head-bg-primary mt-4" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No." HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RowMaterial" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Label ID="RowMaterial" runat="server" Text='<%#Eval("RowMaterial")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inward Qty" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Label ID="InwardQty" runat="server" Text='<%#Eval("TotalInwardQty")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Outward Qty" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Label ID="OutwardQty" runat="server" Text='<%#Eval("TotalOutwardQty")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </form>

</asp:Content>


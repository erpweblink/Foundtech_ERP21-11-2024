﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/LaxmiMaster.master" CodeFile="LaxshmiDashboard.aspx.cs" Inherits="Laxshmi_LaxshmiDashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <br />
    <br />
     <br />
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
      
    </div>



</asp:Content>


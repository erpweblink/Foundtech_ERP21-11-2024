﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Store_StoreList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    CommonCls objcls = new CommonCls();
    DataTable Dt_Component = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserCode"] == null)
        {
            Response.Redirect("../Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                GetstoreList();          
            }
        }


    }



    protected void GVPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPurchase.Rows[rowIndex];
            var inwardNo = GVPurchase.DataKeys[rowIndex]["InwardNo"].ToString();
            var ID = GVPurchase.DataKeys[rowIndex]["ID"].ToString();
            string RowMaterial = ((Label)row.FindControl("RowMaterial")).Text;
            string Thickness = ((Label)row.FindControl("Thickness")).Text;
            string Width = ((Label)row.FindControl("Width")).Text;
            string Length = ((Label)row.FindControl("Length")).Text;  
            string Weight = ((Label)row.FindControl("Weight")).Text;  
            string NeedQty = ((Label)row.FindControl("NeedQty")).Text;  
            GetAvailableDetals(RowMaterial, Thickness, Width, Length, NeedQty, Weight);           
            HDnInward.Value = Convert.ToString(inwardNo);
            HddnID.Value = Convert.ToString(ID);
            this.ModalPopupHistory.Show();
         
        }

        if (e.CommandName == "RejectCancel")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPurchase.Rows[rowIndex];
            var ID = GVPurchase.DataKeys[rowIndex]["ID"].ToString();
            HddnID.Value = Convert.ToString(ID);
            string Mode = "CancelRequest";
            UpdateStatus(Mode);

        }


        if (e.CommandName == "RowDelete")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            var ID = GVPurchase.DataKeys[rowIndex]["ID"].ToString();
            HddnID.Value = Convert.ToString(ID);
            string Mode = "DeleteRecord";
            UpdateStatus(Mode);
            GetstoreList();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete Record Successfully..!!');window.location='StoreList.aspx';", true);
        }



    }

    protected void GVPurchase_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    public void GetstoreList()
    {


        try
        {
            SqlCommand cmd = new SqlCommand("SP_StoreDeatils", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetInwarteddata");
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Columns.Count > 0)
            {
                GVPurchase.DataSource = dt;
                GVPurchase.DataBind();
            }


        }
        catch (Exception ex)
        {

            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);

        }

    }

    public void GetAvailableDetals(string RowMaterial, string Thickness, string Width, string Length, string NeedQty, string Weight)
    {
        try
        {
            SqlCommand cmd = new SqlCommand("SP_StoreDeatils", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetAvialbleDeatils");
            cmd.Parameters.AddWithValue("@RowMaterial", RowMaterial);
            cmd.Parameters.AddWithValue("@Thickness", Thickness);
            cmd.Parameters.AddWithValue("@Width", Width);
            cmd.Parameters.AddWithValue("@Length", Length);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0) // Check if there are any rows in the DataTable
            {
                txtavailableQty.Text = dt.Rows[0]["AvilableQty"].ToString();
                txtThickness.Text = Thickness;
                txtwidth.Text = Width;
                txtlength.Text = Length;
                txtApprovQuantity.Text = NeedQty;
                Txtweight.Text = Weight;
            }
            else
            {
                txtavailableQty.Text = "Not Available";
                txtavailableQty.ForeColor = Color.Red;              
            }


        }
        catch (Exception ex)
        {

            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errorMsg + "') ", true);

        }
    }

    protected void btnSendtopro_Click(object sender, EventArgs e)
    {
        try
        {

            Cls_Main.Conn_Open();
            SqlCommand cmd = new SqlCommand("SP_StoreDeatils", Cls_Main.Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Createdby", Session["UserCode"].ToString());
            cmd.Parameters.AddWithValue("@Mode", "UpdateInwardQty");          
            cmd.Parameters.AddWithValue("@Quantity", txtApprovQuantity.Text);
            cmd.Parameters.AddWithValue("@Thickness", txtThickness.Text);
            cmd.Parameters.AddWithValue("@Width", txtwidth.Text);
            cmd.Parameters.AddWithValue("@Length", txtlength.Text);       
            cmd.Parameters.AddWithValue("@Weight", Txtweight.Text);        
            cmd.Parameters.AddWithValue("@ID", HddnID.Value);
            cmd.ExecuteNonQuery();
            Cls_Main.Conn_Close();
            Cls_Main.Conn_Dispose();
          
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Saved Record Successfully..!!');window.location='StoreList.aspx';", true);
        }
        catch (Exception ex)
        {

            throw;
        }
    }


    public void UpdateStatus(String Mode)
    {
        try
        {
            Cls_Main.Conn_Open();
            SqlCommand cmd1 = new SqlCommand("SP_StoreDeatils", Cls_Main.Conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Mode", Mode);
            cmd1.Parameters.AddWithValue("@ID", SqlDbType.BigInt).Value = Convert.ToInt64(HddnID.Value);
            var result = cmd1.ExecuteScalar();
            Cls_Main.Conn_Close();
            Cls_Main.Conn_Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Reject Request Successfully..!!');window.location='StoreList.aspx';", true);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
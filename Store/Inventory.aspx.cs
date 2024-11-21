﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Store_Inventory : System.Web.UI.Page
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
                FillGrid();
                ViewState["RowNo"] = 0;
                Dt_Component.Columns.AddRange(new DataColumn[3] { new DataColumn("id"), new DataColumn("Defect"), new DataColumn("DefectQty") });
                ViewState["Details"] = Dt_Component;
            }
        }
    }

    //Fill GridView
    private void FillGrid()
    {

        try
        {
            SqlCommand cmd = new SqlCommand("SP_InventoryDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetInventorylist");
            if (txtRowMaterial.Text == null || txtRowMaterial.Text == "")
            {
                cmd.Parameters.AddWithValue("@RowMaterial", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RowMaterial", txtRowMaterial.Text);
            }
            if (txtInwardno.Text == null || txtInwardno.Text == "")
            {
                cmd.Parameters.AddWithValue("@InwardNo", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@InwardNo", txtInwardno.Text);
            }

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

    protected void GVPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            divinwardform.Visible = true;
            divtabl.Visible = false;
            txtSize.Enabled = false;
            txtrowmetarial.Enabled = false;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPurchase.Rows[rowIndex];
            txtrowmetarial.Text = ((Label)row.FindControl("MaterialName")).Text;

            txtTotalQty.Text = ((Label)row.FindControl("InwardQty")).Text;

            txtSize.Text = ((Label)row.FindControl("Size")).Text;
            hdnid.Value = ((Label)row.FindControl("Inwardno")).Text;
            btnsavedata.Text = "Update";
        }
        if (e.CommandName == "RowDelete")
        {
            Cls_Main.Conn_Open();
            SqlCommand Cmd = new SqlCommand("UPDATE [tbl_InwardData] SET IsDeleted=@IsDeleted,DeletedBy=@DeletedBy,DeletedOn=@DeletedOn WHERE ID=@ID", Cls_Main.Conn);
            Cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("@IsDeleted", '1');
            Cmd.Parameters.AddWithValue("@DeletedBy", Session["UserCode"].ToString());
            Cmd.Parameters.AddWithValue("@DeletedOn", DateTime.Now);
            Cmd.ExecuteNonQuery();
            Cls_Main.Conn_Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Inward Entry Deleted Successfully..!!')", true);
            FillGrid();
        }
    }

    protected void GVPurchase_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVPurchase.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GVPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label InwardQty = e.Row.FindControl("InwardQty") as Label;
            //    Label OutwardQty = e.Row.FindControl("OutwardQty") as Label;
            //    Label DefectedQty = e.Row.FindControl("DefectedQty") as Label;
            //    LinkButton btnoutward = (LinkButton)e.Row.FindControl("btnoutward");
            //    LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
            //    LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");

            //    Double total = Convert.ToDouble(OutwardQty.Text) + Convert.ToDouble(DefectedQty.Text);
            //    if (Convert.ToDouble(InwardQty.Text) == total)
            //    {
            //        //e.Row.BackColor = System.Drawing.Color.LightPink;
            //        btnoutward.Visible = false;
            //    }

            //    if (OutwardQty.Text != "0" || DefectedQty.Text != "0")
            //    {
            //        btnEdit.Visible = false;
            //        btnDelete.Visible = false;
            //    }
            //    Label Inwardno = e.Row.FindControl("Inwardno") as Label;
            //    GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;
            //    gvDetails.DataSource = GetData(string.Format("select * from tbl_LM_Defects where InwardNo='{0}'", Inwardno.Text));
            //    gvDetails.DataBind();
            //}
        }
        catch
        {

        }
    }


    //Search RMC Search methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetRMCList(string prefixText, int count)
    {
        return AutoFillRowmName(prefixText);
    }

    public static List<string> AutoFillRowmName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select ComponentName from tbl_ComponentMaster where IsDeleted=0 and " + "ComponentName like '%'+ @Search + '%' AND Status=1";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["ComponentName"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        divinwardform.Visible = true;
        divtabl.Visible = false;
        txtrowmetarial.Enabled = true;
        txtrowmetarial.Text = "";
        txtTotalQty.Text = "";
        txtSize.Text = "";
        txtSize.Enabled = true;
        txtrowmetarial.Enabled = true;
        hdnid.Value = "";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        divinwardform.Visible = false;
        divtabl.Visible = true;
    }

    protected void btnsavedata_Click(object sender, EventArgs e)
    {
        Cls_Main.Conn_Open();
        SqlCommand cmd = new SqlCommand("SP_InventoryDetails", Cls_Main.Conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
        cmd.Parameters.AddWithValue("@Createdby", Session["UserCode"].ToString());
        if (btnsavedata.Text == "Update")
        {
            cmd.Parameters.AddWithValue("@Mode", "UpdateInwarddata");
            Double Total = Convert.ToDouble(txtTotalQty.Text) + Convert.ToDouble(txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@InwardNo", hdnid.Value);
            cmd.Parameters.AddWithValue("@InwardQty", Convert.ToString(Total));
            cmd.Parameters.AddWithValue("@Size", txtSize.Text);
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text);
        }
        else
        {
            cmd.Parameters.AddWithValue("@Mode", "InseartInwarddata");
            cmd.Parameters.AddWithValue("@InwardQty", txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@Size", txtSize.Text);
            cmd.Parameters.AddWithValue("@RowMaterial", txtrowmetarial.Text);
            cmd.Parameters.AddWithValue("@InwardNo", DBNull.Value);
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text);
        }

        cmd.ExecuteNonQuery();
        Cls_Main.Conn_Close();
        Cls_Main.Conn_Dispose();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Saved Record Successfully..!!');window.location='Inventory.aspx';", true);
    }

    protected void GVPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    //protected void txtDefectqty_TextChanged(object sender, EventArgs e)
    //{
    //    Double inwardqty = Convert.ToDouble(txtRemaining.Text);
    //    Double outwardqty = Convert.ToDouble(txtoutwardqty.Text);
    //    Double totaldefect = 0;

    //    totaldefect = Convert.ToDouble(txtDefectqty.Text);
    //    Double total = inwardqty - outwardqty - totaldefect;

    //    this.ModalPopupHistory.Show();
    //}

    //protected void txtoutwardqty_TextChanged(object sender, EventArgs e)
    //{
    //    Double inwardqty = Convert.ToDouble(txtinwardqty.Text);
    //    Double outwardqty = Convert.ToDouble(txtRemaining.Text);
    //    Double totaldefect = 0;
    //    if(inwardqty>=outwardqty)
    //    {
    //        //if (txtDefectqty.Text != "")
    //        //{
    //        //    totaldefect = Convert.ToDouble(txtDefectqty.Text);
    //        //    Double total = inwardqty - outwardqty - totaldefect;
    //        //    txtRemaining.Text = Convert.ToString(total);
    //        //}
    //        //else
    //        //{
    //        //    Double total = inwardqty - outwardqty;
    //        //    txtRemaining.Text = Convert.ToString(total);
    //        //}
    //    }
    //    else
    //    {
    //        txtoutwardqty.Text = "";
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please add quantity less then inward quantity..!!');", true);
    //    }


    //    this.ModalPopupHistory.Show();
    //}

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inventory.aspx");
    }



    private static DataTable GetData(string query)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void txtRowMaterial_TextChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    //Search Inward Number methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetInwardnoList(string prefixText, int count)
    {
        return AutoFillInwardNo(prefixText);
    }

    public static List<string> AutoFillInwardNo(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT InwardNo from tbl_InwardData where  " + "InwardNo like '%' + @Search + '%' and IsDeleted=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["InwardNo"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void txtInwardno_TextChanged(object sender, EventArgs e)
    {
        FillGrid();

    }

    protected void btnSearchData_Click(object sender, EventArgs e)
    {
        FillGrid();

    }

    protected void btnresetfilter_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inventory.aspx");
    }



    protected void txtSize_TextChanged(object sender, EventArgs e)
    {

        DataTable dtpt = Cls_Main.Read_Table("select * from tbl_InwardData WHERE RowMaterial='" + txtrowmetarial.Text.Trim() + "' AND Size='" + txtSize.Text.Trim() + "' AND IsDeleted=0");
        if (dtpt.Rows.Count > 0)
        {
            txtTotalQty.Text = dtpt.Rows[0]["InwardQty"].ToString();
            hdnid.Value = dtpt.Rows[0]["InwardNo"].ToString();
            btnsavedata.Text = "Update";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Already available this size row material please update quantity..!!');", true);
        }
        else
        {
            txtTotalQty.Text = "";
            hdnid.Value = "";
            btnsavedata.Text = "Save";
        }
    }
}


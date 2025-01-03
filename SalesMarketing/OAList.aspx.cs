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


public partial class SalesMarketing_OAList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    CommonCls objcls = new CommonCls();
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

            }
        }
    }


    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddOA.aspx");
    }

    //Fill GridView
    private void FillGrid()
    {
        if (Session["Role"].ToString() == "Admin")
        {
            DataTable Dt = Cls_Main.Read_Table("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE CP.IsDeleted = 0 ORDER BY CP.ID DESC");
            GVPurchase.DataSource = Dt;
            GVPurchase.DataBind();
        }
        else
        {
            DataTable Dt = Cls_Main.Read_Table("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE (CP.CreatedBy='" + Session["UserCode"].ToString() + "' OR CP.UserName='" + Session["UserCode"].ToString() + "') AND  CP.IsDeleted = 0 ORDER BY CP.ID DESC");
            GVPurchase.DataSource = Dt;
            GVPurchase.DataBind();
        }

    }


    protected void GVPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            Response.Redirect("AddOA.aspx?Id=" + objcls.encrypt(e.CommandArgument.ToString()) + "");
        }
        if (e.CommandName == "Sendtoproduction")
        {
            SendtoProduction(Convert.ToInt32(e.CommandArgument.ToString()));
        }
        if (e.CommandName == "RowDelete")
        {
            Cls_Main.Conn_Open();
            SqlCommand Cmd = new SqlCommand("UPDATE [tbl_OrderAcceptanceHdr] SET IsDeleted=@IsDeleted,DeletedBy=@DeletedBy,DeletedOn=@DeletedOn WHERE ID=@ID", Cls_Main.Conn);
            Cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("@IsDeleted", '1');
            Cmd.Parameters.AddWithValue("@DeletedBy", Session["UserCode"].ToString());
            Cmd.Parameters.AddWithValue("@DeletedOn", DateTime.Now);
            Cmd.ExecuteNonQuery();
            Cls_Main.Conn_Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Order Acceptance Deleted Successfully..!!')", true);
            FillGrid();
        }
        if (e.CommandName == "RowView")
        {
            Response.Redirect("Pdf_CustomerPurchase.aspx?Pono=" + objcls.encrypt(e.CommandArgument.ToString()) + " ");
            // Response.Write("<script>window.open ('Pdf_Quotation.aspx?Quotationno=" + (e.CommandArgument.ToString()) + "','_blank');</script>");
        }
    }

    protected void GVPurchase_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVPurchase.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void GVPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Authorization
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
            LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;

            string empcode = Session["UserCode"].ToString();
            DataTable Dt = new DataTable();
            SqlDataAdapter Sd = new SqlDataAdapter("Select ID from tbl_UserMaster where UserCode='" + empcode + "'", con);
            Sd.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                string id = Dt.Rows[0]["ID"].ToString();
                DataTable Dtt = new DataTable();
                SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + id + "' AND PageName = 'CustomerPurchaseOrderList.aspx' AND PagesView = '1'", con);
                Sdd.Fill(Dtt);
                if (Dtt.Rows.Count > 0)
                {
                    btnCreate.Visible = false;
                    //GVQuotation.Columns[15].Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }
    }

    //Search Company Search methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCustomerList(string prefixText, int count)
    {
        return AutoFillCustomerName(prefixText);
    }

    public static List<string> AutoFillCustomerName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "SELECT DISTINCT CustomerName FROM [tbl_OrderAcceptanceHdr] where " + "CustomerName like @Search + '%' and IsDeleted=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["CustomerName"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerName.Text != "" || txtCustomerName.Text != null)
        {
            string company = txtCustomerName.Text;

            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_CustomerPurchaseOrderHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE CP.IsDeleted = 0  CustomerName='" + txtCustomerName.Text + "' ORDER BY CP.ID DESC", Cls_Main.Conn);
            sad.Fill(dt);
            GVPurchase.EmptyDataText = "Not Records Found";
            GVPurchase.DataSource = dt;
            GVPurchase.DataBind();
        }
    }

    protected void btnrefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("OAList.aspx");
    }

    //Search OA.  Search methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCponoList(string prefixText, int count)
    {
        return AutoFillCponoName(prefixText);
    }

    public static List<string> AutoFillCponoName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "SELECT * FROM [tbl_OrderAcceptanceHdr] where " + "SerialNo like @Search + '%' and IsDeleted=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["SerialNo"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }
    protected void txtCpono_TextChanged(object sender, EventArgs e)
    {
        if (txtCpono.Text != "" || txtCpono.Text != null)
        {
            string Cpono = txtCpono.Text;

            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName AND SerialNo='" + Cpono + "' ORDER BY CP.ID DESC", Cls_Main.Conn);
            sad.Fill(dt);
            GVPurchase.EmptyDataText = "Not Records Found";
            GVPurchase.DataSource = dt;
            GVPurchase.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtCustomerName.Text) && string.IsNullOrEmpty(txtCpono.Text) && string.IsNullOrEmpty(txtfromdate.Text) && string.IsNullOrEmpty(txttodate.Text))
            {
                FillGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Search Record');", true);
            }
            else
            {
                if (Session["Role"].ToString() == "Admin")
                {
                    if (txtCpono.Text != "")
                    {
                        string Quono = txtCpono.Text;
                        DataTable dt = new DataTable();
                        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName where SerialNo = '" + Quono + "' AND CP.IsDeleted = 0", Cls_Main.Conn);
                        sad.Fill(dt);
                        GVPurchase.EmptyDataText = "Not Records Found";
                        GVPurchase.DataSource = dt;
                        GVPurchase.DataBind();
                    }
                    if (txtCustomerName.Text != "")
                    {
                        string company = txtCustomerName.Text;

                        DataTable dt = new DataTable();
                        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName where CustomerName = '" + company + "' AND CP.IsDeleted = 0", Cls_Main.Conn);
                        sad.Fill(dt);
                        GVPurchase.EmptyDataText = "Not Records Found";
                        GVPurchase.DataSource = dt;
                        GVPurchase.DataBind();
                    }

                    if (!string.IsNullOrEmpty(txtfromdate.Text) && !string.IsNullOrEmpty(txttodate.Text))
                    {
                        DataTable dt = new DataTable();

                        //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtfromdate.Text + "' AND '" + txttodate.Text + "' ", Cls_Main.Conn);
                        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE  CP.IsDeleted = 0 AND CP.CreatedOn between'" + txtfromdate.Text + "' AND '" + txttodate.Text + "' ", Cls_Main.Conn);
                        sad.Fill(dt);

                        GVPurchase.EmptyDataText = "Not Records Found";
                        GVPurchase.DataSource = dt;
                        GVPurchase.DataBind();
                    }

                }
                else
                {
                    if (txtCpono.Text != "")
                    {
                        string Quono = txtCpono.Text;

                        DataTable dt = new DataTable();
                        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE (CP.CreatedBy='" + Session["UserCode"].ToString() + "' OR CP.UserName='" + Session["UserCode"].ToString() + "') AND  SerialNo = '" + Quono + "' AND CP.IsDeleted = 0", Cls_Main.Conn);
                        sad.Fill(dt);
                        GVPurchase.EmptyDataText = "Not Records Found";
                        GVPurchase.DataSource = dt;
                        GVPurchase.DataBind();
                    }
                    if (txtCustomerName.Text != "")
                    {
                        string company = txtCustomerName.Text;

                        DataTable dt = new DataTable();
                        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE (CP.CreatedBy='" + Session["UserCode"].ToString() + "' OR CP.UserName='" + Session["UserCode"].ToString() + "') AND  CustomerName = '" + company + "' AND CP.IsDeleted = 0", Cls_Main.Conn);
                        sad.Fill(dt);
                        GVPurchase.EmptyDataText = "Not Records Found";
                        GVPurchase.DataSource = dt;
                        GVPurchase.DataBind();
                    }

                    if (!string.IsNullOrEmpty(txtfromdate.Text) && !string.IsNullOrEmpty(txttodate.Text))
                    {
                        DataTable dt = new DataTable();

                        //SqlDataAdapter sad = new SqlDataAdapter(" select [Id],[JobNo],[DateIn],[CustName],[Subcustomer],[Branch],[MateName],[SrNo],[MateStatus],FinalStatus,[TestBy],[ModelNo],[otherinfo],[Imagepath],[CreatedBy],[CreatedDate],[UpdateBy],[UpdateDate] ,ProductFault,RepeatedNo,DATEDIFF(DAY, CreatedDate, getdate()) AS days FROM [tblInwardEntry] Where DateIn between'" + txtfromdate.Text + "' AND '" + txttodate.Text + "' ", Cls_Main.Conn);
                        SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName WHERE (CP.CreatedBy='" + Session["UserCode"].ToString() + "' OR CP.UserName='" + Session["UserCode"].ToString() + "') AND  CP.CreatedOn between'" + txtfromdate.Text + "' AND '" + txttodate.Text + "' AND CP.IsDeleted = 0", Cls_Main.Conn);
                        sad.Fill(dt);

                        GVPurchase.EmptyDataText = "Not Records Found";
                        GVPurchase.DataSource = dt;
                        GVPurchase.DataBind();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Search GST WIse Company methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetGSTList(string prefixText, int count)
    {
        return AutoFillGSTName(prefixText);
    }

    public static List<string> AutoFillGSTName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "SELECT DISTINCT GSTNo FROM [tbl_OrderAcceptanceHdr] where " + "GSTNo like @Search + '%' and IsDeleted=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["GSTNo"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void txtGST_TextChanged(object sender, EventArgs e)
    {
        if (txtGST.Text != "" || txtGST.Text != null)
        {
            string GST = txtGST.Text;

            DataTable dt = new DataTable();
            SqlDataAdapter sad = new SqlDataAdapter("SELECT * FROM [tbl_OrderAcceptanceHdr] AS CP LEFT JOIN tbl_UserMaster AS UM ON UM.UserCode=CP.UserName where GSTNo = '" + GST + "' AND CP.IsDeleted = 0", Cls_Main.Conn);
            sad.Fill(dt);
            GVPurchase.EmptyDataText = "Not Records Found";
            GVPurchase.DataSource = dt;
            GVPurchase.DataBind();
        }
    }

    protected void ImageButtonfile5_Click(object sender, ImageClickEventArgs e)
    {
        string id = ((sender as ImageButton).CommandArgument).ToString();

        Display(id);
    }

    public void Display(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                string CmdText = "select fileName from tbl_OrderAcceptanceHdr where IsDeleted=0 AND ID='" + id + "'";

                SqlDataAdapter ad = new SqlDataAdapter(CmdText, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Response.Write(dt.Rows[0]["Path"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["fileName"].ToString()))
                    {
                        Response.Redirect("~/PDF_Files/" + dt.Rows[0]["fileName"].ToString());
                    }
                    else
                    {
                        //lblnotfound.Text = "File Not Found or Not Available !!";
                    }
                }
                else
                {
                    //lblnotfound.Text = "File Not Found or Not Available !!";
                }

            }
        }
    }

    public void SendtoProduction(Int32 ID)
    {
        DataTable Dt = Cls_Main.Read_Table("select *,OAD.ID as PID FROM [DB_Foundtech].[dbo].[tbl_OrderAcceptanceHdr] AS OAH INNER JOIN tbl_OrderAcceptanceDtls AS OAD ON OAD.Pono = OAH.Pono WHERE OAH.ID = '" + ID + "' AND OAH.IsDeleted=0");
        foreach (DataRow row in Dt.Rows)
        {
            string PID = null;
             PID = row["PID"].ToString();
            Cls_Main.Conn_Open();
            SqlCommand cmd = new SqlCommand("SP_ProductionDept", Cls_Main.Conn);
            cmd.CommandType = CommandType.StoredProcedure; 
            cmd.Parameters.AddWithValue("@ID", PID);
            cmd.Parameters.AddWithValue("@Createdby", Session["UserCode"].ToString());
            cmd.Parameters.AddWithValue("@Mode", "InseartOAinProduction");
            cmd.ExecuteNonQuery();
            Cls_Main.Conn_Close();
            Cls_Main.Conn_Dispose();
           
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OA Send to Production Successfully..!!');window.location='OAList.aspx'; ", true);


    }
}



using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Drawing;


namespace Foodie.User
{
    public partial class Invoice : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userID"] != null)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        rOrderItem.DataSource = GetOrderDetails();
                        rOrderItem.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }

        }

        DataTable GetOrderDetails()
        {
            double grandTotal = 0;
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", con);
            cmd.Parameters.AddWithValue("@Action", "INVOICEBYID");
            cmd.Parameters.AddWithValue("@PaymentId", Convert.ToInt32(Request.QueryString["id"]));
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (!dt.Columns.Contains("TotalPrice"))
            {
                dt.Columns.Add("TotalPrice", typeof(double));
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    // Handle any potential null values
                    double totalPrice = 0;
                    if (drow["TotalPrice"] != DBNull.Value)
                    {
                        totalPrice = Convert.ToDouble(drow["TotalPrice"]);
                    }
                    grandTotal += totalPrice;
                }
            }

            // Add a summary row with the grand total
            DataRow dr = dt.NewRow();
            dr["TotalPrice"] = grandTotal;  // Assuming this column exists in dt
            dt.Rows.Add(dr);

            return dt;
        }

            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach(DataRow drow in dt.Rows)
            //        {
            //            grandTotal += Convert.ToDouble(drow["TotalPrice"]);
            //        }
            //    }
            //    DataRow dr = dt.NewRow();
            //    dr["TotalPrice"] = grandTotal;
            //    dt.Rows.Add(dr);
            //    return dt;
            //}
            //protected void lbDownloadInvoice_Click(object sender, EventArgs e)
            //{

            //}

        //void ExportToPdf(DataTable dtblTable, String strPdfPath, string strHeader)
        //{
        //    FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);

        //    // Create PdfWriter first
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);

        //    // Now create the Document
        //    Document document = new Document(writer);

        //    document.Open();


        //    //Report Header
        //    BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    Font fntHead = new Font(bfntHead, 16, 1, Color.GRAY);
        //    Paragraph prgHeading = new Paragraph();
        //    prgHeading.Alignment = Element.ALIGN_CENTER;
        //    prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
        //    document.Add(prgHeading);

        //    //Author
        //    Paragraph prgAuthor = new Paragraph();
        //    BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    Font fntAuthor = new Font(btnAuthor, 8, 2, Color.GRAY);
        //    prgAuthor.Alignment = Element.ALIGN_RIGHT;
        //    prgAuthor.Add(new Chunk("Order From : Foodie Fast Food", fntAuthor));
        //    prgAuthor.Add(new Chunk("\nOrder Date : " + dtblTable.Rows[0]["OrderDate"].ToString(), fntAuthor));
        //    document.Add(prgAuthor);

        //    //Add a line seperation
        //    Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 1)));
        //    document.Add(p);

        //    //Add line break
        //    document.Add(new Chunk("\n", fntHead));

        //    //Write the table
        //    PdfPTable table = new PdfPTable(dtblTable.Columns.Count - 2);
        //    //Table header
        //    BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    Font fntColumnHeader = new Font(btnColumnHeader, 9, 1, Color.WHITE);
        //    for (int i = 0; i < dtblTable.Columns.Count - 2; i++)
        //    {
        //        PdfPCell cell = new PdfPCell();
        //        cell.BackgroundColor = Color.GRAY;
        //        cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
        //        table.AddCell(cell);
        //    }
        //    //table Data
        //    Font fntColumnData = new Font(btnColumnHeader, 8, 1, Color.BLACK);
        //    for (int i = 0; i < dtblTable.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dtblTable.Columns.Count - 2; j++)
        //        {
        //            PdfPCell cell = new PdfPCell();
        //            cell.AddElement(new Chunk(dtblTable.Rows[i][j].ToString(), fntColumnData));
        //            table.AddCell(cell);
        //        }
        //    }

        //    document.Add(table);
        //    document.Close();
        //    writer.Close();
        //    fs.Close();
        //}



    }
}
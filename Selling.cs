using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PHARMACY_MANAGEMENT
{
    public partial class Selling : Form
    {
        public Selling()
        {
            InitializeComponent();
            ShowMedicine();
            GetCustomer();
            ShowBill();
            
            LblSellerName.Text = Login.User;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\harshitha\Documents\PharmacyCdb.mdf;Integrated Security=True;Connect Timeout=30");

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMobileNo_TextChanged(object sender, EventArgs e)
        {

        }

     /* private void Dasboard_Click(object sender, EventArgs e)
        {
           Dashboard Obj = new Dashboard();
            this.Hide();
            Obj.Show();
        }*/
      
        private void btnManufacturer_Click(object sender, EventArgs e)
        {
            Manufacturer Obj = new Manufacturer();
            this.Hide();
            Obj.Show();
        }

        private void btnMedicines_Click(object sender, EventArgs e)
        {
            Medicines Obj = new Medicines();
            this.Hide();
            Obj.Show();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            this.Hide();
            Obj.Show();
        }

      /* private void btnSellers_Click(object sender, EventArgs e)
        {
            Sellers Obj = new Sellers();
            this.Hide();
            Obj.Show();
        }*/
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void GetCustomer()
        {
            Con.Open();
            
            SqlCommand cmd = new SqlCommand("Select CustomerId from CustomerTbl",Con);
            SqlDataReader Rdr;
            Rdr= cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerId",typeof(int));
            dt.Load(Rdr);
            txtCustomerId.ValueMember = "CustomerId";
            txtCustomerId.DataSource = dt;
           
            Con.Close();

        }
        private void GetCustomerName()
        {
            Con.Open();
            string Query = "Select * from CustomerTbl where CustomerId='" + txtCustomerId.SelectedValue.ToString() + "'";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                txtCustomerName.Text = dr["CustomerName"].ToString();
            }
            Con.Close();
        }
        private void UpdateQnty()
        {
            try
            {
                int NewQnty = Stock - Convert.ToInt32(txtQuantity.Text);
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update MedicineTbl Set MedicineQnty=@MQ where MedicineId=@MKey", Con);
                cmd.Parameters.AddWithValue("@MQ", NewQnty);
                cmd.Parameters.AddWithValue("@MKey", Key);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Medicine updated Successfully");
                Con.Close();
                ShowMedicine();
                //Reset();

            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        
        
        private void ShowMedicine()
        {
            Con.Open();
            string Query = "Select * from MedicineTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVMedicineLists.DataSource = ds.Tables[0];
            Con.Close();

        }
        
    
        int n = 0, GrdTotal;

        private void btnAddtoBill_Click(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "" || Convert.ToInt32(txtQuantity.Text) > Stock)
            {
                MessageBox.Show("Enter Correct Quantity");
            }
            else
            {
                int total = Convert.ToInt32(txtQuantity.Text) * Convert.ToInt32(txtPrice.Text);

                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(DGVBill);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = txtMedicine.Text;
                newRow.Cells[2].Value = txtQuantity.Text;
                newRow.Cells[3].Value = txtPrice.Text;
                newRow.Cells[4].Value = total;
                DGVBill.Rows.Add(newRow);
                GrdTotal += total;
                LblTotal.Text = "Rs " + GrdTotal; // Update the label with the new total
                n++;

                UpdateQnty();
            }
        }
        private void InsertBill()
        {
            if (txtCustomerName.Text == "")
            {
                // Handle the case where the customer name is empty (you can add appropriate code here)
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO BillTbl (SellerName, CustomerId, CustomerName, BillDate, BillAmount) VALUES (@SN, @CI, @CN, @BD, @BA)", Con);
                    cmd.Parameters.AddWithValue("@SN", LblSellerName.Text);
                    cmd.Parameters.AddWithValue("@CI", txtCustomerId.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CN", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                    cmd.Parameters.AddWithValue("@BA", GrdTotal);

                    // Debugging: Check the value of GrdTotal before insertion
                    MessageBox.Show("GrdTotal: " + GrdTotal);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bill Saved Successfully");
                    Con.Close();
                    ShowBill();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error in InsertBill: " + Ex.Message);
                }
            }
        }

        private void ShowBill()
        {
            Con.Open();
            string Query = "Select * from BillTbl where SellerName= '" + LblSellerName.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVTransactions.DataSource = ds.Tables[0];
            Con.Close();

        }




        private void DGVMedicineLists_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMedicine.Text = DGVMedicineLists.SelectedRows[0].Cells[1].Value.ToString();
            Stock = Convert.ToInt32(DGVMedicineLists.SelectedRows[0].Cells[3].Value.ToString());
            txtPrice.Text = DGVMedicineLists.SelectedRows[0].Cells[4].Value.ToString();
            if (txtMedicine.Text=="")
            {
                Key = 0;
            }
            else
            {
                Key= Convert.ToInt32(DGVMedicineLists.SelectedRows[0].Cells[0].Value.ToString());
            }

        }
        int Key = 0,Pos=100,  Stock;
        int MedId, MedPrice, MedQty, MedTot;

        private void btnSeller_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Click(object sender, EventArgs e)
        {

        }

        private void DGVTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            
            PrintDocument.DefaultPageSettings.PaperSize=new System.Drawing.Printing.PaperSize("pprnm",285,600);
            if (PrintPreviewDialog.ShowDialog()==DialogResult.OK)
            {
                PrintDocument.Print();

            }
            InsertBill();



        }

        string MedName;

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Pharmacy", new Font("Century Gothic",12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("ID Medicine Quantity Price Total", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in DGVBill.Rows)
            {
                MedId = Convert.ToInt32(row.Cells["Column1"].Value);
                MedName = "" + row.Cells["column2"].Value;
                MedPrice = Convert.ToInt32(row.Cells["column3"].Value);
                MedQty = Convert.ToInt32(row.Cells["column4"].Value);
                MedTot = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + MedId, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Black, new Point(26, Pos));
                e.Graphics.DrawString("" + MedName, new Font("century Gothic", 8, FontStyle.Bold), Brushes.Black, new Point(45, Pos));
                e.Graphics.DrawString("" + MedPrice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Black, new Point(128, Pos));
                e.Graphics.DrawString("" + MedQty, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Black, new Point(170, Pos));
                e.Graphics.DrawString("" + MedTot, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Black, new Point(235, Pos));
                Pos = Pos + 20;


            }
                e.Graphics.DrawString("Grand Total:Rs" + GrdTotal,new Font( "Century Gothic",10, FontStyle.Bold), Brushes.Crimson, new Point (50, Pos + 50));
                e.Graphics.DrawString("**********Pharmacy**********", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Crimson, new Point(10, Pos + 85));
                DGVBill.Rows.Clear();
                DGVBill.Refresh();
                
                
                n = 0;





            


        }

        private void txtCustomerId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCustomerName();


        }
        
        
    }
}

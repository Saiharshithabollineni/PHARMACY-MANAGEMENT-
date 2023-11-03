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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            CountMedicine();
            CountCustomer();
            CountSeller();
            SumAmount();
            SumAmountBySellers();
            GetBestCustomer();
            GetBestSeller();
            GetSeller();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\harshitha\Documents\PharmacyCdb.mdf;Integrated Security=True;Connect Timeout=30");
        private void CountMedicine()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from MedicineTbl",Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            LblMedicines.Text = dt.Rows[0][0].ToString();
            Con.Close();

        }
        private void CountCustomer()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from CustomerTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            LblCustomers.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountSeller()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from SellerTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            LblSellers.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void SumAmount()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Sum(BillAmount) from BillTbl", Con);
            DataTable dt = new DataTable(); 
            sda.Fill(dt);
            LblSellAmount.Text = dt.Rows[0][0].ToString(); 
            Con.Close();
        }
        private void SumAmountBySellers()
        {
            Con.Open();
            if (SellsBySellertxt.SelectedItem != null)
            {
                string selectedValue = SellsBySellertxt.SelectedItem.ToString();
                SqlDataAdapter sda = new SqlDataAdapter("Select Sum(BillAmount) from BillTbl where SellerName='" + SellsBySellertxt.SelectedValue + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    LblSellsBySeller.Text = "Rs" + dt.Rows[0][0].ToString();
                }
                else
                {
                    // Handle the case where no data is found.
                    LblSellsBySeller.Text = "No data found";
                }
            }
            else
            {
                // Handle the case where txtSellsBySeller.SelectedItem is null.
                LblSellsBySeller.Text = "No seller selected";
            }
            Con.Close();
        }

        private void GetSeller()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select SellerName from SellerTbl", Con);
            SqlDataReader Rdr;
            Rdr= cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("SellerName",typeof(string));
            dt.Load(Rdr);
            SellsBySellertxt.ValueMember = "SellerName";
            SellsBySellertxt.DataSource = dt;
            Con.Close();

            
        }
        private void GetBestCustomer()
        {

            try
            {
                Con.Open();
                string InnerQuery = "Select Max(BillAmount) from BillTbl";
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda1 = new SqlDataAdapter(InnerQuery, Con);
                sda1.Fill(dt1);

                string Query = "Select CustomerName from BillTbl where BillAmount ='" + dt1.Rows[0][0].ToString() + "'";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                LblBestCustomer.Text = dt.Rows[0][0].ToString();
                Con.Close();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }


        }
        private void GetBestSeller()
        {

            try
            {
                Con.Open();
                string InnerQuery = "Select Max(BillAmount) from BillTbl";
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda1 = new SqlDataAdapter(InnerQuery, Con);
                sda1.Fill(dt1);

                string Query = "Select SellerName from BillTbl where BillAmount ='" + dt1.Rows[0][0].ToString() + "'";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                LblBestSeller.Text = dt.Rows[0][0].ToString();
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }


        }



        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
           
        }

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

        private void btnSellers_Click(object sender, EventArgs e)
        {
            Sellers Obj = new Sellers();
            this.Hide();
            Obj.Show();
        }

        private void btnSelling_Click(object sender, EventArgs e)
        {
            Selling Obj = new Selling();
            this.Hide();
            Obj.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();

        }

        private void txtSellsBySeller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SumAmountBySellers();
        }

        private void SellsBySellertxt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LblSellsBySeller_Click(object sender, EventArgs e)
        {

        }
    }
}

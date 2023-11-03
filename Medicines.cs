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
    public partial class Medicines : Form
    {
        public Medicines()
        {
            InitializeComponent();
            ShowMedicine();
            GetManufacturer();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\harshitha\Documents\PharmacyCdb.mdf;Integrated Security=True;Connect Timeout=30");

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ShowMedicine()
        {
            Con.Open();
            string Query = " Select * from MedicineTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGVMedicines.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void Reset()
        {
            txtMedicineName.Text = "";
            txtMedicineType.SelectedIndex = 0;
            txtQuantity.Text = "";
            txtRate.Text = "";
            txtManufacturerName.Text = "";
            Key = 0;


        }
        private void GetManufacturer()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select ManufacturerId from ManufacturerTbl", Con);
            SqlDataReader Rdr;
            Rdr= cmd.ExecuteReader();
            DataTable dt= new DataTable();  
            dt.Columns.Add("ManufacturerId",typeof(int));
            dt.Load(Rdr);
            txtManufacturerId.ValueMember = "ManufacturerId";
            txtManufacturerId.DataSource = dt;
            Con.Close();

        }
        private void GetManufacturerName()
        {
            Con.Open();
            string Query = "SELECT ManufacturerName FROM ManufacturerTbl WHERE ManufacturerId='" + txtManufacturerId.SelectedValue.ToString() + "'";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                txtManufacturerName.Text = dr["ManufacturerName"].ToString();
            }
            Con.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtMedicineName.Text=="" || txtMedicineType.SelectedIndex==-1 || txtQuantity.Text=="" ||txtManufacturerId.SelectedIndex==-1 ||txtManufacturerName.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into MedicineTbl(MedicineName,MedicineType,MedicineQnty,MedicinePrice,MedicineManuId,MedicineManufacturer)values(@MN,@MT,@MQ,@MP,@MMI,@MM)", Con);
                    cmd.Parameters.AddWithValue("@MN", txtMedicineName.Text);
                    cmd.Parameters.AddWithValue("@MT", txtMedicineType.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@MQ", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@MP", txtRate.Text);
                    cmd.Parameters.AddWithValue("@MMI", txtManufacturerId.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@MM", txtManufacturerName.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine Added Successfully");
                    Con.Close();
                    ShowMedicine();
                    Reset();
                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);

                }
            }

        }

        private void txtManufacturerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtManufacturerId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetManufacturerName();
        }
        int Key = 0;
        private void DGVMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMedicineName.Text=DGVMedicines.SelectedRows[0].Cells[1].Value.ToString();
            txtMedicineType.SelectedItem = DGVMedicines.SelectedRows[0].Cells[2].Value.ToString();
            txtQuantity.Text = DGVMedicines.SelectedRows[0].Cells[3].Value.ToString();
            txtRate.Text = DGVMedicines.SelectedRows[0].Cells[4].Value.ToString();
            txtManufacturerId.SelectedValue = DGVMedicines.SelectedRows[0].Cells[5].Value.ToString();
            txtManufacturerName.Text = DGVMedicines.SelectedRows[0].Cells[6].Value.ToString();
            if (txtMedicineName.Text=="")
            {
                Key = 0;
            }
            else
            {
                Key= Convert.ToInt32(DGVMedicines.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Key==0)
            {
                MessageBox.Show("Select the Medicine");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from MedicineTbl where MedicineId=@MKey", Con);
                    cmd.Parameters.AddWithValue("@MKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine Deleted Successfully");
                    Con.Close();
                    ShowMedicine();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(txtMedicineName.Text=="" || txtMedicineType.SelectedIndex==-1 ||txtQuantity.Text==""|| txtManufacturerId.SelectedIndex==-1 ||txtManufacturerName.Text=="")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update MedicineTbl Set MedicineName=@MN,MedicineType=@MT,MedicineQnty=@MQ,MedicinePrice=@MP,MedicineManuId=@MMI,MedicineManufacturer=@MM where MedicineId=@MKey", Con);
                    cmd.Parameters.AddWithValue("@MN", txtMedicineName.Text);
                    cmd.Parameters.AddWithValue("@MT", txtMedicineType.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@MQ", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@MP", txtRate.Text);
                    cmd.Parameters.AddWithValue("@MMI", txtManufacturerId.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@MM", txtManufacturerName.Text);
                    cmd.Parameters.AddWithValue("@MKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine Updated Successfully");
                    Con.Close();
                    ShowMedicine();
                    Reset();


                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnSeller_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Click(object sender, EventArgs e)
        {

        }
    }
}


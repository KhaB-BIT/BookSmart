using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BOOKSMART
{
    public partial class FormDangNhap : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=MINHKHABUI\SQLEXPRESS;Initial Catalog=BOOKSMART;Integrated Security=True");

        public FormDangNhap()
        {
            InitializeComponent();
        }
        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }
        public static string TenNguoiDung = "";
        public static string MaNguoiDung = "";

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (checkBoxQL.Checked == false) //là nhân viên
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count (*) from NhanVien where TenDangNhap= N'" + txtTDN.Text + "' and MatKhau=N'" + txtMK.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    string query = "select * from NhanVien where TenDangNhap= N'" + txtTDN.Text + "' and MatKhau=N'" + txtMK.Text + "'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtTDN.Text == reader.GetString(5)) /* && txtMK.Text==reader.GetString(6)*/
                        {
                            TenNguoiDung = reader.GetString(2);
                            MaNguoiDung = reader.GetString(1);
                        }
                            
                    }
                    con.Close();
                    FormNhanVien formnhanvien = new FormNhanVien();
                    formnhanvien.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu hoặc tên đăng nhập");
                }
                con.Close();
            }
            else //là quản lý
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count (*) from QuanLy where TenDangNhap= N'" + txtTDN.Text + "' and MatKhau=N'" + txtMK.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    string query = "select * from QuanLy where TenDangNhap= N'" + txtTDN.Text + "' and MatKhau=N'" + txtMK.Text + "'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtTDN.Text == reader.GetString(4))   /* && txtMK.Text==reader.GetString(6)*/
                        {
                            TenNguoiDung = reader.GetString(1);
                            MaNguoiDung = reader.GetString(0);
                        }
                    }
                    con.Close();
                    QuanLyGianHang quanlygianhang = new QuanLyGianHang();
                    quanlygianhang.Show();
                    MessageBox.Show("Đăng nhập thành công");
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu hoặc tên đăng nhập");
                }
                con.Close();
            }

        }

        private void txtMK_TextChanged(object sender, EventArgs e)
        {
            txtMK.UseSystemPasswordChar = true;
        }

        private void txtMK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter) btnOK_Click_1(sender, e);
        }

        
    }
}

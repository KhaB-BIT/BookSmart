using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BOOKSMART
{
    public partial class QuanLyNhanVien : Form
    {
        int vitri = -1;
        SqlConnection con= new SqlConnection(@"Data Source=MINHKHABUI\SQLEXPRESS;Initial Catalog=BOOKSMART;Integrated Security=True");
        public QuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void QuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
        }

        private void LoadNhanVien()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from NhanVien", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "NhanVien");
            dataGridViewQLNV.DataSource = null;
            dataGridViewQLNV.DataSource = ds.Tables["NhanVien"];
            dataGridViewQLNV.Columns[2].HeaderText = "Tên nhân viên";
            dataGridViewQLNV.Columns[3].HeaderText = "Số điện thoại";
            dataGridViewQLNV.Columns[4].HeaderText = "Địa chỉ";
            dataGridViewQLNV.Columns[5].HeaderText = "Tên đăng nhập";
            dataGridViewQLNV.Columns[6].HeaderText = "Mật khẩu";
            Reset();
            con.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            txtTenNV.Text = "";
            txtDiachiNV.Text = "";
            txtSdtNV.Text = "";
            txtTDN.Text = "";
            txtMK.Text = "";
        }

        private void bntTimKiem_Click(object sender, EventArgs e)
        {
            con.Open();
            string keyword = txtTimKiem.Text;
            string query = "";
            if (keyword != "")
            {
                query = "select *from NhanVien where TenNV like N'%" + keyword + "%'";
                txtTimKiem.Text = "";
            }
            else query = "select *from NhanVien";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "NhanVien");
            dataGridViewQLNV.DataSource = null;
            dataGridViewQLNV.DataSource = ds.Tables["NhanVien"];
            con.Close();
        }

        private void dataGridViewQLNV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            vitri = e.RowIndex;
            txtTenNV.Text = dataGridViewQLNV.Rows[vitri].Cells[2].Value.ToString();
            txtDiachiNV.Text = dataGridViewQLNV.Rows[vitri].Cells[4].Value.ToString();
            txtSdtNV.Text = dataGridViewQLNV.Rows[vitri].Cells[3].Value.ToString();
            txtTDN.Text = dataGridViewQLNV.Rows[vitri].Cells[5].Value.ToString();
            txtMK.Text = dataGridViewQLNV.Rows[vitri].Cells[6].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenNV.Text == "" || txtSdtNV.Text == "" || txtDiachiNV.Text == "" || txtTDN.Text == "" || txtMK.Text=="")
            {
                MessageBox.Show("Hãy nhập đủ thông tin !");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "insert into NhanVien values(N'" + txtTenNV.Text + "','" + txtSdtNV.Text + "',N'" + txtDiachiNV.Text + "',N'" + txtTDN.Text + "',N'" + txtMK.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery(); 
                    con.Close();
                    LoadNhanVien();
                    MessageBox.Show("Thêm thành công nhân viên mới");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string up = dataGridViewQLNV.Rows[vitri].Cells[0].Value.ToString();
                string query = "update NhanVien set TenNV=N'" + txtTenNV.Text + "', SdtNV='" + txtSdtNV.Text + "', DiachiNV=N'" + txtDiachiNV.Text + "',TenDangNhap=N'" +txtTDN.Text + "',MatKhau=N'" + txtMK.Text + "' where ID=" + up + "";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                LoadNhanVien();
                MessageBox.Show("Đã cập nhật");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult notif = MessageBox.Show($"Bạn có muốn xóa nhân viên {txtTenNV.Text}?", "Xóa nhân viên", MessageBoxButtons.OKCancel);
                if (notif == DialogResult.OK)
                {
                    con.Open();
                    string del = dataGridViewQLNV.Rows[vitri].Cells[1].Value.ToString();
                    string query = "delete from NhanVien where MaNV='" + del + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadNhanVien();
                    MessageBox.Show("Đã xóa nhân viên");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQuanlygianhang_Click(object sender, EventArgs e)
        {
            QuanLyGianHang quanlygianhang = new QuanLyGianHang();
            quanlygianhang.Show();
            this.Hide();
        }

        private void btnQuanlykhachhang_Click(object sender, EventArgs e)
        {
            QuanLyKhachHang quanlykhachhang = new QuanLyKhachHang();
            quanlykhachhang.Show();
            this.Hide();
        }

        private void btnQuanlydoanhthu_Click_1(object sender, EventArgs e)
        {
            QuanLyDoanhThu quanlydoanhthu = new QuanLyDoanhThu();
            quanlydoanhthu.Show();
            this.Hide();
        }

        private void btnDangXuat_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn đăng xuất ?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                FormDangNhap formdn = new FormDangNhap();
                formdn.Show();
            }
            else MessageBox.Show("Chưa đăng xuất");
        }
    }
}

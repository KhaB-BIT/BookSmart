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
    public partial class QuanLyKhachHang : Form
    {
        int vitri = -1;
        SqlConnection con = new SqlConnection(@"Data Source=MINHKHABUI\SQLEXPRESS;Initial Catalog=BOOKSMART;Integrated Security=True");
        public QuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void QuanLyKhachHang_Load(object sender, EventArgs e)
        {
            DinhDangCotVaLoadKhachHang();
        } 
        private void bntTimKiem_Click(object sender, EventArgs e)
        {
            con.Open();
            string keyword = txtTimKiem.Text;
            string query = "";
            if (bntTimKiem.Text == "Tìm kiếm" || keyword != "")
            {
                query = "select *from KhachHang where TenKH like N'%" + keyword + "%'";
                txtTimKiem.Text = "";
                bntTimKiem.Text = "Hủy";
            }
            else
            {
                bntTimKiem.Text = "Tìm kiếm";
                query = "select *from KhachHang";
            }
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "KhachHang");
            dataGridViewQLKH.DataSource = null;
            dataGridViewQLKH.DataSource = ds.Tables["KhachHang"];
            con.Close();
        }
        private void btnQuanlygianhang_Click(object sender, EventArgs e)
        {
            QuanLyGianHang quanlygianhang = new QuanLyGianHang();
            quanlygianhang.Show();
            this.Hide();
        }

        private void btnQuanlynhanvien_Click_1(object sender, EventArgs e)
        {
            QuanLyNhanVien quanlynhanvien = new QuanLyNhanVien();
            quanlynhanvien.Show();
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenKH.Text == "" || txtDiachiKH.Text == "" || txtSdtKH.Text == "" || cbGioiTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Nhập thiếu thông tin");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "insert into KhachHang values(N'" + txtTenKH.Text + "','" + txtSdtKH.Text + "',N'" + txtDiachiKH.Text + "',N'" + cbGioiTinh.SelectedIndex.ToString() + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadKhachHang();
                    MessageBox.Show("Thêm thành công");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void LoadKhachHang()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from KhachHang", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "KhachHang");
            dataGridViewQLKH.DataSource = null;
            dataGridViewQLKH.DataSource = ds.Tables["KhachHang"];
            Reset();
            con.Close();
        }

        private void Reset()
        {
            txtTenKH.Text = ""; txtSdtKH.Text = ""; txtDiachiKH.Text = ""; cbGioiTinh.SelectedIndex = -1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string up = dataGridViewQLKH.Rows[vitri].Cells[0].Value.ToString();
                string query = "update KhachHang set TenKH=N'" +txtTenKH.Text + "', SdtKH='" + txtSdtKH.Text + "', DiachiKH=N'" + txtDiachiKH.Text + "',GioiTinh=N'" + cbGioiTinh.SelectedValue + "' where idKH=" + up + "";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                LoadKhachHang();
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
                DialogResult notif = MessageBox.Show($"Bạn có muốn xóa khách hàng {txtTenKH.Text}?", "Xóa khách hàng", MessageBoxButtons.OKCancel);
                if (notif == DialogResult.OK)
                {
                    con.Open();
                    string del = dataGridViewQLKH.Rows[vitri].Cells[0].Value.ToString();
                    string query = "delete from KhachHang where idKH='" + del + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadKhachHang();
                    MessageBox.Show("Đã xóa khách hàng");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridViewQLKH_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            vitri = e.RowIndex;
            txtTenKH.Text = dataGridViewQLKH.Rows[vitri].Cells[1].Value.ToString();
            txtSdtKH.Text = dataGridViewQLKH.Rows[vitri].Cells[2].Value.ToString();
            txtDiachiKH.Text = dataGridViewQLKH.Rows[vitri].Cells[3].Value.ToString();
            cbGioiTinh.SelectedItem = dataGridViewQLKH.Rows[vitri].Cells[4].Value.ToString();
           
        }
        public void DinhDangCotVaLoadKhachHang()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from KhachHang", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "KhachHang");
            dataGridViewQLKH.DataSource = null;
            dataGridViewQLKH.DataSource = ds.Tables["KhachHang"];
            dataGridViewQLKH.Columns[0].HeaderText = "ID";
            dataGridViewQLKH.Columns[1].HeaderText = "Tên khách hàng";
            dataGridViewQLKH.Columns[2].HeaderText = "Số điện thoại";
            dataGridViewQLKH.Columns[3].HeaderText = "Địa chỉ";
            dataGridViewQLKH.Columns[4].HeaderText = "Giới tính";
            Reset();
            con.Close();
        }
    }
}

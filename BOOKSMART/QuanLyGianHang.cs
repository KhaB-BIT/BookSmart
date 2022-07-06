using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BOOKSMART
{
    public partial class QuanLyGianHang : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=MINHKHABUI\SQLEXPRESS;Initial Catalog=BOOKSMART;Integrated Security=True");
        int vitri = -1;
        public QuanLyGianHang()
        {
            InitializeComponent();

        }
        private void QuanLyGianHang_Load(object sender, EventArgs e)
        {
            LoadSach();
            LoadTheLoai();
        }

        private void bntTimKiem_Click(object sender, EventArgs e)
        {
            con.Open();
            string keyword = txtTimKiem.Text;
            string query = "";
            if (bntTimKiem.Text == "Tìm kiếm" || keyword != "")
            {
                query = "select *from Sach where TenSach like N'%" + keyword + "%'";
                txtTimKiem.Text = "";
                bntTimKiem.Text = "Hủy";
            }
            else
            {
                bntTimKiem.Text = "Tìm kiếm";
                query = "select *from Sach";
            }
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Sach");
            dataGridViewQLGH.DataSource = null;
            dataGridViewQLGH.DataSource = ds.Tables["Sach"];
            con.Close();

        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenSach.Text == "" || txtGiaBan.Text == "" || txtSoLuong.Text == "" || txtTacGia.Text == "" || cbTheLoai.SelectedIndex == -1)
            {
                MessageBox.Show("Nhập thiếu thông tin");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "insert into Sach values(N'" + txtTenSach.Text + "','" + txtGiaBan.Text + "','" + txtSoLuong.Text + "',N'" + cbTheLoai.SelectedIndex.ToString() + "',N'" + txtTacGia.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadSach();
                    MessageBox.Show("Thêm thành công");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }
        private void LoadTheLoai()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from TheLoaiSach", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "TheLoaiSach");
            cbTheLoai.DataSource = ds.Tables["TheLoaiSach"];
            cbTheLoai.DisplayMember = "TenTheLoai";
            cbTheLoai.ValueMember = "MaTheLoai";
            con.Close();
        }
        private void LoadSach()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from Sach", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Sach");
            dataGridViewQLGH.DataSource = null;
            dataGridViewQLGH.DataSource = ds.Tables["Sach"];
            Reset();
            con.Close();

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void dataGridViewQLGH_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            vitri = e.RowIndex;
            txtTenSach.Text = dataGridViewQLGH.Rows[vitri].Cells[2].Value.ToString();
            txtGiaBan.Text = dataGridViewQLGH.Rows[vitri].Cells[3].Value.ToString();
            txtSoLuong.Text = dataGridViewQLGH.Rows[vitri].Cells[4].Value.ToString();
            txtTacGia.Text = dataGridViewQLGH.Rows[vitri].Cells[6].Value.ToString();
            cbTheLoai.SelectedValue = dataGridViewQLGH.Rows[vitri].Cells[5].Value.ToString();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string up = dataGridViewQLGH.Rows[vitri].Cells[0].Value.ToString();
                string query = "update Sach set TenSach=N'" + txtTenSach.Text + "', GiaBan=" + txtGiaBan.Text + ", SoLuong=" + txtSoLuong.Text + ",MaTheLoai=N'" + cbTheLoai.SelectedValue + "',TacGia=N'" + txtTacGia.Text + "' where ID=" + up + "";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                LoadSach();
                MessageBox.Show("Đã cập nhật");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (vitri == -1) MessageBox.Show("Hãy chọn một dòng");
            else
            {
                try
                {
                    DialogResult notif = MessageBox.Show($"Bạn có muốn xóa sản phẩm {txtTenSach.Text}?", "Xóa sản phẩm", MessageBoxButtons.OKCancel);
                    if (notif == DialogResult.OK)
                    {
                        con.Open();
                        string del = dataGridViewQLGH.Rows[vitri].Cells[1].Value.ToString();
                        string query = "delete from Sach where MaSach='" + del + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        LoadSach();
                        MessageBox.Show("Đã xóa");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Reset()
        {
            txtTenSach.Text = "";
            txtSoLuong.Text = "";
            txtGiaBan.Text = "";
            txtTacGia.Text = "";
            cbTheLoai.SelectedIndex = -1;
        }
        private void btnQuanlynhanvien_Click_1(object sender, EventArgs e)
        {
            QuanLyNhanVien quanlynhanvien = new QuanLyNhanVien();
            quanlynhanvien.Show();
            this.Hide();
        }

        private void btnQuanlykhachhang_Click_1(object sender, EventArgs e)
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

        private void btnDangXuat_Click(object sender, EventArgs e)
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

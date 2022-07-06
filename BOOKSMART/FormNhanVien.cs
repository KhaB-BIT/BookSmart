using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BOOKSMART
{
    public partial class FormNhanVien : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=MINHKHABUI\SQLEXPRESS;Initial Catalog=BOOKSMART;Integrated Security=True");
        int vitri = -1, vitrihd = -1;
        public int tongtien = 0;
        ArrayList dshd; //danh sach hoa don
        int STT = 0;
        string makhachhang = "";
        public FormNhanVien()
        {
            InitializeComponent();
            dshd = new ArrayList();
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            lbTenNV.Text = FormDangNhap.TenNguoiDung;
            LoadSach();
        }

        private void LoadSach()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from Sach", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Sach");
            dataGridViewNV_GH.DataSource = null;
            dataGridViewNV_GH.DataSource = ds.Tables["Sach"];
            Reset();
            con.Close();

        }

        private void Reset()
        {
            txtTenSach.Text = "";
            txtGiaBan.Text = "";
            txtSoLuongMua.Text = "0";
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void dataGridViewNV_GH_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            vitri = e.RowIndex;
            txtTenSach.Text = dataGridViewNV_GH.Rows[vitri].Cells[2].Value.ToString();
            txtGiaBan.Text = dataGridViewNV_GH.Rows[vitri].Cells[3].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            con.Open();
            string keyword = txtTimKiem.Text;
            string query = "";
            if (btnTimKiem.Text == "Tìm kiếm" || keyword != "")
            {
                query = "select *from Sach where TenSach like N'%" + keyword + "%'";
                txtTimKiem.Text = "";
                btnTimKiem.Text = "Hủy";
            }
            else
            {
                btnTimKiem.Text = "Tìm kiếm";
                query = "select *from Sach";
            }
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Sach");
            dataGridViewNV_GH.DataSource = null;
            dataGridViewNV_GH.DataSource = ds.Tables["Sach"];
            Reset();
            con.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtSoLuongMua.Text != "0" && txtTenSach.Text != "") // da tang so luong va co san pham
            {
                bool check = KiemTraSoLuongTrenGianHang(); // kiem tra xem so luong co it hon trong gian hang ko
                if (check == true)
                {
                    ArrayList s_tmp = new ArrayList();
                    s_tmp.Add(txtTenSach.Text); //tên sách
                    s_tmp.Add(txtSoLuongMua.Text); // số lượng mua
                    s_tmp.Add(txtGiaBan.Text); // giá bán
                    int xyz = Convert.ToInt32(txtGiaBan.Text) * Convert.ToInt32(txtSoLuongMua.Text);
                    s_tmp.Add(xyz); //thành tiền
                    s_tmp.Add(dataGridViewNV_GH.Rows[vitri].Cells[4].Value.ToString()); // số lượng trên gian hàng
                    s_tmp.Add(dataGridViewNV_GH.Rows[vitri].Cells[1].Value.ToString()); // mã sách
                    dshd.Add(s_tmp);
                    LoadHoaDon(txtTenSach.Text, txtSoLuongMua.Text, txtGiaBan.Text, xyz.ToString());
                    Reset();
                }
                else
                {
                    MessageBox.Show("Sản phẩm chỉ còn " + dataGridViewNV_GH.Rows[vitri].Cells[4].Value.ToString() + " quyển, hãy nhập ít hơn");
                }
            }
            else
            {
                if (txtSoLuongMua.Text == "0") MessageBox.Show("Số lượng đang bằng 0");
                if (txtTenSach.Text == "") MessageBox.Show("Chưa nhập tên sản phẩm");
            }
        }

        private bool KiemTraSoLuongTrenGianHang()
        {
            string slgh = dataGridViewNV_GH.Rows[vitri].Cells[4].Value.ToString();
            if (Convert.ToInt32(txtSoLuongMua.Text) <= Convert.ToInt32(slgh)) return true;
            return false;
        }

        private void LoadHoaDon(string x, string y, string z, string k)
        {
            DataGridViewRow newrow = new DataGridViewRow();
            newrow.CreateCells(dataGridViewNV_HD);
            newrow.Cells[0].Value = STT + 1;
            newrow.Cells[1].Value = x;
            newrow.Cells[2].Value = y;
            newrow.Cells[3].Value = z;
            newrow.Cells[4].Value = k;
            dataGridViewNV_HD.Rows.Add(newrow);
            STT++;
            tongtien = tongtien + Convert.ToInt32(k);
            labelTongTien.Text = "Tổng tiền: " + tongtien + " VND";

        }
        private void ClearAndUpdateHD()
        {

            for (int i = 0; i < dshd.Count; i++)
            {
                ArrayList tmp = (ArrayList)dshd[i];
                LoadHoaDon(tmp[0].ToString(), tmp[1].ToString(), tmp[2].ToString(), tmp[3].ToString());
            }
            STT = dshd.Count;
        }

        private void dataGridViewNV_HD_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            vitrihd = e.RowIndex;
            txtTenSach.Text = dataGridViewNV_HD.Rows[vitrihd].Cells[1].Value.ToString();
            txtGiaBan.Text = dataGridViewNV_HD.Rows[vitrihd].Cells[3].Value.ToString();
            txtSoLuongMua.Text = dataGridViewNV_HD.Rows[vitrihd].Cells[2].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            dshd.RemoveAt((int)vitrihd);
            dataGridViewNV_HD.Rows.Remove(dataGridViewNV_HD.Rows[vitrihd]);
            MessageBox.Show("Đã xóa sản phẩm");

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            ArrayList tmp = (ArrayList)dshd[vitrihd];
            tmp[1] = txtSoLuongMua.Text; //kiemtra
            MessageBox.Show("Đã cập nhật");
            //ClearAndUpdateHD();
        }

        private void btnCong_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(txtSoLuongMua.Text);
            txtSoLuongMua.Text = (x + 1).ToString();
        }

        private void btnTru_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(txtSoLuongMua.Text);
            if (x > 0) txtSoLuongMua.Text = (x - 1).ToString();
        }
        private void btnBangKH_Click_1(object sender, EventArgs e)
        {
            FormKhachHang formkh = new FormKhachHang();
            formkh.truyendulieu = new FormKhachHang.TruyenChoNhanVien(LoadDuLieu);
            formkh.Show();

        }
        public void LoadDuLieu(string x, string y, string z, string k)
        {
            txtTenKH.Text = x;
            txtSdtKH.Text = y;
            txtDiaChi.Text = z;
            makhachhang = k;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            DateTime now = DateTime.Now;
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprmn", 300, 1000);
            e.Graphics.DrawString("BookSmart", new Font("Century Gothic", 36, FontStyle.Bold), Brushes.Blue, new Point(70, 50));
            e.Graphics.DrawString("Địa: Cửa hàng sách BookSmart HCM", new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Blue, new Point(10, 100));
            e.Graphics.DrawString("Email: booksmart@gmail.com", new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Blue, new Point(10, 150));
            e.Graphics.DrawString("Tên nhân viên: " + lbTenNV.Text, new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Blue, new Point(10, 200));
            e.Graphics.DrawString("Ngày xuất: " + now, new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Blue, new Point(10, 250));
            e.Graphics.DrawString("===================================================================", new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.DarkRed, new Point(0, 300));
            e.Graphics.DrawString("Tên sản phẩm                Số lượng            Đơn giá           Thành tiền", new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Blue, new Point(10, 350));
            int vitridong = 400;
            for (int i = 0; i < dataGridViewNV_HD.Rows.Count; i++)
            {
                e.Graphics.DrawString(dataGridViewNV_HD.Rows[i].Cells[1].Value.ToString(), new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Black, new Point(10, vitridong));
                e.Graphics.DrawString(dataGridViewNV_HD.Rows[i].Cells[2].Value.ToString(), new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Black, new Point(350, vitridong));
                e.Graphics.DrawString(dataGridViewNV_HD.Rows[i].Cells[3].Value.ToString(), new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Black, new Point(500, vitridong));
                e.Graphics.DrawString(dataGridViewNV_HD.Rows[i].Cells[4].Value.ToString(), new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.Black, new Point(650, vitridong));
                vitridong += 50;
            }
            e.Graphics.DrawString("===================================================================", new Font("Microsoft Sans Serif", 18, FontStyle.Regular), Brushes.DarkRed, new Point(0, vitridong));
            e.Graphics.DrawString("Thành tiền:   " + tongtien + " VNĐ", new Font("Microsoft Sans Serif", 18, FontStyle.Bold), Brushes.Blue, new Point(115, vitridong + 50));
            e.Graphics.DrawString("BookSmart xin cảm ơn và hẹn gặp lại", new Font("Microsoft Sans Serif", 18, FontStyle.Italic), Brushes.Blue, new Point(40, vitridong + 100));
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (txtTenKH.Text == "")
            {
                MessageBox.Show("Hãy điền thông tin khách hàng");
            }
            else
            {
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
                CapNhatGianHang();
                GhiHDVaoSql();
                ResetAll();
            }

        }
        private void GhiHDVaoSql()
        {
            con.Open();
            DateTime now = DateTime.Today;
            string query = "insert into HoaDon values('" + now + "','" + makhachhang + "','" + FormDangNhap.MaNguoiDung + "','" + tongtien + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void CapNhatGianHang()
        {
            string masach = "";
            for (int i = 0; i < dshd.Count; i++)
            {
                con.Open();
                ArrayList tmp = (ArrayList)dshd[i];
                masach = tmp[5].ToString();
                int soluongcapnhat = Convert.ToInt32(tmp[4]) - Convert.ToInt32(tmp[1]);
                string query = "update Sach set SoLuong=" + soluongcapnhat + " where MaSach='" + masach + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            LoadSach();
        }

        private void ResetAll()
        {
            Reset();
            txtTenKH.Text = "";
            txtSdtKH.Text = "";
            txtDiaChi.Text = "";
            dataGridViewNV_HD.Rows.Clear();
            tongtien = 0;
            labelTongTien.Text = "Tổng tiền:";
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOOKSMART
{
    public partial class QuanLyDoanhThu : Form
    {
        public QuanLyDoanhThu()
        {
            InitializeComponent();
        }

        private void QuanLyDoanhThu_Load(object sender, EventArgs e)
        {

        }
        
        private void btnQuanlygianhang_Click_1(object sender, EventArgs e)
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

        private void btnQuanlykhachhang_Click(object sender, EventArgs e)
        {
            QuanLyKhachHang quanlykhachhang = new QuanLyKhachHang();
            quanlykhachhang.Show();
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

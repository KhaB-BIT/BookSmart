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
    public partial class FormKhachHang : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=MINHKHABUI\SQLEXPRESS;Initial Catalog=BOOKSMART;Integrated Security=True");
        int vitri = -1;
        public FormKhachHang()
        {
            InitializeComponent();
        }
        private void FormKhachHang_Load(object sender, EventArgs e)
        {
            LoadKhachHang();        
        }
        public delegate void TruyenChoNhanVien(string ten, string sdt, string diachi, string makh);
        public TruyenChoNhanVien truyendulieu;
        public void LoadKhachHang()
        {
            con.Open();
            string numberphone = txtTenKH.Text;
            string query = "select *from KhachHang";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds,"KhachHang");
            dataGridViewKH.DataSource = ds.Tables["KhachHang"];
            Reset();
            con.Close();
        }
        public void Reset()
        {
            txtTenKH.Text = "";
            txtSdtKH.Text = "";
            txtDiachiKH.Text = "";
            cbGioiTinh.SelectedIndex = -1;
        }

        private void dataGridViewKH_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           vitri = e.RowIndex;
            txtTenKH.Text = dataGridViewKH.Rows[vitri].Cells[1].Value.ToString();
            txtSdtKH.Text = dataGridViewKH.Rows[vitri].Cells[2].Value.ToString();
            txtDiachiKH.Text = dataGridViewKH.Rows[vitri].Cells[3].Value.ToString();
            cbGioiTinh.SelectedValue= dataGridViewKH.Rows[vitri].Cells[4].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenKH.Text==""|| txtSdtKH.Text == "" || txtDiachiKH.Text == "")
            {
                MessageBox.Show("Hãy nhập đủ thông tin");
            }
            else 
            {
                try
                {
                    con.Open();
                    string query = "insert into KhachHang values(N'" + txtTenKH.Text + "',N'" + txtSdtKH.Text + "',N'" + txtDiachiKH.Text + "',N'" + cbGioiTinh.SelectedItem.ToString() + "')";
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

        private void btnChon_Click(object sender, EventArgs e)
        {
            if (txtTenKH.Text != "" && txtSdtKH.Text != "")
                truyendulieu(txtTenKH.Text, txtSdtKH.Text, txtDiachiKH.Text, dataGridViewKH.Rows[vitri].Cells[0].Value.ToString());
            this.Hide();
        }

        private void txtTimKiem_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiem.Text = "";
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            con.Open();
            string keyword = txtTimKiem.Text;
            string query = "";
            if (btnTimKiem.Text == "Tìm kiếm" || keyword != "")
            {
                query = "select *from KhachHang where SdtKH like '%" + keyword + "%'";
                txtTimKiem.Text = "";
                btnTimKiem.Text = "Hủy";
            }
            else
            {
                btnTimKiem.Text = "Tìm kiếm";
                query = "select *from KhachHang";
            }
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Sach");
            dataGridViewKH.DataSource = null;
            dataGridViewKH.DataSource = ds.Tables["Sach"];
            con.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}

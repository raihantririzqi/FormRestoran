using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormRestoran
{
    public partial class Pembayaran : Form
    {

        public Pembayaran()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox3.Enabled = false; 
        }
        string conStr = "server=localhost;uid=root;pwd=;database=db_penjualan";
        string id_penjualan = Pesanan.pesanan.id_penjualan;

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (textBox2.Text != string.Empty)
                {
                    insert();
                    update();
                    Pesanan pesanan = new Pesanan();
                    pesanan.Close();
                }
                else
                {
                    MessageBox.Show("isi nominal bayar");
                }
            }
        }
        void insert()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("insert into pembayaran (id_penjualan,total_harga,bayar) values ('" + id_penjualan + "','" + textBox1.Text + "','" + textBox2.Text + "')",con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        void update()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("update penjualan set status='done' where id_penjualan='" + id_penjualan + "'", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        private void Pembayaran_Load(object sender, EventArgs e)
        {
            textBox1.Text = Pesanan.pesanan.label1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty)
            {
                try
                {
                    int.Parse(textBox2.Text);
                    int bayar = int.Parse(textBox2.Text);
                    int total = int.Parse(textBox1.Text);
                    int hasil = bayar - total;
                    textBox3.Text = hasil.ToString();
                }
                catch
                {
                    textBox2.Text = string.Empty;
                }
            }
            else
            {
                textBox3.Text = string.Empty;
            }
        }
    }
}

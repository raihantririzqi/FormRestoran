using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormRestoran
{
    public partial class Pesanan : Form
    {
        public string id_kasir = Home.home.id_kasir;
        public string id_pelanggan = Pesan.pesan.comboBox1.SelectedValue.ToString();
        public string id_penjualan;
        string conStr = "server=localhost;uid=root;pwd=;database=db_penjualan";
        public static Pesanan pesanan;
        Pembayaran pembayaran;
        void pembayaran_closed(object sender, FormClosedEventArgs e)
        {
            pembayaran = null;
        }
        public Pesanan()
        {
            InitializeComponent();
            pesanan = this;
            combo();
            datagrid();
            hargaTotal();
            getIdPenjualan();
        }

        private void Pesanan_Load(object sender, EventArgs e)
        {

        }
        void getIdPenjualan()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("select * from penjualan where id_pelanggan='" + id_pelanggan + "' and status='pending'", con))
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id_penjualan = reader[0].ToString();
                    }
                }
            }
        }
        void combo()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("select * from menu", con))
                {
                    using(MySqlDataAdapter sd = new MySqlDataAdapter(cmd))
                    {
                        using(DataSet dt = new DataSet())
                        {
                            sd.Fill(dt);
                            comboBox1.DataSource = dt.Tables[0];
                            comboBox1.DisplayMember = "nama_menu";
                            comboBox1.ValueMember = "id_menu";
                        }
                    }
                }
            }
        }
        void datagrid()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("SELECT pelanggan.nama_pelanggan,alamat,jenis_kelamin,telepon,menu.nama_menu,harga,detail_penjualan.total_menu,total_menu * menu.harga AS total_harga FROM detail_penjualan JOIN penjualan ON penjualan.id_penjualan=detail_penjualan.id_penjualan JOIN pelanggan ON pelanggan.id_pelanggan=penjualan.id_pelanggan JOIN menu ON menu.id_menu=detail_penjualan.id_menu WHERE pelanggan.id_pelanggan='"+id_pelanggan+"' AND penjualan.status='pending'", con))
                {
                    using(MySqlDataAdapter sd = new MySqlDataAdapter(cmd))
                    {
                        using(DataTable dt = new DataTable())
                        {
                            sd.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
        }
        void hargaTotal()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("SELECT SUM(total_menu * menu.harga) FROM detail_penjualan JOIN penjualan ON penjualan.id_penjualan=detail_penjualan.id_penjualan JOIN menu ON menu.id_menu=detail_penjualan.id_menu WHERE id_pelanggan='"+id_pelanggan+"' AND penjualan.status='pending'", con))
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        label1.Text = reader[0].ToString();
                    }
                }
            }
        }
        void insert()
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                using (MySqlCommand cmd = new MySqlCommand("insert into detail_penjualan (id_penjualan,id_menu,total_menu) values ('"+id_penjualan+"','"+comboBox1.SelectedValue+"','"+numericUpDown1.Value.ToString()+"')",con))
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
                using(MySqlCommand cmd = new MySqlCommand("update detail_penjualan set total_menu=total_menu + '" + numericUpDown1.Value.ToString() + "' where id_penjualan='"+id_penjualan+"' and id_menu='"+comboBox1.SelectedValue+"'", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 0)
            {
                using (MySqlConnection con = new MySqlConnection(conStr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select * from detail_penjualan where id_penjualan='" + id_penjualan + "' and id_menu='" + comboBox1.SelectedValue + "'", con))
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            update();
                        }
                        else
                        {
                            insert();
                        }
                        con.Close();
                        datagrid();
                        hargaTotal();
                        numericUpDown1.Value = 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("masukkan jumlah yang diinginkan");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(pembayaran == null)
            {
                pembayaran = new Pembayaran();
                pembayaran.FormClosed += new FormClosedEventHandler(pembayaran_closed);
                pembayaran.ShowDialog();
                this.Close();
            }
            else
            {
                pembayaran.Activate();
            }
        }
    }
}

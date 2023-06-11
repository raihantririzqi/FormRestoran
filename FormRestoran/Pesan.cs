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
    public partial class Pesan : Form
    {
        public string id_kasir = Home.home.id_kasir;
        public static Pesan pesan;
        Pesanan pesanan;
        void pesanan_closed(object sender, FormClosedEventArgs e)
        {
            pesanan = null;
        }
        public Pesan()
        {
            InitializeComponent();
            pesan = this;
            combo();
        }
        string conStr = "server=localhost;uid=root;pwd=;database=db_penjualan";

        private void Pesan_Load(object sender, EventArgs e)
        {

        }
        void combo()
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("select * from pelanggan", con))
                {
                    using(MySqlDataAdapter sd = new MySqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            sd.Fill(ds);
                            comboBox1.DataSource = ds.Tables[0];
                            comboBox1.DisplayMember = "nama_pelanggan";
                            comboBox1.ValueMember = "id_pelanggan";
                        }
                    }
                }
            }
        }
        void pesanan_open()
        {
            if (pesanan == null)
            {
                pesanan = new Pesanan();
                pesanan.FormClosed += new FormClosedEventHandler(pesanan_closed);
                pesanan.ShowDialog();
            }
            else
            {
                pesanan.Activate();
            }
        }
        void insert_penjualan()
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                using (MySqlCommand cmd = new MySqlCommand("insert into penjualan (tgl_penjualan,id_pelanggan,id_kasir,status) values ('" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + comboBox1.SelectedValue + "','" + id_kasir + "','pending')", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        void validation()
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from penjualan where id_pelanggan='" + comboBox1.SelectedValue + "' and status='pending'", con))
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        pesanan_open();
                    }
                    else
                    {
                        insert_penjualan();
                        pesanan_open();
                    }
                    con.Close();
                }
            }
        }
        void update_id()
        {
            using (MySqlConnection con = new MySqlConnection(conStr)) {
                using (MySqlCommand cmd = new MySqlCommand("update penjualan set id_kasir='" + id_kasir + "' where id_pelanggan='" + comboBox1.SelectedValue + "' and status='pending'", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("select * from penjualan where id_pelanggan='"+comboBox1.SelectedValue+"' and status='pending'", con))
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["id_kasir"].ToString() == "0")
                        {
                            update_id();
                            validation();
                        }
                        else
                        {
                            validation();
                        }
                    }
                }
            }
        }
    }
}

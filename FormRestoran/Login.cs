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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }
        string conStr = "server=localhost;uid=root;pwd=;database=db_penjualan";
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                textBox2.Select();
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                textBox1.Select();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                button1.Select();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(MySqlConnection con = new MySqlConnection(conStr))
            {
                using(MySqlCommand cmd = new MySqlCommand("select * from kasir where username='"+textBox1.Text+"' and password='" + textBox2.Text + "'", con))
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Home.home.loginToolStripMenuItem.Enabled = false;
                            Home.home.pemesananToolStripMenuItem.Enabled = true;
                            Home.home.logoutToolStripMenuItem.Enabled = true;
                            Home.home.id_kasir = reader[0].ToString();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("login gagal cek username dan password");
                    }
                }
            }
        }
    }
}

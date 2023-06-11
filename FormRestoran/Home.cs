using System;
using System.Windows.Forms;

namespace FormRestoran
{
    public partial class Home : Form
    {
        public string id_kasir;
        Login login;
        Pesan pesan;
        public static Home home;
        void login_validation(object sender, FormClosedEventArgs e)
        {
            login = null;
        }
        void pesan_validation(object sender, FormClosedEventArgs e)
        {
            pesan = null;
        }
        public Home()
        {
            InitializeComponent();
            home = this;
            logoutToolStripMenuItem.Enabled = false;
            pemesananToolStripMenuItem.Enabled = false;
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }
        void login_open()
        {
            if (login == null)
            {
                login = new Login();
                login.FormClosed += new FormClosedEventHandler(login_validation);
                login.ShowDialog();
            }
            else
            {
                login.Activate();
            }
        }
        private void Home_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.L)
            {
                login_open();
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login_open();
        }

        private void pemesananToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pesan == null)
            {
                pesan = new Pesan();
                pesan.FormClosed += new FormClosedEventHandler(pesan_validation);
                pesan.ShowDialog();
            }
            else
            {
                pesan.Activate();
            }
        }
    }
}

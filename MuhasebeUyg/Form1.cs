using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuhasebeUyg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("provider=microsoft.ace.oledb.12.0;data source=" + Application.StartupPath + "\\muhasebe.accdb");


        private void btn_giris_Click(object sender, EventArgs e)
        {
            baglan.Open();
            OleDbCommand komut = new OleDbCommand("Select * from kullanici where kadi='" + textBox1.Text + "' and sifre='" + textBox2.Text + "'", baglan);
            OleDbDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                Form2 form2 = new Form2();
                form2.Show();
                
                this.Hide();
            }
            else
            {
                MessageBox.Show("Bilgiler Uyuşmuyor. Tekrar Deneyiniz..");

            }
            baglan.Close();
        }

        private void btn_uyeol_Click(object sender, EventArgs e)
        {
            UyeOl uyelik = new UyeOl();
            uyelik.Show();
            this.Hide();
        }

        private void btn_cikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_hakkimizda_Click(object sender, EventArgs e)
        {
            Hakkinda hakkinda = new Hakkinda();
            hakkinda.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SifremiUnuttum sifre = new SifremiUnuttum();
            sifre.Show();
            this.Hide();
        }
    }
}

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
    public partial class SifremiUnuttum : Form
    {
        public SifremiUnuttum()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("provider=microsoft.ace.oledb.12.0;data source=" + Application.StartupPath + "\\muhasebe.accdb");

        private void button1_Click(object sender, EventArgs e)
        {
            doldur();
        }
        public void doldur()
        {
            baglan.Open();
            OleDbCommand komut = new OleDbCommand("Select * from kullanici where kadi='" + textBox1.Text + "' and soru='" + comboBox1.Text + "' and cevap='" + textBox2.Text + "'", baglan);
            OleDbDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                textBox3.Text = dr["sifre"].ToString();
            }
            else
            {

                MessageBox.Show("Bilgiler uyuşmuyor..");
            }


            baglan.Close();


        }

        private void SifremiUnuttum_Load(object sender, EventArgs e)
        {
            //COMBOBOXA VERİTABANINDAN VERİ ÇEKTİK
            OleDbCommand kmt = new OleDbCommand("Select * from kullanici");
            kmt.Connection = baglan;
            kmt.CommandType = CommandType.Text;
            OleDbDataReader dare;
            baglan.Open();
            dare = kmt.ExecuteReader();
            while (dare.Read())
            {
                comboBox1.Items.Add(dare["soru"]);
            }
            baglan.Close();
        }


    }
}

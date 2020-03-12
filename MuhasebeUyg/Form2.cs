using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuhasebeUyg
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");

        private void btn_stok_kontrol_Click(object sender, EventArgs e)
        {
            panel1.Show();     
            panel2.Hide();
            
            listele();
        }

        private void btn_stok_islemleri_Click(object sender, EventArgs e)
        {
            StokIslemleri islem = new StokIslemleri();
            islem.Show();
        }

        private void btn_hesapmak_Click(object sender, EventArgs e)
        {
            Process Process = new Process();//hesap makinesini çalişdıracak kodları girdik
            ProcessStartInfo ProcessInfo;
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + "calc");
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;

            Process = Process.Start(ProcessInfo);
            Process.WaitForExit();
            Process.Close();
        }

        private void btn_hareketler_Click(object sender, EventArgs e)
        {
            panel2.Show();
  
        }

        private void btn_raporlama_Click(object sender, EventArgs e)
        {
            Raporlama raporlama = new Raporlama();
            raporlama.Show();

        }

        private void btn_cari_islemler_Click(object sender, EventArgs e)
        {
            CariIslemler islemler = new CariIslemler();
            islemler.Show();
        }

        private void btn_musteri_Click(object sender, EventArgs e)
        {
            Musteri musteri = new Musteri();
            musteri.Show();
        }

        private void btn_ajanda_Click(object sender, EventArgs e)
        {
            Ajanda ajanda = new Ajanda();
            ajanda.Show();
        }

        private void bt_cek_senet_Click(object sender, EventArgs e)
        {
            btn_ara ceksenet = new btn_ara();
            ceksenet.Show();
        }

        private void btn_kasa_Click(object sender, EventArgs e)
        {
            KasaHesap kasa = new KasaHesap();
            kasa.Show();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void stokİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StokIslemleri islem = new StokIslemleri();
            islem.Show();
        }

        public void notListele()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter notlistele = new OleDbDataAdapter("select * from notlar WHERE month(gtarih)='" + DateTime.Now.Month + "'", baglantim);
                DataSet ds = new DataSet();
                notlistele.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                baglantim.Close();
            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.ToString());
                baglantim.Close();
            }
       
        }
        public void listele()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from stokbil", baglantim);
                DataSet ds = new DataSet();
                listele.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];

                baglantim.Close();
                

            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.ToString());
                baglantim.Close();
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            notListele();
            listele();
        }
    }
}

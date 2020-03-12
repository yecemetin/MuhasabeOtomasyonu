using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuhasebeUyg
{
    public partial class StokIslemleri : Form
    {
        public StokIslemleri()
        {
            InitializeComponent();
        }
        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");
        string DosyaYolu, DosyaAdi = "";

        private void StokIslemleri_Load(object sender, EventArgs e)
        {

            textBox1.MaxLength = 8;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            textBox4.CharacterCasing = CharacterCasing.Upper;
            textBox5.CharacterCasing = CharacterCasing.Upper;
            textBox6.CharacterCasing = CharacterCasing.Upper;
            textBox7.CharacterCasing = CharacterCasing.Upper;

            string[] olcubirimi = { "metre", "santimetre", "karış" };
            comboBox1.Items.AddRange(olcubirimi);

            listele();
            temizle();

        }

        public void temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            comboBox1.SelectedIndex = -1;

        }

        public void listele()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from stokbil", baglantim);
                DataSet ds = new DataSet();
                listele.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                baglantim.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString());
                baglantim.Close();
            }
        }


        private void btn_resimsil_Click(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = "";
            DosyaAdi = "";
        }

        private void btn_resimekle_Click(object sender, EventArgs e)
        {
            if (DosyaAc.ShowDialog() == DialogResult.OK)
            {
                foreach (string i in DosyaAc.FileName.Split('\\'))
                {
                    if (i.Contains(".jpg")) { DosyaAdi = i; }
                    else if (i.Contains(".png")) { DosyaAdi = i; }
                    else { DosyaYolu += i + "\\"; }
                }
                pictureBox1.ImageLocation = DosyaAc.FileName;
            }
            else
            {
                MessageBox.Show("Dosya Girmediniz!");
            }
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {


                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("INSERT INTO stokbil (stokSeriNo,stokModeli,stokAdi,stokAdedi,stokTarih,kayitYapan,dosyaAdi,olcu_birimi,alisfiyat,satisfiyat) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + dateTimePicker1.Text + "','" + textBox5.Text + "','" + DosyaAdi + "','" + comboBox1.Text + "','" + textBox6.Text + "','" + textBox7.Text + "') ", baglantim);
                        eklekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        MessageBox.Show("Eklendi");
                        listele();
                        temizle();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz..!", "  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 8)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from stokbil where stokSeriNo = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OleDbCommand silkomutu = new OleDbCommand("delete from stokbil where stokSeriNo = '" + textBox3.Text + "'", baglantim);
                    silkomutu.ExecuteNonQuery();
                    MessageBox.Show("Silindi");
                    baglantim.Close();
                    listele();
                    temizle();
                    break;

                }
                if (kayit_arama_durumu == false)
                    MessageBox.Show("Silinecek kayıt bulunamadı.");
                baglantim.Close();
                temizle();
            }
            else
            {
                MessageBox.Show("Lütfen 11 karakterli bir sayı giriniz..!", "  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
  
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand eklekomutu = new OleDbCommand("update stokbil set alisfiyati='" + textBox6.Text + "' and satisfiyati='" + textBox7.Text + "' where stokSeriNo='" + textBox1.Text + "'", baglantim);
                    eklekomutu.ExecuteNonQuery();
                    baglantim.Close();
                    MessageBox.Show("Güncellendi");
                    listele();
                    temizle();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    baglantim.Close();
                }
            }
            else
            {
                MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz..!", "  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_arama_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (textBox1.Text.Length == 8)
            {
                baglantim.Open();
                OleDbCommand arakomutu = new OleDbCommand("select * from stokbil where stokSeriNo = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitarama = arakomutu.ExecuteReader();
                while (kayitarama.Read())
                {
                    kayit_arama_durumu = true;
                    textBox2.Text = kayitarama.GetValue(1).ToString();
                    textBox3.Text = kayitarama.GetValue(2).ToString();
                    textBox4.Text = kayitarama.GetValue(3).ToString();
                    dateTimePicker1.Text = kayitarama.GetValue(4).ToString();
                    textBox5.Text = kayitarama.GetValue(5).ToString();
                    comboBox1.Text = kayitarama.GetValue(6).ToString();
                    textBox6.Text = kayitarama.GetValue(7).ToString();
                    textBox7.Text = kayitarama.GetValue(8).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)

                    MessageBox.Show("Aranan kayıt bulunamadı.");


                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen Stok Seri No giriniz.");
                temizle();
            }   
        }

        private void btn_anasayfa_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close();
        }

        

       
    }
}

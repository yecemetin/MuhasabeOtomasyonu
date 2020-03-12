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
    public partial class KasaHesap : Form
    {
        public KasaHesap()
        {
            InitializeComponent();
        }

        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");

        private void KasaHesap_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 5;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            string[] tip = { "Aktif", "Pasif" };
            comboBox1.Items.AddRange(tip);

            listele();
            temizle();
        }

        public void listele()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from kasahesap", baglantim);
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

        public void temizle()
        {

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.SelectedIndex = -1;
            
        }

        private void btn_ara_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (textBox1.Text.Length == 5)
            {
                baglantim.Open();
                OleDbCommand arakomutu = new OleDbCommand("select * from kasahesap where islemkodu = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitarama = arakomutu.ExecuteReader();
                while (kayitarama.Read())
                {
                    kayit_arama_durumu = true;
                    dateTimePicker1.Text = kayitarama.GetValue(1).ToString();
                    textBox2.Text = kayitarama.GetValue(2).ToString();
                    textBox3.Text = kayitarama.GetValue(3).ToString();
                    comboBox1.Text = kayitarama.GetValue(4).ToString();
                    textBox4.Text = kayitarama.GetValue(5).ToString();
                    textBox5.Text = kayitarama.GetValue(6).ToString();
                    textBox6.Text = kayitarama.GetValue(7).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)

                    MessageBox.Show("Aranan kayıt bulunamadı.");


                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen işlem kodu giriniz.");
                temizle();
            }   
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand sorgukomutu = new OleDbCommand("select * from kasahesap where islemkodu = '" + textBox1.Text + "' ", baglantim);
            OleDbDataReader kayitokuma = sorgukomutu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break; //Bu tc no kayıtlı ise hiç birşey yapmadan çıksın
            }
            baglantim.Close();
            if (kayitkontrol == false)
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into kasahesap values ('" + textBox1.Text + "','" + dateTimePicker1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + comboBox1.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "') ", baglantim);
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
            else
            {
                MessageBox.Show("Bu işlem kodu kayıtlıdır.!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 5)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kasahesap where islemkodu = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OleDbCommand silkomutu = new OleDbCommand("Delete from kasahesap where islemkodu = '" + textBox1.Text + "'", baglantim);
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
                MessageBox.Show("Lütfen işlem kodunu giriniz..!", "  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand eklekomutu = new OleDbCommand("update kasahesap set islemkodu ='" + textBox1.Text + "',islemtarihi = '" + dateTimePicker1.Text + "',cari = '" + textBox2.Text + "',aciklama = '" + textBox3.Text + "',tip = '" + comboBox1.Text + "',masraf = '" + textBox4.Text + "',giren = '" + textBox5.Text + "', cikan='" + textBox6.Text + "'", baglantim);
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

        private void btn_anasayfa_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox7.Text = dateTimePicker1.Value.ToShortDateString();
        }
    }
}

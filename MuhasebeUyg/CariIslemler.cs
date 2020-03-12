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
    public partial class CariIslemler : Form
    {
        public CariIslemler()
        {
            InitializeComponent();
        }

        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");

        private void CariIslemler_Load(object sender, EventArgs e)
        {
           
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            textBox4.CharacterCasing = CharacterCasing.Upper;


            /*string[] serino = { "123456", "456789", "987654" };
            comboBox1.Items.AddRange(serino);*/

            string[] odemetipi = { "Nakit", "KrediKartı", "Çek-Senet" };
            comboBox2.Items.AddRange(odemetipi);

            serino_doldur();
            musteri_doldur();
           

            temizle();
            listele();
          
        }

        public void musteri_doldur() {
            
            baglantim.Open();
            OleDbCommand musteri = new OleDbCommand("Select * from musteri", baglantim);
            OleDbDataReader dr = musteri.ExecuteReader();
            while (dr.Read())
            {
                comboBox3.Items.Add(dr["adsoyad"]);
            }
            baglantim.Close();
        }

        public void serino_doldur()
        {

            baglantim.Open();
            OleDbCommand serino = new OleDbCommand("Select * from stokbil", baglantim);
            OleDbDataReader dr = serino.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["stokSeriNo"]);
            }
            baglantim.Close();
        }

        public void temizle() {

           
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        
        }

        public void listele() {
            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from cari",baglantim);
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


        private void btn_ara_Click(object sender, EventArgs e)
        {

            bool kayit_arama_durumu = false;
            if (comboBox1.Text != "")
            {
                baglantim.Open();
                OleDbCommand arakomutu = new OleDbCommand("select * from cari where musteri_adi = '" + comboBox1.Text + "'", baglantim);
                OleDbDataReader kayitarama = arakomutu.ExecuteReader();
                while (kayitarama.Read())
                {
                    kayit_arama_durumu = true;
                    comboBox1.Text = kayitarama.GetValue(1).ToString();
                    comboBox2.Text = kayitarama.GetValue(2).ToString();
                    textBox2.Text = kayitarama.GetValue(3).ToString();
                    textBox3.Text = kayitarama.GetValue(4).ToString();
                    textBox4.Text = kayitarama.GetValue(5).ToString();
                    
                    break;
                }
                if (kayit_arama_durumu == false)

                    MessageBox.Show("Aranan kayıt bulunamadı.");


                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen müşteri adı giriniz.");
                temizle();
            }   
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand sorgukomutu = new OleDbCommand("select * from cari where musteri_adi = '" + comboBox1.Text + "' ", baglantim);
            OleDbDataReader kayitokuma = sorgukomutu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break; //Bu tc no kayıtlı ise hiç birşey yapmadan çıksın
            }
            baglantim.Close();
            if (kayitkontrol == false)
            {
                if (comboBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into cari values ('" + comboBox1.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + dateTimePicker1.Text + "') ", baglantim);
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
                MessageBox.Show("Bu Müşteri kayıtlıdır.!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            try
            {
                baglantim.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter silkomutu = new OleDbDataAdapter("delete from cari where musteri_adi = '" + comboBox1.Text + "'", baglantim);      
                silkomutu.Fill(ds);
                baglantim.Close();
                MessageBox.Show("Silindi");
                listele();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                baglantim.Close();
            }
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand eklekomutu = new OleDbCommand("update cari set alim_miktari='" + textBox2.Text + "' where musteri_adi='" + comboBox1.Text + "'", baglantim);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

       
    }
}

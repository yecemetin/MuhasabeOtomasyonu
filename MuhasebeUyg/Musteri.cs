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
    public partial class Musteri : Form
    {
        public Musteri()
        {
            InitializeComponent();
        }

        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");

    

        private void Musteri_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;        
            textBox4.MaxLength = 11;


            string[] durum = {"Aktif","Pasif" };
            comboBox1.Items.AddRange(durum);

            listele();
            temizle();
        }

        public void listele() {

            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select *  from musteri",baglantim);
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

        public void temizle() {

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void button5_Click(object sender, EventArgs e) //arama
        {
            bool kayit_arama_durumu = false;
            if (textBox1.Text.Length == 11) {
                baglantim.Open();
                OleDbCommand arakomutu = new OleDbCommand("select * from musteri where tc = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitarama = arakomutu.ExecuteReader();
                while (kayitarama.Read())
                {
                    kayit_arama_durumu = true;
                    textBox2.Text = kayitarama.GetValue(1).ToString();
                    textBox3.Text = kayitarama.GetValue(2).ToString();
                    textBox4.Text = kayitarama.GetValue(3).ToString();
                    textBox5.Text = kayitarama.GetValue(4).ToString();
                    textBox6.Text = kayitarama.GetValue(5).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)
                
                    MessageBox.Show("Aranan kayıt bulunamadı.");
                

                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 11 haneli Tc Kimlik numarası giriniz.");
                temizle();
            }   
        }

        private void button1_Click(object sender, EventArgs e) //ekleme
        {
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand sorgukomutu = new OleDbCommand("select * from musteri where tc = '"+textBox1.Text+"' ",baglantim);
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
                        OleDbCommand eklekomutu = new OleDbCommand("insert into musteri values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + comboBox1.Text + "') ", baglantim);
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
                MessageBox.Show("Bu Tc Kimlik Numarası kayıtlıdır.!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)//sil
        {
            if (textBox1.Text.Length == 11)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from musteri where tc = '"+textBox1.Text+"'",baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OleDbCommand silkomutu = new OleDbCommand("Delete from musteri where tc = '"+textBox1.Text+"'",baglantim);
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

        private void button3_Click(object sender, EventArgs e)//güncelle
        {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand guncellekomutu = new OleDbCommand("update musteri set tc='" + textBox1.Text + "', adsoyad='" + textBox2.Text + "', d_tarihi='" + textBox3.Text + "', tel='" + textBox4.Text + "', k_tarihi='" + textBox5.Text + "', k_karti= '" + textBox6.Text + "', durum='" + comboBox1.Text + "'", baglantim);
                        guncellekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        MessageBox.Show("Güncelle");
                        listele();

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

        private void button4_Click(object sender, EventArgs e)//anasayfa
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close();
        }
    }
}

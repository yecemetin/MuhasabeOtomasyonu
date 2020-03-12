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
    public partial class btn_ara : Form
    {
        public btn_ara()
        {
            InitializeComponent();
        }

        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");


        private void btn_ara_Load(object sender, EventArgs e) //Form Load burası
        {
            textBox1.MaxLength = 5;
           
            textBox3.CharacterCasing = CharacterCasing.Upper;
           


            string[] odemeturu = { "ÇEK", "SENET" };
            comboBox1.Items.AddRange(odemeturu);

            string[] vade = { "3 AY", "4 AY",  "6 AY", "12 AY", "24 AY", "36 AY" };
            comboBox2.Items.AddRange(vade);

            listele();
            temizle();
        }

        public void listele()
        {

            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from ceksenet", baglantim);
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
            comboBox2.SelectedIndex = -1;
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
           
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand sorgukomutu = new OleDbCommand("select * from ceksenet where islemkodu = '" + textBox1.Text + "' ", baglantim);
            OleDbDataReader kayitokuma = sorgukomutu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break; //Bu tc no kayıtlı ise hiç birşey yapmadan çıksın
            }
            baglantim.Close();
            if (kayitkontrol == false)
            {
                if (textBox1.Text != "" && comboBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into ceksenet values ('" + textBox1.Text + "','" + comboBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + comboBox1.Text + "') ", baglantim);
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

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (textBox1.Text.Length == 5)
            {
                baglantim.Open();
                OleDbCommand arakomutu = new OleDbCommand("select * from ceksenet where islemkodu = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitarama = arakomutu.ExecuteReader();
                while (kayitarama.Read())
                {
                    kayit_arama_durumu = true;
                    comboBox2.Text = kayitarama.GetValue(1).ToString();
                    textBox3.Text = kayitarama.GetValue(2).ToString();
                    textBox4.Text = kayitarama.GetValue(3).ToString();
                    textBox5.Text = kayitarama.GetValue(4).ToString();        
                    break;
                }
                if (kayit_arama_durumu == false)

                    MessageBox.Show("Aranan kayıt bulunamadı.");


                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 5 haneli işlem kodu giriniz.");
                temizle();
            }   
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 5)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from ceksenet where islemkodu = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OleDbCommand silkomutu = new OleDbCommand("Delete from ceksenet where islemkodu = '" + textBox1.Text + "'", baglantim);
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
                MessageBox.Show("Lütfen 5 haneli islem kodu giriniz..!", "  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" )
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand guncellekomutu = new OleDbCommand("update ceksenet set islemkodu='" + textBox1.Text + "', vade='" + comboBox2.Text + "', odemetarihi='" + textBox3.Text + "', tutar='" + textBox4.Text + "', odeyecek='" + textBox5.Text + "', odemeturu='" + comboBox1.Text + "' where islemkodu = '"+textBox1.Text+"'", baglantim);
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

        private void btn_anasayfa_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close();
        }
    }
}

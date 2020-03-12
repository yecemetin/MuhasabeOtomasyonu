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
    public partial class UyeOl : Form
    {
        public UyeOl()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("provider=microsoft.ace.oledb.12.0;data source=" + Application.StartupPath + "\\muhasebe.accdb");

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == textBox6.Text)
            {
                try
                {
                    if (baglan.State == ConnectionState.Open) //Eğer bağlantı açıksa dmeke oluyor
                    {
                        baglan.Close();
                    }
                    baglan.Open();
                    OleDbCommand komut = new OleDbCommand("insert into kullanici (tc,adsoyad,tel,kadi,sifre,soru,cevap) values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox8.Text + "','" + textBox7.Text + "')", baglan);
                    komut.ExecuteNonQuery();
                    baglan.Close();
                    MessageBox.Show("Kayıt Başarılı");
                    Form2 form2 = new Form2();
                    form2.Show();
                    this.Hide();


                }
                catch (Exception hata)
                {

                    MessageBox.Show(hata.Message);
                }
            }
            else
            {
                MessageBox.Show("Şifreler Uyuşmuyor.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void UyeOl_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.MaxLength = 11;
            textBox4.CharacterCasing = CharacterCasing.Upper;
            textBox5.CharacterCasing = CharacterCasing.Upper;
            textBox6.CharacterCasing = CharacterCasing.Upper;
            textBox7.CharacterCasing = CharacterCasing.Upper;
            textBox8.CharacterCasing = CharacterCasing.Upper;
        }
    }
}

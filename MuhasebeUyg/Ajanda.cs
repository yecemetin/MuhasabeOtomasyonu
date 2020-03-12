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
    public partial class Ajanda : Form
    {
        public Ajanda()
        {
            InitializeComponent();
        }
        public OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=muhasebe.accdb");
       
        private void Ajanda_Load(object sender, EventArgs e)
        {
            textBox1.CharacterCasing = CharacterCasing.Upper;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.Text = DateTime.Now.ToShortDateString();

            listele();
            temizle();
        }

        public void temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            maskedTextBox1.Clear();
        }

        public void listele()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from notlar", baglantim);
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


        private void btn_kaydet_Click(object sender, EventArgs e)
        {
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand sorgukomutu = new OleDbCommand("select * from notlar where adi = '" + textBox1.Text + "' ", baglantim);
            OleDbDataReader kayitokuma = sorgukomutu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break; //Bu tc no kayıtlı ise hiç birşey yapmadan çıksın
            }
            baglantim.Close();
            if (kayitkontrol == false)
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && maskedTextBox1.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into notlar values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + maskedTextBox1.Text + "') ", baglantim);
                        eklekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        MessageBox.Show("Kaydedildi");
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
                MessageBox.Show("Bu Not Başlığı kayıtlıdır.!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_anasayfa_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
    }
}

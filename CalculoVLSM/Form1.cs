using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculos;

namespace CalculoVLSM
{
    public partial class Form1 : Form
    {
        List<Dados> Y = new List<Dados>();
        Calcular calc = new Calcular();
        Dados X = new Dados();
        public Form1()
        {
            InitializeComponent();
            HideItens();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < textBox1.TextLength; i++)
            {
                richTextBox1.Text = Convert.ToString(i);
                richTextBox1.Text = Convert.ToString(i);
                richTextBox1.Text = Convert.ToString(i);
                richTextBox1.Text = Convert.ToString(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" ||textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Campo Vazio, digite algum valor valido!", "Campo Vazio");
                
            }
            //---------- IDENTIFICA SE O VALOR ESTA CORRETO ----------
            else if (int.Parse(textBox1.Text) > 255 || int.Parse(textBox2.Text) > 255 || int.Parse(textBox3.Text) > 255 || int.Parse(textBox4.Text) > 255)
            {
                MessageBox.Show("Valor incorreto!");
            }
            else
            {

                // ---------- IDENTIFICAR A MASCARA E CLASSE ----------
                Double RangeIp = Double.Parse(textBox1.Text);

                // Classe A
                if (RangeIp >= 0 && 128 > RangeIp)
                {
                    calc.Mascara[0] = 255;
                    calc.Mascara[1] = 0;
                    calc.Mascara[2] = 0;
                    calc.Mascara[3] = 0;
                    calc.Classe = "A";
                    calc.MascCust = "255.0.0.0";
                    calc.BinarioMascara = "11111111.00000000.00000000.00000000";
                    calc.MascaraCustomizada = calc.BinarioMascara;
                    calc.QntOcteto = 24;
                }
                //Classe B
                else if (RangeIp >= 128 && 192 > RangeIp)
                {
                    calc.Mascara[0] = 255;
                    calc.Mascara[1] = 255;
                    calc.Mascara[2] = 0;
                    calc.Mascara[3] = 0;
                    calc.Classe = "B";
                    calc.MascCust = "255.255.0.0";
                    calc.BinarioMascara = "11111111.11111111.00000000.00000000";
                    calc.MascaraCustomizada = calc.BinarioMascara;
                    calc.QntOcteto = 16;
                }
                // Classe C
                else if (RangeIp >= 192 && 224 > RangeIp)
                {
                    calc.Mascara[0] = 255;
                    calc.Mascara[1] = 255;
                    calc.Mascara[2] = 255;
                    calc.Mascara[3] = 0;
                    calc.Classe = "C";
                    calc.MascCust = "255.255.255.0";
                    calc.BinarioMascara = "11111111.11111111.11111111.00000000";
                    calc.MascaraCustomizada = calc.BinarioMascara;
                    calc.QntOcteto = 8;
                }
                AtivarBotoes();
                DesableFunction();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            X = new Dados();
            dvg.DataSource = null;
            X.hosts = Convert.ToInt16(qntHost.Value);
            X.hostsMais2 = Convert.ToInt16(qntHost.Value + 2);
            X.hostsPotencia = calc.EncontrarPotencia(qntHost.Value);
            calc.IpSubredes[0] = int.Parse(textBox1.Text);
            calc.IpSubredes[1] = int.Parse(textBox2.Text);
            calc.IpSubredes[2] = int.Parse(textBox3.Text);
            calc.IpSubredes[3] = int.Parse(textBox4.Text);
            Y.Add(X);
            dvg.DataSource = Y;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            qntHost.Minimum = 2;
            qntHost.Maximum = 10000;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Y = Y.OrderByDescending(x => x.hostsPotencia).ToList();
            calc.MascaraCustom(calc.MascaraCustomizada);
            calc.MascaraCustomBits();
            Mostrar();
        }
        private  void ConverterBinario(double ipBase)
        {
            string bin = "";
            Console.WriteLine("Informe o IP base para calculo: ");
            ipBase = int.Parse(Console.ReadLine());

            while (true)
            {
                if ((ipBase % 2) != 0)
                    bin = "1" + bin;
                else
                    bin = "0" + bin;
                ipBase /= 2;
                if (ipBase <= 0)
                    break;
            }
            while (bin.Count() < 8)
            {
                string aux = "0";
                if (bin.Count() < 8)
                {
                    bin = aux + bin;
                }
            }
        }

        public void Mostrar()
        {
                richTextBox1.Text += $"Utilizando IP base: {calc.JuntarIP(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text)}\n" +
                               $"Endereço da Classe: {calc.Classe}\n" +
                               $"Mascara Padrão: {calc.MascCust}\n\n" +
                               $"{calc.Binario(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text)}\n" +
                               $"{calc.BinarioMascara}\n\n";
            double x = 0;
            foreach (Dados C in Y)
            {
                x += C.hostsPotencia;
                if(x > 255)
                {
                    calc.IpSubredes[3] = 0;
                    calc.IpSubredes[2]++;
                    x -= 255;
                }
                calc.IpSubredes[3] += Convert.ToInt32(x);
                richTextBox1.Text +=
                               $"Subnet 0: {calc.IpSubredes[0]}.{calc.IpSubredes[1]}.{calc.IpSubredes[2]}.{calc.IpSubredes[3]}  / {calc.Mascara[0]}.{calc.Mascara[1]}.{calc.Mascara[2]}.{calc.Mascara[3]} \n" +
                               $"{calc.MascaraCustomizada}\n" +
                               $"Broadcast:{calc.IpSubredes[0]}.{calc.IpSubredes[1]}.{calc.IpSubredes[2]}.{calc.IpSubredes[3] - 1}\n\n" +
                               $"";

            }
        }
        public void HideItens()
        {
            label3.Hide();
            button3.Enabled = false;
            button3.Hide();
            button4.Enabled = false;
            button4.Hide();
            qntHost.Enabled = false;
            qntHost.Hide();
            dvg.Enabled = false;
            dvg.Hide();
        }
        public void AtivarBotoes()
        {
            button4.Show();
            button3.Show();
            dvg.Show();
            qntHost.Show();
            label3.Show();
            button3.Enabled = true;
            button4.Enabled = true;
            qntHost.Enabled = true;
            dvg.Enabled = true;
            richTextBox1.Size = new System.Drawing.Size(750, 250);
            richTextBox1.Location = new Point(-1, 248);
            richTextBox1.Clear();
        }

        public void DesableFunction()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HideItens();
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            button1.Enabled = true;
            richTextBox1.Size = new System.Drawing.Size(750, 450);
            richTextBox1.Location = new Point(-1, 48);
            richTextBox1.Clear();
        }

    }
}

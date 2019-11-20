using CalculoVLSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculos
{
    class Calcular
    {
        public string MascCust { get; set; }
        public string Classe { get; set; }
        public string BinarioMascara { get; set; }
        public int QntOcteto { get; set; }
        public string MascaraCustomizada { get; set; }
        public int[] IpSubredes = new int[4];
        int Potencia = 0;
        public int[] Mascara = new int[4];
        public int mascCustom;
        int Potencia2 =0;

        public string JuntarIP(string ip1, string ip2, string ip3, string ip4)
        {
            string Ip = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
            return Ip;
        }

        public double EncontrarPotencia(decimal z)
        {
            double y = Convert.ToDouble(z) + 2;
            double x = 0;
            for (; Potencia < 32; Potencia++)
            {
                x = Math.Pow(2, Potencia);
                if (x >= y)
                {
                    Potencia2 = Potencia;
                    break;
                }
            }
            Potencia = 0;
            return x;
        }

        public string Binario(string ipBase1, string ipBase2, string ipBase3, string ipBase4)
        {
            int[] id = new int[] { Convert.ToInt32(ipBase1), Convert.ToInt32(ipBase2), Convert.ToInt32(ipBase3), Convert.ToInt32(ipBase4) };
            string[] bin2 = new string[4];
            string bin = "";

            for (int i = 0; i < 4; i++)
            {
                while (true)
                {
                    if ((id[i] % 2) != 0)
                        bin = "1" + bin;
                    else
                        bin = "0" + bin;
                    id[i] /= 2;
                    if (id[i] <= 0)
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
                bin2[i] = bin;
                bin = "";
            }
            bin = bin2[0] + "." + bin2[1] + "." + bin2[2] + "." + bin2[3];
            return bin;
        }

        public void MascaraCustomBits()
        {
            int aux = 0;
            int potencia = 128;
            aux = QntOcteto - Potencia2;
            mascCustom = 0;
            for (int i = 0; i < aux; i++)
            {
                mascCustom += potencia;
                potencia /= 2;
                if (potencia == 0)
                {
                    potencia = 128;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (Mascara[i] == 0 && mascCustom >= 255)
                {
                    Mascara[i] = 255;
                    mascCustom -= 255;
                }
                else if (Mascara[i] == 0 && mascCustom < 255)
                {
                    Mascara[i] = mascCustom;
                    mascCustom -= mascCustom;
                }
            }
        }
        public void MascaraCustom(string bin)
        {
            if (Potencia2 >= 9)
            {
                Potencia2++;
            }
            if (Potencia2 >= 18)
            {
                Potencia2 += 2;
            }
            char[] mascArray = bin.ToCharArray();
            for (int k = 0; k < mascArray.Count() - Potencia2; k++)
            {
                if (mascArray[k] == '0')
                {
                    mascArray[k] = '1';
                }
            }
            bin = new string(mascArray);
            MascaraCustomizada = bin;
            Potencia2 = 0;
        }
    }
}

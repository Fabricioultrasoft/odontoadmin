using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;

namespace OdontoADM.Util
{
    class Receita
    {
        private Point myPoint;
        private Color myColor;
        private SolidBrush myBrush;
        private Font myFont;
        private Pen myPen;

        public int PacienteID { get; set; }
        public DataTable dtMedicamentos { get; set; }

        OdontoADMDll.BLL.Paciente p = new OdontoADMDll.BLL.Paciente();

        public void FormatarReceita(System.Drawing.Printing.PrintPageEventArgs e)
        {
            p = p.GetPaciente(PacienteID, false);

            string cabecalho = "                                      DR. WILLIANS HIDEKI NAKAZONE";
            string titulo = "RECEITA";
            string DadosDoPacienteTitulo = "Dados do Paciente: ";
            string DadosDoPaciente = "\n\nNome: " + p.Nome + "\n\n";
            DadosDoPaciente += "R.G.: " + p.RG + "\n\n";
            DadosDoPaciente += "CPF: " + p.CPF + "\n\n";
            string remediosTitulo = "Medicação:";

            string LocalEData = "São Paulo, ____/__________/________";

            string Assinatura = "Dr. Willians H. Nakazone";
            string AssinaturaEnd = "          Cirurgião Dentista";
            AssinaturaEnd += "\n              CRO 78.867";

            string Footer = "                                                           Av. Conselheiro Carrão, 3422 - Sala 02 - Vila Carrão - Tel.: 3441-6307";
            Footer += "\n                                                             Praça Aurélio Lombardi, 04 - Sala 01 - Vila Carrão - Tel.: 3453-5007";
            Footer += "\n                                           R. Pau D'Arco Roxo, 300 - Sala 02 - Jd. Pedro José Nunes - São Miguel Paulista - Tel.: 3487-2193";

            // Cabeçalho
            myFont = new Font("Tahoma", 14, FontStyle.Regular);
            myColor = Color.Black;
            myBrush = new SolidBrush(myColor);
            myPoint.X = 45;
            myPoint.Y = 20;
            e.Graphics.DrawString(cabecalho, myFont, myBrush, myPoint);

            // Line
            myPen = new Pen(Color.Black);
            Point myP1 = new Point(50, 56);
            Point myP2 = new Point(775, 56);
            e.Graphics.DrawLine(myPen, myP1, myP2);

            // Título
            myFont = new Font("Tahoma", 20, FontStyle.Regular);
            myColor = Color.Black;
            myBrush = new SolidBrush(myColor);
            myPoint.X = 365;
            myPoint.Y = 65;
            e.Graphics.DrawString(titulo, myFont, myBrush, myPoint);

            // Print Dados Gerais
            myPoint.X = 50;
            myPoint.Y = 120;
            myFont = new Font("Tahoma", 12, FontStyle.Bold);
            e.Graphics.DrawString(DadosDoPacienteTitulo, myFont, myBrush, myPoint);

            myPoint.X = 50;
            myPoint.Y = 125;
            myFont = new Font("Tahoma", 12, FontStyle.Regular);
            e.Graphics.DrawString(DadosDoPaciente, myFont, myBrush, myPoint);

            // Print lista de medicamentos
            myPoint.X = 50;
            myPoint.Y = 300;
            myFont = new Font("Tahoma", 12, FontStyle.Bold);
            e.Graphics.DrawString(remediosTitulo, myFont, myBrush, myPoint);

            Single lineHeight = myFont.GetHeight(e.Graphics);
            myPoint.X = 50;
            myPoint.Y = 340;
            myFont = new Font("Tahoma", 12, FontStyle.Regular);
            string Medicamento = "";
            string Dosagem = "";
            List<string> ParsedLines = new List<string>();

            //foreach (DataRow r in dtMedicamentos.Rows)
            //{
            //    Medicamento = r["descricao"].ToString();
            //    Dosagem = r["dosagem"].ToString();

            //    myFont = new Font("Tahoma", 12, FontStyle.Regular);
            //    e.Graphics.DrawString(Medicamento, myFont, myBrush, myPoint);
            //    myPoint.Y += (int)lineHeight + 10;

            //    myFont = new Font("Tahoma", 10, FontStyle.Regular);
            //    e.Graphics.DrawString(Dosagem, myFont, myBrush, myPoint);
            //    myPoint.Y += (int)lineHeight + 10;
            //    myPoint.Y += (int)lineHeight;
            //}

            foreach (DataRow r in dtMedicamentos.Rows)
            {
                Medicamento = r["descricao"].ToString();
                Dosagem = r["dosagem"].ToString();

                myFont = new Font("Tahoma", 12, FontStyle.Regular);
                e.Graphics.DrawString(Medicamento, myFont, myBrush, myPoint);
                //myPoint.Y += (int)lineHeight + 10;
                myPoint.X += (int)lineHeight + 250;

                myFont = new Font("Tahoma", 10, FontStyle.Regular);
                e.Graphics.DrawString(Dosagem, myFont, myBrush, myPoint);
                myPoint.X -= (int)lineHeight + 250;
                myPoint.Y += (int)lineHeight + 10;
                //myPoint.Y += (int)lineHeight;
            }

            //// Print Local e Data
            //myPoint.X = 50;
            //myPoint.Y = 950;
            //e.Graphics.DrawString(LocalEData, myFont, myBrush, myPoint);

            //// Print Assinatura
            //myFont = new Font("Tahoma", 12, FontStyle.Regular);
            //myPoint.X = 570;
            //myPoint.Y = 1020;
            //e.Graphics.DrawString(Assinatura, myFont, myBrush, myPoint);

            //// Print AssinaturaEnd
            //myFont = new Font("Tahoma", 10, FontStyle.Regular);
            //myPoint.X = 570;
            //myPoint.Y = 1040;
            //e.Graphics.DrawString(AssinaturaEnd, myFont, myBrush, myPoint);

            //// Print Line
            //myP1 = new Point(50, 1090);
            //myP2 = new Point(775, 1090);
            //e.Graphics.DrawLine(myPen, myP1, myP2);

            //// Print Footer
            //myPoint.X = 50;
            //myPoint.Y = 1100;
            //myFont = new Font("Tahoma", 8, FontStyle.Regular);
            //e.Graphics.DrawString(Footer, myFont, myBrush, myPoint);


            // Print Local e Data
            myPoint.X = 50;
            myPoint.Y = 950;
            e.Graphics.DrawString(LocalEData, myFont, myBrush, myPoint);

            // Print Assinatura
            myFont = new Font("Tahoma", 12, FontStyle.Regular);
            myPoint.X = 570;
            myPoint.Y = 970;
            e.Graphics.DrawString(Assinatura, myFont, myBrush, myPoint);

            // Print AssinaturaEnd
            myFont = new Font("Tahoma", 10, FontStyle.Regular);
            myPoint.X = 570;
            myPoint.Y = 990;
            e.Graphics.DrawString(AssinaturaEnd, myFont, myBrush, myPoint);

            // Print Line
            myP1 = new Point(50, 1030);
            myP2 = new Point(775, 1030);
            e.Graphics.DrawLine(myPen, myP1, myP2);

            // Print Footer
            myPoint.X = 50;
            myPoint.Y = 1040;
            myFont = new Font("Tahoma", 8, FontStyle.Regular);
            e.Graphics.DrawString(Footer, myFont, myBrush, myPoint);
        }
    }
}

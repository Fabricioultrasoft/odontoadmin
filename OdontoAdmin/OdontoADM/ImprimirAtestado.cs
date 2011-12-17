using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OdontoADMDll.BLL;

namespace OdontoADM
{
    public partial class ImprimirAtestado : Form
    {
        private OdontoADMDll.BLL.Paciente oPaciente = null;
        private CID oCID = null;
        DataTable dt = null;

        public ImprimirAtestado()
        {
            InitializeComponent();
        }

        public ImprimirAtestado(OdontoADMDll.BLL.Paciente p)
        {
            InitializeComponent();
            carregarBoxes();
            oPaciente = p;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1; // Marco
            ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized;
            printPreviewDialog1.ShowDialog(); // Marco

            printDialog1.Document = printDocument1;
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string nomeArquivo = "Atestado_" + oPaciente.Nome + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString(); // Marco
                nomeArquivo = nomeArquivo.Replace(":", "_"); // Marco
                nomeArquivo = nomeArquivo.Replace("/", "-"); // Marco
                printDocument1.DocumentName = nomeArquivo;// Marco
                printDocument1.Print();
            }
            oPaciente = null;
            this.Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Util.Atestado atestado = new Util.Atestado();
            OdontoADMDll.BLL.CID c = new CID();
            atestado.Periodo = comboPeriodo.SelectedIndex + 1;
            c.CIDID = int.Parse(comboCID.SelectedValue.ToString());
            //atestado.CodigoCID = c.GetCID().Descricao;
            atestado.CodigoCID = c.GetCID().Codigo;    // Marco (07-12-2011)
            atestado.Dia = DateTime.Parse(txtDia.Text);
            atestado.FormatarAtestado(oPaciente, e);
        }

        private void carregarBoxes()
        {
            dt = null;
            oCID = new CID();
            dt = oCID.ListarCIDs();
            comboCID.ValueMember = dt.Columns["id"].ToString();
            comboCID.DisplayMember = dt.Columns["descricao"].ToString();
            comboCID.DataSource = dt;

            comboPeriodo.DataSource = criarListaPeriodo();

            txtDia.Text = DateTime.Now.ToShortDateString();
        }

        private List<string> criarListaPeriodo()
        {
            List<string> lPeriodos = new List<string>();

            string periodo1 = "Selecione o período...";
            string periodo2 = "No horário de consulta";
            string periodo3 = "No dia de hoje";
            string periodo4 = "Num período determinado";

            lPeriodos.Add(periodo1);
            lPeriodos.Add(periodo2);
            lPeriodos.Add(periodo3);
            lPeriodos.Add(periodo4);

            return lPeriodos;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            oPaciente = null;
            this.Close();
        }
    }
}

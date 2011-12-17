using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OdontoADMDll.BLL;

namespace OdontoADM
{
    public partial class ListaOrcamentos : Form
    {
        OdontoADMDll.BLL.Orcamento o = null;
        public int IdPaciente { get; set; }

        public ListaOrcamentos()
        {
            InitializeComponent();
        }

        public ListaOrcamentos(int PacienteID)
        {
            InitializeComponent();
            IdPaciente = PacienteID;
            listOrcamentos(IdPaciente);
        }

        public void listOrcamentos(int PacienteID)
        {
            DataTable dt = new DataTable();
            o = new OdontoADMDll.BLL.Orcamento();
            dt = o.ListarOrcamentosPorPaciente(IdPaciente);
            
            lboxOrcamentos.DisplayMember = "dataOrcamento";
            lboxOrcamentos.ValueMember = "id";
            lboxOrcamentos.DataSource = dt;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (lboxOrcamentos.SelectedIndex > -1)
            {
                int IdOrcamento = int.Parse(lboxOrcamentos.SelectedValue.ToString());
                Orcamento o = new Orcamento(IdPaciente, IdOrcamento);
                o.Show();
            }
            else
            {
                MessageBox.Show("Selecione um orçamento!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

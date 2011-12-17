using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OdontoADMDll.BLL;
using OrbitControls;

namespace OdontoADM
{
    //public delegate void NovoPacienteAdicionadoEventHandler();

    public partial class mainForm : Form
    {
        private OdontoADMDll.BLL.Usuario oUser = null;
        private OdontoADMDll.BLL.Paciente oPaciente = null;

        public mainForm()
        {
            InitializeComponent();
        
            pnlPacientes.Visible = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            oUser = new Usuario();
            oUser.UserName = txtUsuario.Text.Trim();
            oUser.Senha = txtSenha.Text;

            if (oUser.VerificaLogin())
            {
                lblErro.Visible = false;
                txtSenha.Text = "";
                txtUsuario.Text = "";
                pnlLogin.Visible = false;
                menuStripGeral.Visible = true;
                pnlPacientes.Visible = true;
                gbPacientes.Visible = false;

                //carregarListPacientes();
            }
            else
            {
                lblErro.Visible = true;
                lblErro.Text = "*Usuário e senha não existe!";
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oUser = null;
            menuStripGeral.Visible = false;
            pnlPacientes.Visible = false;
            pnlLogin.Visible = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //public void carregarListPacientes()
        //{
        //    dt = null;
        //    //lboxPacientes.Items.Clear();

        //    oPaciente = new Paciente();
        //    try
        //    {
        //        dt = oPaciente.ListarPacientes();
        //        if (dt.Rows.Count > 0)
        //        {
        //            gbPacientes.Visible = true;
        //            lboxPacientes.DisplayMember = "nome";
        //            lboxPacientes.ValueMember = "id";
        //            lboxPacientes.DataSource = dt;
        //        }
        //        else
        //        {
        //            lboxPacientes.DataSource = null;
        //            lboxPacientes.Refresh(); 
        //            gbPacientes.Visible = false;
        //            MessageBox.Show("Não existe nenhum paciente. Cadastra!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        //Log    
        //    }
            
        //}

        //private void lboxPacientes_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (lboxPacientes.SelectedValue != null)
        //    {
        //        string ID = lboxPacientes.SelectedValue.ToString();
        //        oPaciente = new Paciente();
        //        oPaciente = oPaciente.GetPaciente(int.Parse(ID), false);

        //        if (oPaciente != null)
        //        {
        //            gbPacientes.Visible = true;
        //            txtNome.Text = oPaciente.Nome;
        //            txtRG.Text = oPaciente.RG;
        //            txtCPF.Text = oPaciente.CPF;
        //            txtDtNascimento.Text = oPaciente.DtNascimento.ToShortDateString();
        //            txtLogradouro.Text = oPaciente.Logradouro;
        //            txtNumero.Text = oPaciente.Numero.ToString();
        //            txtCEP.Text = oPaciente.CEP;
        //            txtComplemento.Text = oPaciente.Complemento;
        //            txtTelefone.Text = oPaciente.Telefone;
        //            txtCelular.Text = oPaciente.Celular;
        //            txtEmail.Text = oPaciente.Email;
        //        }
        //    }            
        //}

        //private void adicionarNovoPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    PacienteDetalhe novoPacienteForm = new PacienteDetalhe();
        //    novoPacienteForm.Visible = true;
        //    novoPacienteForm.Text = "Adicionar novo paciente";
        //    novoPacienteForm.NovoPacienteAdicionado += new NovoPacienteAdicionadoEventHandler(carregarListPacientes);
        //}

        private void editarDadosDoPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lboxPacientes.Items.Count != 0)
            {
                if (int.Parse(lboxPacientes.SelectedValue.ToString()) == 0)
                {
                    MessageBox.Show("Selecione um paciente!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string ID = lboxPacientes.SelectedValue.ToString();
                    PacienteDetalhe pdetalhe = new PacienteDetalhe(int.Parse(ID));
                    pdetalhe.Text = "Editar dados do paciente";
                    //pdetalhe.NovoPacienteAdicionado += new NovoPacienteAdicionadoEventHandler(carregarListPacientes);
                    pdetalhe.Show();
                }
            }
            else
            {
                MessageBox.Show("Ainda não existe paciente! Cadastra pelo menos um!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void btnOrcamento_Click(object sender, EventArgs e)
        {
            string ID = lboxPacientes.SelectedValue.ToString();
            Orcamento pOrcamento = new Orcamento(int.Parse(ID),0);
            pOrcamento.Show();
        }

        private void btnEmitirAtestado_Click(object sender, EventArgs e)
        {
            ImprimirAtestado ia = new ImprimirAtestado(oPaciente);
            ia.Show();
        }

        private void listaDeCIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CadastroCID cadastroCID = new CadastroCID();
            //cadastroCID.Show();
        }

        private void imprimirRecomendaçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.Landscape = true;
            DialogResult result = printDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Util.Recomendacoes rec = new Util.Recomendacoes();
            rec.FormatarRecomendacoes(e);
        }

        private void marcarNovaConsultaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarcarConsulta mc = new MarcarConsulta(oPaciente);
            mc.Show();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            MarcarConsulta mc = new MarcarConsulta(oPaciente);
            mc.Show();
        }

        private void listaDeMedicamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // CadastroMedicamento cadastroMeds = new CadastroMedicamento();
            //cadastroMeds.Show();
        }

        private void consultarAgêndaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Agenda agenda = new Agenda();
            agenda.Show();
        }
    }
}

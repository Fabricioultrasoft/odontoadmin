using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OdontoADMDll.BLL;
using System.IO;

namespace OdontoADM
{
    public delegate void NovoServicoAdicionadoEventHandler();

    public partial class Orcamento : Form
    {
        private Bitmap MyImagem { get; set; }
        public int IDPaciente { get; set; }
        public int IDOrcamento { get; set; }
        private Paciente oPaciente;
        private OdontoADMDll.BLL.Orcamento oOrcamento;
        private List<OdontoADMDll.BLL.Servico> lServicos = new List<OdontoADMDll.BLL.Servico>();
        OdontoADMDll.BLL.Servico oServico = null;

        // Drawing Variables
        private bool draw = false;
        private Bitmap mainImage = null;
        private int s = 3;
        private Color myColor = Color.Red;
        private Size mySize = new Size(690, 325);
        
        public Orcamento()
        {
            InitializeComponent();
        }

        public Orcamento(int ID, int IDOrc)
        {
            InitializeComponent();
            FormatarGrid();
            this.IDPaciente = ID;
            if (IDOrc != 0)
            {
                this.IDOrcamento = IDOrc;
            }
            else
            {
                oOrcamento = new OdontoADMDll.BLL.Orcamento();
                lblDtOrcamento.Text = IDOrcamento.ToString();
                listarServicos();
            }
        }

        private void Orcamento_Load(object sender, EventArgs e)
        {
            oPaciente = new Paciente();
            oOrcamento = new OdontoADMDll.BLL.Orcamento();
            oPaciente = oPaciente.GetPaciente(IDPaciente, false);
            lblNomePaciente.Text = oPaciente.Nome;
            lblDtData.Text = DateTime.Now.ToShortDateString();

            if (IDOrcamento != 0)
            {
                oOrcamento.OrcamentoID = IDOrcamento;
                oOrcamento = oOrcamento.GetOrcamento();
                carregarOrcamento(oOrcamento);
            }
            else
            {
                this.IDOrcamento = oOrcamento.GetNextOrcamentoID();
                lblDtOrcamento.Text = IDOrcamento.ToString();
                loadImage();
                btnImprimir.Visible = false;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            IDPaciente = 0;

            // Verify that Orcamento was already saved
            OdontoADMDll.BLL.Orcamento o = new OdontoADMDll.BLL.Orcamento();
            int idO = int.Parse(lblDtOrcamento.Text);
            o.OrcamentoID = idO;
            o = o.GetOrcamento();
            if (o.ValorTotal == 0)
            {
                oServico = new OdontoADMDll.BLL.Servico();
                oServico.ExcluirServicosPorOrcamento(idO);
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void listarServicos()
        {
            decimal valorTotal = 0;
            oServico = new OdontoADMDll.BLL.Servico();
            DataTable dt = new DataTable();
            lServicos = new List<OdontoADMDll.BLL.Servico>();
            dt = oServico.ListarServicosPorOrcamento(IDOrcamento);
            gridServicos.DataSource = dt;

            if (gridServicos.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gridServicos.Rows)
                {
                    valorTotal += decimal.Parse(dr.Cells["valor"].Value.ToString());
                }
                btnImprimir.Visible = true;
            }

            lblValorTotalDt.Text = valorTotal.ToString();
        }

        private void btnAddServico_Click(object sender, EventArgs e)
        {
            gridServicos.Visible = true;
            Servico s = new Servico(IDOrcamento);
            s.Show();
            s.NovoServicoAdicionado += new NovoServicoAdicionadoEventHandler(listarServicos);
        }

        private void btnExcluirServico_Click(object sender, EventArgs e)
        {
            if (gridServicos.SelectedRows.Count > 0)
            {
                int id = int.Parse(gridServicos.CurrentRow.Cells[0].Value.ToString());
                oServico.ExcluirServico(id);
                MessageBox.Show("Serviço selecionado excluído com sucesso!");
                listarServicos();
            }   
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            OdontoADMDll.BLL.Servico s = new OdontoADMDll.BLL.Servico();
            if (s.ExcluirServicosPorOrcamento(IDOrcamento))
            {
                MessageBox.Show("Serviços excluídos com sucesso!");
                listarServicos();
            }
            else
            {
                MessageBox.Show("Erro em excluir os serviços!");
            }

        }

        private void btnEditarServico_Click(object sender, EventArgs e)
        {
            if (gridServicos.SelectedRows.Count > 0)
            {
                int id = int.Parse(gridServicos.CurrentRow.Cells[0].Value.ToString());
                Servico s = new Servico(this.IDOrcamento, id);
                s.Show();
                s.NovoServicoAdicionado += new NovoServicoAdicionadoEventHandler(listarServicos);
            }            
        }

        #region ToothsBox Drawing Events and Methods

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pBoxTooths.Image = null;
            loadImage();
        }

        private void loadImage()
        {
            if (MyImagem == null)
            {
                pBoxTooths.Image = null;
                MyImagem = new Bitmap(Properties.Resources.ToothChartAdult);
                mainImage = new Bitmap(MyImagem, mySize);
                pBoxTooths.Image = mainImage;
            }
            else
            {
                pBoxTooths.Image = null;                
                mainImage = new Bitmap(MyImagem, mySize);
                pBoxTooths.Image = mainImage;
            }
            
        }

        private void pBoxTooths_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            Graphics g = Graphics.FromImage(mainImage);
            Pen pen1 = new Pen(myColor,2);

            g.DrawRectangle(pen1, e.X, e.Y,1,1);
            g.Save();
            pBoxTooths.Image = mainImage;
        }

        private void pBoxTooths_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        private void pBoxTooths_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Graphics g = Graphics.FromImage(mainImage);
                
                SolidBrush brush = new SolidBrush(myColor);
                g.FillRectangle(brush, e.X, e.Y, s, s);
                g.Save();
                pBoxTooths.Image = mainImage;
            }
        }

        #endregion

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            OdontoADMDll.BLL.Orcamento o = null;
            OdontoADMDll.DAL.OrcamentoImageDAO oImageDAO = null;

            if (gridServicos.Rows.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos um Serviço!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                // Salva Imagem
                string filePath = @"C:\Program Files\OdontoAdmin\OrcamentoImages\";
                string fileName = IDOrcamento + "dentalMap.jpg";
                Bitmap imageDentalMap = new Bitmap(pBoxTooths.Image);
                FileInfo fInfo = new FileInfo(filePath + fileName);
                FileStream fs = null;

                if (!fInfo.Exists)
                {
                    fs = fInfo.Create();
                    imageDentalMap.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageDentalMap.Dispose();
                    fs.Close();
                    fs.Dispose();

                    oImageDAO = new OdontoADMDll.DAL.OrcamentoImageDAO();
                    oImageDAO.SalvarNovaImagem(IDOrcamento, (filePath + fileName));

                    o = new OdontoADMDll.BLL.Orcamento();
                    int idImage = oImageDAO.GetLastImageID();
                    o.PacienteID = IDPaciente;
                    o.ImagemID = idImage;
                    o.DataOrcamento = DateTime.Now;
                    o.Deletado = false;
                    o.ValorTotal = decimal.Parse(lblValorTotalDt.Text);
                    o.CriarNovoOrcamento();
                }
                else 
                {
                    // Funcione
                    fInfo.Delete();

                    fs = fInfo.Create();
                    imageDentalMap.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageDentalMap.Dispose();
                    fs.Close();
                    fs.Dispose();
                }         
                
                // Verificar os Serviços Salvos
                MessageBox.Show("Orçamento Cadastrado com sucesso!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void FormatarGrid()
        {
            gridServicos.AutoGenerateColumns = false;
            gridServicos.MultiSelect = false;
            DataGridViewColumn col = new DataGridViewColumn();
            DataGridViewCell cell = new DataGridViewTextBoxCell();

            col.Visible = false;
            col.CellTemplate = cell;
            col.DataPropertyName = "id";
            gridServicos.Columns.Add(col);

            col = new DataGridViewColumn();
            col.CellTemplate = cell;
            col.Name = "descricao";
            col.HeaderText = "Descrição";
            //col.Width = 568;
            col.Width = 520;
            col.DataPropertyName = "descricao";

            gridServicos.Columns.Add(col);

            col = new DataGridViewColumn();
            col.DataPropertyName = "valor";
            col.Name = "valor";
            //col.Width = 120;
            col.Width = 80;
            col.HeaderText = "Valor";
            col.CellTemplate = new DataGridViewTextBoxCell();

            gridServicos.Columns.Add(col);

            DataGridViewButtonColumn colButton = new DataGridViewButtonColumn();
            colButton.HeaderText = "";
            colButton.Text = "Remover";
            colButton.Name = "btnGridRemover";
            colButton.Width = 70;
            colButton.UseColumnTextForButtonValue = true;
            gridServicos.Columns.Add(colButton);
        }

        private void gridServicos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                int id = int.Parse(gridServicos.CurrentRow.Cells[0].Value.ToString());
                oServico.ExcluirServico(id);
                MessageBox.Show("Serviço selecionado excluído com sucesso!");
                listarServicos();
            }
        }

        private void carregarOrcamento(OdontoADMDll.BLL.Orcamento o)
        {
            lblValorTotalDt.Text = o.ValorTotal.ToString();
            lblDtData.Text = DateTime.Parse(o.DataOrcamento.ToString()).ToShortDateString();
            lblDtOrcamento.Text = o.OrcamentoID.ToString();

            DataTable dt = new DataTable();
            OdontoADMDll.BLL.Servico s = new OdontoADMDll.BLL.Servico();
            dt = s.ListarServicosPorOrcamento(o.OrcamentoID);

            gridServicos.DataSource = dt;

            // Carregar Imagem
            string imageUrl = o.GetImagemPorOrcamento(o.OrcamentoID);
            FileStream fs = new FileStream(imageUrl, FileMode.Open);
            MyImagem = (Bitmap)Image.FromStream(fs);
            fs.Flush();
            fs.Close();

            loadImage();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1; // Marco
            ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized; // Marco
            printPreviewDialog1.ShowDialog(); // Marco
            
            printDialog1.Document = printDocument1;
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string nomeArquivo = "Orçamento" + IDOrcamento + "_" + oPaciente.Nome + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString(); // Marco
                    nomeArquivo = nomeArquivo.Replace(":", "_"); // Marco
                    nomeArquivo = nomeArquivo.Replace("/", "-"); // Marco
                    printDocument1.DocumentName = nomeArquivo;   // Marco

                    printDocument1.Print();
                }
                catch (Win32Exception ex)
                {
                    MessageBox.Show("Fecha primeiro o Orçamento aberto!" + ex);
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Util.ImpressaoOrcamento impOrcamento = new Util.ImpressaoOrcamento();
            impOrcamento.ImageOrcamento = pBoxTooths.Image;
            impOrcamento.NumeroOrcamento = IDOrcamento;
            impOrcamento.PacienteID = IDPaciente;
            DataTable dtServicos = formatarServicos();
            DataRow r = null;

            foreach (DataGridViewRow dr in gridServicos.Rows)
            {
                r = dtServicos.NewRow();
                r["Descricao"] = dr.Cells["descricao"].Value.ToString();
                r["Valor"] = (decimal)dr.Cells["valor"].Value;
                dtServicos.Rows.Add(r);
            }
            impOrcamento.dtServicos = dtServicos;
            impOrcamento.FormatarOrcamento(e);            
        }

        private DataTable formatarServicos()
        {
            DataTable dt = new DataTable();
            DataColumn c = new DataColumn();
            c.ColumnName = "Descricao";
            c.DataType = System.Type.GetType("System.String");

            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Valor";
            c.DataType = System.Type.GetType("System.Decimal");

            dt.Columns.Add(c);

            return dt;
        }      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdontoADMDll.DAL;
using System.Data;

namespace OdontoADMDll.BLL
{
    public class Paciente
    {
        private int _PacienteID;
        private string _Nome;
        private string _RG;
        private string _CPF;
        private DateTime _DtNascimento;
        private string _Logradouro;
        private int _Numero;
        private string _Complemento;
        private string _CEP;
        private string _Telefone;
        private string _Celular;
        private string _Email;
        private Boolean _Deletado;

        private PacienteDAO oPacienteDAO = null;
        private Boolean retvalor = false;
        private DataTable dt = null;
        private Paciente oPaciente = null;

        public int PacienteID { get { return _PacienteID; } set { _PacienteID = value; } }
        public string Nome { get { return _Nome; } set { _Nome = value; } }
        public string RG { get { return _RG; } set { _RG = value; } }
        public string CPF { get { return _CPF; } set { _CPF = value; } }
        public DateTime DtNascimento { get { return _DtNascimento; } set { _DtNascimento = value; } }
        public string Logradouro { get { return _Logradouro; } set { _Logradouro = value; } }
        public int Numero { get { return _Numero; } set { _Numero = value; } }
        public string Complemento { get { return _Complemento; } set { _Complemento = value; } }
        public string CEP { get { return _CEP; } set { _CEP = value ;} }
        public string Telefone { get { return _Telefone; } set { _Telefone = value; } }
        public string Celular { get { return _Celular; } set { _Celular = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public Boolean Deletado { get { return _Deletado; } set { _Deletado = value; } }

        public Boolean CriarNovoPaciente()
        {
            retvalor = false;

            try
            {
                oPacienteDAO = new PacienteDAO();
                retvalor = oPacienteDAO.CriarNovoPaciente(Nome, RG, CPF, DtNascimento, Logradouro, Numero, Complemento, CEP, Telefone, Celular, Email, Deletado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oPacienteDAO = null;
            }

            return retvalor;
        }

        public Boolean AtualizaPaciente()
        {
            retvalor = false;

            try
            {
                oPacienteDAO = new PacienteDAO();
                retvalor = oPacienteDAO.AtualizaPaciente(PacienteID, Nome, RG, CPF, DtNascimento, Logradouro,
                                                         Numero, Complemento, CEP, Telefone, Celular, Email, Deletado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oPacienteDAO = null;
            }

            return retvalor;
        }

        public Boolean ExcluiPaciente(int id)
        {
            retvalor = false;

            try
            {
                oPacienteDAO = new PacienteDAO();
                retvalor = oPacienteDAO.ExcluiPaciente(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oPacienteDAO = null;
            }

            return retvalor;            
        }

        public Paciente GetPaciente(int id, Boolean deletado)
        {
            oPaciente = new Paciente();

            try
            {
                oPacienteDAO = new PacienteDAO();
                dt = oPacienteDAO.GetPaciente(id, deletado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oPacienteDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oPaciente.PacienteID = int.Parse(dt.Rows[0]["id"].ToString());
                oPaciente.Nome = dt.Rows[0]["nome"].ToString();
                oPaciente.RG = dt.Rows[0]["rg"].ToString();
                oPaciente.CPF = dt.Rows[0]["cpf"].ToString();
                oPaciente.DtNascimento = DateTime.Parse(dt.Rows[0]["dtNascimento"].ToString());
                oPaciente.Logradouro = dt.Rows[0]["logradouro"].ToString();
                oPaciente.Numero = int.Parse(dt.Rows[0]["numero"].ToString());
                oPaciente.CEP = dt.Rows[0]["cep"].ToString();
                oPaciente.Complemento = dt.Rows[0]["complemento"].ToString();
                oPaciente.Telefone = dt.Rows[0]["telefone"].ToString();
                oPaciente.Celular = dt.Rows[0]["celular"].ToString();
                oPaciente.Email = dt.Rows[0]["email"].ToString();
                oPaciente.Deletado = Boolean.Parse(dt.Rows[0]["deletado"].ToString());
            }

            return oPaciente;
        }

        public DataTable ListarPacientes()
        {
            dt = new DataTable();
            try
            {
                oPacienteDAO = new PacienteDAO();
                dt = oPacienteDAO.ListarPacientes();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                oPacienteDAO = null;
            }

            return dt;
        }
    }
}

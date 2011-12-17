using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OdontoADMDll.DAL;

namespace OdontoADMDll.BLL
{
    public class Medicamento
    {
        private int _MedicamentoID;
        private string _Descricao;
        private string _Codigo;

        private MedicamentoDAO oMedicamentoDAO = null;
        private bool retvalor = false;
        private DataTable dt = null;
        private Medicamento oMedicamento = null;

        public int MedicamentoID { get { return _MedicamentoID; } set { _MedicamentoID = value; } }
        public string Descricao { get { return _Descricao; } set { _Descricao = value; } }
        public string Codigo { get { return _Codigo; } set { _Codigo = value; } }

        public bool CriarNovoMedicamento()
        {
            retvalor = false;

            try
            {
                oMedicamentoDAO = new MedicamentoDAO();
                retvalor = oMedicamentoDAO.CriarNovoMedicamento(Descricao, Codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMedicamentoDAO = null;
            }

            return retvalor;
        }

        public bool AtualizarMedicamento()
        {
            retvalor = false;
            try
            {
                oMedicamentoDAO = new MedicamentoDAO();
                retvalor = oMedicamentoDAO.AtualizaMedicamento(MedicamentoID, Descricao, Codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMedicamentoDAO = null;
            }

            return retvalor;
        }

        public bool ExcluirMedicamento()
        {
            retvalor = false;

            try
            {
                oMedicamentoDAO = new MedicamentoDAO();
                retvalor = oMedicamentoDAO.ExcluiMedicamento(MedicamentoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMedicamentoDAO = null;
            }

            return retvalor;
        }

        public Medicamento GetMedicamento()
        {
            oMedicamento = new Medicamento();

            try
            {
                oMedicamentoDAO = new MedicamentoDAO();
                dt = oMedicamentoDAO.GetMedicamento(MedicamentoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMedicamentoDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oMedicamento.MedicamentoID = int.Parse(dt.Rows[0]["id"].ToString());
                oMedicamento.Descricao = dt.Rows[0]["descricao"].ToString();
                oMedicamento.Codigo = dt.Rows[0]["codigo"].ToString();
            }

            return oMedicamento;
        }

        public DataTable ListarMedicamentos()
        {
            dt = new DataTable();
            try
            {
                oMedicamentoDAO = new MedicamentoDAO();
                dt = oMedicamentoDAO.ListarMedicamentos();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                oMedicamentoDAO = null;
            }

            return dt;
        }
    }
}

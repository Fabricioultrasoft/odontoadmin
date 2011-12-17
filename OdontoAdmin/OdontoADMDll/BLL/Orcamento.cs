using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OdontoADMDll.DAL;

namespace OdontoADMDll.BLL
{
    public class Orcamento
    {
        private int _OrcamentoID;
        private int _PacienteID;
        private int _ImagemID;
        private decimal _ValorTotal;
        private DateTime _DataOrcamento;
        private Boolean _Deletado;

        private DAL.OrcamentoDAO oOrcamentoDAO = null;
        private Boolean retvalor = false;
        private DataTable dt = null;
        private Orcamento oOrcamento = null;

        public int OrcamentoID { get { return _OrcamentoID; } set { _OrcamentoID = value; } }
        public int PacienteID { get { return _PacienteID; } set { _PacienteID = value; } }
        public int ImagemID { get { return _ImagemID; } set { _ImagemID = value; } }
        public decimal ValorTotal { get { return _ValorTotal; } set { _ValorTotal = value; } }
        public DateTime DataOrcamento { get { return _DataOrcamento; } set { _DataOrcamento = value; } }
        public Boolean Deletado { get { return _Deletado; } set { _Deletado = value; } }

        public Boolean CriarNovoOrcamento()
        {
            retvalor = false;

            try
            {
                oOrcamentoDAO = new OrcamentoDAO();
                retvalor = oOrcamentoDAO.CriarNovoOrcamento(PacienteID, ImagemID, ValorTotal, DataOrcamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oOrcamentoDAO = null;
            }

            return retvalor;
        }

        public Boolean AtualizaOrcamento()
        {
            retvalor = false;

            try
            {
                oOrcamentoDAO = new OrcamentoDAO();
                retvalor = oOrcamentoDAO.AtualizaOrcamento(OrcamentoID, PacienteID, ImagemID, ValorTotal, DataOrcamento, Deletado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oOrcamentoDAO = null;
            }

            return retvalor;
        }

        public Boolean ExcluiOrcamento()
        {
            retvalor = false;

            try
            {
                oOrcamentoDAO = new OrcamentoDAO();
                retvalor = oOrcamentoDAO.ExcluiOrcamento(OrcamentoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oOrcamentoDAO = null;
            }

            return retvalor;
        }

        public Orcamento GetOrcamento()
        {
            oOrcamento = new Orcamento();

            try
            {
                oOrcamentoDAO = new OrcamentoDAO();
                dt = oOrcamentoDAO.GetOrcamento(OrcamentoID, Deletado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oOrcamentoDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oOrcamento.OrcamentoID = int.Parse(dt.Rows[0]["id"].ToString());
                oOrcamento.PacienteID = int.Parse(dt.Rows[0]["idPaciente"].ToString());
                oOrcamento.ImagemID = int.Parse(dt.Rows[0]["idImagem"].ToString());
                oOrcamento.ValorTotal = decimal.Parse(dt.Rows[0]["valorTotal"].ToString());
                oOrcamento.Deletado = Boolean.Parse(dt.Rows[0]["deletado"].ToString());
                oOrcamento.DataOrcamento = DateTime.Parse(dt.Rows[0]["dataOrcamento"].ToString());
            }

            return oOrcamento;
        }

        public int GetNextOrcamentoID()
        {
            int id = 0;
            try
            {
                oOrcamentoDAO = new OrcamentoDAO();
                id = oOrcamentoDAO.GetNextOrcamentoID();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        public DataTable ListarOrcamentosPorPaciente(int PacienteID)
        {
            dt = new DataTable();
            try
            {
                oOrcamentoDAO = new OrcamentoDAO();
                dt = oOrcamentoDAO.ListarOrcamentosPorPaciente(PacienteID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oOrcamentoDAO = null;
            }

            return dt;
        }

        public string GetImagemPorOrcamento(int OrcamentoID)
        {
            string imagePath = "";
            try
            {
                OdontoADMDll.DAL.OrcamentoImageDAO oImageDAO = new OrcamentoImageDAO();
                imagePath = oImageDAO.GetImagem(OrcamentoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return imagePath;
        }

        public bool AtualizarImagem(int idOrcamento, string imageUrl)
        {
            try
            {
                OdontoADMDll.DAL.OrcamentoImageDAO oImageDAO = new OrcamentoImageDAO();
                retvalor = oImageDAO.AtualizarImagem(idOrcamento, imageUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retvalor;
        }
    }
}

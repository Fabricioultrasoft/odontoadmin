using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OdontoADMDll.DAL;

namespace OdontoADMDll.BLL
{
    public class Servico
    {
        private int _ServicoID = 0;
        private string _Descricao = "";
        private decimal _Valor = 0;
        private int _IdOrcamento = 0;

        private ServicoDAO oServicoDAO = null;
        private Boolean retvalor = false;
        private DataTable dt = null;
        private Servico oServico = null;

        public int ID { get { return _ServicoID; } set { _ServicoID = value; } }
        public string Descricao { get { return _Descricao; } set { _Descricao = value; } }
        public decimal Valor { get { return _Valor; } set { _Valor = value; } }
        public int IDOrcamento { get { return _IdOrcamento; } set { _IdOrcamento = value; } }

        public Boolean CriarNovoServico(int idOrcamento)
        {
            retvalor = false;

            try
            {
                oServicoDAO = new ServicoDAO();
                retvalor = oServicoDAO.CriarNovoServico(Descricao, Valor, idOrcamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            return retvalor;
        }

        public Boolean AtualizaServico()
        {
            retvalor = false;
            try
            {
                oServicoDAO = new ServicoDAO();
                retvalor = oServicoDAO.AtualizaServico(ID, Descricao, Valor, IDOrcamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            return retvalor;
        }

        public Boolean ExcluirServico(int id)
        {
            retvalor = false;

            try
            {
                oServicoDAO = new ServicoDAO();
                retvalor = oServicoDAO.ExcluiServico(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            return retvalor;
        }

        public Servico GetServico(int id)
        {
            oServico = new Servico();

            try
            {
                oServicoDAO = new ServicoDAO();
                dt = oServicoDAO.GetServico(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oServico.ID = int.Parse(dt.Rows[0]["id"].ToString());
                oServico.Descricao = dt.Rows[0]["descricao"].ToString();
                oServico.Valor = decimal.Parse(dt.Rows[0]["valor"].ToString());
            }

            return oServico;
        }

        public DataTable ListarServicosPorOrcamento(int idOrcamento)
        {
            dt = new DataTable();
            try
            {
                oServicoDAO = new ServicoDAO();
                dt = oServicoDAO.ListarServicosPorOrcamento(idOrcamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            return dt;
        }

        public bool ExcluirServicosPorOrcamento(int idOrcamento)
        {
            retvalor = false;

            try
            {
                oServicoDAO = new ServicoDAO();
                retvalor = oServicoDAO.ExcluirServicosPorOrcamento(idOrcamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            return retvalor;
        }

        public int SelecionaUltimoID()
        {
            int id = 0;
            try
            {
                oServicoDAO = new ServicoDAO();
                id = oServicoDAO.GetUltimoID();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oServicoDAO = null;
            }

            return id;
        }
    }
}

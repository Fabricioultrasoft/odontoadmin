using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OdontoADMDll.DAL;

namespace OdontoADMDll.BLL
{
    public class CID
    {
        private int _CIDID;
        private string _Descricao;
        private string _Codigo;

        private CIDDAO oCIDDAO = null;
        private bool retvalor = false;
        private DataTable dt = null;
        private CID oCID = null;

        public int CIDID { get { return _CIDID; } set { _CIDID = value; } }
        public string Descricao { get { return _Descricao; } set { _Descricao = value; } }
        public string Codigo { get { return _Codigo; } set { _Codigo = value; } }

        public bool CriarNovoCID()
        {
            retvalor = false;
            try
            {
                oCIDDAO = new CIDDAO();
                retvalor = oCIDDAO.CriarNovoCID(Descricao, Codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCIDDAO = null;
            }

            return retvalor;
        }

        public bool AtualizarCID()
        {
            retvalor = false;
            try
            {
                oCIDDAO = new CIDDAO();
                retvalor = oCIDDAO.AtualizaCID(CIDID, Descricao, Codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCIDDAO = null;
            }

            return retvalor;
        }

        public bool ExcluirCID()
        {
            retvalor = false;
            try
            {
                oCIDDAO = new CIDDAO();
                retvalor = oCIDDAO.ExcluiCID(CIDID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCIDDAO = null;
            }

            return retvalor;
        }

        public CID GetCID()
        {
            oCID = new CID();

            try
            {
                oCIDDAO = new CIDDAO();
                dt = oCIDDAO.GetCID(CIDID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCIDDAO = null;
            }

            if (dt.Rows.Count > 0)
            { 
                oCID.CIDID = int.Parse(dt.Rows[0]["id"].ToString());
                oCID.Descricao = dt.Rows[0]["descricao"].ToString();
                oCID.Codigo = dt.Rows[0]["codigo"].ToString();
            }

            return oCID;
        }

        public DataTable ListarCIDs()
        {
            dt = new DataTable();
            try
            {
                oCIDDAO = new CIDDAO();
                dt = oCIDDAO.ListCIDs();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                oCIDDAO = null;
            }

            return dt;
        }

    }
}

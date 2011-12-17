using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OdontoADMDll.DAL;

namespace OdontoADMDll.BLL
{
    public class Consulta
    {
        private int _ConsultaID;
        private int _PacienteID;
        private DateTime _DataConsulta;

        private ConsultaDAO oConsultaDAO = null;
        private Boolean retvalor = false;
        private DataTable dt = null;
        private Consulta oConsulta = null;

        public int ConsultaID { get { return _ConsultaID; } set { _ConsultaID = value; } }
        public int PacienteID { get { return _PacienteID; } set { _PacienteID = value; } }
        public DateTime DataConsulta { get { return _DataConsulta; } set { _DataConsulta = value; } }

        public bool CriarNovaConsulta()
        {
            retvalor = false;
            try
            {
                oConsultaDAO = new ConsultaDAO();
                retvalor = oConsultaDAO.CriarNovaConsulta(PacienteID, DataConsulta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConsultaDAO = null;
            }

            return retvalor;
        }

        public bool AtualizarConsulta()
        {
            retvalor = false;
            try
            {
                oConsultaDAO = new ConsultaDAO();
                retvalor = oConsultaDAO.AtualizarConsulta(ConsultaID, PacienteID, DataConsulta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConsultaDAO = null;
            }

            return retvalor;
        }

        public bool ExcluirConsulta(int id)
        {
            retvalor = false;
            try
            {
                oConsultaDAO = new ConsultaDAO();
                retvalor = oConsultaDAO.ExcluirConsulta(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConsultaDAO = null;
            }

            return retvalor;
        }

        public Consulta GetConsulta(int id)
        {
            oConsulta = new Consulta();

            try
            {
                oConsultaDAO = new ConsultaDAO();
                dt = oConsultaDAO.GetConsulta(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConsultaDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oConsulta.ConsultaID = int.Parse(dt.Rows[0]["id"].ToString());
                oConsulta.PacienteID = int.Parse(dt.Rows[0]["idPaciente"].ToString());
                oConsulta.DataConsulta = DateTime.Parse(dt.Rows[0]["data"].ToString());
            }

            return oConsulta;
        }

        public DataTable ListarConsultas()
        {
            dt = null;

            try
            {
                oConsultaDAO = new ConsultaDAO();
                dt = oConsultaDAO.ListarConsultas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConsultaDAO = null;
            }

            return dt;
        }

    }
}

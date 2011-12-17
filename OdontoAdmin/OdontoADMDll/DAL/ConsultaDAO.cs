using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class ConsultaDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataTable dt = null;
        private Boolean retvalor = false;
        private int ConsultaID = 0;
        private SQLHelper helper = null;

        public bool CriarNovaConsulta(int idPaciente, DateTime data)
        {
            retvalor = false;
            ConsultaID = 0;
            helper = new SQLHelper();
            ConsultaID = helper.ProximoID("tb_consulta");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_consulta " +
                                        "(id, idPaciente, data) VALUES " +
                                        "(@id, @idPaciente, @data)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ConsultaID;
                myCommand.Parameters.Add("@idPaciente", SqlDbType.Int).Value = idPaciente;
                myCommand.Parameters.Add("@data", SqlDbType.DateTime2).Value = data;
                myCommand.Transaction = myTransaction;
                myCommand.ExecuteNonQuery();
                myCommand.Parameters.Clear();

                myTransaction.Commit();

                retvalor = true;
            }
            catch (Exception ex)
            {
                myTransaction.Rollback();
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return retvalor;
        }

        public bool AtualizarConsulta(int idConsulta, int idPaciente, DateTime data)
        {
            retvalor = false;
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "UPDATE tb_consulta " +
                                        "SET idPaciente = @idPaciente, data = @data " +
                                        "WHERE id = @idConsulta";
                myCommand.Parameters.Add("@idPaciente", SqlDbType.Int).Value = idPaciente;
                myCommand.Parameters.Add("@data", SqlDbType.DateTime2).Value = data;
                myCommand.Parameters.Add("@idConsulta", SqlDbType.Int).Value = idConsulta;
                myCommand.Transaction = myTransaction;
                myCommand.ExecuteNonQuery();
                myCommand.Parameters.Clear();

                myTransaction.Commit();
                retvalor = true;
            }
            catch (Exception ex)
            {
                myTransaction.Rollback();
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return retvalor;
        }

        public bool ExcluirConsulta(int idConsulta)
        {
            retvalor = false;
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "DELETE FROM tb_consulta WHERE id = @idConsulta";
                myCommand.Parameters.Add("@idConsulta", SqlDbType.Int).Value = idConsulta;
                myCommand.Transaction = myTransaction;
                myCommand.ExecuteNonQuery();
                myCommand.Parameters.Clear();

                myTransaction.Commit();
                retvalor = true;
            }
            catch (Exception ex)
            {
                myTransaction.Rollback();
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return retvalor;                
        }

        public DataTable GetConsulta(int idConsulta)
        {
            dt = null;

            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT * FROM tb_consulta WHERE id = @idConsulta";
                myCommand.Parameters.Add("@idConsulta", SqlDbType.Int).Value = idConsulta;
                myCommand.Transaction = myTransaction;

                myDataAdapter = new SqlDataAdapter(myCommand);
                dt = new DataTable();
                myDataAdapter.Fill(dt);
                myCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                dt = null;
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return dt;
        }

        public DataTable ListarConsultas()
        { 
            dt = null;

            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT C.id, P.nome, C.data FROM tb_consulta AS C " +
                                        "INNER JOIN tb_paciente AS P " +
                                        "ON P.id = C.idPaciente";
                myCommand.Transaction = myTransaction;

                myDataAdapter = new SqlDataAdapter(myCommand);
                dt = new DataTable();
                myDataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return dt;
        }

        private void clearObjects()
        {
            if (myTransaction != null)
            {
                myTransaction.Dispose();
                myTransaction = null;
            }

            if (myDataAdapter != null)
            {
                myDataAdapter.Dispose();
                myDataAdapter = null;
            }

            if (myCommand != null)
            {
                myCommand.Cancel();
                myCommand.Dispose();
                myCommand = null;
            }

            if (myCon != null)
            {
                if (myCon.State == ConnectionState.Open)
                {
                    myCon.Close();
                }
                myCon.Dispose();
                myCon = null;
            }
        }
    }
}

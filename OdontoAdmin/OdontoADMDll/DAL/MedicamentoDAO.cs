using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class MedicamentoDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataTable dt = null;
        private Boolean retvalor = false;
        private int MedicamentoID = 0;
        private SQLHelper helper = null;

        public Boolean CriarNovoMedicamento(string descricao, string codigo)
        { 
            retvalor = false;
            MedicamentoID = 0;
            helper = new SQLHelper();
            MedicamentoID = helper.ProximoID("tb_medicamento");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_medicamento " +
                                        "(id, descricao, codigo) VALUES (@id, @descricao, @codigo)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = MedicamentoID;
                myCommand.Parameters.Add("@descricao", SqlDbType.NVarChar, 120).Value = descricao;
                myCommand.Parameters.Add("@codigo", SqlDbType.NVarChar, 20).Value = codigo;
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

        public Boolean AtualizaMedicamento(int ID, string descricao, string codigo)
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
                 myCommand.CommandText = "UPDATE tb_medicamento " +
                                         "SET descricao = @descricao, codigo = @codigo " +
                                         "WHERE id = @id";
                 myCommand.Parameters.Add("@descricao", SqlDbType.NVarChar, 120).Value = descricao;
                 myCommand.Parameters.Add("@codigo", SqlDbType.NVarChar, 20).Value = codigo;
                 myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ID;
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

        public Boolean ExcluiMedicamento(int ID)
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
                myCommand.CommandText = "DELETE FROM tb_medicamento WHERE id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ID;
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

        public DataTable GetMedicamento(int ID)
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
                myCommand.CommandText = "SELECT * FROM tb_medicamento WHERE id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ID;
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

        public DataTable ListarMedicamentos()
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
                myCommand.CommandText = "SELECT * " +
                                        "FROM tb_medicamento " +
                                        "ORDER BY tb_medicamento.descricao";

                myCommand.Transaction = myTransaction;
                myDataAdapter = new SqlDataAdapter(myCommand);
                dt = new DataTable();
                myDataAdapter.Fill(dt);

                myTransaction.Commit();
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

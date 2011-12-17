using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OdontoADMDll.DAL
{
    public class CIDDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataTable dt = null;
        private Boolean retvalor = false;
        private int CIDID = 0;
        private SQLHelper helper = null;

        public bool CriarNovoCID(string descricao, string codigo)
        { 
            retvalor = false;
            CIDID = 0;
            helper = new SQLHelper();
            CIDID = helper.ProximoID("tb_cid");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_cid " +
                                        "(id, descricao, codigo) VALUES " +
                                        "(@id, @descricao, @codigo)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = CIDID;
                myCommand.Parameters.Add("@descricao", SqlDbType.NVarChar, 200).Value = descricao.Trim();
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

        public bool AtualizaCID(int id, string descricao, string codigo)
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
                myCommand.CommandText = "UPDATE tb_cid " +
                                        "SET descricao = @descricao, codigo = @codigo " +
                                        "WHERE id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                myCommand.Parameters.Add("@descricao", SqlDbType.NVarChar, 200).Value = descricao.Trim();
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

        public bool ExcluiCID(int id)
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
                myCommand.CommandText = "DELETE FROM tb_cid WHERE id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
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

        public DataTable GetCID(int id)
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
                myCommand.CommandText = "SELECT * FROM tb_cid WHERE id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
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

        public DataTable ListCIDs()
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
                myCommand.CommandText = "SELECT tb_cid.id, tb_cid.codigo + '  ' + tb_cid.descricao AS descricao " +
                                        "FROM tb_cid " +
                                        "ORDER BY tb_cid.codigo";

                myCommand.Transaction = myTransaction;
                myDataAdapter = new SqlDataAdapter(myCommand);
                dt = new DataTable();
                myDataAdapter.Fill(dt);

                myTransaction.Commit();
            }
            catch(Exception ex)
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

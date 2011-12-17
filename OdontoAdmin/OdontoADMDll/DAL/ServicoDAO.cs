using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class ServicoDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataTable dt = null;
        private Boolean retvalor = false;
        private int ServicoID = 0;
        private SQLHelper helper = null;

        public Boolean CriarNovoServico(String Descricao, Decimal ValorPorServico, int IdOrcamento)
        {
            retvalor = false;
            ServicoID = 0;
            helper = new SQLHelper();
            ServicoID = helper.ProximoID("tb_servico");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_servico " +
                                        "(id, descricao, valor, idOrcamento) VALUES (@id, @descricao, @valor, @idOrcamento)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ServicoID;
                myCommand.Parameters.Add("@descricao", SqlDbType.NVarChar, 150).Value = Descricao.Trim();
                myCommand.Parameters.Add("@valor", SqlDbType.Decimal).Value = ValorPorServico;
                myCommand.Parameters.Add("@idOrcamento", SqlDbType.Int).Value = IdOrcamento;
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

        public Boolean AtualizaServico(int ID, String Descricao, Decimal ValorPorServico, int IdOrcamento)
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
                myCommand.CommandText = "UPDATE tb_servico " +
                                        "SET descricao = @descricao, valor = @valor, idOrcamento = @idOrcamento " +
                                        "WHERE id = @ServicoId";
                myCommand.Parameters.Add("@ServicoId", SqlDbType.Int).Value = ID;
                myCommand.Parameters.Add("@descricao", SqlDbType.NVarChar, 150).Value = Descricao.Trim();
                myCommand.Parameters.Add("@valor", SqlDbType.Decimal).Value = ValorPorServico;
                myCommand.Parameters.Add("@idOrcamento", SqlDbType.Int).Value = IdOrcamento;
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

        public Boolean ExcluiServico(int ServicoID)
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
                myCommand.CommandText = "DELETE FROM tb_servico WHERE id = @servicoID";
                myCommand.Parameters.Add("@servicoID", SqlDbType.Int).Value = ServicoID;
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

        public DataTable GetServico(int ServicoID)
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
                myCommand.CommandText = "SELECT * FROM tb_servico WHERE id = @ServicoID";
                myCommand.Parameters.Add("@ServicoID", SqlDbType.Int).Value = ServicoID;
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

        public DataTable ListarServicosPorOrcamento(int OrcamentoID)
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
                myCommand.CommandText = "SELECT * FROM tb_servico " +
                                        "WHERE idOrcamento = @IdOrcamento " +
                                        "ORDER BY descricao";
                myCommand.Parameters.Add("@IdOrcamento", SqlDbType.Int).Value = OrcamentoID;
                myCommand.Transaction = myTransaction;

                myDataAdapter = new SqlDataAdapter(myCommand);
                dt = new DataTable();
                myDataAdapter.Fill(dt);

                myTransaction.Commit();
            }
            catch (Exception ex)
            {
                myTransaction.Rollback();
                dt = null;
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return dt;
        }

        public int GetUltimoID()
        {
            int id = 0;

            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT MAX(id) AS id FROM tb_servico";
                myCommand.Transaction = myTransaction;

                SqlDataReader r = myCommand.ExecuteReader();

                if (r.Read())
                {
                    id = int.Parse(r["id"].ToString());
                }
                r.Close();

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

            return id;
        }

        public bool ExcluirServicosPorOrcamento(int idOrcamento)
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
                myCommand.CommandText = "DELETE FROM tb_servico WHERE idOrcamento = @idOrcamento";
                myCommand.Parameters.Add("@idOrcamento", SqlDbType.Int).Value = idOrcamento;
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

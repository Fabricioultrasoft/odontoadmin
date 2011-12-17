using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class OrcamentoDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataTable dt = null;
        private Boolean retvalor = false;
        private int OrcamentoID = 0;
        private SQLHelper helper = null;

        public Boolean CriarNovoOrcamento(int idPaciente,
                                          int idImagem, 
                                          decimal valorTotal,
                                          DateTime dataOrcamento)
        {
            retvalor = false;
            OrcamentoID = 0;
            helper = new SQLHelper();
            OrcamentoID = helper.ProximoID("tb_orcamento");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_orcamento " +
                                        "(id, idPaciente, idImagem, valorTotal, deletado, dataOrcamento) VALUES " +
                                        "(@id, @idPaciente, @idImagem, @valorTotal, @deletado, @dataOrcamento)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = OrcamentoID;
                myCommand.Parameters.Add("@idPaciente", SqlDbType.Int).Value = idPaciente;
                myCommand.Parameters.Add("@idImagem", SqlDbType.Int).Value = idImagem;
                myCommand.Parameters.Add("@valorTotal", SqlDbType.Decimal).Value = valorTotal;
                myCommand.Parameters.Add("@dataOrcamento", SqlDbType.DateTime).Value = dataOrcamento;
                myCommand.Parameters.Add("@deletado", SqlDbType.Bit).Value = false;
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

        public Boolean AtualizaOrcamento(int idOrcamento, 
                                         int idPaciente, 
                                         int idImagem,
                                         decimal valorTotal,
                                         DateTime dataOrcamento,
                                         bool deletado)
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
                myCommand.CommandText = "UPDATE tb_orcamento " +
                                        "SET idPaciente = @idPaciente, idImagem = @idImagem, " +
                                        "valorTotal = @valorTotal, deletado = @deletado, dataOrcamento = @dataOrcamento " +
                                        "tituloOrcamento = @tituloOrcamento " +
                                        "WHERE id = @id";
                myCommand.Parameters.Add("@idPaciente", SqlDbType.Int).Value = idPaciente;
                myCommand.Parameters.Add("@idImagem", SqlDbType.Int).Value = idImagem;
                myCommand.Parameters.Add("@valorTotal", SqlDbType.Decimal).Value = valorTotal;
                myCommand.Parameters.Add("@dataOrcamento", SqlDbType.DateTime).Value = dataOrcamento;
                myCommand.Parameters.Add("@deletado", SqlDbType.Bit).Value = deletado;
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = idOrcamento;
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

        public Boolean ExcluiOrcamento(int idOrcamento)
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
                myCommand.CommandText = "UPDATE tb_orcamento SET deletado = 1 WHERE id = @idOrcamento";
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

        public DataTable GetOrcamento(int idOrcamento, Boolean deletado)
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
                myCommand.CommandText = "SELECT * FROM tb_orcamento WHERE id = @idOrcamento AND deletado = @deletado";
                myCommand.Parameters.Add("@idOrcamento", SqlDbType.Int).Value = idOrcamento;
                myCommand.Parameters.Add("@deletado", SqlDbType.Bit).Value = deletado;
                myCommand.Transaction = myTransaction;

                myDataAdapter = new SqlDataAdapter(myCommand);
                dt = new DataTable();
                myDataAdapter.Fill(dt);
                myCommand.Parameters.Clear();
                myTransaction.Connection.Close();
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

        public int GetNextOrcamentoID()
        {
            int id = 0;

            try
            {
                helper = new SQLHelper();
                id = helper.ProximoID("tb_orcamento");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        public DataTable ListarOrcamentosPorPaciente(int PacienteID)
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
                myCommand.CommandText = "SELECT * FROM tb_orcamento AS O " +
                                        "INNER JOIN tb_paciente AS P " +
                                        "ON P.id = O.idPaciente " +
                                        "INNER JOIN tb_orcamento_imagem AS I " +
                                        "ON I.id = O.idImagem " +
                                        "WHERE O.idPaciente = @idPaciente";
                myCommand.Parameters.Add("@idPaciente", SqlDbType.Int).Value = PacienteID;
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

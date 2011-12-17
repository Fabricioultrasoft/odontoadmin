using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class PacienteDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataTable dt = null;
        private Boolean retvalor = false;
        private int PacienteID = 0;
        private SQLHelper helper = null;        

        public Boolean CriarNovoPaciente(String Nome, String RG, String CPF, DateTime DtNascimento, String Logradouro, int Numero,
                                         String Complemento, String CEP, String Telefone, String Celular, String Email, Boolean Deletado)
        {
            retvalor = false;
            PacienteID = 0;
            helper = new SQLHelper();
            PacienteID = helper.ProximoID("tb_paciente");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_paciente " +
                                        "(id, nome, rg, cpf, dtNascimento, logradouro, numero, complemento, cep, telefone, celular, email, deletado) VALUES " +
                                        "(@id, @nome, @rg, @cpf, @dtNascimento, @logradouro, @numero, @complemento, @cep, @telefone, @celular, @email, @deletado)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = PacienteID;
                myCommand.Parameters.Add("@nome", SqlDbType.NVarChar, 150).Value = Nome.Trim();
                myCommand.Parameters.Add("@rg", SqlDbType.NVarChar, 15).Value = RG;
                myCommand.Parameters.Add("@cpf", SqlDbType.NVarChar, 11).Value = CPF;
                myCommand.Parameters.Add("@dtNascimento", SqlDbType.Date).Value = DtNascimento;
                myCommand.Parameters.Add("@logradouro", SqlDbType.NVarChar, 120).Value = Logradouro.Trim();
                myCommand.Parameters.Add("@numero", SqlDbType.Int).Value = Numero;
                myCommand.Parameters.Add("@complemento", SqlDbType.NVarChar, 120).Value = Complemento;
                myCommand.Parameters.Add("@cep", SqlDbType.NVarChar, 20).Value = CEP;
                myCommand.Parameters.Add("@telefone", SqlDbType.NVarChar, 20).Value = Telefone;
                myCommand.Parameters.Add("@celular", SqlDbType.NVarChar, 20).Value = Celular;
                myCommand.Parameters.Add("@email", SqlDbType.NVarChar, 120).Value = Email;
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

        public Boolean AtualizaPaciente(int PacienteID, String Nome, String RG, String CPF, DateTime DtNascimento, String Logradouro, int Numero,
                                         String Complemento, String CEP, String Telefone, String Celular, String Email, Boolean Deletado)
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
                myCommand.CommandText = "UPDATE tb_paciente " +
                    "SET nome = @nome, rg = @rg, cpf = @cpf, dtNascimento = @dtNascimento, logradouro = @logradouro, numero = @numero, complemento = @complemento, " +
                    "cep = @cep, telefone = @telefone, celular = @celular, email = @email, deletado = @deletado " +
                    "WHERE id = @pacienteID";
                myCommand.Parameters.Add("@nome", SqlDbType.NVarChar, 150).Value = Nome.Trim();
                myCommand.Parameters.Add("@rg", SqlDbType.NVarChar, 15).Value = RG;
                myCommand.Parameters.Add("@cpf", SqlDbType.NVarChar, 11).Value = CPF;
                myCommand.Parameters.Add("@dtNascimento", SqlDbType.Date).Value = DtNascimento;
                myCommand.Parameters.Add("@logradouro", SqlDbType.NVarChar, 120).Value = Logradouro.Trim();
                myCommand.Parameters.Add("@numero", SqlDbType.Int).Value = Numero;
                myCommand.Parameters.Add("@complemento", SqlDbType.NVarChar, 120).Value = Complemento.Trim();
                myCommand.Parameters.Add("@cep", SqlDbType.NVarChar, 20).Value = CEP;
                myCommand.Parameters.Add("@telefone", SqlDbType.NVarChar, 20).Value = Telefone;
                myCommand.Parameters.Add("@celular", SqlDbType.NVarChar, 20).Value = Celular;
                myCommand.Parameters.Add("@email", SqlDbType.NVarChar, 120).Value = Email;
                myCommand.Parameters.Add("@deletado", SqlDbType.Bit).Value = Deletado;
                myCommand.Parameters.Add("@pacienteID", SqlDbType.Int).Value = PacienteID;
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

        public Boolean ExcluiPaciente(int PacienteID)
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
                myCommand.CommandText = "UPDATE tb_paciente SET deletado = 1 WHERE id = @pacienteID";
                myCommand.Parameters.Add("@pacienteID", SqlDbType.Int).Value = PacienteID;
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

        public DataTable GetPaciente(int PacienteID, Boolean Deletado)
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
                myCommand.CommandText = "SELECT * FROM tb_paciente WHERE id = @pacienteID AND deletado = @deletado";
                myCommand.Parameters.Add("@pacienteID", SqlDbType.Int).Value = PacienteID;
                myCommand.Parameters.Add("@deletado", SqlDbType.Bit).Value = Deletado;
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

        public DataTable ListarPacientes()
        {
            dt = new DataTable();
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT id, nome FROM tb_paciente WHERE deletado = 0 ORDER BY nome";
                myCommand.Transaction = myTransaction;

                myDataAdapter = new SqlDataAdapter(myCommand);
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

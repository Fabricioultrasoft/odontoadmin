using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class UsuarioDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private Boolean retvalor = false;
        private DataTable dt = null;
        private SqlDataAdapter myDataAdapter = null;
        private int UserID = 0;
        private SQLHelper helper = null;

        public Boolean CriarNovoUsuario(String Nome,
                                        String Usuario,
                                        String Senha)
        {
            retvalor = false;
            UserID = 0;
            helper = new SQLHelper();
            UserID = helper.ProximoID("tb_usuario");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_usuario " +
                    "(id, nome, usuario, senha, deleted) VALUES " +
                    "(@id, @nome, @usuario, @senha, @deleted)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = UserID;
                myCommand.Parameters.Add("@nome", SqlDbType.NVarChar, 120).Value = Nome.Trim();
                myCommand.Parameters.Add("@usuario", SqlDbType.NVarChar, 15).Value = Usuario.Trim();
                myCommand.Parameters.Add("@senha", SqlDbType.NVarChar, 15).Value = Senha.Trim();
                myCommand.Parameters.Add("@deleted", SqlDbType.Bit).Value = false;

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

        public Boolean VerifyUser(String User, String Password)
        {
            retvalor = false;
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT TOP 1 * FROM tb_usuario " +
                                        "WHERE usuario = @User AND senha = @Password " +
                                        "AND deleted = 0 ";
                myCommand.Parameters.Add("@User", SqlDbType.NVarChar, 15).Value = User;
                myCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 15).Value = Password;

                SqlDataReader dr = myCommand.ExecuteReader();

                if (dr.Read())
                {
                    retvalor = true;
                    dr.Close();
                }
                myCommand.Parameters.Clear();
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

        public Boolean AtualizarUsuario(int ID, string Nome, string Usuario, String Senha, bool Deleted)
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
                myCommand.CommandText = "UPDATE tb_usuario " +
                                        "SET nome = @nome, usuario = @usuario, senha = @senha, deleted = @deleted " +
                                        "WHERE id = @id";
                myCommand.Parameters.Add("@nome", SqlDbType.NVarChar, 120).Value = Nome.Trim();
                myCommand.Parameters.Add("@usuario", SqlDbType.NVarChar, 15).Value = Usuario.Trim();
                myCommand.Parameters.Add("@senha", SqlDbType.NVarChar, 15).Value = Senha.Trim();
                myCommand.Parameters.Add("@deleted", SqlDbType.Bit).Value = Deleted;
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

        public Boolean ExcluirUsuario(int ID)
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
                myCommand.CommandText = "UPDATE tb_usuario SET deleted = 1 WHERE id = @id";
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

        public DataTable GetUsuario(int ID, bool Deletado)
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
                myCommand.CommandText = "SELECT * FROM tb_usuario WHERE id = @id AND deleted = @deletado";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ID;
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

        public DataTable GetUsuarioPorUserName(string UserName)
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
                myCommand.CommandText = "SELECT * FROM tb_usuario WHERE usuario = @usuario";
                myCommand.Parameters.Add("@usuario", SqlDbType.NVarChar,15).Value = UserName;
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

        public DataTable ListarUsuarios()
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
                myCommand.CommandText = "SELECT id, nome FROM tb_usuario WHERE deleted = 0 ORDER BY nome";
                myCommand.Transaction = myTransaction;

                myDataAdapter = new SqlDataAdapter(myCommand);
                myDataAdapter.Fill(dt);

                myTransaction.Commit();
            }
            catch (Exception ex)
            {
                myTransaction.Rollback();
                dt = null;
                //throw ex;
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

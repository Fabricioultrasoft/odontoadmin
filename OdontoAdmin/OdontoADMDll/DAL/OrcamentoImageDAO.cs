using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    public class OrcamentoImageDAO
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataAdapter myDataAdapter = null;
        private Boolean retvalor = false;
        private int ImageID = 0;
        private SQLHelper helper = null;

        public Boolean SalvarNovaImagem(int IdOrcamento, String ImageUrl)
        {
            retvalor = false;
            ImageID = 0;
            helper = new SQLHelper();
            ImageID = helper.ProximoID("tb_orcamento_imagem");
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO tb_orcamento_imagem " +
                                        "(id, imageurl) VALUES (@id, @imageurl)";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ImageID;
                myCommand.Parameters.Add("@imageurl", SqlDbType.NVarChar, 200).Value = ImageUrl.Trim();
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

        public bool AtualizarImagem(int idOrcamento, string imageUrl)
        { 
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "UPDATE tb_orcamento_imagem AS img " +
                                        "INNER JOIN tb_orcamento AS o ON o.idImagem = img.id " +
                                        "SET img.imageurl = @imageurl " +
                                        "WHERE o.id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = idOrcamento;
                myCommand.Parameters.Add("@imageurl", SqlDbType.NVarChar, 200).Value = imageUrl.Trim();
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

        public Boolean ExcluirImagem(int ImageID)
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
                myCommand.CommandText = "DELETE FROM tb_orcamento_imagem WHERE id = @id";
                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = ImageID;
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

        public String GetImagem(int OrcamentoID)
        {
            string imageUrl = "";
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myTransaction = myCon.BeginTransaction(IsolationLevel.ReadCommitted);
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT i.imageurl FROM tb_orcamento_imagem AS i " +
                                        "INNER JOIN tb_orcamento AS o ON o.idImagem = i.id " +     
                                        "WHERE o.id = @OrcamentoID";
                myCommand.Parameters.Add("@OrcamentoID", SqlDbType.Int).Value = OrcamentoID;
                myCommand.Transaction = myTransaction;

                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    imageUrl = reader["imageurl"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return imageUrl;
        }

        public int GetLastImageID()
        {
            string sql = "SELECT MAX(id) FROM tb_orcamento_imagem";
            int id = 0;
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                id = int.Parse(myCommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clearObjects();
            }

            return id;

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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADMDll.DAL
{
    class SQLHelper
    {
        private String myConString = ConfigurationManager.ConnectionStrings["Smilodon"].ConnectionString;
        private SqlConnection myCon = null;
        private SqlCommand myCommand = null;
        private object idvalor = null;

        public int ProximoID(string table)
        {
            idvalor = null;
            try
            {
                myCon = new SqlConnection(myConString);
                myCon.Open();
                myCommand = new SqlCommand();

                myCommand.Connection = myCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT ISNULL (MAX(id), 0) + 1 AS id FROM " + table;
                idvalor = myCommand.ExecuteScalar();

                if (idvalor == null || idvalor.Equals(System.DBNull.Value))
                {
                    idvalor = 1;
                }
            }
            catch (Exception ex)
            {
                idvalor = null;
                throw ex;
            }
            finally
            {
                myCommand.Dispose();
                if (myCon.State == ConnectionState.Open)
                {
                    myCon.Close();
                    myCon.Dispose();
                }
            }

            return Int32.Parse(idvalor.ToString());
        }
    }
}

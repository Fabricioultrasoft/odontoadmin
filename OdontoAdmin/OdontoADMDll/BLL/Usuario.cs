using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using OdontoADMDll.DAL;

namespace OdontoADMDll.BLL
{
    public class Usuario
    {
        private int _UserID;
        private string _Nome;
        private string _Usuario;
        private string _Senha;
        private Boolean _Deletado;

        private DAL.UsuarioDAO oUserDAO = null;
        private Boolean retvalor = false;
        private DataTable dt = null;
        private Usuario oUser = null;

        public int UserID { get { return _UserID; } set { _UserID = value; } }
        public string Nome { get { return _Nome; } set { _Nome = value; } }
        public string UserName { get { return _Usuario; } set { _Usuario = value; } }
        public string Senha { get { return _Senha; } set { _Senha = value; } }
        public Boolean Deletado { get { return _Deletado; } set { _Deletado = value; } }

        public Boolean VerificaLogin()
        {
            retvalor = false;
            try
            {
                oUserDAO = new UsuarioDAO();
                retvalor = oUserDAO.VerifyUser(UserName, Senha);
            }
            catch //(Exception ex)
            {
                // throw ex;  //comentado Marco (06-12-2011 22:56)
            }
            finally
            {
                oUserDAO = null;
            }
            return retvalor;
        }

        public Boolean CriarNovoUsuario()
        {
            retvalor = false;

            try
            {
                oUserDAO = new UsuarioDAO();
                retvalor = oUserDAO.CriarNovoUsuario(Nome, UserName, Senha);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            return retvalor;
        }

        public Boolean AtualizarUsuario()
        {
            retvalor = false;
            try
            {
                oUserDAO = new UsuarioDAO();
                retvalor = oUserDAO.AtualizarUsuario(UserID, Nome, UserName, Senha, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            return retvalor;
        }

        public Boolean EditarUsuario()
        {
            retvalor = false;

            try
            {
                oUserDAO = new UsuarioDAO();
                retvalor = oUserDAO.AtualizarUsuario(UserID, Nome, UserName, Senha, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            return retvalor;
        }

        public Boolean ExcluirUsuario()
        {
            retvalor = false;

            try
            {
                oUserDAO = new UsuarioDAO();
                retvalor = oUserDAO.ExcluirUsuario(UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            return retvalor;     
        }

        public Usuario GetUsuario(int id, Boolean Deletado)
        {
            oUser = new Usuario();

            try
            {
                oUserDAO = new UsuarioDAO();
                dt = oUserDAO.GetUsuario(id,Deletado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oUser.UserID = int.Parse(dt.Rows[0]["id"].ToString());
                oUser.Nome = dt.Rows[0]["nome"].ToString();
                oUser.UserName = dt.Rows[0]["usuario"].ToString();
                oUser.Senha = dt.Rows[0]["senha"].ToString();
                oUser.Deletado = Boolean.Parse(dt.Rows[0]["deleted"].ToString());
            }

            return oUser;
        }

        public Usuario GetUsuarioPorUserName(string UserName)
        {
            oUser = new Usuario();

            try
            {
                oUserDAO = new UsuarioDAO();
                dt = oUserDAO.GetUsuarioPorUserName(UserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            if (dt.Rows.Count > 0)
            {
                oUser.UserID = int.Parse(dt.Rows[0]["id"].ToString());
                oUser.Nome = dt.Rows[0]["nome"].ToString();
                oUser.UserName = dt.Rows[0]["usuario"].ToString();
                oUser.Senha = dt.Rows[0]["senha"].ToString();
                oUser.Deletado = Boolean.Parse(dt.Rows[0]["deleted"].ToString());
            }

            return oUser;
        }

        public DataTable ListarUsuarios()
        {
            dt = new DataTable();

            try
            {
                oUserDAO = new UsuarioDAO();
                dt = oUserDAO.ListarUsuarios();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                oUserDAO = null;
            }

            return dt;
        }
    }
}

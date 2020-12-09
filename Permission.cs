using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AvencaLib
{
    public static class AvencaPermission
    {
        public static AvencaFuncionario Usuario;

        public static int RequestLogin(Form frmCaller)
        {
            return RequestLogin(frmCaller, false);
        }
        public static int RequestLogin(Form frmCaller, bool isLogoff = false)
        {
            int userId = 0;

            while (userId <= 0)
            {
                try
                {
                    using (frmLogin fLogin = new frmLogin())
                    {
                        if (fLogin.ShowDialog(frmCaller, isLogoff) == DialogResult.OK)
                        {
                            Usuario = fLogin.User;
                            userId = ValidateUser(Usuario);
                            if (userId > 0)
                                if (!HasPermission(frmCaller))
                                    userId = 0;
                        }
                        else
                            break;
                    }
                }
                catch (Exception ex)
                {
                    AvencaErrorHandler.eventLogError(ex);
                }

                if (userId == 0)
                    MessageBox.Show(frmCaller, "Permissão Negada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return userId;
        }

        public static void Logoff()
        {
            Usuario.Clear();
        }

        public static bool HasPermission(Type callerType, bool showMessage = false)
        {
            return HasPermission(AvencaPermission.Usuario.Username, callerType.Name, showMessage);
        }

        public static bool HasPermission(Control callerObject, bool showMessage = false)
        {
            bool permissionGranted = false;

            try
            {
                if (((string)callerObject.Tag == "granted") || ((callerObject.Tag != null) && (callerObject.Tag.ToString().Contains(Usuario.IdPermissionGroup.ToString()))))
                    permissionGranted = true;
                else
                {
                    string objName = (callerObject.Parent != null) ? string.Format("{0}.{1}", callerObject.Parent.Name, callerObject.Name) : callerObject.Name;
                    permissionGranted = (AvencaPermission.Usuario != null) && (HasPermission(AvencaPermission.Usuario.Username, objName, showMessage));
                }

                if (permissionGranted)
                {
                    callerObject.Tag = "granted";

                    for (int i = 0; i < callerObject.Controls.Count; i++)
                        HasPermission(callerObject.Controls[i]);
                }
                else if ((callerObject.Tag != null) && ((string)callerObject.Tag != ""))
                    callerObject.Enabled = false;
            }
            catch(Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
            return permissionGranted;
        }

        public static bool HasPermission(string pUsername, string objectName, bool showMessage = false)
        {
            bool granted = false;

            using (SqlConnection SqlConn = new SqlConnection(AvencaDB.ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand sqlcomm = new System.Data.SqlClient.SqlCommand("GET_PERMISSION", SqlConn))
                {
                    try
                    {
                        SqlConn.Open();
                        sqlcomm.CommandType = CommandType.StoredProcedure;

                        sqlcomm.Parameters.AddWithValue("@Username", pUsername);
                        sqlcomm.Parameters.AddWithValue("@ObjectName", string.Format("{0}.{1}", Application.ProductName, objectName));

                        SqlParameter retval = new SqlParameter("@result", SqlDbType.Bit, 1);
                        retval.Direction = ParameterDirection.Output;
                        sqlcomm.Parameters.Add(retval);

                        sqlcomm.ExecuteNonQuery();
                        SqlConn.Close();

                        granted = (bool)retval.Value;

                        if (!granted && showMessage)
                            MessageBox.Show(null, "Permissão Negada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        AvencaErrorHandler.eventLogError(ex);
                    }
                }
            }

            return granted;
        }

        public static int ValidateUser(AvencaFuncionario funcionario)
        {
            using (SqlConnection SqlConn = new SqlConnection(AvencaDB.ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand sqlcomm = new System.Data.SqlClient.SqlCommand("VALIDATE_USER", SqlConn))
                {
                    try
                    {
                        SqlConn.Open();
                        sqlcomm.CommandType = CommandType.StoredProcedure;

                        sqlcomm.Parameters.AddWithValue("@Username", funcionario.Username);
                        sqlcomm.Parameters.AddWithValue("@Password", funcionario.Password);

                        SqlParameter retval = new SqlParameter("@result", SqlDbType.Int);
                        retval.Direction = ParameterDirection.Output;
                        sqlcomm.Parameters.Add(retval);

                        sqlcomm.ExecuteNonQuery();
                        SqlConn.Close();

                        return (int) retval.Value;
                    }
                    catch (Exception ex)
                    {
                        AvencaErrorHandler.eventLogError(ex);
                    }
                }
            }

            return 0;
        }

        public static string HashPassword(string inputString)
        {
            string hash = "";

            try
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(inputString);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                hash = System.Text.Encoding.ASCII.GetString(data);
            }
            catch(Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
            return hash;
        }
    }
}
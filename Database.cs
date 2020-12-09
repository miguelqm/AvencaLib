using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AvencaLib
{
    public static class AvencaDB
    {
        private static string connectionString;
        private static string connStr = String.Format("{0}.{1}.{2}", Application.ProductName, "Properties.Settings", "AvencaLibConnectionString");

        public static string ConnectionString
        {
            get 
            {
                if (connectionString == null)
                {
                    connectionString =  ConfigurationManager.ConnectionStrings[connStr].ConnectionString.ToString().Replace("***", "migSql@#");
                }

                return connectionString;
            }
        }

        public static void ReplaceConnectionString()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings[connStr].ConnectionString = ConnectionString;
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static void EncryptConnStr()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.ConnectionStrings.ConnectionStrings[connStr].ConnectionString = ConnectionString;
            //config.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("connectionStrings");

            ConfigurationSection configSection;

            configSection = config.GetSection("connectionStrings");

            if (configSection != null)
            {
                if (!(configSection.SectionInformation.IsLocked))
                {
                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save();
                }
            }
        }

        public static void DecryptConnStr()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection configSection;

            configSection = config.GetSection("connectionStrings");

            if (configSection != null)
            {
                if (!(configSection.SectionInformation.IsLocked))
                {
                    configSection.SectionInformation.UnprotectSection(); 
                    config.Save();
                }
            }
        }

        public static bool FuncionarioAdd(AvencaFuncionario newUser)
        {
            var res = false;

            using (var connection = new SqlConnection(AvencaDB.ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        @"INSERT INTO [dbo].[Funcionario] 
                                (Nome,CPF,Telefone,Email,Endereco,DataNascimento,HorarioEntrada,
                                    HorarioSaida,Sexo,Setor,Username,Password,IdPermissionGroup)
                          VALUES (@Nome,@CPF,@Telefone,@Email,@Endereco,@DataNascimento,@HorarioEntrada,
                                     @HorarioSaida,@Sexo,@Setor,@Username,@Password,@IdPermissionGroup)";

                    command.Parameters.AddWithValue("@Nome", newUser.Nome);
                    command.Parameters.AddWithValue("@CPF", newUser.CPF);
                    command.Parameters.AddWithValue("@Telefone", newUser.Telefone);
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@Endereco", newUser.Endereco);
                    command.Parameters.AddWithValue("@DataNascimento", newUser.DataNascimento);
                    command.Parameters.AddWithValue("@HorarioEntrada", newUser.HorarioEntrada);
                    command.Parameters.AddWithValue("@HorarioSaida", newUser.HorarioSaida);
                    command.Parameters.AddWithValue("@Sexo", newUser.Sexo);
                    command.Parameters.AddWithValue("@Setor", newUser.Setor);
                    command.Parameters.AddWithValue("@Username", newUser.Username);
                    command.Parameters.AddWithValue("@Password", newUser.Password);
                    command.Parameters.AddWithValue("@IdPermissionGroup", newUser.IdPermissionGroup);

                    try
                    {
                        connection.Open();
                        var recordsAffected = command.ExecuteNonQuery();
                        res = recordsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        AvencaErrorHandler.eventLogError(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return res;
        }

        public static AvencaFuncionario FuncionarioGet(string pUsername, string pPassword)
        {
            AvencaFuncionario funcionario = null;

            using (var connection = new SqlConnection(AvencaDB.ConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("SELECT * FROM FUNCIONARIO WHERE Username = @Username AND Password = @Password");

                        command.Parameters.AddWithValue("@Username", pUsername);
                        command.Parameters.AddWithValue("@Password", AvencaPermission.HashPassword(pPassword));

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            funcionario = new AvencaFuncionario();
                            funcionario.Username = pUsername;
                            funcionario.Password = pPassword;
                            funcionario.Id = (int)reader["Id"];
                            funcionario.Nome = reader["Nome"].ToString();
                            funcionario.CPF = reader["CPF"].ToString();
                            funcionario.DataNascimentoStr = reader["DataNascimento"].ToString();
                            funcionario.Email = reader["Email"].ToString();
                            funcionario.Endereco = reader["Endereco"].ToString();
                            funcionario.HorarioEntradaStr = reader["HorarioEntrada"].ToString();
                            funcionario.HorarioSaidaStr = reader["HorarioSaida"].ToString();
                            funcionario.Setor = reader["Setor"].ToString();
                            funcionario.Sexo = reader["Sexo"].ToString();
                            funcionario.Telefone = reader["Telefone"].ToString();
                            funcionario.IdPermissionGroup = (int)reader["IdPermissionGroup"];
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        AvencaErrorHandler.eventLogError(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return funcionario;
        }
    }
}
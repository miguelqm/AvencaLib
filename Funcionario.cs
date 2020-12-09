using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace AvencaLib
{
    public class AvencaFuncionario
    {
        private int id;
        private int idPermissionGroup;
        private string nome;
        private string cpf;
        private string telefone;
        private string email;
        private string endereco;
        private DateTime dataNasc;
        private DateTime horarioEntrada;
        private DateTime horarioSaida;
        private string sexo;
        private string setor;
        private string username;
        private string password;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int IdPermissionGroup
        {
            get { return idPermissionGroup; }
            set { idPermissionGroup = value; }
        }
        public string Nome
        {
            get { return nome == null ? "" : nome; }
            set { nome = value; }
        }
        public string CPF
        {
            get { return cpf == null ? "" : cpf; }
            set { cpf = value; }
        }        
        public string Telefone
        {
            get { return telefone == null ? "" : telefone; }
            set { telefone = value; }
        }        
        public string Email
        {
            get { return email == null ? "" : email; }
            set { email = value; }
        }        
        public string Endereco
        {
            get { return endereco == null ? "" : endereco; }
            set { endereco = value; }
        }
        public DateTime DataNascimento
        {
            get { return dataNasc; }
            set { dataNasc = value; }
        }
        public DateTime HorarioEntrada
        {
            get { return horarioEntrada; }
            set { horarioEntrada = value; }
        }
        public DateTime HorarioSaida
        {
            get { return horarioSaida; }
            set { horarioSaida = value; }
        }
        public string DataNascimentoStr
        {
            get { return dataNasc == null ? "" : dataNasc.ToString(); }
            set { dataNasc = DateTime.Parse(value); }
        }
        public string HorarioEntradaStr
        {
            get { return horarioEntrada == null ? "" : horarioEntrada.ToString(); }
            set { horarioEntrada = DateTime.Parse(value); }
        }
        public string HorarioSaidaStr
        {
            get { return horarioSaida == null ? "" : horarioSaida.ToString(); }
            set { horarioSaida = DateTime.Parse(value); }
        }        
        public string Sexo
        {
            get { return sexo == null ? "" : sexo; }
            set { sexo = value; }
        }        
        public string Setor
        {
            get { return setor == null ? "" : setor; }
            set { setor = value; }
        }        
        public string Username
        {
            get { return username == null ? "" : username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password == null ? "" : password; }
            set { password = AvencaPermission.HashPassword(value); }
        }

        public AvencaFuncionario()
        {
            idPermissionGroup = 1;
        }

        public AvencaFuncionario(string pUsername, string pPassword)
        {
            AvencaFuncionario newUser;
            try
            {
                Username = pUsername;
                Password = pPassword;

                if ((newUser = GetFromDB(pUsername, pPassword)) != null)
                {
                    this.Id = newUser.Id;
                    this.Nome = newUser.Nome;
                    this.CPF = newUser.CPF;
                    this.DataNascimento = newUser.DataNascimento;
                    this.Email = newUser.Email;
                    this.Endereco = newUser.Endereco;
                    this.HorarioEntrada = newUser.HorarioEntrada;
                    this.HorarioSaida = newUser.HorarioSaida;
                    this.Setor = newUser.Setor;
                    this.Sexo = newUser.Sexo;
                    this.Telefone = newUser.Telefone;
                    this.IdPermissionGroup = newUser.IdPermissionGroup;
                }
            }
            catch (Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
        }

        public void Clear()
        {
            this.Id = 0;
            this.IdPermissionGroup = 0;
            this.Nome = "";
            this.CPF = "";
            this.Telefone = "";
            this.Email = "";
            this.Endereco = "";
            this.DataNascimento = DateTime.MinValue;
            this.HorarioEntrada = DateTime.MinValue;
            this.HorarioSaida = DateTime.MinValue;
            this.Sexo = "";
            this.Setor = "";
            this.Username = "";
            this.Password = "";
        }

        public bool AddToDB()
        {
            return AvencaDB.FuncionarioAdd(this);
        }

        public static AvencaFuncionario GetFromDB(string pUsername, string pPassword)
        {
            AvencaFuncionario user = AvencaDB.FuncionarioGet(pUsername, pPassword);
            return user;
        }

        public static void NovoFuncionario()
        {
            new frmNovoFuncionario();
        }
    }
}
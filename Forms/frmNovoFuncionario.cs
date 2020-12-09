using System;
using System.Windows.Forms;

namespace AvencaLib
{
    public partial class frmNovoFuncionario : AvencaForm
    {
        public frmNovoFuncionario()
        {
            InitializeComponent();

            if (AvencaPermission.HasPermission(this, true))
                this.ShowDialog();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createFuncionario();
        }

        private AvencaFuncionario createFuncionario()
        {
            try
            {
                AvencaFuncionario newUser = new AvencaFuncionario(txtUsername.Text.ToLower(), txtPassword.Text);

                newUser.Nome = txtNome.Text.ToUpper();
                newUser.CPF = txtCPF.Text.ToUpper();
                newUser.DataNascimentoStr = dtpDataNascimento.Text.ToUpper();
                newUser.Email = txtEmail.Text.ToUpper();
                newUser.Endereco = txtEndereco.Text.ToUpper();
                newUser.HorarioEntradaStr = dtpHorarioEntrada.Text.ToUpper();
                newUser.HorarioSaidaStr = dtpHorarioSaida.Text.ToUpper();
                newUser.Setor = txtSetor.Text.ToUpper();
                newUser.Sexo = txtSexo.Text.ToUpper();
                newUser.Telefone = txtTelefone.Text.ToUpper();
                newUser.IdPermissionGroup = cbGrupo.SelectedIndex;

                if (newUser.AddToDB())
                    return newUser;
                else return null;
            }
            catch (Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
            return null;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            bool filled = true;

            for (int i = 0; i < this.Controls.Count; i++)
                if (((this.Controls[i] is TextBox) && (this.Controls[i].Text == "")) || ((this.Controls[i] is ComboBox) && (((ComboBox)this.Controls[i]).SelectedIndex < 0)))
                {
                    MessageBox.Show("Todos os campos devem estar preenchidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    filled = false;
                    break;
                }

            if (filled)
            {
                if (createFuncionario() != null)
                {
                    MessageBox.Show("Usuário criado com sucesso");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}
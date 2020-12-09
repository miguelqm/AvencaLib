using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvencaLib
{
    public partial class frmLogin : Form
    {
        public AvencaFuncionario User;
        private bool isLogoff;

        public frmLogin()
        {
            InitializeComponent();
            txtUsername.Text = Properties.Settings.Default.LastUsername;
        }

        public DialogResult ShowDialog(IWin32Window owner, bool loggedOff)
        {
            isLogoff = loggedOff;
                
            return base.ShowDialog(owner);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            User = new AvencaFuncionario(txtUsername.Text.ToLower(), txtPassword.Text);
            Properties.Settings.Default.LastUsername = txtUsername.Text.ToLower();
            Properties.Settings.Default.Save();
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            if((txtUsername.Text.Length > 0) && (!isLogoff))
                txtPassword.Focus();
        }
    }
}

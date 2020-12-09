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
    public partial class AvencaForm : Form
    {
        public AvencaForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (!this.DesignMode)
                if ((string)this.Tag != "granted")
                    AvencaPermission.HasPermission(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThiVeMyThuat
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmDsAnh f = new FrmDsAnh();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmDsPhongThi f = new FrmDsPhongThi();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmDsDinhChinh f = new FrmDsDinhChinh();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmDsThuLePhi f = new FrmDsThuLePhi();
            f.Show();
        }
    }
}

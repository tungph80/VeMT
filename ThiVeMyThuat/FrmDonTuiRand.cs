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
    public partial class FrmDonTuiRand : Form
    {
        private int index_of_lst_tui = 0;

        public FrmDonTuiRand()
        {
            InitializeComponent();
        }

        private void FrmDonTuiRand_Load(object sender, EventArgs e)
        {
            Utils.LoadFullScreen(this);

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            index_of_lst_tui += 1;
            if (index_of_lst_tui > Utils.lst_tui.Count()-1 )
            {
                index_of_lst_tui = Utils.lst_tui.Count()-1;
            }

            dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
            textBox1.Text = (index_of_lst_tui + 1).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index_of_lst_tui -= 1;
            if (index_of_lst_tui <0)
            {
                index_of_lst_tui = 0;
            }

            dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
            textBox1.Text = (index_of_lst_tui + 1).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int sobaithi_theotui = int.Parse(txt_sobaithi_theotui.Text);
                Utils.TronTuiBaiThi(sobaithi_theotui);
                dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
                textBox1.Text = (index_of_lst_tui+1).ToString();
                txt_tongtuibaithi.Text = Utils.sotuibaithi.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }


    }
}

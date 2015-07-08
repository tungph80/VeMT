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
            if (index_of_lst_tui > Utils.sotuibaithi -1 )
            {
                index_of_lst_tui = Utils.sotuibaithi -1;
            }

            //dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
            index_page.Value = index_of_lst_tui + 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index_of_lst_tui -= 1;
            if (index_of_lst_tui <0)
            {
                index_of_lst_tui = 0;
            }

            //dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
            index_page.Value = index_of_lst_tui + 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.sobaithi_theotui = int.Parse(txt_sobaithi_theotui.Text);                
                Utils.TronTuiBaiThi();
                index_page.Minimum = 1;
                index_page.Maximum = Utils.sotuibaithi;
                //dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
                index_page.Value = index_of_lst_tui + 1;
                txt_tongtuibaithi.Text = Utils.sotuibaithi.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }        

        private void txt_index_page_ValueChanged(object sender, EventArgs e)
        {
            if (Utils.sotuibaithi > 0 )
            {
                index_of_lst_tui = (int)index_page.Value - 1;
                if (index_of_lst_tui < 0)
                {
                    index_of_lst_tui = 0;
                }
                else
                {
                    if (index_of_lst_tui > Utils.sotuibaithi - 1)
                    {
                        index_of_lst_tui = Utils.sotuibaithi - 1;
                    }
                }

                dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
            }
            
        }

        private void btn_Update_DB_Click(object sender, EventArgs e)
        {
            
            if (Utils.sotuibaithi > 0)
            {
                Utils.Update_DB();
            }
            else
            {
                MessageBox.Show("Vui lòng thực hiện trộn túi bài thi trước!");
            }
        }

        


    }
}

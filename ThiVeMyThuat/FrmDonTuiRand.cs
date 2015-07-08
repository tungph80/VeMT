using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            if (index_of_lst_tui > Utils.sotuibaithi - 1)
            {
                index_of_lst_tui = Utils.sotuibaithi - 1;
            }

            //dataGridView1.DataSource = Utils.lst_tui[index_of_lst_tui].Lst_baithi_theotui;
            index_page.Value = index_of_lst_tui + 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index_of_lst_tui -= 1;
            if (index_of_lst_tui < 0)
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
                index_page.Value = index_page.Value + 1;
                index_page.Value = 1;

                txt_tongtuibaithi.Text = Utils.sotuibaithi.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void txt_index_page_ValueChanged(object sender, EventArgs e)
        {
            if (Utils.sotuibaithi > 0)
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
                dataGridView1.AutoResizeColumns();
            }

        }

        private void btn_Update_DB_Click(object sender, EventArgs e)
        {

            if (Utils.sotuibaithi > 0)
            {
                if (MessageBox.Show("Bạn có thực sự muốn thay đổi số phách hiện tại?", "Xác nhận!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    Utils.Update_DB();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng thực hiện trộn túi bài thi trước!");
            }
        }

        private void btn_backup_Click(object sender, EventArgs e)
        {
            try
            {

                string strPath = Application.StartupPath + @"\SQLBackup\";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string strname = "VeMT(" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ").bak";
                string strBackup = " BACKUP DATABASE [XDA] TO DISK = N'" + strPath + strname + "'";
                strBackup += " WITH COPY_ONLY, NOFORMAT, NOINIT, NAME = N'TenCSDL-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                //Connect to DB
                SqlConnection connect;
                string con = System.Configuration.ConfigurationManager.ConnectionStrings["ThiVeMyThuat.Properties.Settings.XDAConnectionString"].ConnectionString; // "Data Source=THAONHI-PC\\SQL2014;Initial Catalog=XDA;Integrated Security=True";
                connect = new SqlConnection(con);
                connect.Open();
                //----------------------------------------------------------------------------------------------------

                //Execute SQL---------------
                SqlCommand command;
                command = new SqlCommand(strBackup, connect);
                command.ExecuteNonQuery();
                //-------------------------------------------------------------------------------------------------------------------------------

                connect.Close();

                MessageBox.Show("Tạo bản sao dữ liệu thành công", "Back", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_restore_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string strPath = ofd.FileName;

                    if (File.Exists(strPath))
                    {
                        if (MessageBox.Show("Bạn có chắc chắn muốn khôi phục dữ liệu từ bản sao này không?", "Back", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //Connect SQL-----------
                            //Connect to DB
                            SqlConnection connect;
                            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ThiVeMyThuat.Properties.Settings.XDAConnectionString"].ConnectionString; // "Data Source=THAONHI-PC\\SQL2014;Initial Catalog=XDA;Integrated Security=True";Data Source=thaonhi-pc\sql2014;Initial Catalog=XDA;Integrated Security=True
                            connect = new SqlConnection(con);
                            connect.Open();
                            //-----------------------------------------------------------------------------------------

                            //Excute SQL----------------
                            SqlCommand command;
                            command = new SqlCommand("use master", connect);
                            command.ExecuteNonQuery();
                            command = new SqlCommand(@"restore database XDA from disk = N'" + strPath + "'", connect);
                            command.ExecuteNonQuery();
                            //--------------------------------------------------------------------------------------------------------
                            connect.Close();

                            MessageBox.Show("Khôi phục dữ liệu thành công", "Restoration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                        MessageBox.Show(@"Không tồn tại file vừa chọn.", "Restoration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




    }
}

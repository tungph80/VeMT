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
    public partial class FrmDonTuiDanhPhach : Form
    {
        public FrmDonTuiDanhPhach()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //sap xep theo phong thi
                dbVeMTDataContext db = new dbVeMTDataContext();
                var list = db.vemts.OrderBy(a => a.phongthi);
                //danh stt
                int n = list.ToList().Count;
                for (int i = 0; i < n; i++)
                {
                    list.ToList()[i].stt = i + 1;
                }
                
                //don tui bai thi
                for (int i = 0; i < n; i++)
                {
                    decimal? stt = list.ToList()[i].stt;
                    list.ToList()[i].tui = Convert.ToDouble((int)((stt - 1) / numericUpDown1.Value)) + 1;
                }
                //var x = list.GroupBy(p => p.tui);
                //foreach (vemt item in x)
                //{
                //    item.phach = item.tui * 10;
                //}

                //danh phach theo tung tui, cac phach trong mot tui la lien tuc
                //phac giua cac tui cach nhau la 1 so nguyen tu 1 den 10

                db.SubmitChanges();
                MessageBox.Show("Đã thực hiện xong");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

﻿using System;
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
    public partial class FrmDsThuLePhi : Form
    {
        public FrmDsThuLePhi()
        {
            InitializeComponent();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            Utils.LoadFullScreen(this);

            DsVeMTTableAdapters.vemtTableAdapter da = new DsVeMTTableAdapters.vemtTableAdapter();
            da.Fill(dsVeMT1.vemt);
            inlineReportSlot1.RenderDocument();
        }

        private void FrmDsThuLePhi_Load(object sender, EventArgs e)
        {

        }
    }
}

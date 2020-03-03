using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Cheese_Factory
{
    public partial class ReportForm_SuppliesTransaction : Form
    {
        //CrystalReport1 crSource = new CrystalReport1();
        CrystalReport_SuppliesTransaction crSource = new CrystalReport_SuppliesTransaction();
        public ReportForm_SuppliesTransaction()
        {
            InitializeComponent();
            //crSource.SetDatabaseLogon("DOMUS\\Wendy","");
            crystalReportViewer1.ReportSource = crSource;
            crystalReportViewer1.Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cheese_Factory
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(_MainForm.getInstance());
            //Application.Run(new FC_SalesInvoice ());
            //Application.Run(new HR_ManageEmployee ());
            //Application.Run(new IS_FinalGoodsReceipt ());
            //Application.Run(new IS_SuppliesInvoice ());
            //Application.Run(new IS_ViewPurchaseOrder ());
            //Application.Run(new OP_OutgoingReport ());
            //Application.Run(new OP_ViewPackingSchedule ());
            //Application.Run(new QM_CheckIncomingSupplies ());
            //Application.Run(new QM_CheckOutgoingProduct ());
            //Application.Run(new QM_DefectiveProduct ());
            //Application.Run(new SD_ManageCustomer ());
            //Application.Run(new SD_SalesTransaction ());
            //Application.Run(new SP_Cheese ());
            //Application.Run(new SP_Milk ());
            //Application.Run(new SP_SuppliesTransaction ());
            //Application.Run(new SP_Vendor());

            //Application.Run(new SP_VerifyInvoice());
            //Application.Run(new FC_SuppliesInvoice());
        }
    }
}

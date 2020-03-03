using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cheese_Factory
{
    public partial class _MainForm : Form
    {
        private static _MainForm instance;

        public static _MainForm getInstance()
        {
            if (instance == null)
            {
                instance = new _MainForm();
            }
            return instance;
        }

        public static string userID = "";
        public static string userName = "";
        private _LoginForm loginForm;
        private FC_SalesInvoice salesInvoice;
        private HR_ManageEmployee employee;
        private IS_FinalGoodsReceipt finalGoodsReceipt;
        private IS_SuppliesInvoice suppliesInvoice;
        private IS_ViewPurchaseOrder viewPurchaseOrder;
        private OP_OutgoingReport outgoingReport;
        private OP_ViewPackingSchedule viewPackingSchedule;
        private QM_CheckIncomingSupplies checkIncomingSupplies;
        private QM_CheckOutgoingProduct checkOutgoingProduct;
        private QM_DefectiveProduct checkDefectiveProduct;
        private SD_ManageCustomer customer;
        private SD_SalesTransaction salesTransaction;
        private SP_Cheese cheese;
        private SP_Milk milk;
        private SP_SuppliesTransaction suppiesTransaction;
        private SP_Vendor vendor;
        private SP_VerifyInvoice verifyInvoice;
        private FC_SuppliesInvoice suppliesInvoiceFC;
        private ReportForm_SuppliesTransaction reportSuppliesTransaction;

        public _MainForm()
        {
            InitializeComponent();
            logoutStatus(true);
            showDivision(false);
        }

        public void logoutStatus(bool status)
        {
            loginToolStripMenuItem.Visible = status;
            logoutToolStripMenuItem.Visible = !status;
            exitToolStripMenuItem.Visible = status;
        }

        public void login(string division, string employee)
        {
            if (division == "Incoming Supplies")
            {
                warehouseIncomingSuppliesISDivisionToolStripMenuItem.Visible = true;
            }
            else if (division == "Outgoing Product")
            {
                warehouseOutgoingProductsOPDivisionToolStripMenuItem.Visible = true;
            }
            else if (division == "Quality Management")
            {
                warehouseQualityManagementQMDivisionToolStripMenuItem.Visible = true;
            }
            else if (division == "Sales and Distribution")
            {
                salesAndDistributionSDDivisionToolStripMenuItem.Visible = true;
            }
            else if (division == "Supplies")
            {
                suppliesDivisionToolStripMenuItem.Visible = true;
            }
            else if (division == "Finance")
            {
                financeDivisionToolStripMenuItem.Visible = true;
            }
            else if (division == "Human Resource")
            {
                humanResourcesToolStripMenuItem.Visible = true;
            }
            toolStripStatusLabel1.Text = "Welcome, " + employee;
            logoutStatus(false);
        }

        public void showDivision(bool status)
        {
            warehouseIncomingSuppliesISDivisionToolStripMenuItem.Visible = status;
            warehouseOutgoingProductsOPDivisionToolStripMenuItem.Visible = status;
            warehouseQualityManagementQMDivisionToolStripMenuItem.Visible = status;
            salesAndDistributionSDDivisionToolStripMenuItem.Visible = status;
            suppliesDivisionToolStripMenuItem.Visible = status;
            financeDivisionToolStripMenuItem.Visible = status;
            humanResourcesToolStripMenuItem.Visible = status;
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loginForm == null || loginForm.IsDisposed)
            {
                loginForm = new _LoginForm();
                loginForm.MdiParent = this;
            }
            loginForm.Visible = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logoutStatus(true);
            showDivision(false);
            if (employee != null && !employee.IsDisposed)
                employee.Dispose();
                toolStripStatusLabel1.Text = "Welcome, Guest";
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (employee == null || employee.IsDisposed)
            {
                employee = new HR_ManageEmployee();
                employee.MdiParent = this;
            }
            employee.Visible = true;
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customer == null || customer.IsDisposed)
            {
                customer = new SD_ManageCustomer();
                customer.MdiParent = this;
            }
            customer.Visible = true;
        }

        private void cheeseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cheese == null || cheese.IsDisposed)
            {
                cheese = new SP_Cheese();
                cheese.MdiParent = this;
            }
            cheese.Visible = true;
        }

        private void milkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (milk == null || milk.IsDisposed)
            {
                milk = new SP_Milk();
                milk.MdiParent = this;
            }
            milk.Visible = true;
        }

        private void vendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vendor == null || vendor.IsDisposed)
            {
                vendor = new SP_Vendor();
                vendor.MdiParent = this;
            }
            vendor.Visible = true;
        }

        private void suplliesTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (suppiesTransaction == null || suppiesTransaction.IsDisposed)
            {
                suppiesTransaction = new SP_SuppliesTransaction();
                suppiesTransaction.MdiParent = this;
            }
            suppiesTransaction.Visible = true;
        }

        private void salesTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (salesTransaction == null || salesTransaction.IsDisposed)
            {
                salesTransaction = new SD_SalesTransaction();
                salesTransaction.MdiParent = this;
            }
            salesTransaction.Visible = true;
        }

        private void viewPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewPurchaseOrder == null || viewPurchaseOrder.IsDisposed)
            {
                viewPurchaseOrder = new IS_ViewPurchaseOrder();
                viewPurchaseOrder.MdiParent = this;
            }
            viewPurchaseOrder.Visible = true;
        }

        private void createFinalGoodsReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (finalGoodsReceipt == null || finalGoodsReceipt.IsDisposed)
            {
                finalGoodsReceipt = new IS_FinalGoodsReceipt();
                finalGoodsReceipt.MdiParent = this;
            }
            finalGoodsReceipt.Visible = true;
        }

        private void suppliesInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (suppliesInvoice == null || suppliesInvoice.IsDisposed)
            {
                suppliesInvoice = new IS_SuppliesInvoice();
                suppliesInvoice.MdiParent = this;
            }
            suppliesInvoice.Visible = true;
        }

        private void viewPackingScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewPackingSchedule == null || viewPackingSchedule.IsDisposed)
            {
                viewPackingSchedule = new OP_ViewPackingSchedule();
                viewPackingSchedule.MdiParent = this;
            }
            viewPackingSchedule.Visible = true;
        }

        private void createOutgoingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (outgoingReport == null || outgoingReport.IsDisposed)
            {
                outgoingReport = new OP_OutgoingReport();
                outgoingReport.MdiParent = this;
            }
            outgoingReport.Visible = true;
        }

        private void checkIncomingSuppliesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkIncomingSupplies == null || checkIncomingSupplies.IsDisposed)
            {
                checkIncomingSupplies = new QM_CheckIncomingSupplies();
                checkIncomingSupplies.MdiParent = this;
            }
            checkIncomingSupplies.Visible = true;
        }

        private void checkOutgoingProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkOutgoingProduct == null || checkOutgoingProduct.IsDisposed)
            {
                checkOutgoingProduct = new QM_CheckOutgoingProduct();
                checkOutgoingProduct.MdiParent = this;
            }
            checkOutgoingProduct.Visible = true;
        }

        private void defectiveProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkDefectiveProduct == null || checkDefectiveProduct.IsDisposed)
            {
                checkDefectiveProduct = new QM_DefectiveProduct();
                checkDefectiveProduct.MdiParent = this;
            }
            checkDefectiveProduct.Visible = true;
        }

        private void salesInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (salesInvoice == null || salesInvoice.IsDisposed)
            {
                salesInvoice = new FC_SalesInvoice();
                salesInvoice.MdiParent = this;
            }
            salesInvoice.Visible = true;
        }

        private void suppliesInvoiceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (suppliesInvoiceFC == null || suppliesInvoiceFC.IsDisposed)
            {
                suppliesInvoiceFC = new FC_SuppliesInvoice();
                suppliesInvoiceFC.MdiParent = this;
            }
            suppliesInvoiceFC.Visible = true;
        }

        private void suppliesVerifyInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (verifyInvoice == null || verifyInvoice.IsDisposed)
            {
                verifyInvoice = new SP_VerifyInvoice();
                verifyInvoice.MdiParent = this;
            }
            verifyInvoice.Visible = true;
        }

        private void createMonthlyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reportSuppliesTransaction == null || reportSuppliesTransaction.IsDisposed)
            {
                reportSuppliesTransaction = new ReportForm_SuppliesTransaction();
                reportSuppliesTransaction.MdiParent = this;
            }
            reportSuppliesTransaction.Visible = true;
        }


    }
}

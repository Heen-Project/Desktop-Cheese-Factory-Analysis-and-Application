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
    public partial class FC_SalesInvoice : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public FC_SalesInvoice()
        {
            InitializeComponent();
            loadData();
            clear();
            btnDisabled();
        }
        public void loadData()
        {
            var outgoingReport = from x in cheese.OutgoingReports
                                 where (x.DeliveryStatus.Equals("Delivered"))
                                 orderby x.CheckStatus descending
                                 select new { x.OutgoingReportID, x.SalesTransactionID, x.ScheduleID, x.CheckStatus };
            dataGridView1.DataSource = outgoingReport.ToList();
            if (outgoingReport != null)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }
        }

        public void loadDetail()
        {
            var detailSalesTransaction = from x in cheese.DetailSalesTransactions
                                         where (x.SalesTransactionID.Equals(textBox2.Text))
                                         select new { x.CheeseID, x.ItemPrice, x.Quantity };
            dataGridView3.DataSource = detailSalesTransaction.ToList();

            Schedule s = (from x in cheese.Schedules where (x.ScheduleID.Equals(textBox3.Text)) select x).First();
            OutgoingReport oR = (from x in cheese.OutgoingReports where x.OutgoingReportID.Equals(textBox1.Text) select x).First();

            if (radioButton1.Checked == true)
            {
                if (dataGridView3.Rows.Count > 0)
                {
                    textBox6.Text = s.BatchNumberSchedule + " From " + dataGridView2.Rows[0].Cells[3].Value.ToString();
                }
                int temp = 0;
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    temp += Int32.Parse(row.Cells[1].Value.ToString());
                }
                textBox7.Text = temp.ToString();
                textBox8.Text = dataGridView2.Rows[0].Cells[5].Value.ToString();
                int tempTotal = Int32.Parse(textBox7.Text) + Int32.Parse(textBox8.Text);
                textBox9.Text = tempTotal.ToString();
                label9.Text = "Batches Payment";
                if (oR.CheckStatus == "Checked")
                {
                    textBox10.Text = oR.OutgoingReportDescription;
                    btnEnabled(true);
                    SalesTransaction sT = (from x in cheese.SalesTransactions where (x.SalesTransactionID.Equals(textBox2.Text)) select x).First();
                    if (sT.SalesTransactionStatus != "Ongoing")
                    {
                        //btnDisabled();
                    }
                }
                else
                {
                    btnEnabled(false);
                }
            }
            else if (radioButton2.Checked == true)
            {
                textBox6.Text = dataGridView2.Rows[0].Cells[3].Value.ToString() + " Batch";
                if (s.ScheduleID.ToString().Equals(textBox6.Text))
                {
                    textBox7.Text = dataGridView3.Rows[0].Cells[1].Value.ToString();
                    int temp = (int)(Int32.Parse(dataGridView2.Rows[0].Cells[5].Value.ToString()) / Int32.Parse(dataGridView2.Rows[0].Cells[3].Value.ToString()));
                    textBox8.Text = temp.ToString();
                    int tempTotal = Int32.Parse(textBox7.Text) + Int32.Parse(textBox8.Text);
                    label9.Text = "Total Payment";
                    if (oR.CheckStatus == "Checked")
                    {
                        textBox10.Text = oR.OutgoingReportDescription;
                        btnEnabled(true);
                        SalesTransaction sT = (from x in cheese.SalesTransactions where (x.SalesTransactionID.Equals(textBox2.Text)) select x).First();
                        if (sT.SalesTransactionStatus != "Ongoing")
                        {
                            //btnDisabled();
                        }
                    }
                    else
                    {
                        btnEnabled(false);
                    }
                }
                else
                {
                    btnDisabled();
                    textBox9.Text = "Can't Create Invoice";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    MessageBox.Show("Can't Created Invoice Yet, Batches " + s.BatchNumberSchedule + " From " + dataGridView2.Rows[0].Cells[3].Value.ToString());
                }
            }

        }

        public void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            label11.Visible = false;
            radioButton1.Checked = true;
        }

        public void btnDisabled()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        public void btnEnabled(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = !status;
            button1.Enabled = !status;
            button2.Enabled = status;
            radioButton1.Enabled = !status;
            radioButton2.Enabled = !status;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.SelectedRows != null)
            {
                clear();
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                var salesTransaction = from x in cheese.SalesTransactions
                                       where (x.SalesTransactionID.Equals(textBox2.Text))
                                       select new { x.SalesTransactionID, x.CustomerID, x.PaymentType, x.NumberOfBatch, x.Transportation, x.TransportPrice };
                dataGridView2.DataSource = salesTransaction.ToList();
                if (dataGridView2.Rows.Count > 0)
                {
                    textBox4.Text = dataGridView2.Rows[0].Cells[1].Value.ToString();
                    textBox5.Text = dataGridView2.Rows[0].Cells[2].Value.ToString();
                    loadDetail();
                }
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            label11.Visible = false;

            if (radioButton1.Checked == true && textBox1.Text != "")
            {
                Schedule s = (from x in cheese.Schedules where (x.ScheduleID.Equals(textBox3.Text)) select x).First();
                OutgoingReport oR = (from x in cheese.OutgoingReports where x.OutgoingReportID.Equals(textBox1.Text) select x).First();

                if (dataGridView3.Rows.Count > 0)
                {
                    textBox6.Text = s.BatchNumberSchedule + " From " + dataGridView2.Rows[0].Cells[3].Value.ToString();
                }
                int temp = 0;
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    temp += Int32.Parse(row.Cells[1].Value.ToString());
                }
                textBox7.Text = temp.ToString();
                textBox8.Text = dataGridView2.Rows[0].Cells[5].Value.ToString();
                int tempTotal = Int32.Parse(textBox7.Text) + Int32.Parse(textBox8.Text);
                textBox9.Text = tempTotal.ToString();
                label9.Text = "Batches Payment";
                if (oR.CheckStatus == "Checked")
                {
                    textBox10.Text = oR.OutgoingReportDescription;
                    btnEnabled(true);
                    SalesTransaction sT = (from x in cheese.SalesTransactions where (x.SalesTransactionID.Equals(textBox2.Text)) select x).First();
                    if (sT.SalesTransactionStatus != "Ongoing")
                    {
                        //btnDisabled();
                    }
                }
                else
                {
                    btnEnabled(false);
                }
            }
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            label11.Visible = false;

            if (radioButton2.Checked == true && textBox1.Text != "")
            {
                Schedule s = (from x in cheese.Schedules where (x.ScheduleID.Equals(textBox3.Text)) select x).First();
                OutgoingReport oR = (from x in cheese.OutgoingReports where x.OutgoingReportID.Equals(textBox1.Text) select x).First();

                textBox6.Text = dataGridView2.Rows[0].Cells[3].Value.ToString() + " Batch";
                textBox6.Text = dataGridView2.Rows[0].Cells[3].Value.ToString() + " Batch";
                if (s.ScheduleID.ToString().Equals(textBox6.Text))
                {
                    textBox7.Text = dataGridView3.Rows[0].Cells[1].Value.ToString();
                    int temp = (int)(Int32.Parse(dataGridView2.Rows[0].Cells[5].Value.ToString()) / Int32.Parse(dataGridView2.Rows[0].Cells[3].Value.ToString()));
                    textBox8.Text = temp.ToString();
                    int tempTotal = Int32.Parse(textBox7.Text) + Int32.Parse(textBox8.Text);
                    label9.Text = "Total Payment";
                    if (oR.CheckStatus == "Checked")
                    {
                        textBox10.Text = oR.OutgoingReportDescription;
                        btnEnabled(true);
                        SalesTransaction sT = (from x in cheese.SalesTransactions where (x.SalesTransactionID.Equals(textBox2.Text)) select x).First();
                        if (sT.SalesTransactionStatus != "Ongoing")
                        {
                            //btnDisabled();
                        }
                    }
                    else
                    {
                        btnEnabled(false);
                    }
                }
                else
                {
                    btnDisabled();
                    textBox9.Text = "Can't Create Invoice";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    MessageBox.Show("Can't Created Invoice Yet, Batches " + s.BatchNumberSchedule + " From " + dataGridView2.Rows[0].Cells[3].Value.ToString());

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox9.Text.Equals("Can't Create Invoice") == false && textBox10.Text != "")
            {
                DialogResult dr = MessageBox.Show("Sent " + textBox1.Text + " Invoice", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    OutgoingReport oR = (from x in cheese.OutgoingReports where x.OutgoingReportID.Equals(textBox1.Text) select x).First();

                    if (oR.CheckStatus != "Checked")
                    {
                        oR.CheckedBy = _MainForm.userID;
                        oR.CheckedDate = DateTime.Now;
                        oR.CheckStatus = "Checked";
                        oR.OutgoingReportDescription = textBox10.Text;
                        cheese.SaveChanges();
                        loadData();
                        dataGridView2.DataSource = null;
                        dataGridView3.DataSource = null;
                        clear();
                        btnDisabled();
                        MessageBox.Show("Invoice Sent");
                    }
                }
            }
            else if (textBox10.Text == "")
            {
                label11.Text = "Description is Empty";
                label11.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox9.Text != "Can't Create Invoice" && textBox10.Text != "")
            {
                DialogResult dr = MessageBox.Show("Confirm " + textBox1.Text + " Payment", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    OutgoingReport oR = (from x in cheese.OutgoingReports where x.OutgoingReportID.Equals(textBox1.Text) select x).First();

                    if (oR.CheckStatus == "Checked")
                    {
                        SalesTransaction sT = (from x in cheese.SalesTransactions where x.SalesTransactionID.Equals(textBox2.Text) select x).First();
                       
                            sT.ConfirmBy = _MainForm.userID;
                            sT.ConfirmDate = DateTime.Now;
                            sT.SalesTransactionStatus = "Paid for " + textBox6.Text;
                            cheese.SaveChanges();
                            loadData();
                            dataGridView2.DataSource = null;
                            dataGridView3.DataSource = null;
                            clear();
                            btnDisabled();
                            MessageBox.Show("Payment Confirm");
                        
                    }
                }
            }
        }

    }
}

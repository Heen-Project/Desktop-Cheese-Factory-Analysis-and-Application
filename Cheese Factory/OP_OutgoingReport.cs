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
    public partial class OP_OutgoingReport : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public OP_OutgoingReport()
        {
            InitializeComponent();
            loadData();
            clear();
            btnDisabled();
            timer1.Enabled = true;
        }

        public void loadData()
        {
            var schedule = from x in cheese.Schedules
                           where (x.ScheduleStatus.Equals("Processed to Delivery") || x.ScheduleStatus.Equals("Completed"))
                           orderby x.ScheduleStatus descending, x.ShippingDate ascending
                           select new { x.ScheduleID, x.SalesTransactionID, x.BatchNumberSchedule, x.ShippingDate, x.ScheduleStatus };
            dataGridView1.DataSource = schedule.ToList();
            if (schedule != null)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }
        }

        public void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        public void btnDisabled()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        public void btnEnabled(bool status)
        {
            button1.Enabled = status;
            button2.Enabled = !status;
        }

        public string generateOutgoingReportID()
        {
            string newID = "";
            var outgoingReport = from temp in cheese.OutgoingReports select temp;
            var countOfRows = outgoingReport.Count();

            if (countOfRows != 0)
            {
                var data = outgoingReport.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.OutgoingReportID;
                string prefix = curr.Substring(0, 3);
                string num = curr.Substring(3, 4);
                int id = Int32.Parse(num);
                id++;
                if (id < 10000)
                {
                    newID = prefix + "000" + id;

                }
                else if (id < 1000)
                {
                    newID = prefix + "00" + id;

                }
                else if (id < 100)
                {
                    newID = prefix + "0" + id;
                }
                else if (id < 10)
                {
                    newID = prefix + id;
                }
                return newID;
            }
            else
            {
                return "OGP0001";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime besok = DateTime.Today.AddDays(1);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                /*if (row.Cells[4].Value.ToString() == "")
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
                else*/ if (row.Cells[4].Value.ToString() != "Processed to Delivery")
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                else
                {
                    if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()), DateTime.Today) < 0)
                    {
                        if (row.Cells[4].Value.ToString() == "Processed to Delivery")
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                    else if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()), DateTime.Today) == 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.Pink;
                    }
                    else if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()), besok) == 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()), besok) > 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }
                }
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells[1].Value.ToString() == "Sent")
                {
                    row.DefaultCellStyle.BackColor = Color.YellowGreen;
                }
                else if (row.Cells[1].Value.ToString() == "Delivered")
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.SelectedRows != null)
            {
                clear();
                textBox1.Text = "";
                
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

                var outgoingReport = from x in cheese.OutgoingReports
                                     where (x.SalesTransactionID.Equals(textBox2.Text) && x.ScheduleID.Equals(textBox3.Text))
                                       select new { x.OutgoingReportID, x.DeliveryStatus, x.CheckStatus };
                dataGridView2.DataSource = outgoingReport.ToList();
                if (dataGridView2.Rows.Count >0) { 
                    textBox1.Text = dataGridView2.Rows[0].Cells[0].Value.ToString();
                    textBox4.Text = dataGridView2.Rows[0].Cells[1].Value.ToString();
                }
                if (textBox1.Text == "")
                {
                    textBox1.Text = generateOutgoingReportID();
                }
            }
            if (textBox3.Text != "")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "Processed to Delivery")
                {
                    btnEnabled(true);
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "Completed")
                {
                    if (dataGridView2.Rows[0].Cells[1].Value.ToString() != "Sent")
                    {
                        btnDisabled();
                    }
                    else { 
                        btnEnabled(false);
                    }
                }
            }
            else
            {
                btnDisabled();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Create " + textBox1.Text + " Outgoing Report", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    Schedule s = (from x in cheese.Schedules where (x.ScheduleID.Equals(textBox3.Text) && x.SalesTransactionID.Equals(textBox2.Text)) where x.SalesTransactionID.Equals(textBox2.Text) select x).First();
                    if (s.ScheduleStatus == "Processed to Delivery")
                    {
                        s.ScheduleStatus = "Completed";
                        OutgoingReport oR = new OutgoingReport();
                        oR.OutgoingReportID = textBox1.Text;
                        oR.SalesTransactionID = textBox2.Text;
                        oR.ScheduleID = textBox3.Text;
                        oR.ShippedBy = _MainForm.userID;
                        //oR.CheckedBy = "";
                        //oR.CheckedDate = null;
                        //oR.CheckStatus = "";
                        //oR.PackedBy = "";
                        //oR.OutgoingReportDescription = "";
                        oR.DeliveryStatus = "Sent";
                        cheese.OutgoingReports.Add(oR);
                        //cheese.SaveChanges();
                        cheese.SaveChanges();
                        loadData();
                        clear();
                        btnDisabled();
                        MessageBox.Show("Outgoing Report Created");
                        dataGridView2.DataSource = null;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Update " + textBox1.Text + " Delivery Status", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    OutgoingReport oR = (from x in cheese.OutgoingReports where x.OutgoingReportID.Equals(textBox1.Text) select x).First();
                    if (oR.DeliveryStatus == "Sent")
                    {
                        oR.DeliveryStatus = "Delivered";
                        cheese.SaveChanges();
                        loadData();
                        clear();
                        btnDisabled();
                        MessageBox.Show("Delivery Status Updated");
                        dataGridView2.DataSource = null;
                    }
                }
            }
        }
    }
}

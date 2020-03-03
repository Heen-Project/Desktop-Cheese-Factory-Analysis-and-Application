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
    public partial class QM_CheckOutgoingProduct : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public QM_CheckOutgoingProduct()
        {
            InitializeComponent();
            loadData();
            clear();
            btnEnabled(false);
        }
        public void loadData(){
            var schedule = from x in cheese.Schedules
                           where x.ScheduleStatus.Equals("Packed")
                           orderby x.ScheduleStatus descending, x.PackingDate ascending
                           select new { x.ScheduleID, x.SalesTransactionID, x.BatchNumberSchedule, x.PackingDate, x.ScheduleStatus };
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
            textBox1.Text = "";
            textBox2.Text = "";
        }

        public void btnEnabled(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = status;
            button2.Enabled = status;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                btnEnabled(false);
            }
            else
            {
                btnEnabled(true);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.SelectedRows!=null) {
                
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                var salesTransaction = from x in cheese.SalesTransactions
                                       where x.SalesTransactionID.Equals(textBox2.Text)
                                       select new { x.CustomerID, x.Transportation, x.ShippingLocation };
                dataGridView2.DataSource = salesTransaction.ToList();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Approve " + textBox1.Text + " Packing Status", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    Schedule s = (from x in cheese.Schedules where x.ScheduleID.Equals(textBox1.Text) where x.SalesTransactionID.Equals(textBox2.Text) select x).First();
                    if (s.ScheduleStatus == "Packed")
                    {
                        s.ScheduleStatus = "Processed to Delivery";
                        cheese.SaveChanges();
                        loadData();
                        clear();
                        btnEnabled(false);
                        MessageBox.Show("Status Updated");
                        dataGridView2.DataSource = null;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Reject " + textBox1.Text + " Packing Status", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    Schedule s = (from x in cheese.Schedules where x.ScheduleID.Equals(textBox1.Text) where x.SalesTransactionID.Equals(textBox2.Text) select x).First();
                    if (s.ScheduleStatus == "Packed")
                    {
                        s.ScheduleStatus = "Rejected";
                        cheese.SaveChanges();
                        loadData();
                        clear();
                        btnEnabled(false);
                        MessageBox.Show("Status Updated");
                        dataGridView2.DataSource = null;
                    }
                }
            }
        }
    

    }
}

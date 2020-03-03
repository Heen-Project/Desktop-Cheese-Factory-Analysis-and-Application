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
    public partial class OP_ViewPackingSchedule : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public OP_ViewPackingSchedule()
        {
            InitializeComponent();
            loadData();
            clear();
            btnEnabled(false);
            timer1.Enabled = true;
            informationBox();
        }

        public void loadData()
        {
            var schedule = from x in cheese.Schedules
                           //where x.ScheduleStatus.Equals("Waiting")
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
            textBox3.Text = "";
            textBox9.Text = "";
        }

        public void btnEnabled(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox9.Enabled = false;
            button1.Enabled = status;
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
                                       select new {x.CustomerID, x.Transportation, x.ShippingLocation};
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                dataGridView2.DataSource = salesTransaction.ToList();
                textBox9.Text = dataGridView2.Rows[0].Cells[1].Value.ToString();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Update " + textBox1.Text+ " Packing Status", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    Schedule s = (from x in cheese.Schedules where x.ScheduleID.Equals(textBox1.Text) where x.SalesTransactionID.Equals(textBox2.Text) select x).First();
                    SalesTransaction sT = (from x in cheese.SalesTransactions where x.SalesTransactionID.Equals(textBox2.Text) select x).First();
                    if (s.ScheduleStatus == "Waiting" || s.ScheduleStatus == "Rejected")
                    {
                        if (s.BatchNumberSchedule == 1)
                        {
                            sT.SalesTransactionStatus = "Ongoing";
                        }
                        s.ScheduleStatus = "Packed";
                        cheese.SaveChanges();
                        loadData();
                        clear();
                        MessageBox.Show("Status Updated");
                    }
                    else
                    {
                        MessageBox.Show("Items Already Packed");
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime besok = DateTime.Today.AddDays(1);
            foreach (DataGridViewRow  row in dataGridView1.Rows)
            {
                if (row.Cells[4].Value.ToString() == "Rejected")
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
                else if (row.Cells[4].Value.ToString() != "Waiting")
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                } 
                else { 
                if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()).Date, DateTime.Today) < 0)
                {
                    if (row.Cells[4].Value.ToString() == "Waiting"){
                        row.DefaultCellStyle.BackColor = Color.Red; 
                    }
                    else {
                        row.DefaultCellStyle.BackColor = Color.Green; 
                    }
                }
                else if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()).Date, DateTime.Today) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Pink; 
                }
                else if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()).Date, besok) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow; 
                }
                else if (DateTime.Compare(DateTime.Parse(row.Cells[3].Value.ToString()).Date, besok) > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Orange; 
                }
                }
            }
        }

        public void informationBox()
        {
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox4.BackColor = Color.Red;
            textBox5.BackColor = Color.Pink;
            textBox6.BackColor = Color.Yellow;
            textBox7.BackColor = Color.Orange;
            textBox8.BackColor = Color.Green;
        }

    }
}

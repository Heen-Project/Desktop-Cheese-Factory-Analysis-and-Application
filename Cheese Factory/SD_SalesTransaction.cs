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
    public partial class SD_SalesTransaction : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        DataTable tableCart = new DataTable();
        int flag = 0, cartRow = 0, selectedCartRow = -1;
        List<String> paymentType = new List<String>();
        List<String> transportation = new List<String>();
        DateTime hariIni = DateTime.Now;
        DateTime lusa;

        public SD_SalesTransaction()
        {
            InitializeComponent();
            insertComboBox();
            refreshData();
            resetCart();
            resetTransaction();
            transactionCartDisable();
            btnClear();
            dataGridView2.Enabled = false;
            lusa = hariIni.AddDays(2);
            //dateTimePicker1.MinDate = lusa;
        }

        public string generateScheduleID()
        {
            string newID = "";
            var schedule = from temp in cheese.Schedules select temp;
            var countOfRows = schedule.Count();

            if (countOfRows != 0)
            {
                var data = schedule.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.ScheduleID;
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
                return "SCH0001";
            }
        }

        public string generateSalesTransactionID()
        {
            string newID = "";
            var salesTransaction = from temp in cheese.SalesTransactions select temp;
            var countOfRows = salesTransaction.Count();

            if (countOfRows != 0)
            {
                var data = salesTransaction.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.SalesTransactionID;
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
                return "SLT0001";
            }
        }

        private void refreshData()
        {
            var salesTransaction = from x in cheese.SalesTransactions
                                   select new { x.SalesTransactionID, x.CustomerID, x.PaymentType, x.ShippingLocation, x.Transportation, x.ShippingTime, x.NumberOfBatch, x.TransportPrice };
            dataGridView1.DataSource = salesTransaction.ToList();
            if (salesTransaction != null)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }
        }

        private void transactionCartDisable()
        {
            groupBox3.Enabled = false;
            groupBox5.Enabled = false;
            textBox1.Enabled = false;
        }

        private void transactionTrueCartFalse(bool status)
        {
            groupBox3.Enabled = status;
            groupBox5.Enabled = !status;
            textBox1.Enabled = false;
        }

        public void resetTransaction()
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            textBox2.Text = "";
            comboBox3.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            trackBar1.Value = 1;
            label11.Text = trackBar1.Value.ToString();
            if (comboBox3.SelectedValue.ToString().Equals("Car"))
            {
                label12.Text = trackBar1.Value * 10000 + "";
            }
            else if (comboBox3.SelectedValue.ToString().Equals("Ship"))
            {
                label12.Text = trackBar1.Value * 20000 + "";
            }
            else if (comboBox3.SelectedValue.ToString().Equals("Plane"))
            {
                label12.Text = trackBar1.Value * 30000 + "";
            }
            label10.Visible = false;
        }

        public void resetCart()
        {
            comboBox4.SelectedIndex = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            label20.Visible = false;
        }

        public void btnClear()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
        }

        public void btnInsertTransaction()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
        }

        public void btnUpdateTransaction()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = true;
            button12.Enabled = false;
        }

        public void btnDefaultCart()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void insertComboBox()
        {
            var customer = from x in cheese.Customers select x;
            var cheesekeju = from x in cheese.Cheese select x;
            paymentType.Add("Cash");
            paymentType.Add("Credit");
            transportation.Add("Car");
            transportation.Add("Ship");
            transportation.Add("Plane");
            comboBox1.DataSource = customer.ToList();
            comboBox1.DisplayMember = "CustomerName";
            comboBox2.DataSource = paymentType;
            comboBox3.DataSource = transportation;
            comboBox4.DataSource = cheesekeju.ToList();
            comboBox4.DisplayMember = "CheeseName";
            tableCart.Columns.Add("Cheese ID");
            tableCart.Columns.Add("Quantity");
            tableCart.Columns.Add("Item Price");
            tableCart.Columns.Add("Total Price");
            dataGridView2.DataSource = tableCart;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = generateSalesTransactionID();
            resetTransaction();
            transactionTrueCartFalse(true);
            btnInsertTransaction();
            flag = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            transactionTrueCartFalse(true);
            btnUpdateTransaction();
            flag = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resetTransaction();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            transactionCartDisable();
            textBox1.Text = "";
            btnClear();
            resetCart();
            resetTransaction();
            tableCart.Clear();
            flag = 0;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                insertData();
            }
            else if (flag == 2)
            {
                updateData();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Delete " + textBox1.Text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    SalesTransaction deleteST = (from x in cheese.SalesTransactions
                                                 where x.SalesTransactionID.Equals(textBox1.Text)
                                                 select x).First();
                    var deleteDST = (from x in cheese.DetailSalesTransactions
                                     where x.SalesTransactionID.Equals(textBox1.Text)
                                     select x);
                    var deleteS = (from x in cheese.Schedules
                                     where x.SalesTransactionID.Equals(textBox1.Text)
                                     select x);
                    if (deleteST != null && deleteST.SalesTransactionStatus == "")
                    {
                        cheese.SalesTransactions.Remove(deleteST);
                        foreach (DetailSalesTransaction detailSalesTransaction in deleteDST)
                        {
                            cheese.DetailSalesTransactions.Remove(detailSalesTransaction);
                        }
                        foreach (Schedule schedule in deleteS)
                        {
                            cheese.Schedules.Remove(schedule);
                        }
                        cheese.SaveChanges();
                        refreshData();
                        resetCart();
                        resetTransaction();
                        textBox1.Text = "";
                        dataGridView2.DataSource = tableCart;
                    }
                    else
                    {
                        refreshData();
                        resetCart();
                        resetTransaction();
                        label10.Text = "Status Restricted";
                        label10.Visible = true;
                    }
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label11.Text = trackBar1.Value.ToString();
            if (comboBox3.SelectedValue.ToString().Equals("Car"))
            {
                label12.Text = trackBar1.Value * 10000 + "";
            }
            else if (comboBox3.SelectedValue.ToString().Equals("Ship"))
            {
                label12.Text = trackBar1.Value * 20000 + "";
            }
            else if (comboBox3.SelectedValue.ToString().Equals("Plane"))
            {
                label12.Text = trackBar1.Value * 30000 + "";
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedValue.ToString().Equals("Car"))
            {
                label12.Text = trackBar1.Value * 10000 + "";
            }
            else if (comboBox3.SelectedValue.ToString().Equals("Ship"))
            {
                label12.Text = trackBar1.Value * 20000 + "";
            }
            else if (comboBox3.SelectedValue.ToString().Equals("Plane"))
            {
                label12.Text = trackBar1.Value * 30000 + "";
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                label10.Text = "Address Must be Filled";
                label10.Visible = true;
            }
            else if (DateTime.Compare(dateTimePicker1.Value, lusa) <= 0)
            {
                label10.Text = "Delivery Min in 2 Day Period";
                label10.Visible = true;
            }
            else
            {
                btnDefaultCart();
                label10.Visible = false;
                transactionTrueCartFalse(false);
                dataGridView2.Enabled = true;
                dataGridView2.DataSource = tableCart;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                label20.Text = "Quantity Must be More Than 0";
                label20.Visible = true;
            }
            else if (numericUpDown2.Value < 1)
            {
                label20.Text = "Price Must Be Inpputed";
                label20.Visible = true;
            }
            else
            {
                var cheeseKeju = from x in cheese.Cheese select x;//all cheese
                string ckName = cheeseKeju.ToArray()[comboBox4.SelectedIndex].CheeseName;//cheese name
                var cheeseKejuInput = (from x in cheese.Cheese where x.CheeseName.Equals(ckName) select x).First();//cheese data

                tableCart.Rows.Add(cheeseKejuInput.CheeseID, (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown1.Value * numericUpDown2.Value);
                cartRow++;
                resetCart();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (selectedCartRow >= 0)
            {
                DataRow currRow;
                var cheeseKeju = from x in cheese.Cheese select x;//all cheese
                string ckName = cheeseKeju.ToArray()[comboBox4.SelectedIndex].CheeseName;//cheese name
                var cheeseKejuInput = (from x in cheese.Cheese where x.CheeseName.Equals(ckName) select x).First();//cheese data

                currRow = tableCart.Rows[selectedCartRow];
                currRow.BeginEdit();
                currRow["Cheese ID"] = cheeseKejuInput.CheeseID.ToString();
                currRow["Quantity"] = (int)numericUpDown1.Value;
                currRow["Item Price"] = (int)numericUpDown2.Value;
                currRow["Total Price"] = (int)numericUpDown1.Value * numericUpDown2.Value;
                currRow.EndEdit();
                MessageBox.Show("Record Edited Successfully");

                resetCart();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DataRow[] foundRows = tableCart.Select();
            if (selectedCartRow < 0)
            {
                label20.Text = "You Must Select an item in the cart";
                label20.Visible = true;
                return;
            }
            else
            {
                if (textBox1.Text != "" && foundRows.Length > 0)
                {

                    DialogResult dr = MessageBox.Show("Delete " + tableCart.Rows[selectedCartRow], "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        tableCart.Rows[selectedCartRow].Delete();
                        resetCart();
                        selectedCartRow = -1;
                        cartRow--;
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tableCart.Clear();
            resetCart();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            btnClear();
            btnInsertTransaction();
            label20.Visible = false;
            transactionTrueCartFalse(true);
            dataGridView2.Enabled = false;
        }

        public void insertData()
        {
            if (cartRow > 0)
            {
                var customer = from x in cheese.Customers select x;//all customer
                string cName = customer.ToArray()[comboBox1.SelectedIndex].CustomerName; // customer name
                var customerInput = (from x in cheese.Customers where x.CustomerName.Equals(cName) select x).First();//customer data

                SalesTransaction sT = new SalesTransaction();
                sT.SalesTransactionID = textBox1.Text;
                sT.CustomerID = customerInput.CustomerID;
                sT.SalesTransactionDate = DateTime.Now;
                sT.PaymentType = comboBox2.SelectedValue.ToString();
                sT.ShippingLocation = textBox2.Text;
                sT.Transportation = comboBox3.SelectedValue.ToString();
                sT.ShippingTime = dateTimePicker1.Value;
                sT.NumberOfBatch = trackBar1.Value;
                sT.SalesTransactionStatus = "";
                //sT.ConfirmBy = "";
                //sT.ConfirmDate = DateTime.Today;
                sT.InsertBy = _MainForm.userID;
                sT.InsertDate = DateTime.Now;
                sT.TransportPrice = Int32.Parse(label12.Text);
                cheese.SalesTransactions.Add(sT);
                //cheese.SaveChanges();
                foreach (DataRow row in tableCart.Rows)
                {
                    DetailSalesTransaction dST = new DetailSalesTransaction();
                    dST.SalesTransactionID = textBox1.Text;
                    dST.CheeseID = row.Field<string>(0);
                    dST.Quantity = Int32.Parse(row.Field<string>(1));//Int32.(row["Quantity"].ToString());
                    dST.ItemPrice = Int32.Parse(row.Field<string>(2));
                    dST.TotalPrice = Int32.Parse(row.Field<string>(3));//row["VendorID"].ToString();
                    cheese.DetailSalesTransactions.Add(dST);
                    //cheese.SaveChanges();
                }

                cheese.SaveChanges();
                int leapdays=0;
                DateTime tempDate = dateTimePicker1.Value;
                for (int i = 0; i < trackBar1.Value; i++)
                {
                    leapdays = i*7;
                    Schedule s = new Schedule();
                    s.ScheduleID = generateScheduleID();
                    s.SalesTransactionID = textBox1.Text;
                    s.BatchNumberSchedule = i + 1;
                    s.PackingDate = tempDate.AddDays(leapdays-2);
                    s.ShippingDate = tempDate.AddDays(leapdays);
                    s.ScheduleStatus= "Waiting";
                    cheese.Schedules.Add(s);
                    cheese.SaveChanges();
                }
                refreshData();
                resetCart();
                resetTransaction();
                transactionCartDisable();
                btnClear();
                dataGridView2.Enabled = false;
                tableCart.Clear();
                flag = 0;
            }
            else
            {
                label20.Text = "Cart Empty";
                label20.Visible = true;
            }
        }

        private void updateData()
        {
            if (textBox1.Text != "")
            {
                SalesTransaction sT = (from x in cheese.SalesTransactions where x.SalesTransactionID.Equals(textBox1.Text) select x).First();
                var deleteS = (from x in cheese.Schedules
                               where x.SalesTransactionID.Equals(textBox1.Text)
                               select x);
                if (sT != null && sT.SalesTransactionStatus == "")
                {
                    
                    sT.SalesTransactionDate = DateTime.Now;
                    sT.PaymentType = comboBox2.SelectedValue.ToString();
                    sT.ShippingLocation = textBox2.Text;
                    sT.Transportation = comboBox3.SelectedValue.ToString();
                    sT.ShippingTime = dateTimePicker1.Value;
                    sT.NumberOfBatch = trackBar1.Value;
                    sT.SalesTransactionStatus = "";
                    //sT.ConfirmBy = "";
                    //sT.ConfirmDate = DateTime.Today;
                    sT.InsertBy = _MainForm.userID;
                    sT.InsertDate = DateTime.Now;
                    sT.TransportPrice = Int32.Parse(label12.Text);

                    foreach (Schedule schedule in deleteS)
                    {
                        cheese.Schedules.Remove(schedule);
                    }
                    int leapdays = 0;
                    DateTime tempDate = dateTimePicker1.Value;
                    for (int i = 0; i < trackBar1.Value; i++)
                    {
                        leapdays = i * 7;
                        Schedule s = new Schedule();
                        s.ScheduleID = generateScheduleID();
                        s.SalesTransactionID = textBox1.Text;
                        s.BatchNumberSchedule = i + 1;
                        s.PackingDate = tempDate.AddDays(leapdays - 2);
                        s.ShippingDate = tempDate.AddDays(leapdays);
                        s.ScheduleStatus = "Not Done";
                        cheese.Schedules.Add(s);
                    }
                    cheese.SaveChanges();
                    refreshData();
                    resetCart();
                    resetTransaction();
                    transactionCartDisable();
                    btnClear();
                    dataGridView2.Enabled = false;
                    tableCart.Clear();
                    flag = 0;
                }
                else
                {

                    refreshData();
                    resetCart();
                    resetTransaction();
                    transactionCartDisable();
                    btnClear();
                    dataGridView2.Enabled = false;
                    tableCart.Clear();
                    label10.Text = "Status Restricted";
                    label10.Visible = true;
                    flag = 0;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (flag != 1 )
                {
                    var customer = from x in cheese.Customers select x;//all customer
                    string cName = customer.ToArray()[comboBox1.SelectedIndex].CustomerName; // customer name
                    var customerInput = (from x in cheese.Customers where x.CustomerName.Equals(cName) select x).First();//customer data

                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                    int tempCustomer = 0;
                    while (tempCustomer < customer.Count())
                    {
                        if (customer.ToArray()[tempCustomer].CustomerID.Equals(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()))
                        {
                            break;
                        }
                        else
                        {
                            tempCustomer++;
                        }
                    }
                    comboBox1.SelectedIndex = tempCustomer;
                    if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "Cash")
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    else if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "Cash")
                    {
                        comboBox2.SelectedIndex = 1;
                    }
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "Car")
                    {
                        comboBox3.SelectedIndex = 0;
                    }
                    else if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "Ship")
                    {
                        comboBox3.SelectedIndex = 1;
                    }
                    else if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "Plane")
                    {
                        comboBox3.SelectedIndex = 2;
                    }
                    dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                    trackBar1.Value = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                    label11.Text = trackBar1.Value.ToString();
                    label12.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                    var detailSalesTransaction = from x in cheese.DetailSalesTransactions
                                                 where x.SalesTransactionID == textBox1.Text
                                                 select new { x.CheeseID, x.Quantity, x.ItemPrice, x.TotalPrice };
                    dataGridView2.DataSource = detailSalesTransaction.ToList();
                    dataGridView2.Enabled = false;
                    resetCart();
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var cheeseKeju = from x in cheese.Cheese select x;//all cheese
            string ckName = cheeseKeju.ToArray()[comboBox4.SelectedIndex].CheeseName;//cheese name
            var cheeseKejuInput = (from x in cheese.Cheese where x.CheeseName.Equals(ckName) select x).First();//cheese data

            selectedCartRow = e.RowIndex;
            int tempCheese = 0;
            while (tempCheese < cheeseKeju.Count())
            {
                if (cheeseKeju.ToArray()[tempCheese].CheeseID.Equals(dataGridView2.Rows[selectedCartRow].Cells[0].Value.ToString()))
                {
                    break;
                }
                else
                {
                    tempCheese++;
                }
            }
            comboBox4.SelectedIndex = tempCheese;
            numericUpDown1.Value = Int32.Parse(dataGridView2.Rows[selectedCartRow].Cells[1].Value.ToString());
            numericUpDown2.Value = Int32.Parse(dataGridView2.Rows[selectedCartRow].Cells[2].Value.ToString());

        }

    }
}

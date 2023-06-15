using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Module3
{
    public partial class Form1 : Form
    {
        public PrivateFontCollection pfc { get; set; } = new PrivateFontCollection();
        public WSC2017_Session3Entities ent { get; set; } = new WSC2017_Session3Entities();
        public Form1()
        {
            InitializeComponent();
            
            radioButton1.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2017_Session3DataSet.CabinTypes' table. You can move, or remove it, as needed.
            this.cabinTypesTableAdapter.Fill(this.wSC2017_Session3DataSet.CabinTypes);
            // TODO: This line of code loads data into the 'wSC2017_Session3DataSet1.Airports' table. You can move, or remove it, as needed.
            this.airportsTableAdapter1.Fill(this.wSC2017_Session3DataSet1.Airports);
            // TODO: This line of code loads data into the 'wSC2017_Session3DataSet.Airports' table. You can move, or remove it, as needed.
            this.airportsTableAdapter.Fill(this.wSC2017_Session3DataSet.Airports);
            this.BackColor = Color.FromArgb(247, 148, 32);
            pfc.AddFontFile(Application.StartupPath + "\\font.TTF");

            try
            {
                Font = new Font(pfc.Families[0], 8f);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("All the required are required!", "Alert");
                return;
            }

            var fromId = (int)comboBox1.SelectedValue;
            var toId = (int)comboBox2.SelectedValue;
            var cabinTypeId = (int)comboBox3.SelectedValue;

            if (fromId == toId)
            {
                MessageBox.Show("From Airport cannot be the same as to airport.", "Alert");
                return;
            }

            var fromDate = dateTimePicker1.Value.Date;
            var returnDate = dateTimePicker2.Value.Date;

            if (radioButton1.Checked)
            {
                if (fromDate >= returnDate)
                {
                    MessageBox.Show("Return date must be bigger than outbound date.", "Alert");
                    return;
                }
            }

            this.loadDataGrid(fromId, toId, fromDate, cabinTypeId, 1, checkBox1.Checked);
            this.loadDataGrid(toId, fromId, returnDate, cabinTypeId, 2, checkBox2.Checked);
        }

        private List<Tickets> datagrid1 = new List<Tickets>();
        private List<Tickets> datagrid2 = new List<Tickets>();

        private void loadDataGrid(int fromId, int toId, DateTime fromDate, int cabinTypeId, int DataGridId, bool isCheck)
        {
            var schedules = ent.Schedules.OrderBy(x => x.Date).ThenBy(x => x.Time).ToList();

            List<Tickets> datagrid = new List<Tickets>();

            for (int i = 0; i < schedules.Count; i++)
            {
                var schedule = schedules[i];

                if (schedule.Route.DepartureAirportID != fromId)
                {
                    continue;
                }

                if (isCheck)
                {
                    DateTime startDate = fromDate.AddDays(-3);
                    DateTime endDate = fromDate.AddDays(3);

                    if (schedule.Date < startDate || schedule.Date > endDate)
                    {
                        continue;
                    }
                }
                else
                {
                    if (fromDate != schedule.Date)
                    {
                        continue;
                    }
                }

                Tickets dataGridContent = new Tickets();
                dataGridContent.Id = i;
                dataGridContent.CabinTypeId = cabinTypeId;
                dataGridContent.CabinType = ent.CabinTypes.FirstOrDefault(x => x.ID == cabinTypeId).Name;
                dataGridContent.From = ent.Airports.FirstOrDefault(x => x.ID == fromId).IATACode;
                dataGridContent.To = ent.Airports.FirstOrDefault(x => x.ID == toId).IATACode;
                dataGridContent.Date = schedule.Date.ToString("dd/MM/yyyy");
                dataGridContent.Time = schedule.Time.ToString();
                dataGridContent.CabinPrice += schedule.EconomyPrice;
                dataGridContent.FlightNumber = schedule.FlightNumber;
                dataGridContent.schedulesId.Add(schedule.ID);

                if (schedule.Route.ArrivalAirportID == toId)
                {
                    if (cabinTypeId == 2)
                    {
                        dataGridContent.CabinPrice *= 1.35m;
                    }
                    else if (cabinTypeId == 3)
                    {
                        dataGridContent.CabinPrice *= 1.35m;
                        dataGridContent.CabinPrice *= 1.30m;
                    }

                    datagrid.Add(dataGridContent);
                    continue;
                }

                var arriveId = schedule.Route.ArrivalAirportID;

                for (int j = i + 1; j < schedules.Count; j++)
                {
                 
                    var arriveSchedule = schedules[j];

                    if (arriveSchedule.Route.DepartureAirportID != arriveId || arriveSchedule.Route.ArrivalAirportID == fromId)
                    {
                        continue;
                    }

                    arriveId = arriveSchedule.Route.ArrivalAirportID;
                    dataGridContent.NumberOfStops++;
                    dataGridContent.FlightNumber += $"- {arriveSchedule.FlightNumber}";
                    dataGridContent.CabinPrice += schedule.EconomyPrice;
                    dataGridContent.schedulesId.Add(arriveSchedule.ID);

                    if (arriveSchedule.Route.ArrivalAirportID == toId)
                    {
                        if (cabinTypeId == 2)
                        {
                            dataGridContent.CabinPrice *= 1.35m;
                        }
                        else if (cabinTypeId == 3)
                        {
                            dataGridContent.CabinPrice *= 1.35m;
                            dataGridContent.CabinPrice *= 1.30m;
                        }

                        datagrid.Add(dataGridContent);
                        break;
                    }
                }
            }

            if (DataGridId == 1)
            {
                datagrid1 = datagrid;
                toTicket = new Tickets();

                dataGridView1.Rows.Clear();

                foreach (var content in datagrid1)
                {
                    dataGridView1.Rows.Add(content.Id, content.From, content.To, content.Date, content.Time, content.FlightNumber, content.CabinPrice, content.NumberOfStops);
                }
            }
            else
            {
                datagrid2 = datagrid;
                returnTicket = new Tickets();

                dataGridView2.Rows.Clear();

                foreach (var content in datagrid)
                {
                    dataGridView2.Rows.Add(content.Id, content.From, content.To, content.Date, content.Time, content.FlightNumber, content.CabinPrice, content.NumberOfStops);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label5.Visible = true;
                dateTimePicker2.Visible = true;
                panel2.Visible = true;
            }
            else
            {
                label5.Visible = false;
                dateTimePicker2.Visible = false;
                panel2.Visible = false;
            }
        }

        private Tickets toTicket;
        private Tickets returnTicket;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                toTicket = datagrid1.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var id = int.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                returnTicket = datagrid2.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("the number of passengers must be greater than 0", "Alert");
                return;
            }

            if (radioButton1.Checked)
            {
                if (toTicket.Id == null || returnTicket.Id == null)
                {
                    MessageBox.Show("Two fligths must be selected.", "Alert");
                    return;
                }

                this.Hide();
                BookingConfirmationForm form = new BookingConfirmationForm(toTicket, returnTicket, (int)numericUpDown1.Value);
                form.ShowDialog();
            }
            else
            {
                if (toTicket.Id == null)
                {
                    MessageBox.Show("To fligth must be selected.", "Alert");
                    return;
                }

                this.Hide();
                BookingConfirmationForm form = new BookingConfirmationForm(toTicket, (int)numericUpDown1.Value);
                form.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

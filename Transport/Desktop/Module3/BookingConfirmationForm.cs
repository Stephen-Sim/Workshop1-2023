using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Module3
{
    public partial class BookingConfirmationForm : Form
    {
        public PrivateFontCollection pfc { get; set; } = new PrivateFontCollection();
        public WSC2017_Session3Entities ent { get; set; } = new WSC2017_Session3Entities();
        public BookingConfirmationForm()
        {
            InitializeComponent();
        }

        private Tickets toTicket = new Tickets();
        private Tickets returnTicket = null;
        private int numberOfPassenger = 0;

        public BookingConfirmationForm(Tickets toTicket, Tickets returnTicket, int numberOfPassenger)
        {
            InitializeComponent();

            this.toTicket = toTicket;
            this.returnTicket = returnTicket;
            this.numberOfPassenger = numberOfPassenger;

            label2.Text = toTicket.From;
            label3.Text = toTicket.To;
            label10.Text = toTicket.Date;
            label7.Text = toTicket.Date;
            label8.Text = toTicket.FlightNumber;

            label19.Text = returnTicket.From;
            label17.Text = returnTicket.To;
            label11.Text = returnTicket.Date;
            label14.Text = returnTicket.Date;
            label12.Text = returnTicket.FlightNumber;

        }
        public BookingConfirmationForm(Tickets toTicket, int numberOfPassenger)
        {
            InitializeComponent();

            groupBox2.Visible = false;
            this.toTicket = toTicket;
            this.numberOfPassenger = numberOfPassenger;

            label2.Text = toTicket.From;
            label3.Text = toTicket.To;
            label10.Text = toTicket.Date;
            label7.Text = toTicket.Date;
            label8.Text = toTicket.FlightNumber;
        }

        private void BookingConfirmationForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2017_Session3DataSet.Countries' table. You can move, or remove it, as needed.
            this.countriesTableAdapter.Fill(this.wSC2017_Session3DataSet.Countries);
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
        }

        public List<TicketData> toTickets { get; set; } = new List<TicketData>();
        public List<TicketData> returnTickets { get; set; } = new List<TicketData>();


        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            var form = (Form1)Application.OpenForms["Form1"];
            form.Show();
        }

        private int count = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("All the fields are required.", "Alert");
                return;
            }

            if (numberOfPassenger == toTickets.Count)
            {
                MessageBox.Show("You have reached the maximum of passengers.", "Alert");
                return;
            }

            var ticketData = new TicketData
            {
                ID = ++count,
                Firstname = textBox1.Text,
                Lastname = textBox2.Text,
                PassportNumber = textBox3.Text,
                BirthDate = dateTimePicker1.Value,
                PassportCountryID = (int)comboBox1.SelectedValue,
                Phone = textBox4.Text,
                PassportCountry = ent.Countries.FirstOrDefault(x => x.ID == (int)comboBox1.SelectedValue).Name,
                CabinPrice = toTicket.CabinPrice,
                CabinTypeId = toTicket.CabinTypeId,
                schedulesId = toTicket.schedulesId,
            };

            toTickets.Add(ticketData);

            if (returnTicket != null)
            {
                var ticketData1 = new TicketData
                {
                    ID = ++count,
                    Firstname = textBox1.Text,
                    Lastname = textBox2.Text,
                    PassportNumber = textBox3.Text,
                    BirthDate = dateTimePicker1.Value,
                    PassportCountryID = (int)comboBox1.SelectedValue,
                    Phone = textBox4.Text,
                    PassportCountry = ent.Countries.FirstOrDefault(x => x.ID == (int)comboBox1.SelectedValue).Name,
                    CabinPrice = returnTicket.CabinPrice,
                    CabinTypeId = returnTicket.CabinTypeId,
                    schedulesId = returnTicket.schedulesId,
                };

                returnTickets.Add(ticketData1);
            }

            this.loadDataGrid();
        }

        private void loadDataGrid()
        {
            dataGridView1.Rows.Clear();

            foreach (var ticketData in toTickets)
            {
                dataGridView1.Rows.Add(ticketData.ID, ticketData.Firstname, ticketData.Lastname, ticketData.BirthDate, ticketData.PassportNumber, ticketData.PassportCountry, ticketData.Phone);
            }
        }

        int selectId = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectId ==0)
            {
                MessageBox.Show("no selected data row", "alert");
                return;
            }

            toTickets.Remove(toTickets.FirstOrDefault(x => x.ID == selectId));

            if (returnTicket != null)
            {
                returnTickets.Remove(returnTickets.FirstOrDefault(x => x.ID == selectId));
            }

            this.loadDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectId = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            }
            catch (Exception)
            {
                selectId = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (toTickets.Count != numberOfPassenger)
            {
                MessageBox.Show($"You should add {numberOfPassenger} passengers.", "Alert");
                return;
            }

            if (returnTicket != null)
            {
                new BillingConfirmationForm(toTickets, returnTickets).ShowDialog();
                this.Hide();
            }
            else
            {
                new BillingConfirmationForm(toTickets).ShowDialog();
                this.Hide();
            }
        }
    }
}

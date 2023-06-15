using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Module3
{
    public partial class BillingConfirmationForm : Form
    {
        public PrivateFontCollection pfc { get; set; } = new PrivateFontCollection();
        public BillingConfirmationForm()
        {
            InitializeComponent();

            radioButton1.Checked = true;
        }

        public List<TicketData> toTickets { get; set; } = new List<TicketData>();
        public List<TicketData> returnTickets { get; set; } = new List<TicketData>();

        public BillingConfirmationForm(List<TicketData> toTickets)
        {
            InitializeComponent();

            radioButton1.Checked = true;
            this.toTickets = toTickets;

            var amount = toTickets.Sum(x => x.CabinPrice);

            label3.Text = amount.ToString("0.00");
        }

        public BillingConfirmationForm(List<TicketData> toTickets, List<TicketData> returnTickets)
        {
            InitializeComponent();

            radioButton1.Checked = true;
            this.toTickets = toTickets;
            this.returnTickets = returnTickets;

            var amount = toTickets.Sum(x => x.CabinPrice) + returnTickets.Sum(x => x.CabinPrice);

            label3.Text = amount.ToString("0.00");
        }

        public WSC2017_Session3Entities ent { get; set; } = new WSC2017_Session3Entities();

        private void BillingConfirmationForm_Load(object sender, EventArgs e)
        {
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
        }

        public string GetBookingReference()
        {
            string data = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string result = "";
            bool unique = false;
            Random rand = new Random();

            while (!unique)
            {
                for (int i = 0; i < 6; i++)
                {
                    result += data[rand.Next(0, data.Length)];
                }

                var q = ent.Tickets.Where(x => x.BookingReference == result).Count();
                if (q > 0)
                {
                    result = "";
                }
                else
                {
                    unique = true;
                }
            }

            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

            new Form1().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bookingReference = GetBookingReference();

            foreach (var ticket in toTickets)
            {
                foreach (var scheduleId in ticket.schedulesId)
                {
                    Ticket t = new Ticket()
                    {
                        UserID = 1,
                        ScheduleID = scheduleId,
                        CabinTypeID = ticket.CabinTypeId,
                        Firstname = ticket.Firstname,
                        Lastname = ticket.Lastname,
                        Phone = ticket.Phone,
                        PassportNumber = ticket.PassportNumber,
                        PassportCountryID = ticket.PassportCountryID,
                        BookingReference = bookingReference,
                        Confirmed = true
                    };
                    ent.Tickets.Add(t);
                    ent.SaveChanges();
                }
            }

            foreach (var ticket in returnTickets)
            {
                foreach (var scheduleId in ticket.schedulesId)
                {
                    Ticket t = new Ticket()
                    {
                        UserID = 1,
                        ScheduleID = scheduleId,
                        CabinTypeID = ticket.CabinTypeId,
                        Firstname = ticket.Firstname,
                        Lastname = ticket.Lastname,
                        Phone = ticket.Phone,
                        PassportNumber = ticket.PassportNumber,
                        PassportCountryID = ticket.PassportCountryID,
                        BookingReference = bookingReference,
                        Confirmed = true
                    };
                    ent.Tickets.Add(t);
                    ent.SaveChanges();
                }
            }

            this.Hide();

            MessageBox.Show("Booking is made!", "Alert");

            new Form1().ShowDialog();
        }
    }
}

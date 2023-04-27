using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Sponsor_Desktop
{
    public partial class CertForm : Form
    {
        public ASC2023_SponsorEntities ent { get; set; }
        public CertForm(long sponsorId, string category)
        {
            InitializeComponent();

            ent = new ASC2023_SponsorEntities();

            if (category == "gold")
            {
                this.BackgroundImage = Properties.Resources.Slide1;
            }
            else if (category == "silver")
            {
                this.BackgroundImage = Properties.Resources.Slide2;
            }
            else if (category == "bronze")
            {
                this.BackgroundImage = Properties.Resources.Slide3;
            }
            else if (category == "none")
            {
                this.BackgroundImage = Properties.Resources.Slide4;
            }

            var sponsor = ent.Sponsorships.FirstOrDefault(x => x.Id == sponsorId);

            label1.Text = $"Greetings and heartfelt gratitude to {sponsor.SponsorName} for their generous support of the Worldskills ASEAN 2023.";
            label2.Text = $"Your contribution to the competitor {sponsor.Competitor.Name} From {sponsor.Competitor.Country.Name} in {sponsor.Competitor.Skill.Name} category was invaluable and played a crucial role in ensuring the success of the competition.";
            label3.Text = $"We are proud to announce that {sponsor.SponsorName} contributed a total of {sponsor.Currency.Name} {sponsor.Amount} towards the event.";
        }

        private void CertForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG File| *.PNG";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(this.Width, this.Height);
            
                this.FormBorderStyle = FormBorderStyle.None;

                this.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, this.Width, this.Height));

                bitmap.Save(sfd.FileName);
            }

            MessageBox.Show("Certificate is successfully save.", "Alert");

            this.Close();
        }
    }
}

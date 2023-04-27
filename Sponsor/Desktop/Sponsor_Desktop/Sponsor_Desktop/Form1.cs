using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sponsor_Desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ent = new ASC2023_SponsorEntities();

            this.loadDataGrid1();

            groupBox2.Enabled = false;
        }

        public ASC2023_SponsorEntities ent { get; set; }

        private void loadDataGrid1(string text = null)
        {
            var coms = ent.Competitors.ToList().Where(x =>
                text == null ||
                x.Name.ToLower().Contains(text.ToLower()) ||
                x.Skill.Name.ToLower().Contains(text.ToLower()) ||
                x.Country.Name.ToLower().Contains(text.ToLower())).Select(x => new
            {
                x.Id,
                x.Name,
                SkillName = x.Skill.Name,
                CountryName = x.Country.Name,
                RequiredAmount = x.RequiredAmount,
                ShortfallAmount = new Func<string>(() =>
                {
                    string str = (x.RequiredAmount - (ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Any() ? ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Sum(y => y.Amount * y.Currency.Rate) : 0)).ToString("0.00");

                    if (str == "0.00")
                    {
                        return "Fully Sponsor";
                    }

                    return str;
                })(),
                Percentage = (ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Any() ? ent.Sponsorships.Where(y => y.CompetitorId == x.Id).Sum(y => y.Amount * y.Currency.Rate) : 0.00m) / x.RequiredAmount * 100
            }).ToList();

            dataGridView1.Rows.Clear();

            for (int i = 0; i < coms.Count; i++)
            {
                dataGridView1.Rows.Add(coms[i].Id, coms[i].Name, coms[i].SkillName, coms[i].CountryName, coms[i].RequiredAmount, coms[i].ShortfallAmount);

                if (coms[i].Percentage < 30)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else if (coms[i].Percentage >= 30 && coms[i].Percentage < 60)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                }
                else if (coms[i].Percentage >= 60 && coms[i].Percentage < 100)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if(coms[i].Percentage == 100)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.loadDataGrid1(textBox1.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var comId = (long)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                this.loadDataGrid2(comId);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);

                return;
            }
        }

        public List<Sponsorship> SponsorList { get; set; }

        private void loadDataGrid2(long comId)
        {
            SponsorList = ent.Sponsorships.Where(x => x.CompetitorId == comId).ToList();

            var sponsors = ent.Sponsorships.Where(x => x.CompetitorId == comId).ToList().Select(x => new
            {
                x.Id,
                x.SponsorName,
                DateTime = x.DateTime.ToString("dd/MM/yyyy"),
                x.Description,
                Amount = (x.Amount * x.Currency.Rate).ToString("0.00"),
                Category = new Func<string>(() =>
                {
                    var percentage = x.Amount * x.Currency.Rate / x.Competitor.RequiredAmount * 100;

                    if (percentage >= 30 && percentage < 60)
                    {
                        return "bronze";
                    }
                    else if (percentage >= 60 && percentage < 90)
                    {
                        return "silver";
                    }
                    else if (percentage >= 90)
                    {
                        return "gold";
                    }

                    return "none";
                })()

            }).ToList();

            dataGridView2.Rows.Clear();

            if (sponsors.Count >= 1)
            {
                groupBox2.Enabled = true;
            }
            else 
                groupBox2.Enabled = false;

            for (int i = 0; i < sponsors.Count; i++)
            {
                dataGridView2.Rows.Add(sponsors[i].Id, sponsors[i].SponsorName, sponsors[i].DateTime, sponsors[i].Description, sponsors[i].Amount, sponsors[i].Category);

                switch (sponsors[i].Category)
                {
                    case "bronze":
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(205, 127, 50);
                        break;

                    case "silver":
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);
                        break;

                    case "gold":
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 215, 0);
                        break;

                    default:
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SponsorList.Count >= 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV FILE | *.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = new StreamWriter(sfd.FileName))
                    {
                        stream.WriteLine("No, Name, Date, Description, Sponsor Amount");

                        for (int i = 0; i < SponsorList.Count; i++)
                        {
                            stream.WriteLine("{0}, {1}, {2}, {3}, {4}", i + 1, SponsorList[i].SponsorName, SponsorList[i].DateTime.ToString("dd/MM/yyyy"), SponsorList[i].Description, (SponsorList[i].Amount * SponsorList[i].Currency.Rate).ToString("0.00"));
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (long) dataGridView2.SelectedRows[0].Cells[0].Value;
                var cetogory = dataGridView2.SelectedRows[0].Cells[5].Value.ToString();

                var form = new CertForm(id, cetogory);
                form.ShowDialog();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);

                MessageBox.Show("No Selected Row!", "Alert");
                return;
            }
        }
    }
}

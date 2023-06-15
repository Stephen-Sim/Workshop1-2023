using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class AddOrEditListingForm : Form
    {
        private long userId;
        private long? itemId;

        PrivateFontCollection pfc = new PrivateFontCollection();

        WSC2022SE_Session1Entities ent = new WSC2022SE_Session1Entities();
        public AddOrEditListingForm(long userId)
        {
            InitializeComponent();

            this.userId = userId;

            this.BackColor = Color.FromArgb(245, 230, 23);

            pfc.AddFontFile("Helvetica-Normal.ttf");
            this.panel1.Font = new Font(pfc.Families[0], 8.75f, this.panel1.Font.Style);

            this.Text = "Seoul Stay - Add Listing";

            button2.Text = "Cancel";

            loadDataGrid();
        }

        public AddOrEditListingForm(long userId, long itemId)
        {
            InitializeComponent();

            this.userId = userId;
            this.itemId = itemId;

            this.BackColor = Color.FromArgb(245, 230, 23);

            pfc.AddFontFile("Helvetica-Normal.ttf");
            this.panel1.Font = new Font(pfc.Families[0], 8.75f, this.panel1.Font.Style);

            this.Text = "Seoul Stay - Edit Listing";

            button1.Visible = false;
            button2.Text = "Close";

            loadDataGrid();
        }

        private void loadDataGrid()
        {
            if (itemId == null)
            {
                dataGridView1.Rows.Clear();

                var amens = ent.Amenities.ToList();

                foreach (var item in amens)
                {
                    dataGridView1.Rows.Add(item.ID, item.Name, false);
                }

                dataGridView2.Rows.Clear();

                var attracs = ent.Attractions.ToList();

                foreach (var item in attracs)
                {
                    dataGridView2.Rows.Add(item.ID, item.AreaID, item.Name, item.Area.Name);
                }
            }
            else
            {
                dataGridView1.Rows.Clear();

                var amens = ent.Amenities.ToList().Select(x => new
                {
                    x.ID,
                    x.Name,
                    isTrue = ent.ItemAmenities.Where(y => y.ItemID == itemId && y.AmenityID == x.ID).Any()
                });

                foreach (var item in amens)
                {
                    dataGridView1.Rows.Add(item.ID, item.Name, item.isTrue);
                }

                dataGridView2.Rows.Clear();

                var attracs = ent.Attractions.ToList().Select(x =>
                {
                    var value = new
                    {
                        x.ID,
                        x.AreaID,
                        x.Name,
                        AreaName = x.Area.Name,
                        ItemAttraction = new Func<ItemAttraction>(() =>
                        {
                            if (ent.ItemAttractions.Where(y => y.ItemID == itemId && y.AttractionID == x.ID).Any())
                            {
                                return ent.ItemAttractions.Where(y => y.ItemID == itemId && y.AttractionID == x.ID).First();
                            }

                            return null;
                        })(),
                    };
                    return value;
                }).OrderByDescending(x => x.ItemAttraction != null).ThenBy(x => x.ItemAttraction?.Distance).ToList();

                foreach (var item in attracs)
                {
                    if (item.ItemAttraction == null)
                    {
                        dataGridView2.Rows.Add(item.ID, item.AreaID, item.Name, item.AreaName);
                    }
                    else
                    {
                        dataGridView2.Rows.Add(item.ID, item.AreaID, item.Name, item.AreaName, item.ItemAttraction.Distance, item.ItemAttraction.DurationOnFoot, item.ItemAttraction.DurationByCar);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                foreach (Control item in this.tabPage1.Controls)
                {
                    if (item is NumericUpDown && ((NumericUpDown)item).Value <= 0)
                    {
                        MessageBox.Show("all the value must greater than 0.", "Alert");
                        return;
                    }

                    if (item is TextBox && ((TextBox)item).Text == string.Empty)
                    {
                        MessageBox.Show("all the text fields are required", "Alert");
                        return;
                    }
                }

                tabControlSelectedIndex++;
                tabControl1.SelectedIndex = tabControlSelectedIndex;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                tabControlSelectedIndex++;
                button1.Visible = false;
                button2.Text = "Finish";
            }
        }

        int tabControlSelectedIndex = 0;

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (itemId == null)
            {
                MessageBox.Show("You can't change tab index.", "Alert");
                tabControl1.SelectedIndex = tabControlSelectedIndex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Cancel")
            {
                this.Hide();
                new UserManagementForm(userId).ShowDialog();
            }
            else if (button2.Text == "Finish" || button2.Text == "Close")
            {
                if (dataGridView2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[4].Value != null).Count() < 2)
                {
                    MessageBox.Show("You should enter at least 2 attraction details", "Alert");
                    return;
                }

                var item = (itemId == null) ? new Item() : ent.Items.FirstOrDefault(x => x.ID == itemId);

                item.ItemTypeID = (long)comboBox1.SelectedValue;

                item.UserID = userId;
                item.GUID = Guid.NewGuid();

                item.AreaID = long.Parse(dataGridView2.Rows.Cast<DataGridViewRow>()
                    .Where(x => x.Cells[4].Value != null)
                    .OrderBy(x => decimal.Parse(x.Cells[4].Value.ToString()))
                    .FirstOrDefault().Cells[1].Value.ToString());

                item.Title = textBox1.Text;
                item.ApproximateAddress = textBox2.Text;
                item.ExactAddress = textBox3.Text;
                item.Description = textBox4.Text;
                item.HostRules = textBox5.Text;

                item.Capacity = (int)numericUpDown1.Value;
                item.NumberOfBeds = (int)numericUpDown2.Value;
                item.NumberOfBedrooms = (int)numericUpDown3.Value;
                item.NumberOfBathrooms = (int)numericUpDown4.Value;
                item.MinimumNights = (int)numericUpDown5.Value;
                item.MaximumNights = (int)numericUpDown6.Value;

                if (itemId == null)
                {
                    ent.Items.Add(item);
                }

                if (itemId != null)
                {
                    ent.ItemAmenities.RemoveRange(item.ItemAmenities);

                    ent.ItemAttractions.RemoveRange(item.ItemAttractions);
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[2].Value != null && (bool)row.Cells[2].Value == true)
                    {
                        ItemAmenity itemAmenity = new ItemAmenity() { 
                            GUID = Guid.NewGuid(),
                            ItemID = item.ID,
                            AmenityID = (long) row.Cells[0].Value
                        };

                        ent.ItemAmenities.Add(itemAmenity);
                    }
                }

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if(row.Cells[4].Value != null)
                    {
                        try
                        {
                            ItemAttraction itemAttraction = new ItemAttraction()
                            {
                                GUID = Guid.NewGuid(),
                                ItemID = item.ID,
                                AttractionID = (long)row.Cells[0].Value,
                                Distance = decimal.Parse(row.Cells[4].Value.ToString()),
                                DurationOnFoot = long.Parse(row.Cells[5].Value.ToString()),
                                DurationByCar = long.Parse(row.Cells[6].Value.ToString()),
                            };

                            ent.ItemAttractions.Add(itemAttraction);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Invalid value for attractions", "Alert");
                            return;
                        }

                    }
                }

                ent.SaveChanges();

                MessageBox.Show("Item added or changed!", "Alert");

                this.Hide();

                new UserManagementForm(userId).ShowDialog();
            }
        }

        private void AddOrEditListingForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2022SE_Session1DataSet.ItemTypes' table. You can move, or remove it, as needed.
            this.itemTypesTableAdapter.Fill(this.wSC2022SE_Session1DataSet.ItemTypes);

            comboBox1.SelectedIndex = 0;

            if (itemId != null)
            {
                var item = ent.Items.FirstOrDefault(x => x.ID == itemId);
                comboBox1.SelectedValue = item.ItemTypeID;
                textBox1.Text = item.Title;
                textBox2.Text = item.ApproximateAddress;
                textBox3.Text = item.ExactAddress;
                textBox4.Text = item.Description;
                textBox5.Text = item.HostRules;

                numericUpDown1.Value = (decimal)item.Capacity;
                numericUpDown2.Value = (decimal)item.NumberOfBeds;
                numericUpDown3.Value = (decimal)item.NumberOfBedrooms;
                numericUpDown4.Value = (decimal)item.NumberOfBathrooms;
                numericUpDown5.Value = (decimal)item.MinimumNights;
                numericUpDown6.Value = (decimal)item.MaximumNights;
            }
        }
    }
}

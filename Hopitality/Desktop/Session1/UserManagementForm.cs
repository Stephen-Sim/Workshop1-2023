using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class UserManagementForm : Form
    {
        private long userId;
        public UserManagementForm(long userId)
        {
            InitializeComponent();

            this.userId = userId;

            this.BackColor = Color.FromArgb(245, 230, 23);

            pfc.AddFontFile("Helvetica-Normal.ttf");
            this.panel1.Font = new Font(pfc.Families[0], 8.75f, this.panel1.Font.Style);

            loadDataGrid1();
        }

        PrivateFontCollection pfc = new PrivateFontCollection();

        WSC2022SE_Session1Entities ent = new WSC2022SE_Session1Entities();

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserId = 0;
            Properties.Settings.Default.UserTypeId = 0;
            Properties.Settings.Default.SelectedUserId = 0;

            Properties.Settings.Default.Save();

            this.Hide();

            new Form1().ShowDialog();
        }

        private void loadDataGrid1()
        {
            var items = ent.Items.ToList().Where(x => x.Title.Contains(textBox1.Text) || x.Area.Name.Contains(textBox1.Text) ||
            x.ItemAttractions.Any(y => y.Attraction.Name.Contains(textBox1.Text) && y.Distance < 1)).ToList();

            dataGridView1.Rows.Clear();

            label1.Text = $"{items.Count} items found.";

            foreach (var item in items)
            {
                dataGridView1.Rows.Add(item.Title, item.Capacity, item.Area.Name, item.ItemType.Name);
            }
        }

        private void loadDataGrid2()
        {
            var items = ent.Items.ToList().Where(x => x.UserID == userId).ToList();

            dataGridView2.Rows.Clear();

            label1.Text = $"{items.Count} items found.";

            foreach (var item in items)
            {
                dataGridView2.Rows.Add(item.ID, item.Title, item.Capacity, item.Area.Name, item.ItemType.Name, "Edit Details");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadDataGrid1();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                loadDataGrid1();
            }
            else
            {
                loadDataGrid2();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            new AddOrEditListingForm(userId).ShowDialog();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var itemId = long.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                this.Hide();

                new AddOrEditListingForm(userId, itemId).ShowDialog();
            }
        }
    }
}

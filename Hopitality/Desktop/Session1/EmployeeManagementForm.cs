using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class EmployeeManagementForm : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();

        WSC2022SE_Session1Entities ent = new WSC2022SE_Session1Entities();

        private long selectedUserId;

        public EmployeeManagementForm(long selectedUserId)
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(245, 230, 23);

            pfc.AddFontFile("Helvetica-Normal.ttf");
            this.panel1.Font = new Font(pfc.Families[0], 8.75f, this.panel1.Font.Style);
            
            this.selectedUserId = selectedUserId;

            this.loadDataGrid();
        }

        private void loadDataGrid()
        {
            dataGridView1.Rows.Clear();

            var items = ent.Items.Where(x => x.UserID == selectedUserId).ToList();

            foreach (var item in items)
            {
                dataGridView1.Rows.Add(item.Title, item.Capacity, item.Area.Name, item.ItemType.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserId = 0;
            Properties.Settings.Default.UserTypeId = 0;
            Properties.Settings.Default.SelectedUserId = 0;

            Properties.Settings.Default.Save();

            this.Hide();

            new Form1().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

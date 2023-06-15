using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class CreateAccountForm : Form
    {
        public CreateAccountForm()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(245, 230, 23);

            pfc.AddFontFile("Helvetica-Normal.ttf");
            this.panel1.Font = new Font(pfc.Families[0], 8.75f, this.panel1.Font.Style);

            radioButton1.Checked = true;
        }

        PrivateFontCollection pfc = new PrivateFontCollection();

        WSC2022SE_Session1Entities ent = new WSC2022SE_Session1Entities();
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("All the fields are required", "Alert");
                return;
            }

            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Family Count must greater than 0.", "Alert");
                return;
            }

            if (textBox3.Text.Length < 5)
            {
                MessageBox.Show("Password length must at least 5.", "Alert");
                return;
            }

            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Retype Password not matched.", "Alert");
                return;
            }

            if (!checkBox1.Checked)
            {
                MessageBox.Show("You need to agree the terms and condition.", "Alert");
                return;
            }

            var user = new User 
            {
                Username = textBox1.Text,
                GUID = Guid.NewGuid(),
                FullName = textBox2.Text,
                Password = textBox3.Text,
                Gender = radioButton1.Checked,
                BirthDate = dateTimePicker1.Value,
                UserTypeID = 2,
                FamilyCount = (int)numericUpDown1.Value
            };

            ent.Users.Add(user);
            ent.SaveChanges();

            this.Hide();

            new UserManagementForm(user.ID).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = (Form1)Application.OpenForms["Form1"];
            form.Show();
        }

        bool isRead = false;

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (!isRead)
            {
                MessageBox.Show("You must read terms and conditions at least once", "Alert");
                checkBox1.Checked = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (StreamReader stream = new StreamReader("Terms.txt"))
            {
                MessageBox.Show(stream.ReadToEnd(), "Alert");
                isRead = true;
            }
        }
    }
}

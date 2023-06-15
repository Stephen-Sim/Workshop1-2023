using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1
{
    public partial class Form1 : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection();

        WSC2022SE_Session1Entities ent = new WSC2022SE_Session1Entities();
        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(245, 230, 23);

            pfc.AddFontFile("Helvetica-Normal.ttf");
            this.panel1.Font = new Font(pfc.Families[0], 8.75f, this.panel1.Font.Style);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Username and password fields are required","Alert");
                return;
            }

            int userTypeId = 2;
            var username = textBox2.Text;
            var password = textBox3.Text;

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                userTypeId = 1;
                username = textBox1.Text;
            }

            var user = ent.Users.FirstOrDefault(x => x.UserTypeID == userTypeId && x.Username == username && x.Password == password);

            if (user == null)
            {
                MessageBox.Show((userTypeId == 1 ? "Employee" : "User") + " not found!", "Alert");
                return;
            }

            if (userTypeId == 1)
            {
                var selectedUser = ent.Users.FirstOrDefault(x => x.Username == textBox2.Text);
                
                if (selectedUser == null)
                {
                    MessageBox.Show("User not found!", "Alert");
                    return;
                }

                if (checkBox1.Checked)
                {
                    Properties.Settings.Default.UserId = user.ID;
                    Properties.Settings.Default.UserTypeId = userTypeId;
                    Properties.Settings.Default.SelectedUserId = selectedUser.ID;

                    Properties.Settings.Default.Save();
                }

                this.Hide();

                new EmployeeManagementForm(selectedUser.ID).ShowDialog();
            }
            else if (userTypeId == 2)
            {
                this.Hide();

                if (checkBox1.Checked)
                {
                    Properties.Settings.Default.UserId = user.ID;
                    Properties.Settings.Default.UserTypeId = userTypeId;

                    Properties.Settings.Default.Save();
                }

                new UserManagementForm(user.ID).ShowDialog();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = !checkBox2.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new CreateAccountForm().ShowDialog();
        }
    }
}

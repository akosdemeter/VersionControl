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
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public int selectedID;
        public Form1()
        {
            InitializeComponent();
            label1.Text = Resource1.FullName;
            button1.Text = Resource1.Add;

            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";

            button2.Text = Resource1.FileOutput;
            button3.Text = Resource1.Delete;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = new User() {
                FullName = textBox1.Text
            };
            users.Add(u);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Comma Separated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() != DialogResult.OK) return;
            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                foreach (var us in users)
                {
                    sw.Write(us.ID);
                    sw.Write(",");
                    sw.Write(us.FullName);
                    sw.WriteLine();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //lis
            try
            {
                selectedID = listBox1.SelectedIndex;
                int counter = 0;
                foreach (var us in users)
                {
                    if (selectedID==counter)
                    {
                        users.Remove(us);
                    }
                    counter++;
                }
            }
            catch (Exception){}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e){}
    }
}

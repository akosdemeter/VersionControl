using GiftFactory.Abstractions;
using GiftFactory.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GiftFactory
{
    public partial class Form1 : Form
    {
        private List<Toy> _toys = new List<Toy>();
        private Toy _nexttoy;
        private IToyFactory _factory;
        public IToyFactory Factory
        {
            get { return _factory; }
            set 
            {  
                _factory = value;
                DisplayNext();
            }
        }
        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var toy = Factory.CreateNew();
            _toys.Add(toy);
            toy.Left = -toy.Width;
            mainPanel.Controls.Add(toy);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxposition = 0;
            foreach (var toy in _toys)
            {
                toy.MoveToy();
                if (toy.Left > maxposition)
                {
                    maxposition = toy.Left;
                }
            }
            if (maxposition > 1000)
            {
                var oldesttoy = _toys[0];
                mainPanel.Controls.Remove(oldesttoy);
                _toys.Remove(oldesttoy);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }

        private void DisplayNext() {
            if (_nexttoy != null)
            {
                Controls.Remove(_nexttoy);
            }
            _nexttoy = Factory.CreateNew();
            _nexttoy.Top = label1.Top + label1.Height + 20;
            _nexttoy.Left = label1.Left;
            Controls.Add(_nexttoy);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var colorpicker = new ColorDialog();
            colorpicker.Color = button.BackColor;
            if (colorpicker.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            button.BackColor = colorpicker.Color;
        }
    }
}

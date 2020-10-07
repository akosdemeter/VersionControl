using Porfolio.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Porfolio
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> ticks;
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();

        public Form1()
        {
            InitializeComponent();
            ticks = context.Ticks.ToList();
            dataGridView1.DataSource = ticks;
            CreatePortfolio();
            List<decimal> Nyereségek = new List<decimal>();
            int intervallum = 30;
            DateTime kezdődátum = (from x in ticks select x.TradingDay).Min();
            DateTime záródátum = new DateTime(2016,12,30);
            TimeSpan z = záródátum - kezdődátum;
            for (int i = 0; i < z.Days - intervallum; i++)
            {
                decimal ny = GetPortfolioValue(kezdődátum.AddDays(i + intervallum))
                            - GetPortfolioValue(kezdődátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }
            var nyereségekrendezve = (from x in Nyereségek orderby x select x).ToList();
            MessageBox.Show(nyereségekrendezve[nyereségekrendezve.Count()/5].ToString());
        }
        private void CreatePortfolio() {
            Portfolio.Add(new PortfolioItem() {Index="OTP",Volume=10});
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });
            dataGridView2.DataSource = Portfolio;
        }
        private decimal GetPortfolioValue(DateTime date) {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in ticks 
                            where item.Index == x.Index.Trim()
                            && date<=x.TradingDay select x).First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }
    }
}

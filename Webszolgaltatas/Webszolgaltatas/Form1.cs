using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using Webszolgaltatas.Entities;
using Webszolgaltatas.MnbServiceReference;

namespace Webszolgaltatas
{
    public partial class Form1 : Form
    {
        BindingList<RateDate> rates = new BindingList<RateDate>();
        BindingList<String> Currency = new BindingList<String>();
        public Form1()
        {
            InitializeComponent();
            RefreshData();
            var mnbService2 = new MNBArfolyamServiceSoapClient();
            var request2 = new GetCurrenciesRequestBody();
            var response2 = mnbService2.GetCurrencies(request2);
            var result2 = response2.GetCurrenciesResult;
            XmlDocument xml2 = new XmlDocument();
            xml2.LoadXml(result2);
            foreach (XmlElement element2 in xml2.DocumentElement)
            {
                string curren = "";
                Currency.Add(curren);
                var childelement2 = (XmlElement)element2.ChildNodes[0];
                if (childelement2==null)
                {
                    continue;
                }
                curren = childelement2.GetAttribute("curr");
            }
            comboBox1.DataSource = Currency;
            
        }

        private void RefreshData()
        {
            rates.Clear();
            dataGridView1.DataSource = rates;
            GettingResults();
            ShowChart();
        }

        public void GettingResults() {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.Items.ToString(),
                startDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"),
                endDate = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd")
            };
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateDate();
                rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));
                var childelement = (XmlElement)element.ChildNodes[0];
                if (childelement==null)
                {
                    continue;
                }
                rate.Currency = childelement.GetAttribute("curr");
                var unit = decimal.Parse(childelement.GetAttribute("unit"));
                var value = decimal.Parse(childelement.InnerText);
                if (unit!=0)
                {
                    rate.Value = value / unit;
                }
            }
        }
        public void ShowChart() {
            chartRateData.DataSource = rates;
            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;
            chartRateData.Legends[0].Enabled = false;
            chartRateData.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRateData.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chartRateData.ChartAreas[0].AxisY.IsStartedFromZero = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Webszolgaltatas.Entities;
using Webszolgaltatas.MnbServiceReference;

namespace Webszolgaltatas
{
    public partial class Form1 : Form
    {
        BindingList<RateDate> rates = new BindingList<RateDate>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = rates;
            GettingResults();
            XmlFeldolgoz();
        }
        public void GettingResults() {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
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
                rate.Currency = childelement.GetAttribute("curr");
                var unit = decimal.Parse(element.GetAttribute("unit"));
                var value = decimal.Parse(element.InnerText);
                if (unit!=0)
                {
                    rate.Value = value / unit;
                }
            }
        }
        public void XmlFeldolgoz() {
            
        }
    }
}

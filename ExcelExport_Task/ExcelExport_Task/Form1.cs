using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport_Task
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> Flats;
        Excel.Application xlApp;
        Excel.Workbook xlWb;
        Excel.Worksheet xlSheet;
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }
        private void LoadData() {
            Flats = context.Flats.ToList();
        }
        private void CreateExcel() {
            try
            {
                xlApp = new Excel.Application();
                xlWb = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWb.ActiveSheet;
                CreateTable();
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}",ex.Message,ex.Source);
                MessageBox.Show(errMsg,"Error");
                xlWb.Close(false,Type.Missing,Type.Missing);
                xlApp.Quit();
                xlWb = null;
                xlApp = null;
            }
        }
        private void CreateTable()
        {
            string[] header = new string[] {
                "Kód",
                 "Eladó",
                 "Oldal",
                 "Kerület",
                 "Lift",
                 "Szobák száma",
                 "Alapterület (m2)",
                 "Ár (mFt)",
                 "Négyzetméter ár (Ft/m2)"
            };
            for (int i = 0; i < 9; i++)
            {
                xlSheet.Cells[1, i+1] = header[i];
            }
            object[,] values = new object[Flats.Count, header.Length];
            int counter = 0;
            foreach (Flat f in Flats)
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;
                values[counter, 4] = f.Elevator;
                values[counter, 5] = f.NumberOfRooms;
                values[counter, 6] = f.FloorArea;
                values[counter, 7] = f.Price;
                values[counter, 8] = "="+GetCell(counter+2,header.Length-1)+
                    "/"+GetCell(counter+2,header.Length-2);
                counter++;
            }
            xlSheet.get_Range(GetCell(2,1),
                GetCell(1+values.GetLength(0),values.GetLength(1))).Value2=values;

            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, header.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            int lastRowID = xlSheet.UsedRange.Rows.Count;
            Excel.Range tableRange = xlSheet.get_Range(GetCell(1, 1), GetCell(lastRowID, header.Length));
            tableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
            Excel.Range firstColRange = xlSheet.get_Range(GetCell(1, 1), GetCell(lastRowID, 1));
            firstColRange.Font.Bold = true;
            firstColRange.Interior.Color = Color.LightYellow;
            Excel.Range lastColRange = xlSheet.get_Range(GetCell(1, header.Length), GetCell(lastRowID, header.Length));
            lastColRange.Interior.Color = Color.LightGreen;
            lastColRange.NumberFormat = "0.00";
        }
        private string GetCell(int x, int y) {
            string ExcelCordinate = "";
            int dividend = y;
            int modulo;
            while (dividend>0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCordinate = Convert.ToChar(65 + modulo).ToString()+ExcelCordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCordinate += x.ToString();
            return ExcelCordinate;
        }
    }
}

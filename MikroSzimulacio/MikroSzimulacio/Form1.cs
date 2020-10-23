﻿using MikroSzimulacio.Entities;
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

namespace MikroSzimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProb> BirthProbs = new List<BirthProb>();
        List<DeathProb> DeathProbs = new List<DeathProb>();
        List<int> NbrOfMalesInYears = new List<int>();
        List<int> NbrOfFemalesInYears = new List<int>();
        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();
        }

        private void Simulation()
        {
            Population = GetPopulation(@textBox1.Text.ToString());
            BirthProbs = GetBirthProb(@"C:\Hivatalos_dok\születés.csv");
            DeathProbs = GetDeathProb(@"C:\Hivatalos_dok\halál.csv");
            for (int i = 2005; i < numericUpDown1.Value; i++)
            {
                for (int j = 0; j < Population.Count; j++)
                {
                    SimStep(i, Population[j]);
                }
                int NbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male
             && x.IsAlive
                                  select x).Count();
                int NBOfFemales = (from x in Population
                                   where x.Gender == Gender.Female
             && x.IsAlive
                                   select x).Count();
                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", i, NbrOfMales, NBOfFemales));
                NbrOfMalesInYears.Add(NbrOfMales);
                NbrOfFemalesInYears.Add(NBOfFemales);
            }
        }

        public List<Person> GetPopulation(string csvpath){
            List<Person> population = new List<Person>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthDate = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }
        public List<BirthProb> GetBirthProb(string csvpath)
        {
            List<BirthProb> birthProbs = new List<BirthProb>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbs.Add(new BirthProb()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }
            return birthProbs;
        }
        public List<DeathProb> GetDeathProb(string csvpath)
        {
            List<DeathProb> deathProbs = new List<DeathProb>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbs.Add(new DeathProb()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }
            return deathProbs;
        }
        private void SimStep(int year, Person person) {
            if (!person.IsAlive) return;
            int age = (int)(year - person.BirthDate);
            double pDeath = (from x in DeathProbs
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            if (rng.NextDouble() <= pDeath) person.IsAlive = false;
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbs
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                if (rng.NextDouble() <= pBirth)
                {
                    Person ujszulott = new Person();
                    ujszulott.BirthDate = year;
                    ujszulott.NbrOfChildren = 0;
                    ujszulott.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(ujszulott);
                }
            }
        }

        private void DisplayResults() {
            for (int years = 2005; years < numericUpDown1.Value; years++)
            {
                richTextBox1.Text += ("Szimulációs év: {0} \n \t Fiúk: {1} \n \t " +
                                      "Lányok: {2} \n \n", years, NbrOfMalesInYears[years-2005],
                                       NbrOfFemalesInYears[years-2005]);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files (*.csv)|*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NbrOfMalesInYears.Clear();
            NbrOfFemalesInYears.Clear();
            richTextBox1.Clear();
            Simulation();
            DisplayResults();
        }
    }
}

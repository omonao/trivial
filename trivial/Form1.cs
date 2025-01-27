﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using InoueLab;

namespace trivial {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            Console.SetOut(new ConsoleWindow(this, listBox1));
        }

        private void button1_Click(object sender, EventArgs e) {
            backgroundWorker1.DoWork += (_s, _e) => {
                Console.WriteLine("start");
                string dirName = "snpPerGene";
                var fnames = Directory.GetFiles(dirName, "*.report");
                var lines = fnames.Select(fname => File.ReadLines(fname));
                var geneNameCounts = fnames.Select(fname => { 
                    var _data = File.ReadLines(fname).Where(line => line.Length > 10 && !line.Contains("DIST"));
                    int length = _data.Count();
                    var names = _data.Where(line => line[0] != ' ').Select(line=>line.Split(':').First());
                    var __data = _data.Select((line, i) => new { line, i }).Where(x => x.line[0] != ' ').Select(x => x.i).Concat(new int[] { length });
                    var _nums = __data.Skip(1);
                    return new { fname, dic= names.Zip(_nums.Zip(__data, (a, b) => a - b), (name, count) => new { name, count }).ToDictionary(x => x.name, x => x.count) };
                }).ToArray();
                var _fullNames = geneNameCounts.First().dic.Keys.Union(geneNameCounts.Skip(1).First().dic.Keys);
                foreach (var item in geneNameCounts.Skip(2)) {
                    _fullNames = _fullNames.Union(item.dic.Keys);
                }
                var fullNames = _fullNames.ToArray();
                var outName = dirName + "\\result.txt";
                File.WriteAllLines(outName,new string[] { "chr\tgene\t" + geneNameCounts.Select(data => data.fname.Split('\\').Last().Split("_plink").First()).Join("\t") });
                File.AppendAllLines(outName, fullNames.Select(name =>
                      name.Split(' ').Last()+"\t"+ name.Split(' ').First()+"\t"+ geneNameCounts.Select(data => data.dic[name].ToString()).Join("\t")
                ));
                Console.WriteLine("finish");
            };
            backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e) {
            Form x = new Form2();
            x.Show();
        }
    }
}

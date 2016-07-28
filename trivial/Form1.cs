using System;
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
                string dirName = "snpPerGene";
                var fnames = Directory.GetFiles(dirName, "*.report");
                var lines = fnames.Select(fname => File.ReadLines(fname));
                var geneNameCounts = lines.Select(data => {
                    var _data = data.Where(line => line.Length>10 && !line.Contains("DIST"));
                    int length = _data.Count();
                    var names = _data.Where(line => line[0] != ' ');
                    var __data = _data.Select((line, i) => new { line, i }).Where(x => x.line[0] != ' ').Select(x=>x.i).Concat(new int[] { length });
                    var _nums = __data.Skip(1);
                    return names.Zip( _nums.Zip(__data, (a, b) => a - b),(name,count)=>new { name, count }).OrderBy(x=>x.name);
                });

            };
            backgroundWorker1.RunWorkerAsync();
        }
    }
}

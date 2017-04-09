using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.Statistics.Kernels;
using Accord.Imaging.Converters;
using System.IO;


namespace car
{
    public partial class Form1 : Form
    {
        public Bitmap original;
        HistogramsOfOrientedGradients hog = new HistogramsOfOrientedGradients();
        public MulticlassSupportVectorMachine<DynamicTimeWarping>  svm;
        public List<double[]> test;
        public string path = @"C:\Users\Rahul\Documents\Visual Studio 2015\Projects\car\car\front of car";
        public string path1 = @"C:\Users\Rahul\Documents\Visual Studio 2015\Projects\car\car\negative";
        public string path2 = @"C:\Users\Rahul\Documents\Visual Studio 2015\Projects\car\car\test";
        public string[] filePaths;
        //=Directory.GetFiles(@"C:\Users\Rahul\Documents\Visual Studio 2015\Projects\car\car", "*.jpg");
        public string[] filepaths1;
        public string[] filepaths2;
        //public MulticlassSupportVectorMachine<DynamicTimeWarping> teacher = new MulticlassSupportVectorMachine();
        public object dataGridImage;
        public List<double[]> array = new List<double[]>();
        public List<double[]> testarray = new List<double[]>();
        public double[][] array1;
        public double[][] array2;
        int[] z;
        int[] p;
        int[] q;
        public static Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            return (Bitmap)(new Bitmap(imgToResize, size));
        }
        public Form1()
        {
            InitializeComponent();
            filePaths  = Directory.GetFiles(path, "*.jpg",SearchOption.TopDirectoryOnly);
            filepaths1 = Directory.GetFiles(path1, "*.jpg", SearchOption.TopDirectoryOnly);
            filepaths2 = Directory.GetFiles(path2, "*.jpg", SearchOption.TopDirectoryOnly);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            {

                OpenFileDialog openDialog = new OpenFileDialog();
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    original = new Bitmap(openDialog.FileName);
                    original = resizeImage(original, new Size(480, 270));
                    //pictureBox1.Image = original;
                }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void hOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> y = new List<int>();
            svm = new MulticlassSupportVectorMachine<DynamicTimeWarping>(inputs: 0, kernel: new DynamicTimeWarping(2), classes: 2);
            foreach (var i in filePaths)
            {
                Bitmap carImage = new Bitmap(i);
                carImage = resizeImage(carImage, new Size(480, 270));
                //System.IO.File.Copy(i, @"C:\Users\Rahul\Documents\Visual Studio 2015\Projects\car\car\test" + Path.GetFileName(i));
                //carImage.Save(@"C:\Users\Rahul\Documents\Visual Studio 2015\Projects\car\car\test\" + Path.GetFileName(i));
                List<double[]> histogram = hog.ProcessImage(carImage);
                List<double> H = new List<double>();
                foreach (var histo in histogram)
                {
                    H.AddRange(histo);
                }
                array.Add(H.ToArray<double>());
                y.Add(3);
            }
            foreach (var i in filepaths1)
            {
                Bitmap noImage = new Bitmap(i);
                noImage = resizeImage(noImage, new Size(480, 270));
                List<double[]> histogram1 = hog.ProcessImage(noImage);
                List<double> H1 = new List<double>();
                foreach (var histo in histogram1)
                {
                    H1.AddRange(histo);
                }
                array.Add(H1.ToArray<double>());
                y.Add(1);
            }

            z = y.ToArray();
            array1 = array.ToArray<double[]>();
            //Accord.Imaging.Converters.IConverter < original,List<double[]> test>;
            var teacher = new MulticlassSupportVectorLearning<DynamicTimeWarping>();
            svm = teacher.Learn(array1, z);
            //this.svm = teacher.Learn(original, histogram);
            //SupportVectorMachine svm = new SupportVectorMachine(390);
            //MulticlassSupportVectorMachine svm = new MulticlassSupportVectorMachine(390, 2); 
            foreach(var testimg in filepaths2)
            {
                Bitmap testImage = new Bitmap(testimg);
                testImage = resizeImage(testImage, new Size(480, 270));
                List<double[]> histogram2 = hog.ProcessImage(testImage);
                List<double> H2 = new List<double>();
                foreach (var histo in histogram2)
                {
                    H2.AddRange(histo);
                }
                testarray.Add(H2.ToArray<double>());
                array2 = testarray.ToArray<double[]>();
                p = svm.Decide(array2);
            }
            
        } 


    }
}
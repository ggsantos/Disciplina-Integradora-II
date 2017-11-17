using Accord.Imaging;
using Accord.MachineLearning;
using Accord.Statistics.Distributions.DensityKernels;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using CrossLib;
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

namespace Sidewalk
{
    public partial class FormFeatures : Form
    {
        private int fotoNumber;
        private Bitmap imgOriginal;

        public FormFeatures()
        {
            InitializeComponent();
            fotoNumber = 0;
        }

        private void FormFeatures_Load(object sender, EventArgs e)
        {
            buttonLoadImage_Click(null, null);
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            string[] filePaths = Directory.GetFiles(textBoxPasta.Text, txtFile2.Text);

            if (filePaths.Length == 0) return;

            if (fotoNumber >= filePaths.Length) fotoNumber = 0;

            imgOriginal = new Bitmap(filePaths[fotoNumber]);

            pictureBoxOriginal.Image = imgOriginal;

            fotoNumber++;
        }

        private void buttonHarrisCornersDetector_Click(object sender, EventArgs e)
        {
            // Create a new Harris Corners Detector using the given parameters
            HarrisCornersDetector harris = new HarrisCornersDetector();

            // Create a new AForge's Corner Marker Filter
            CornersMarker corners = new CornersMarker(harris, Color.Green);

            // Apply the filter and display it on a picturebox
            pictureBoxResult.Image = corners.Apply(imgOriginal);
        }

        private void buttonMCO_Click(object sender, EventArgs e)
        {
            string arquivo = @"C:\Users\Dragleer\Documents\Side Walk\Fotos\video1_1159.png";

            Bitmap bitmap = new Bitmap(arquivo);
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);

            var matrix = new Accord.Imaging.GrayLevelCooccurrenceMatrix();

            matrix.Degree = CooccurrenceDegree.Degree0;
            matrix.Distance = 1;
            matrix.Normalize = true;

            double[,] resp = matrix.Compute(unmanagedImage);

            int x = matrix.Pairs;

        }

        private void buttonLocalBinaryPattern_Click(object sender, EventArgs e)
        {

            string arquivo = @"C:\Users\Dragleer\Documents\Side Walk\Fotos\video1_1159.png";

            Bitmap bitmap = new Bitmap(arquivo);
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);

            var lbp = new Accord.Imaging.LocalBinaryPattern();

            List<Double[]> x = lbp.ProcessImage(unmanagedImage);

            var x1 = lbp.BlockSize;
            var x2 = lbp.CellSize;
            var x3 = lbp.Histograms;
            var x4 = lbp.Normalize;
            var x5 = lbp.Patterns;


        }

        private void buttonHOG_Click(object sender, EventArgs e)
        {

            string arquivo = @"C:\Users\Dragleer\Documents\Side Walk\Fotos\video1_1159.png";

            Bitmap bitmap = new Bitmap(arquivo);
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);

            var hog = new Accord.Imaging.HistogramsOfOrientedGradients();
            List<Double[]> x = hog.ProcessImage(unmanagedImage);

        }

        private void buttonHaralick_Click(object sender, EventArgs e)
        {
            string arquivo = @"C:\Users\Dragleer\Documents\Side Walk\Fotos\video1_1159.png";

            Bitmap bitmap = new Bitmap(arquivo);
            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(bitmap);


            var haralick = new Accord.Imaging.Haralick();

            //IFeatureDetector<FeatureDescriptor, Double[]> x = haralick.ProcessImage(unmanagedImage);
            List<Double[]> x = haralick.ProcessImage(unmanagedImage);

            var x1 = haralick.CellSize;
            var x2 = haralick.Degrees;
            var x3 = haralick.Descriptors;
            var x4 = haralick.Features;
            var x5 = haralick.Matrix;
            var x6 = haralick.Mode;
            var x7 = haralick.Normalize;



        }

        public class dados
        {
            public int x;
            public int y;
            public Bitmap img;
            public List<double[]> features;
        }


        private void buttonProcessa_Click(object sender, EventArgs e)
        {
            Bitmap imagem = (Bitmap)imgOriginal.Clone();
            int width = imagem.Width;
            int height = imagem.Height;
            int xstep = width / 5;
            int ystep = height / 10;

            quadricula(imagem, xstep, ystep);

            List<dados> lista = new List<dados>();
            //double[][] observations = getFeaturesHaralick_01(width, height, xstep, ystep, imagem, lista);
            double[][] observations = getFeaturesLBP_01(width, height, xstep, ystep, imagem, lista, 2);


            bool lKmean = false;
            int classes = 2;

            for (int featureNumber = 0; featureNumber < observations[0].GetLength(0); featureNumber++)
            {

                double[][] obs2 = new double[observations.GetLength(0)][];
                for (int iObs = 0; iObs < observations.GetLength(0); iObs++)
                {
                    obs2[iObs] = new double[1];
                    obs2[iObs][0] = observations[iObs][featureNumber];
                }

                int[] labels;
                if (lKmean)
                {
                    KMeans kmeans = new KMeans(classes);
                    labels = kmeans.Compute(obs2);
                }
                else
                {
                    BinarySplit gmm = new BinarySplit(classes);
                    labels = gmm.Compute(obs2);
                }

                paintLabels(width, height, xstep, ystep, labels, lista);

                pictureBoxResult.Image.Save(@"C:\Users\Dragleer\Documents\Side Walk\Fotos Features\img_" + featureNumber + ".jpg");
            }


            int[] labels2;
            if (lKmean)
            {
                KMeans kmeans = new KMeans(classes);
                labels2 = kmeans.Compute(observations);
            }
            else
            {
                BinarySplit gmm = new BinarySplit(classes);
                labels2 = gmm.Compute(observations);
            }

            paintLabels(width, height, xstep, ystep, labels2, lista);

            pictureBoxResult.Image.Save(@"C:\Users\Dragleer\Documents\Side Walk\Fotos Features\img_tudo.jpg");


        }

        private List<double[]> LbpGetFeature(Bitmap imgS, int opcao)
        {
            List<double[]> lst = new List<double[]>();


            if (opcao == 1)
            {

                ImageMCO img = new ImageMCO(imgS);
                img.MCO_Size = 2;
                img.AddTextureUnit();
                img.Run();

                double[] lst1 = new double[7];
                lst1[0] = img.LBPBWS;
                lst1[1] = img.LBPGS;
                lst1[2] = img.LBPDD;
                lst1[3] = img.Assimetria;
                lst1[4] = img.Curtose;
                lst1[5] = img.Media;
                lst1[6] = img.Variancia;

                lst.Add(lst1);
            }
            else
            {
                ImageMCO img = new ImageMCO(imgS);
                img.calcMCO = false;
                img.calcLBP = false;
                img.Run();

                double[] lst1 = new double[2];
                lst1[0] = img.Assimetria;
                lst1[1] = img.Curtose;
                lst.Add(lst1);

            }

            return lst;
        }

        private double[][] getFeaturesLBP_01(int width, int height, int xstep, int ystep, Bitmap imagem, List<dados> lista, int opcao)
        {

            for (int x = 0; x < width; x = x + xstep)
            {
                for (int y = 0; y < height; y = y + ystep)
                {

                    Crop filter = new Crop(new Rectangle(x, y, xstep, ystep));

                    dados d = new dados();

                    d.x = x;
                    d.y = y;
                    d.img = filter.Apply(imagem);
                    d.features = LbpGetFeature(d.img, opcao);

                    lista.Add(d);

                }
            }

            double[][] observations = new double[lista.Count][];

            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].features.Count > 0)
                    {
                        var p = lista[i].features[0].Length;

                        observations[i] = new double[p];
                        for (int j = 0; j < p; j++)
                        {
                            observations[i][j] = lista[i].features[0][j];
                        }
                    }
                }
            }
            return observations;

        }

        private double[][] getFeaturesHaralick_01(int width, int height, int xstep, int ystep, Bitmap imagem, List<dados> lista)
        {
            Haralick haralick = new Haralick();
            haralick.Mode = HaralickMode.NormalizedAverage;

            for (int x = 0; x < width; x = x + xstep)
            {
                for (int y = 0; y < height; y = y + ystep)
                {

                    Crop filter = new Crop(new Rectangle(x, y, xstep, ystep));

                    dados d = new dados();

                    d.x = x;
                    d.y = y;
                    d.img = filter.Apply(imagem);
                    d.features = haralick.ProcessImage(d.img);

                    lista.Add(d);

                }
            }

            double[][] observations = new double[lista.Count][];

            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].features.Count > 0)
                    {
                        var p = lista[i].features[0].Length;

                        observations[i] = new double[p];
                        for (int j = 0; j < p; j++)
                        {
                            observations[i][j] = lista[i].features[0][j];
                        }
                    }
                }
            }
            return observations;
        }

        private void paintLabels(int width, int height, int xstep, int ystep, int[] labels, List<dados> lista)
        {
            List<SolidBrush> pen = new List<SolidBrush>();
            pen.Add(new SolidBrush(Color.Tomato));
            pen.Add(new SolidBrush(Color.Violet));
            pen.Add(new SolidBrush(Color.Yellow));
            pen.Add(new SolidBrush(Color.Green));

            Bitmap final = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(final))
            {
                g.Clear(Color.White);

                for (int i = 0; i < lista.Count; i++)
                {
                    g.FillRectangle(pen[labels[i]], lista[i].x, lista[i].y, xstep - 1, ystep - 1);
                }

            }

            pictureBoxResult.Image = final;
        }

        private void quadricula(Bitmap imagem, int xstep, int ystep)
        {
            int width = imagem.Width;
            int height = imagem.Height;

            for (int x = 0; x < width; x = x + xstep)
            {
                using (Graphics g = Graphics.FromImage(imagem))
                {
                    g.DrawLine(new Pen(Color.White, 10), new Point(x, 0), new Point(x, height - 1));
                }
            }

            for (int y = 0; y < height; y = y + ystep)
            {
                using (Graphics g = Graphics.FromImage(imagem))
                {
                    g.DrawLine(new Pen(Color.White, 10), new Point(0, y), new Point(width - 1, y));
                }
            }

            pictureBoxOriginal.Image = imagem;

        }

        private void buttonProcessaCor_Click(object sender, EventArgs e)
        {

            Bitmap imagem = (Bitmap)imgOriginal.Clone();
            int width = imagem.Width;
            int height = imagem.Height;
            int xstep = width / 3;
            int ystep = height / 4;

            Bitmap final = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(final))
            {

                int bloco = 0;
                double cb = 0;
                double cr = 0;

                for (int x = 0; x < width; x = x + xstep)
                {
                    for (int y = 0; y < height; y = y + ystep)
                    {

                        if (bloco == 7)
                        {
                            Crop filter = new Crop(new Rectangle(x, y, xstep, ystep));
                            Bitmap img = filter.Apply(imagem);

                            MyImageStatisticsYCbCr stat2 = new MyImageStatisticsYCbCr(img);
                            cb = stat2.Cb.Mean;
                            cr = stat2.Cr.Mean;

                        }

                        bloco++;

                    }
                }


                g.Clear(Color.White);

                bloco = 0;
                for (int x = 0; x < width; x = x + xstep)
                {
                    for (int y = 0; y < height; y = y + ystep)
                    {

                        Crop filter = new Crop(new Rectangle(x, y, xstep, ystep));

                        Bitmap img = filter.Apply(imagem);

                        // gather statistics
                        ImageStatistics stat = new ImageStatistics(img);

                        MyImageStatisticsYCbCr stat2 = new MyImageStatisticsYCbCr(img);

                        // int gray = (int)(stat.Red.Mean + stat.Green.Mean + stat.Blue.Mean) / 3;
                        // int gray2 = (int)(stat.RedWithoutBlack.Mean + stat.GreenWithoutBlack.Mean + stat.BlueWithoutBlack.Mean) / 3; 


                        //var pen = new SolidBrush(Color.FromArgb((int)stat.Red.Mean, (int)stat.Green.Mean, (int)stat.Blue.Mean));

                        var pen = new SolidBrush(Color.FromArgb((int)stat.Red.Mean, (int)stat.Green.Mean, (int)stat.Blue.Mean));

                        g.FillRectangle(pen, x, y, xstep - 1, ystep - 1);


                        g.DrawString("Cb = " + String.Format("{0:0.000000}", stat2.Cb.Mean) + "\n" +
                                     "Cr = " + String.Format("{0:0.000000}", stat2.Cr.Mean) + "\n" +
                                     (bloco == 7 ? "\nUSUARIO" : "") +
                                     (bloco != 7 ? "Dif Cb = " + String.Format("{0:0.000000}", (cb - stat2.Cb.Mean)) + "\n" : "") +
                                     (bloco != 7 ? "Dif Cr = " + String.Format("{0:0.000000}", (cr - stat2.Cr.Mean)) : "")

                                     , new Font("courier", 40), new SolidBrush(Color.White), new Rectangle(x, y, xstep, ystep));


                        bloco++;

                    }
                }

            }

            pictureBoxResult.Image = final;

            quadricula(imagem, xstep, ystep);
        }

        private void buttonLoadImageNew_Click(object sender, EventArgs e)
        {
            string[] filePaths = Directory.GetFiles(textBoxPasta.Text, txtFile2.Text);

            if (filePaths.Length == 0) return;

            fotoNumber = 0;

            imgOriginal = new Bitmap(filePaths[fotoNumber]);

            pictureBoxOriginal.Image = imgOriginal;

            fotoNumber++;
        }

        

    }
}

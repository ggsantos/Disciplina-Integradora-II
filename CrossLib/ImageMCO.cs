using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrossLib
{

    public class ImageMCO
    {
        //propriedades
        public int MCO_Size { get; set; }
        public bool MCO_Double { get; set; }

        //Unidade de Textura
        public double[][] LBPHistogram;
        public double LBPBWS;
        public double LBPGS;
        public double LBPDD;


        //Matrizes da Imagem
        public Byte[][] Matrix;
        public Byte[][] MatrixNormalized;
        public Byte[][] MatrixOriginal;

        public int LinesCount;
        public int ColsCount;
        public int Count;

        //Medidas
        public double Media;
        public double Variancia;
        public double Assimetria;
        public double Curtose;

        //MCOs
        public List<MCO_Proper> MCOs;


        //privates
        private Bitmap Source;

        private int minByte;
        private int maxByte;


        //-------------------------------------------------------------------------------------
        public ImageMCO(Bitmap image)
        {
            Default(image);

        }

        //-------------------------------------------------------------------------------------
        public ImageMCO(string imageFile)
        {
            Default(Imaging.LoadImage(imageFile));

        }

        //-------------------------------------------------------------------------------------
        private void Default(Bitmap image)
        {
            Source = image;

            if (!(Source.PixelFormat == PixelFormat.Format8bppIndexed))
            {
                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                Source = filter.Apply(Source);

                //FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();
                // add filters to the sequence
                //filter.Add(new Grayscale(0.299, 0.587, 0.114));
                //filter.Add(new SobelEdgeDetector());
                // apply the filter sequence
                //Source = filter.Apply(Source);
            }

            //default
            MCO_Size = 255;
            MCO_Double = true;

            MCOs = new List<MCO_Proper>();

        }

        //-------------------------------------------------------------------------------------
        public void Run()
        {
            if (MCO_Double)
            {
                if (Source.Width != Source.Height)
                {
                    MCO_Double = false;
                }
            }

            GetMatrix();

            if (calcMCO)
            {
                ProcessMCOs();
                CalcRelations();
            }

            if (calcLBP)
            {
                MakeLBP();
            }

        }

        //-------------------------------------------------------------------------------------
        int UPos(int pos, int grau)
        {
            pos += grau;
            if (pos > 8)
            {
                pos = pos - 8;
            }
            return pos;
        }


        //-------------------------------------------------------------------------------------
        // http://www.jakemdrew.com/blog/RgbProjector.htm
        //https://svnserv.csiro.au/svn/Spring/pip/tags/start/src/PIP_texture.c
        void MakeLBP()
        {
            if (calcLBP)
            {
                LBPHistogram = new double[8][];
                for (int i = 0; i < 8; i++)
                {
                    LBPHistogram[i] = new double[256];
                    for (int j = 0; j < 256; j++)
                    {
                        LBPHistogram[i][j] = 0;
                    }
                }

                //Carrega Unidades de Textura
                for (int i = 1; i < ColsCount - 1; i++)
                {
                    for (int j = 1; j < LinesCount - 1; j++)
                    {
                        int[] U = new int[9];

                        if (false)
                        {
                            U[0] = MatrixOriginal[i][j];

                            U[1] = MatrixOriginal[i - 1][j - 1];
                            U[2] = MatrixOriginal[i - 1][j];
                            U[3] = MatrixOriginal[i - 1][j + 1];

                            U[8] = MatrixOriginal[i][j - 1];
                            U[4] = MatrixOriginal[i][j + 1];

                            U[7] = MatrixOriginal[i + 1][j - 1];
                            U[6] = MatrixOriginal[i + 1][j];
                            U[5] = MatrixOriginal[i + 1][j + 1];
                        }
                        else
                        {
                            U[0] = MatrixNormalized[i][j];

                            U[1] = MatrixNormalized[i - 1][j - 1];
                            U[2] = MatrixNormalized[i - 1][j];
                            U[3] = MatrixNormalized[i - 1][j + 1];

                            U[8] = MatrixNormalized[i][j - 1];
                            U[4] = MatrixNormalized[i][j + 1];

                            U[7] = MatrixNormalized[i + 1][j - 1];
                            U[6] = MatrixNormalized[i + 1][j];
                            U[5] = MatrixNormalized[i + 1][j + 1];

                        }

                        for (int g = 0; g < 8; g++)
                        {
                            int uLBP = 0;

                            uLBP  += (U[UPos(1, g)] < U[0] ? 0 : 1);
                            uLBP  += (U[UPos(2, g)] < U[0] ? 0 : 2);
                            uLBP  += (U[UPos(3, g)] < U[0] ? 0 : 4);
                            uLBP  += (U[UPos(4, g)] < U[0] ? 0 : 8);
                            uLBP  += (U[UPos(5, g)] < U[0] ? 0 : 16);
                            uLBP  += (U[UPos(6, g)] < U[0] ? 0 : 32);
                            uLBP  += (U[UPos(7, g)] < U[0] ? 0 : 64);
                            uLBP  += (U[UPos(8, g)] < U[0] ? 0 : 128);

                            LBPHistogram[g][uLBP]++;
                        }

                    }
                }

                // normalize & quantize histogram.
                for (int j = 0; j < 8; j++)
                {
                    double max = 0;
                    for (int i = 0; i < 256; i++)
                    {
                        max = Math.Max(LBPHistogram[j][i], max);
                    }
                    for (int i = 0; i < 256; i++)
                    {
                        LBPHistogram[j][i] = Math.Floor((LBPHistogram[j][i] / max) * 100);
                    }
                }

                //Calcula BWS - Black-White Symmetry 
                LBPBWS = 0;
                double sum = 0;

                double x = 0;
                double y = 0;

                for (int i = 0; i < 128; i++)
                {
                    x += LBPHistogram[0][i];
                    y += LBPHistogram[0][127 + i];
                    LBPBWS += LBPHistogram[0][i] - LBPHistogram[0][127 + i];
                    sum += LBPHistogram[0][i] + LBPHistogram[0][127 + i];
                }
                if (sum > 0)
                {
                    LBPBWS = (1 - (LBPBWS / sum)) * 100;
                }
                else
                {
                    LBPBWS = 0;
                }

                //Calcula GS - Geometric Symmetry
                LBPGS = 0;
                for (int j = 0; j < 4; j++)
                {
                    double tmp = 0;
                    double sum2 = 0;
                    for (int i = 0; i < 256; i++)
                    {
                        tmp += Math.Abs(LBPHistogram[j][i] - LBPHistogram[j + 4][i]);
                        sum2 += LBPHistogram[j][i];
                    }
                    LBPGS += tmp / (2.0 * sum2);
                }
                LBPGS = (1.0 - LBPGS / 4.0) * 100;

                //Calcula DD - Degree of Direction 
                LBPDD = 0;
                for (int m = 0; m < 3; m++)
                {
                    for (int n = m + 1; n < 4; n++)
                    {
                        double tmp = 0;
                        double sum3 = 0;
                        for (int i = 0; i < 256; i++)
                        {
                            tmp += Math.Abs(LBPHistogram[m][i] - LBPHistogram[n][i]);
                            sum3 += LBPHistogram[m][i];
                        }
                        LBPDD += tmp / (2.0 * sum3);
                    }
                }
                LBPDD = (1 - (LBPDD / 6)) * 100;

                //Calcula Orientational features 

            }

        }

        //-------------------------------------------------------------------------------------
        void CalcRelations()
        {
            foreach (var item in MCOs)
            {
                item.Energy = 0;
                item.Entropy = 0;
                item.Contrast = 0;
                item.Homogeneity = 0;

                for (int i = 0; i < MCO_Size; i++)
                {
                    for (int j = 0; j < MCO_Size; j++)
                    {
                        if (item.MCO[i][j] > 0)
                        {

                            item.Energy += Math.Pow(item.MCO_Normalized[i][j], 2);

                            item.Entropy += item.MCO[i][j] * Math.Log(item.MCO[i][j], 2);

                            item.Contrast += Math.Pow((i - j), 2) * item.MCO_Normalized[i][j];

                            item.Homogeneity += item.MCO_Normalized[i][j] / (1 + Math.Pow((i - j), 2));
                        }
                    }
                }

                item.Entropy = item.Entropy * -1;
            }


        }


        //-------------------------------------------------------------------------------------
        void ProcessMCOs()
        {
            //Cria Tabelas
            foreach (var item in MCOs)
            {
                item.MCO = McoModel();
                item.MCO_Normalized = McoModel();
            }

            //Carrega MCOs
            for (int i = 0; i < ColsCount ; i++)
            {
                for (int j = 0; j < LinesCount; j++)
                {
                    foreach (var item in MCOs)
                    {
                        int i2 = i;
                        int j2 = j;

                        if (item.Angle == 0)
                            j2 += item.Distance;
                        else if (item.Angle == 45)
                        {
                            i2 -= item.Distance;
                            j2 += item.Distance;
                        }
                        else if (item.Angle == 90)
                            i2 += item.Distance;
                        else if (item.Angle == 135)
                        {
                            i2 += item.Distance;
                            j2 += item.Distance;
                        }

                        if (i2 >= 0 && j2 >= 0 && i2 < LinesCount && j2 < ColsCount)
                        {
                            
                            int i1 = Matrix[i][j];
                            int j1 = Matrix[i2][j2];
                            item.MCO[i1][j1]++;

                            item.total++;

                            if (MCO_Double)
                            {
                                item.MCO[j1][i1]++;
                                item.total++;
                            }
                        }
                    }
                }
            }

            //Normaliza MCOs
            foreach (var item in MCOs)
            {
                for (int i = 0; i < MCO_Size; i++)
                {
                    for (int j = 0; j < MCO_Size; j++)
                    {
                        item.MCO_Normalized[i][j] = item.MCO[i][j] / item.total;
                    }
                }
            }


        }


        //-------------------------------------------------------------------------------------
        double[][] McoModel()
        {
            double[][] __McoModel = new double[MCO_Size][];
            for (int i = 0; i < MCO_Size; ++i)
            {
                __McoModel[i] = new double[MCO_Size];
                for (int j = 0; j < MCO_Size; j++)
                {
                    __McoModel[i][j] = 0;
                }
            }
            return __McoModel;
        }


        //-------------------------------------------------------------------------------------
        private void GetMatrix()
        {
            BitmapData data = Source.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), ImageLockMode.ReadOnly, Source.PixelFormat);

            try
            {

                IntPtr ptr = data.Scan0;
                int qtd = Math.Abs(data.Stride) * Source.Height;
                byte[] pixels = new byte[qtd];
                Marshal.Copy(ptr, pixels, 0, qtd);

                // transforma em Matrix
                byte[][] linhas = new byte[Source.Height][];
                for (int y = 0; y < Source.Height; y++)
                {
                    byte[] colunas = new byte[data.Stride];
                    Array.Copy(pixels, y * data.Stride, colunas, 0, data.Stride);

                    linhas[y] = colunas;
                }

                // tamanhos
                LinesCount = data.Stride - 2;
                ColsCount = Source.Height;
                Count = ColsCount * LinesCount;

                // busca valores minimos e máximos para poder equalizar depois o histograma
                minByte = 255;
                maxByte = 0;

                //Ajusta a matriz
                MatrixOriginal = new byte[ColsCount][];
                MatrixNormalized = new byte[ColsCount][];
                Matrix = new byte[ColsCount][];

                for (int x = 0; x < ColsCount; x++)
                {
                    MatrixOriginal[x] = new byte[LinesCount];
                    MatrixNormalized[x] = new byte[LinesCount];
                    Matrix[x] = new byte[LinesCount];

                    for (int y = 0; y < LinesCount; y++)
                    {
                        MatrixOriginal[x][y] = linhas[x][y];

                        minByte = Math.Min(MatrixOriginal[x][y], minByte);
                        maxByte = Math.Max(MatrixOriginal[x][y], maxByte);
                    }
                }



                //Medidas
                Media = 0;
                Variancia = 0;
                Assimetria = 0;
                Curtose = 0;

                if ((maxByte - minByte) == 0) 
                {
                    maxByte = 1;
                }

                //Equaliza Matriz e Cria Matriz de Tons de Cinza
                double div = (256 / MCO_Size) ;
                for (int x = 0; x < ColsCount ; x++)
                {
                    for (int y = 0; y < LinesCount; y++)
                    {
                        MatrixNormalized[x][y] = (byte)(int)((MatrixOriginal[x][y] - minByte) * 255 / (maxByte - minByte));
                        Matrix[x][y] = (byte)Math.Min((int)((MatrixNormalized[x][y] / div)), MCO_Size-1);

                        //media
                        Media += MatrixNormalized[x][y];
                    }
                }
                Media = Media / Count;

                //Variancia
                for (int x = 0; x < ColsCount; x++)
                {
                    for (int y = 0; y < LinesCount; y++)
                    {
                        Variancia += Math.Pow((MatrixNormalized[x][y] - Media), 2);
                    }
                }
                Variancia = Math.Sqrt(Variancia / Count);

                //Assimetria
                for (int x = 0; x < ColsCount; x++)
                {
                    for (int y = 0; y < LinesCount; y++)
                    {
                        Assimetria += Math.Pow((MatrixNormalized[x][y] - Media), 3);
                    }
                }
                Assimetria = Assimetria / (Count * Math.Pow(Variancia, 3));


                //Curtose
                for (int x = 0; x < ColsCount; x++)
                {
                    for (int y = 0; y < LinesCount; y++)
                    {
                        Curtose += Math.Pow((MatrixNormalized[x][y] - Media), 4);
                    }
                }
                Curtose = (Curtose / (Count * Math.Pow(Variancia, 4))) - 3;

            }
            finally
            {
                Source.UnlockBits(data);
            }


        }

        //-------------------------------------------------------------------------------------
        public void AddMCO(int angle, int distance)
        {
            MCOs.Add(new MCO_Proper(angle, distance));
        }

        public void AddTextureUnit()
        {
            calcLBP = true;
        }

        public bool calcLBP { get; set; }

        public bool calcMCO = true;
    }

    public class MCO_Proper
    {
        public int Angle;
        public int Distance;

        public int total;

        public double[][] MCO;
        public double[][] MCO_Normalized;

        public MCO_Proper(int angle, int distance)
        {
            this.Angle = angle;
            this.Distance = distance;
            this.total = 0;
        }

        public double Energy { get; set; }
        public double Entropy { get; set; }
        public double Contrast { get; set; }
        public double Homogeneity { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Formats;
using AForge.Imaging.Filters;
using System.Runtime.InteropServices;

namespace CrossLib
{
    public class Imaging
    {

        private FiltersSequence filterSequenceAS;
        private FilterIterator filterIteratorAS;

        private ChannelFiltering filterColorAS;

        private FiltersSequence filterSequenceASroad;
        private FilterIterator filterIteratorASroad;

        //-------------------------------------------------------------------------------------
        public Imaging()
        {
            //AreaSelection 

            // SATELITE
            // create filters sequence
            filterSequenceAS = new AForge.Imaging.Filters.FiltersSequence();

            // create filter
            filterSequenceAS.Add(new ExtractChannel(RGB.R));
            filterSequenceAS.Add(new Threshold(250));
            filterSequenceAS.Add(new BinaryErosion3x3());
            filterSequenceAS.Add(new BinaryErosion3x3());

            // create filter iterator for 10 iterations
            filterIteratorAS = new FilterIterator(new BinaryDilatation3x3(), 35);

            // ROAD
            // create filters sequence
            filterSequenceASroad = new AForge.Imaging.Filters.FiltersSequence();

            // create filter
            filterSequenceASroad.Add(new ExtractChannel(RGB.R));
            filterSequenceASroad.Add(new Threshold(250));

            filterSequenceASroad.Add(new BinaryErosion3x3());
           // filterSequenceASroad.Add(new BinaryErosion3x3());


            filterIteratorASroad = new FilterIterator(new BinaryDilatation3x3(), 20);

            // create filter
            filterColorAS = new ChannelFiltering();
            // set channels' ranges to keep
            filterColorAS.Red = new IntRange(0, 0);
            filterColorAS.Green = new IntRange(0, 0);
            filterColorAS.Blue = new IntRange(0, 0);

        }

        //-------------------------------------------------------------------------------------
        public Bitmap RoadPrepare(Bitmap imagemRoad)
        {
            Bitmap newImageRoad = filterSequenceAS.Apply(imagemRoad);
            newImageRoad = filterIteratorAS.Apply(newImageRoad);
            return newImageRoad;
        }

        //-------------------------------------------------------------------------------------
        public Bitmap RoadPrepareCorners(Bitmap imagemRoad)
        {
            Bitmap newImageRoad = filterSequenceASroad.Apply(imagemRoad);
            newImageRoad = filterIteratorASroad.Apply(newImageRoad);

            return newImageRoad;
        }


        //-------------------------------------------------------------------------------------
        private Bitmap RetiraArvores(Bitmap imagem)
        {
            Bitmap newBitmap = new Bitmap(imagem);

            for (int x = 0; x < newBitmap.Width; x++)
            {
                for (int y = 0; y < newBitmap.Height; y++)
                {
                    Color firstPixel = newBitmap.GetPixel(x, y);
                    if (
                        firstPixel.G > 50 &&
                        firstPixel.G > firstPixel.B + 10 &&
                        firstPixel.G > firstPixel.R + 10
                        )
                    {
                        newBitmap.SetPixel(x, y, Color.Black);
                    }

                }
            }

            return newBitmap;
        }

        //-------------------------------------------------------------------------------------
        public double distanciaRetas(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        //-------------------------------------------------------------------------------------
        private void PesquisaEsquina(Byte[][] img, int X, int Y, int direcao, ref int Xout, ref int Yout)
        {
            PesquisaEsquina(img, X, Y, direcao, -1, ref Xout, ref Yout, 0, -1, 0, 0, 0);
        }

        //-------------------------------------------------------------------------------------
        private void PesquisaEsquina(Byte[][] img, int X, int Y, int direcao, int orientacao, ref int Xout, ref int Yout, int contador, double orientacaoInicial, int xContador, int yContador, int total )
        {
            total++;

            int[,] kernel = new int[8, 2];
            if (direcao == 1)
            {
                kernel = new int[8, 2] { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 } };
                //kernel = new int[8, 2] { { -2, -2 }, { 0, -2 }, { 2, -2 }, { 2, 0 }, { 2, 2 }, { 0, 2 }, { -2, 2 }, { -2, 0 } };
            }
            else
            {
                kernel = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 } };
                //kernel = new int[8, 2] { { -2, -2 }, { -2, 0 }, { -2, 2 }, { 0, 2 }, { 2, 2 }, { 2, 0 }, { 2, -2 }, { 0, -2 } };
            }

            if (X > img.Length - 5 || Y > img[0].Length - 5 || X < 5 || Y < 5)
            {
                Xout = X;
                Yout = Y;
                return;
            }

            //if (X > 372)
            //{
            //    X = X;
            //}
            int limitOrientacao = 15;

            // acha o primeiro branco
            bool achou = false;
            // acha o primeiro branco
            int posOrientacaoFB = 0;

            for (int posOrientacao = 0; posOrientacao < 8; posOrientacao++)
            {

                if (achou == false)
                {
                    if (img[X + kernel[posOrientacao, 0]][Y + kernel[posOrientacao, 1]] > 100) //branco
                    {
                        achou = true;
                        posOrientacaoFB = posOrientacao;
                    }
                }
                else // acha o primeiro preto
                {
                    //final vizinhos, 
                    if (posOrientacao == 7 && posOrientacaoFB > 0 && (img[X + kernel[posOrientacao, 0]][Y + kernel[posOrientacao, 1]] > 100))
                    {
                        for (int j = 0; j < posOrientacaoFB; j++)
                        {
                            if (img[X + kernel[j, 0]][Y + kernel[j, 1]] < 100) 
                            {
                                posOrientacao = j;
                                break;
                            }
                        }
                        posOrientacaoFB = 0;
                    }


                    if (img[X + kernel[posOrientacao, 0]][Y + kernel[posOrientacao, 1]] < 100) //preto
                    {
                        if (orientacaoInicial == -1) orientacaoInicial = posOrientacao;


                        if (orientacao != -1 && Math.Abs(orientacaoInicial - posOrientacao) >= 1 && Math.Abs(orientacaoInicial - posOrientacao) <= 6)
                        {
                            if (contador == 0)
                            {
                                xContador = X;
                                yContador = Y;
                            }
                            contador++;
                            if (contador > limitOrientacao)
                            {
                                Xout = xContador;
                                Yout = yContador;
                                return;
                            }
                        }
                        else
                        {
                            contador = 0;
                        }


                        if (total > 600)
                        {
                            Xout = X;
                            Yout = Y;
                            return;

                        } else if (orientacao != -1 && Math.Abs(orientacao - posOrientacao) > 1 && Math.Abs(orientacao - posOrientacao) < 6 && contador > limitOrientacao)
                        {
                            Xout = X;
                            Yout = Y;
                            return;
                        }
                        else
                        {

                            if (total < 10 && (Math.Abs(orientacaoInicial-posOrientacao)<=1 || Math.Abs(orientacaoInicial-posOrientacao)>=7))
                            {
                                if (Math.Abs(orientacaoInicial-posOrientacao)>=7) {
                                   orientacaoInicial = 7;
                                } else {
                                   orientacaoInicial = (posOrientacao + orientacaoInicial) /2;
                                }
                            }
                            else if (total < 15)
                            {
                                orientacaoInicial = Math.Round(orientacaoInicial, 0);
                            }


                            Xout = X + kernel[posOrientacao, 0];
                            Yout = Y + kernel[posOrientacao, 1];

                            PesquisaEsquina(img, X + kernel[posOrientacao, 0], Y + kernel[posOrientacao, 1], direcao, posOrientacao, ref  Xout, ref Yout, contador, orientacaoInicial, xContador, yContador, total);
                            return;
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------
        public void LocalizaEsquinas(Bitmap newImageRoad, ref int xDir, ref int yDir, ref int xEsq, ref int yEsq)
        {
            int xUser = (newImageRoad.Width / 2);
            int yUser = (newImageRoad.Height / 2);

            LocalizaEsquinas(newImageRoad, ref xDir, ref yDir, ref xEsq, ref yEsq, ref xUser, ref yUser);
        }


        //-------------------------------------------------------------------------------------
        public void LocalizaEsquinas(Bitmap newImageRoad, ref int xDir, ref int yDir, ref int xEsq, ref int yEsq, ref int xUser, ref int yUser)
        {
            int x = xUser;
            int y = yUser;

            Byte[][] img = toMatrizOld(newImageRoad);

            bool debug = false;
            BitmapData imageData = newImageRoad.LockBits(new Rectangle(0, 0, newImageRoad.Width, newImageRoad.Height), ImageLockMode.ReadOnly, newImageRoad.PixelFormat);

            if (debug)
            {
                Drawing.FillRectangle(imageData, new Rectangle(x, y, 10, 10), Color.Red);
            }


            int eu = img[x][y];

            int xFinal = -1;
            int yFinal = -1;

            int kernel = 200;
            double menorDist = 10000;

            //encontra aborda mais próxima
            for (int x1 = x - kernel; x1 < x + kernel; x1++)
            {
                for (int y1 = y - kernel; y1 < y + kernel; y1++)
                {
                    if ((eu > 100 && img[x1][y1] < 100) || (eu < 100 && img[x1][y1] > 100))
                    {
                        double dist = distanciaRetas(x, y, x1, y1);
                        if (dist < menorDist)
                        {
                            menorDist = dist;
                            xFinal = x1;
                            yFinal = y1;
                        }
                    }
                }
            }


            if (xFinal == -1)
            {
                return; // newImageRoad;
            }

            int borda = img[xFinal][yFinal];

            //eu quero ficar na borda preta que é a calçada
            if (borda > 100)
            {
                for (int x1 = 0; x1 < 3; x1++)
                {
                    for (int y1 = 0; y1 < 3; y1++)
                    {
                        if (img[xFinal - x1][yFinal - y1] < 100)
                        {
                            xFinal = xFinal - x1;
                            yFinal = yFinal - y1;
                            borda = img[xFinal][yFinal];

                            x1 = 10;
                            y1 = 10;
                        }
                        if (img[xFinal + x1][yFinal + y1] < 100)
                        {
                            xFinal = xFinal + x1;
                            yFinal = yFinal + y1;
                            borda = img[xFinal][yFinal];

                            x1 = 10;
                            y1 = 10;
                        }
                    }
                }
            }

            if (debug)
            {
                Drawing.FillRectangle(imageData, new Rectangle(xFinal, yFinal, 10, 10), Color.Red);
            }

            xUser = xFinal;
            yUser = yFinal;

            // Eu quero encontrar a esquina, para isso tenho que achar o meio da calçada
            xDir = -1;
            yDir = -1;
            PesquisaEsquina(img, xFinal, yFinal, 1, ref xDir, ref yDir);

            xEsq = -1;
            yEsq = -1;
            PesquisaEsquina(img, xFinal, yFinal, -1, ref xEsq, ref yEsq);


            if (debug)
            {
                Drawing.FillRectangle(imageData, new Rectangle(xDir, yDir, 10, 10), Color.Red);
                Drawing.FillRectangle(imageData, new Rectangle(xEsq, yEsq, 10, 10), Color.Red);
            }


            newImageRoad.UnlockBits(imageData);
            if (debug)
            {
                newImageRoad.Save(@"..\..\..\..\tempo\road1.bmp", ImageFormat.Bmp);
            }

        }


        //-------------------------------------------------------------------------------------
        private Bitmap invertImage(Bitmap newImageRoad)
        {
            //Lock Bits for processing
            BitmapData imageData = newImageRoad.LockBits(new Rectangle(0, 0, newImageRoad.Width, newImageRoad.Height), ImageLockMode.ReadOnly, newImageRoad.PixelFormat);
            unsafe
            {
                //Count red and black pixels
                try
                {
                    UnmanagedImage img = new UnmanagedImage(imageData);

                    int height = img.Height;
                    int width = img.Width;
                    int pixelSize = 1;
                    byte* p = (byte*)img.ImageData.ToPointer();

                    // for each line
                    for (int y = 0; y < height; y++)
                    {
                        // for each pixel
                        for (int x = 0; x < width; x++, p += pixelSize)
                        {
                            if ((int)p[0] == 0)
                            {
                                p[0] = 1;
                            }
                            else
                            {
                                p[0] = 0;
                            }
                        }
                    }
                }
                finally
                {
                    newImageRoad.UnlockBits(imageData); //Unlock
                }
            }

            return newImageRoad;
        }


        //-------------------------------------------------------------------------------------
        public Bitmap AreaSelection(Bitmap imagemRoad, Bitmap imageSatellite)
        {



            // apply the filter
            System.Drawing.Bitmap newImageRoad = filterSequenceAS.Apply(imagemRoad);

            newImageRoad.Save(@"..\..\..\..\tempo\x1.png", ImageFormat.Jpeg);

            newImageRoad = filterIteratorAS.Apply(newImageRoad);

            newImageRoad.Save(@"..\..\..\..\tempo\x2.png", ImageFormat.Jpeg);

            newImageRoad = invertImage(newImageRoad);


            // create the filter
            MaskedFilter maskedFilter = new MaskedFilter(filterColorAS, newImageRoad);
            // apply the filter
            Bitmap imagem = maskedFilter.Apply(imageSatellite);

            //Marcelo - 12/04/2015 - Não retiro mais as arvores
            //return RetiraArvores(imagem);
            return imagem;

        }

        //-------------------------------------------------------------------------------------
        public static Bitmap LoadImage(string fileName)
        {
            Bitmap image = null;
            try
            {
                ImageInfo imageInfo = null;

                image = ImageDecoder.DecodeFromFile(fileName, out imageInfo);

                BitmapData imageData = image.LockBits(
                    new Rectangle(1000, 0, image.Width - 1000, image.Height),
                    ImageLockMode.ReadWrite, image.PixelFormat);
                try
                {
                    UnmanagedImage unmanagedImage = new UnmanagedImage(imageData);
                    // apply several routines to the unmanaged image
                }
                finally
                {
                    image.UnlockBits(imageData);
                }
            }
            catch (NotSupportedException ex)
            {
                //MessageBox.Show("Image format is not supported: " + ex.Message, "Error",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                //MessageBox.Show("Invalid image: " + ex.Message, "Error",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                //MessageBox.Show("Failed loading the image", "Error",
                //   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return image;
        }


        //-------------------------------------------------------------------------------------
        public static string Imprime(double[][] vetor)
        {
            string texto = Environment.NewLine;

            for (int i = 0; i < vetor.Length; i++)
            {
                texto += i + " => ";
                for (int j = 0; j < vetor[i].Length; j++)
                {
                    texto += vetor[i][j] + ", ";
                }
                texto += Environment.NewLine;
            }
            return texto;
        }
        //-------------------------------------------------------------------------------------
        public static string Imprime(Byte[][] vetor)
        {
            string texto = "";

            for (int i = 0; i < vetor.Length; i++)
            {
                texto += i + " => ";
                for (int j = 0; j < vetor[i].Length; j++)
                {
                    texto += vetor[i][j] + ", ";
                }
                texto += Environment.NewLine;
            }
            return texto;
        }

        //-------------------------------------------------------------------------------------
        public static double[] VectorNormalizeRN(Byte[] vetor)
        {
            double[] v1 = new double[vetor.Length];

            for (int i = 0; i < vetor.Length; i++)
            {
                v1[i] = ((float)vetor[i] / 255) - 0.5f;
            }

            return v1;
        }


        //-------------------------------------------------------------------------------------
        public static Byte[] toVector(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            try
            {
                IntPtr ptr = data.Scan0;
                int qtd = Math.Abs(data.Stride) * image.Height;
                byte[] pixels = new byte[qtd];
                Marshal.Copy(ptr, pixels, 0, qtd);

                return pixels;
            }
            finally
            {
                image.UnlockBits(data);
            }


        }


        //-------------------------------------------------------------------------------------
        public static Byte[][] toMatrizOld(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            try
            {
                IntPtr ptr = data.Scan0;
                int qtd = Math.Abs(data.Stride) * image.Height;
                byte[] pixels = new byte[qtd];
                Marshal.Copy(ptr, pixels, 0, qtd);

                // transforma em Matrix
                byte[][] linhas = new byte[image.Height][];
                for (int y = 0; y < image.Height; y++)
                {
                    byte[] colunas = new byte[data.Stride];
                    Array.Copy(pixels, y * data.Stride, colunas, 0, data.Stride);

                    linhas[y] = colunas;
                }

                //intevert a matrix
                byte[][] matrix = new byte[data.Stride][];
                for (int x = 0; x < data.Stride; x++)
                {
                    matrix[x] = new byte[image.Height];

                    for (int y = 0; y < image.Height; y++)
                    {
                        matrix[x][y] = linhas[y][x];
                    }
                }


                return matrix;
            }
            finally
            {
                image.UnlockBits(data);
            }


        }


        //-------------------------------------------------------------------------------------
        public static Byte[][] toMatriz(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            try
            {
                IntPtr ptr = data.Scan0;
                int qtd = Math.Abs(data.Stride) * image.Width;
                byte[] pixels = new byte[qtd];
                Marshal.Copy(ptr, pixels, 0, qtd);

                // transforma em Matrix
                byte[][] linhas = new byte[image.Width][];
                for (int y = 0; y < image.Width; y++)
                {
                    byte[] colunas = new byte[data.Stride];
                    Array.Copy(pixels, y * data.Stride, colunas, 0, data.Stride);

                    linhas[y] = colunas;
                }

                //justa a matrix
                byte[][] matrix = new byte[data.Stride - 2][];
                for (int x = 0; x < data.Stride - 2; x++)
                {
                    matrix[x] = new byte[image.Width];

                    for (int y = 0; y < image.Width; y++)
                    {
                        matrix[x][y] = linhas[x][y];
                    }
                }


                return matrix;
            }
            finally
            {
                image.UnlockBits(data);
            }


        }




        // **********************************************************************
        public static int McoLen = 25;
        public static int McoArea = 10;

        // **********************************************************************
        public struct PointMCO
        {
            public int x;
            public int y;

            public float Energia1;
            public float Entropia1;
            public float Contraste1;
            public float Homogeneidade1;

            public float Energia2;
            public float Entropia2;
            public float Contraste2;
            public float Homogeneidade2;

            public float Energia3;
            public float Entropia3;
            public float Contraste3;
            public float Homogeneidade3;

            public float Energia4;
            public float Entropia4;
            public float Contraste4;
            public float Homogeneidade4;

            public float Energia5;
            public float Entropia5;
            public float Contraste5;
            public float Homogeneidade5;

            public float Energia6;
            public float Entropia6;
            public float Contraste6;
            public float Homogeneidade6;

        }

        // **********************************************************************
        public static PointMCO McoGetAtributos(Byte[][] array, int x, int y)
        {

            PointMCO pMco = new PointMCO();

            pMco.x = x;
            pMco.y = y;

            float[][] mco = CriaMCO(array, x, y, 1);
            pMco.Energia1 = McoEnergia(mco);
            pMco.Entropia1 = McoEntropia(mco);
            pMco.Contraste1 = McoContraste(mco);
            pMco.Homogeneidade1 = McoHomogeneidade(mco);

            mco = CriaMCO(array, x, y, 2);
            pMco.Energia2 = McoEnergia(mco);
            pMco.Entropia2 = McoEntropia(mco);
            pMco.Contraste2 = McoContraste(mco);
            pMco.Homogeneidade2 = McoHomogeneidade(mco);

            mco = CriaMCO(array, x, y, 3);
            pMco.Energia3 = McoEnergia(mco);
            pMco.Entropia3 = McoEntropia(mco);
            pMco.Contraste3 = McoContraste(mco);
            pMco.Homogeneidade3 = McoHomogeneidade(mco);

            mco = CriaMCO(array, x, y, 4);
            pMco.Energia4 = McoEnergia(mco);
            pMco.Entropia4 = McoEntropia(mco);
            pMco.Contraste4 = McoContraste(mco);
            pMco.Homogeneidade4 = McoHomogeneidade(mco);

            mco = CriaMCO(array, x, y, 5);
            pMco.Energia5 = McoEnergia(mco);
            pMco.Entropia5 = McoEntropia(mco);
            pMco.Contraste5 = McoContraste(mco);
            pMco.Homogeneidade5 = McoHomogeneidade(mco);

            mco = CriaMCO(array, x, y, 6);
            pMco.Energia6 = McoEnergia(mco);
            pMco.Entropia6 = McoEntropia(mco);
            pMco.Contraste6 = McoContraste(mco);
            pMco.Homogeneidade6 = McoHomogeneidade(mco);

            return pMco;
        }


        // **********************************************************************
        //matriz de co-ocorrência
        static float[][] CriaMCO(Byte[][] array, int x1, int y1, int direcao)
        {

            //criando matriz
            float[][] MatrixMCO = new float[McoLen][];
            for (int i = 0; i < McoLen; ++i)
            {
                MatrixMCO[i] = new float[McoLen];
                for (int j = 0; j < McoLen; j++)
                {
                    MatrixMCO[i][j] = 0;
                }
            }

            int contador = 0;
            int p1;
            int p2;



            for (int x = x1; x < Math.Min(array.Length, x1 + McoArea) - 2; x++)
            {
                for (int y = y1; y < Math.Min(array[x].Length, y1 + McoArea) - 2; y++)
                {
                    p1 = array[x][y];
                    p2 = 0;

                    if (direcao == 1)
                    {
                        p2 = array[x + 1][y];
                    }
                    else if (direcao == 2)
                    {
                        p2 = array[x][y + 1];
                    }
                    else if (direcao == 3)
                    {
                        p2 = array[x + 1][y + 1];
                    }
                    else if (direcao == 4)
                    {
                        p2 = array[x + 2][y];
                    }
                    else if (direcao == 5)
                    {
                        p2 = array[x][y + 2];
                    }
                    else if (direcao == 6)
                    {
                        p2 = array[x + 2][y + 2];
                    }

                    if (p1 > 0) p1 = p1 / McoLen;
                    if (p2 > 0) p2 = p2 / McoLen;

                    MatrixMCO[p1][p2] += 1;
                    MatrixMCO[p2][p1] += 1;

                    contador += 2;
                }
            }

            for (int i = 0; i < McoLen; i++)
            {
                for (int j = 0; j < McoLen; j++)
                {
                    MatrixMCO[i][j] = MatrixMCO[i][j] / contador;
                }
            }

            return MatrixMCO;

        }

        // **********************************************************************
        static float McoEnergia(float[][] mco)
        {

            float energia = 0;
            for (int i = 0; i < McoLen; i++)
            {
                for (int j = 0; j < McoLen; j++)
                {
                    if (mco[i][j] > 0) energia += mco[i][j] * mco[i][j];
                }
            }

            return energia;
        }

        // **********************************************************************
        static float McoEntropia(float[][] mco)
        {
            float entropia = 0;
            for (int i = 0; i < McoLen; i++)
            {
                for (int j = 0; j < McoLen; j++)
                {
                    if (mco[i][j] > 0) entropia += mco[i][j] * (float)Math.Log(mco[i][j], 2);
                }
            }

            return entropia;
        }

        // **********************************************************************
        static float McoContraste(float[][] mco)
        {
            float contraste = 0;
            for (int i = 0; i < McoLen; i++)
            {
                for (int j = 0; j < McoLen; j++)
                {
                    if (mco[i][j] > 0) contraste += (i - j) * (i - j) * mco[i][j];
                }
            }

            return contraste;
        }


        // **********************************************************************
        static float McoHomogeneidade(float[][] mco)
        {
            float homogeneidade = 0;
            for (int i = 0; i < McoLen; i++)
            {
                for (int j = 0; j < McoLen; j++)
                {
                    if (mco[i][j] > 0) homogeneidade += mco[i][j] / (1 + (i - j) * (i - j));
                }
            }

            return homogeneidade;
        }

    }



}

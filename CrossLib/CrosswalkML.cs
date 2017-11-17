using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Neuro;

namespace CrossLib
{
    public class CrosswalkML
    {

        //--------------------------------------------------------------------------------------
        public static string MetodoTransform = "sobel";
        public static int QtdNeuronio = 2;
        public static string arquivoRNA = @"../../../imagens/rn_" + QtdNeuronio + "_1_" + MetodoTransform + ".obj";
        //--------------------------------------------------------------------------------------

        private Network network;

        //--------------------------------------------------------------------------------------
        public void LoadNetwork()
        {
           network = Network.Load(arquivoRNA);
        }

        //--------------------------------------------------------------------------------------
        public List<PointXY> Search(Bitmap imagem)
        {
            List<PointXY> retorno = new List<PointXY>();

            Bitmap tmpImg = (Bitmap)imagem.Clone();
            tmpImg = TransformImage(tmpImg);

            Byte[][] array = CrossLib.Imaging.toMatrizOld(tmpImg);

            Bitmap newBitmap = new Bitmap(tmpImg.Width, tmpImg.Height, PixelFormat.Format8bppIndexed);
            BitmapData imageData = newBitmap.LockBits(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), ImageLockMode.ReadOnly, newBitmap.PixelFormat);

            for (int x = 0; x < tmpImg.Width - 30 - 15; x += 30)
            {
                for (int y = 0; y < tmpImg.Height - 30 - 15; y += 30)
                {

                    if (array[x][y] > 0)
                    {
                        Crop filter = new Crop(new Rectangle(x, y, 30, 30));
                        Bitmap newImage = filter.Apply(tmpImg);
                        double[] vetor = GetFeature(newImage);
                        double resp = network.Compute(vetor)[0];

                        if (resp > 0.35)
                        {
                            Crop filter2 = new Crop(new Rectangle(x + 15, y, 30, 30));
                            Bitmap newImage2 = filter.Apply(tmpImg);
                            double[] vetor2 = GetFeature(newImage2);
                            double resp2 = network.Compute(vetor2)[0];

                            if (resp2 > 0.35)
                            {
                                Crop filter3 = new Crop(new Rectangle(x, y + 15, 30, 30));
                                Bitmap newImage3 = filter.Apply(tmpImg);
                                double[] vetor3 = GetFeature(newImage3);
                                double resp3 = network.Compute(vetor3)[0];

                                if (resp3 > 0.35)
                                {

                                    Crop filter4 = new Crop(new Rectangle(x, y + 15, 30, 30));
                                    Bitmap newImage4 = filter.Apply(tmpImg);
                                    double[] vetor4 = GetFeature(newImage4);
                                    double resp4 = network.Compute(vetor4)[0];

                                    if (resp4 > 0.35)
                                    {
                                        retorno.Add(new PointXY(x,y));

                                        //Drawing.Rectangle(imageData, new Rectangle(x, y, 30, 30), Color.Red);

                                        //Bitmap imgSave = filter.Apply(tmpImg);
                                        //string fileName = @"../../../imagens/imgTest/img_" + count + "_" + x + "_" + y + ".png";
                                        //imgSave.Save(fileName);
                                    }
                                }

                            }
                        }

                    }

                }

            }

            // unlock source image
            newBitmap.UnlockBits(imageData);
           // setImage(newBitmap);

            return retorno; 

        }
        

        //--------------------------------------------------------------------------------------
        public static double[] GetFeature(Bitmap img)
        {
            double[] retorno;

            Byte[] vetor = CrossLib.Imaging.toVector(img);
            retorno = CrossLib.Imaging.VectorNormalizeRN(vetor);

            //List<CrossLib.Imaging.PointMCO> list = FeatureExtract();

            return retorno;

        }

        //--------------------------------------------------------------------------------------
        public static Bitmap TransformImage(Bitmap imagem)
        {
            Bitmap imgTmp = imagem;

            if (CrosswalkML.MetodoTransform == "YCbCrY")
            {
                YCbCrExtractChannel filter = new YCbCrExtractChannel(YCbCr.YIndex);
                imgTmp = filter.Apply(imagem);
            }

            if (CrosswalkML.MetodoTransform == "gray")
            {
                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                imgTmp = filter.Apply(imagem);
            }

            if (CrosswalkML.MetodoTransform == "sobel")
            {
                // create filters sequence
                FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();

                // add filters to the sequence
                filter.Add(new Grayscale(0.299, 0.587, 0.114));
                filter.Add(new SobelEdgeDetector());

                // apply the filter sequence
                imgTmp = filter.Apply(imagem);

            }
            return imgTmp;
        }
    }
}

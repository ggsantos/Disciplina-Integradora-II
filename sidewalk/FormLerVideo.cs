using Accord.MachineLearning;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.FFMPEG;
using CrossLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidewalk
{
    public partial class FormLerVideo : Form
    {
        private bool rodando;
        private bool pausa;
        private bool salvando;
        private bool processando;

        private int frameNumber;

        //variavel para salvar resultados
        private int testcount = 0;

        public FormLerVideo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rodando)
            {
                rodando = false;
                buttonRun.Text = "Start";
            }
            else
            {
                rodando = true;
                buttonRun.Text = "Cancel";

                //this.rodar();
                Thread t = new Thread(new ThreadStart(this.rodar));
                t.Start();
            }
        }

        public class ProcCor
        {
            public int pos;

            public int x;
            public int y;

            public double Cb;
            public double Cr;

            public double soma2;
            public double soma3;
            public double soma4;

            public bool cuidado;
            public bool imprime;

            public double calc1, calc2, calc3, calc4, calc5, calc6;

        }

        static int lastFrameNumber = 0;
        List<ProcCor> lista = new List<ProcCor>();

        // Classe para guardar as informacoes das linhas
        public class Lines
        {
            public double theta;
            public double radius;

            public double x0;
            public double y0;

            public double x1;
            public double y1;
        }

        private void processImageCor(Bitmap image, int frameNumber)
        {

            int cols = 3;
            int lins = 4;

            int width = image.Width;
            int height = image.Height;

            int xstep = width / cols;
            int ystep = height / lins;

            if (Math.Abs(frameNumber - lastFrameNumber) > 4)
            {
                lastFrameNumber = frameNumber;

                List<ProcCor> listaCores = new List<ProcCor>();
                
                // Transformada de Hough
                HoughLineTransformation lineTransform = new HoughLineTransformation();

                Bitmap image_aux = new Bitmap(image.Width, image.Height);
                Graphics graph = Graphics.FromImage(image_aux);
                graph.DrawImage(image, 0, 0);
                Bitmap clone = image_aux.Clone(new Rectangle(0, 0, image_aux.Width, image_aux.Height), PixelFormat.Format8bppIndexed);

                // create filter
                Median median_filter = new Median();
                // apply the filter
                median_filter.ApplyInPlace(clone);
                // create filter
                CannyEdgeDetector canny_filter = new CannyEdgeDetector();
                // apply the filter
                canny_filter.ApplyInPlace(clone);

                // Salva imagem apos aplicacao do filtro
                // clone.Save("C:\\Users\\Dragleer\\Documents\\Side Walk\\Fotos Analise\\Video4_Canny\\" + "video4_" + testcount + ".png", ImageFormat.Png);

                // apply Hough line transofrm
                lineTransform.ProcessImage(clone);
                Bitmap houghLineImage = lineTransform.ToBitmap();

                // get lines using relative intensity
                HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity(0);

                // Cria uma lista para guardar as linhas relevantes
                List<Lines> relevant_lines = new List<Lines>();

                // Seleciona as linhas que possuem uma inclinacao entre 45 e 135 graus
                foreach (HoughLine line in lines)
                {
                    Lines l = new Lines();

                    l.theta = line.Theta;
                    l.radius = line.Radius;

                    if (l.radius < 0)
                    {
                        l.radius = Math.Abs(l.radius);
                        l.theta = l.theta + 180;
                    }

                    // get image centers (all coordinate are measured relative
                    // to center)
                    int w2 = image.Width / 2;
                    int h2 = image.Height / 2;

                    if (line.Theta != 0)
                    {
                        // none-vertical line
                        l.x0 = -w2; // most left point
                        l.x1 = w2;  // most right point

                        // calculate corresponding y values
                        l.y0 = (-Math.Cos(l.theta) * l.x0 + l.radius) / Math.Sin(l.theta);
                        l.y1 = (-Math.Cos(l.theta) * l.x1 + l.radius) / Math.Sin(l.theta);
                    }
                    else
                    {
                        // vertical line
                        l.x0 = line.Radius;
                        l.x1 = line.Radius;

                        l.y0 = h2;
                        l.y1 = -h2;
                    }

                    if (l.y1 - l.y0 > 0)
                    {
                        int slope = (int)(l.x1 - l.x0) / (int)(l.y1 - l.y0);
                        double angle = Math.Atan(l.theta);
                        // angle > 0.785398 || angle < 2.35619
                        // angle > 1.22173 && angle < 1.91986
                        if (l.theta > 70 && l.theta < 110)
                        {
                            //using (Graphics g = Graphics.FromImage(image))
                            //{
                            //    g.DrawLine(new Pen(Color.Black, 3), new Point((int)l.x0 + w2, h2 - (int)l.y0), new Point((int)l.x1 + w2, h2 - (int)l.y1));
                            //}
                            relevant_lines.Add(l);
                        }
                    }
                }

                // Verifica quais linhas sao paralelas
                List<Lines> parallel_lines = new List<Lines>();

                foreach (Lines line in relevant_lines)
                {
                    foreach (Lines line_aux in relevant_lines)
                    {
                        if (line != line_aux)
                        {
                            int slope = (int)(line.x1 - line.x0) / (int)(line.y1 - line.y0);
                            int slope_aux = (int)(line_aux.x1 - line_aux.x0) / (int)(line_aux.y1 - line_aux.y0);

                            double angle = Math.Atan(line.theta);
                            double angle_aux = Math.Atan(line_aux.theta);

                            // Math.Abs(angle - angle_aux) <= 0.05
                            double theta_dif = Math.Abs(line.theta - line_aux.theta);
                            // theta_dif >= 0 && theta_dif <= 5
                            if (Math.Abs(angle - angle_aux) < 0.08)
                            {
                                double b1 = line.y0 - line.x0 * (angle * 180 / Math.PI);
                                double b2 = line_aux.y0 - line_aux.x0 * (angle_aux * 180 / Math.PI);

                                double angle_m = (angle + angle_aux) / 2;

                                double distance = Math.Abs(((-b1) / (angle * 180 / Math.PI) - ((-b2) / (angle_aux * 180 / Math.PI))));
                                // Math.Abs(b2 - b1) / Math.Sqrt(Math.Pow(angle_m, 2) + 1);
                                // Math.Sqrt(Math.Pow(Math.Abs(x0_aux - x0), 2) + Math.Pow(Math.Abs(y0_aux - y0), 2));

                                

                                //Console.WriteLine(distance);

                                if (distance > 0 && distance < 100)
                                {
                                    //Console.WriteLine(distance);
                                    //using (Graphics g = Graphics.FromImage(image))
                                    //{
                                    //    g.DrawLine(new Pen(Color.Black, 3), new Point((int)line.x0 + (image.Width / 2), (image.Height / 2) - (int)line.y0), new Point((int)line.x1 + (image.Width / 2), (image.Height / 2) - (int)line.y1));
                                    //}
                                    Console.WriteLine(line.theta);
                                    parallel_lines.Add(line);
                                }
                            }
                        }
                    }
                }

                int pos = 0;
                for (int x = 0; x < width; x = x + xstep)
                {
                    for (int y = 0; y < height; y = y + ystep)
                    {

                        if (pos == 5 ||
                            pos == 2 || pos == 6 || pos == 10 ||
                                        pos == 7 || pos == 1 || pos == 3 || pos == 9 || pos == 11)
                        {

                            Crop filter = new Crop(new Rectangle(x, y, xstep, ystep));
                            Bitmap img = filter.Apply(image);
                            MyImageStatisticsYCbCr stat2 = new MyImageStatisticsYCbCr(img);

                            ProcCor item = new ProcCor();

                            item.imprime = (pos != 7);

                            item.pos = pos;

                            item.x = x;
                            item.y = y;

                            item.Cb = stat2.Cb.Mean;
                            item.Cr = stat2.Cr.Mean;

                            listaCores.Add(item);

                        }

                        pos++;

                    }
                }

                int POS7 = 5;
                bool line1 = true;
                bool line2 = true;
                bool line3 = true;
                foreach (var item in listaCores)
                {
                    if (item.imprime)
                    {
                        item.soma2 = Math.Abs(item.Cb - listaCores[POS7].Cb) + Math.Abs(item.Cr - listaCores[POS7].Cr);

                        item.soma3 = Math.Abs(item.Cb - listaCores[POS7].Cb);
                        item.soma4 = Math.Abs(item.Cr - listaCores[POS7].Cr);

                        item.cuidado = item.soma2 > 0.03 || item.soma3 > 0.017 || item.soma4 > 0.017;

                        if (item.pos == 5 || item.pos == 6 || item.pos == 7)
                        {
                            // Calcula a diferenca de cor entre o bloco do meio com os da ponta, para cada linha
                            item.calc1 = Math.Abs(item.Cb - listaCores[item.pos - 5].Cb) + Math.Abs(item.Cr - listaCores[item.pos - 5].Cr);
                            item.calc2 = Math.Abs(item.Cb - listaCores[item.pos + 1].Cb) + Math.Abs(item.Cr - listaCores[item.pos + 1].Cr);

                            item.calc3 = Math.Abs(item.Cb - listaCores[item.pos - 5].Cb);
                            item.calc4 = Math.Abs(item.Cb - listaCores[item.pos + 1].Cb);

                            item.calc5 = Math.Abs(item.Cr - listaCores[item.pos - 5].Cr);
                            item.calc6 = Math.Abs(item.Cr - listaCores[item.pos + 1].Cr);

                            // Se a diferenca eh relevante, altera a flag da linha que possui tal diferenca
                            if (item.pos == 5)
                            {
                                line1 = item.calc1 > 0.03 || item.calc2 > 0.03 || item.calc3 > 0.017 || item.calc4 > 0.017 || item.calc5 > 0.017 || item.calc6 > 0.017;
                            }
                            else if (item.pos == 6)
                            {
                                line2 = item.calc1 > 0.03 || item.calc2 > 0.03 || item.calc3 > 0.017 || item.calc4 > 0.017 || item.calc5 > 0.017 || item.calc6 > 0.017;
                                
                            }
                            else line3 = item.calc1 > 0.03 || item.calc2 > 0.03 || item.calc3 > 0.017 || item.calc4 > 0.017 || item.calc5 > 0.017 || item.calc6 > 0.017;
                        }
                    }
                }

                // Caso duas linhas seguidas nao possuam diferencas na propria linha, eh considerado
                // que ambas nao possuem obstaculos
                if (!line1 && !line2)
                {
                    //pos = 1, 5, 9
                    listaCores[0].cuidado = false;
                    listaCores[3].cuidado = false;
                    listaCores[6].cuidado = false;
                    //pos = 2, 6, 10
                    listaCores[1].cuidado = false;
                    listaCores[4].cuidado = false;
                    listaCores[7].cuidado = false;
                }

                if (!line2 && !line3)
                {
                    //pos = 2, 6, 10
                    listaCores[1].cuidado = false;
                    listaCores[4].cuidado = false;
                    listaCores[7].cuidado = false;
                    //pos = 3, 7, 11
                    listaCores[2].cuidado = false;
                    listaCores[5].cuidado = false;
                    listaCores[8].cuidado = false;
                }

                // Verifica se alguma linha paralela esta em um bloco, se sim, ativa o cuidado para o respectivo bloco
                foreach (var item in listaCores)
                {
                    if (item.pos != 7)
                    {
                        foreach (Lines line in parallel_lines)
                        {
                            double angle = Math.Atan(line.theta);

                            double b1 = line.y0 - line.x0 * angle;

                            // Comparacao com o segmento inferior
                            int x3 = item.x;
                            int y3 = item.y;
                            int x4 = item.x + xstep;
                            int y4 = item.y + xstep;

                            double intersec_x = (((line.x0 * line.y1 - line.y0 * line.x1) * (x3 - x4) - (line.x0 - line.x1) * (x3 * y4 - y3 * x4)) / ((line.x0 - line.x1) * (y3 - y4) - (line.y0 - line.y1) * (x3 - x4)));
                            double intersec_y = (((line.x0 * line.y1 - line.y0 * line.x1) * (y3 - y4) - (line.y0 - line.y1) * (x3 * y4 - y3 * x4)) / ((line.x0 - line.x1) * (y3 - y4) - (line.y0 - line.y1) * (x3 - x4)));

                            if (intersec_x <= x4 && intersec_x >= x3 && intersec_y <= y4 && intersec_y >= y3)
                            {
                                //item.cuidado = true;
                                //Console.WriteLine("Segmento inferior");
                            }
                            else
                            {
                                // Comparacao do segmento superior
                                x3 = item.x + ystep;
                                y3 = item.y + ystep;
                                x4 = x3 + xstep;
                                y4 = y3 + xstep;

                                intersec_x = (((line.x0 * line.y1 - line.y0 * line.x1) * (x3 - x4) - (line.x0 - line.x1) * (x3 * y4 - y3 * x4)) / ((line.x0 - line.x1) * (y3 - y4) - (line.y0 - line.y1) * (x3 - x4)));
                                intersec_y = (((line.x0 * line.y1 - line.y0 * line.x1) * (y3 - y4) - (line.y0 - line.y1) * (x3 * y4 - y3 * x4)) / ((line.x0 - line.x1) * (y3 - y4) - (line.y0 - line.y1) * (x3 - x4)));

                                if (intersec_x <= x4 && intersec_x >= x3 && intersec_y <= y4 && intersec_y >= y3)
                                {
                                    //item.cuidado = true;
                                    //Console.WriteLine("Segmento superior");
                                }
                            }
                        }
                    }
                }

                lock (lista)
                {
                    lista.Clear();
                    foreach (var item in listaCores)
                    {
                        if (item.pos != 1 && item.pos != 3 && item.pos != 9 && item.pos != 11) lista.Add(item);
                    }
                }


            }

            

            if (true)
            {
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                var pen = new SolidBrush(Color.FromArgb(200, 200, 200, 200));
                

                lock (lista)
                {
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        foreach (var item in lista)
                        {
                            g.FillRectangle(pen, item.x, item.y, xstep - 1, ystep - 1);

                            //g.DrawString("Cb=" + item.Cb + "\n" +
                            //             "Cr=" + item.Cr + "\n" +
                            //             "S=" + item.soma2 + "\n" +
                            //             "S=" + item.soma3 + "\n" +
                            //             "S=" + item.soma4 + "\n",
                            //             new Font("courier", 30), new SolidBrush(Color.Black), new Rectangle(item.x, item.y, xstep, ystep));

                            if (item.cuidado)
                            {
                                g.DrawString("CUIDADO",
                                             new Font("courier", 15), new SolidBrush(Color.Red), new Rectangle(item.x, item.y, xstep, ystep), sf);
                            }

                            

                        }

                        
                    }

                    //salvando o resultado
                    image.Save("C:\\Users\\Dragleer\\Documents\\Side Walk\\Fotos Analise\\Teste4\\" + "video4_" + testcount + ".png", ImageFormat.Png);
                    testcount++;

                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBoxResult.Image = image;
                    });
                }
            }

        }


        private void processImage(Bitmap image)
        {
            int cols = 5;
            int lins = 10;

            int width = image.Width;
            int height = image.Height;
            int xstep = width / cols;
            int ystep = height / lins;

            double[][] observations = new double[cols * lins][];
            int qtd = 0;

            for (int x = 0; x < width; x = x + xstep)
            {
                for (int y = 0; y < height; y = y + ystep)
                {

                    Crop filter = new Crop(new Rectangle(x, y, xstep, ystep));

                    ImageMCO img = new ImageMCO(filter.Apply(image));
                    img.calcMCO = false;
                    img.calcLBP = false;
                    img.Run();

                    double[] lst1 = new double[2];
                    lst1[0] = img.Assimetria;
                    lst1[1] = img.Curtose;

                    observations[qtd] = lst1;
                    qtd++;

                }
            }

            KMeans kmeans = new KMeans(2);
            int[] labels = kmeans.Compute(observations);


            List<SolidBrush> pen = new List<SolidBrush>();
            pen.Add(new SolidBrush(Color.Yellow));
            pen.Add(new SolidBrush(Color.Green));

            Bitmap final = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(final))
            {
                g.Clear(Color.White);

                int pos = 0;
                for (int x = 0; x < width; x = x + xstep)
                {
                    for (int y = 0; y < height; y = y + ystep)
                    {
                        g.FillRectangle(pen[labels[pos]], x, y, xstep - 1, ystep - 1);
                        pos++;
                    }
                }

            }

            this.Invoke((MethodInvoker)delegate
            {
                pictureBoxResult.Image = final;
            });

        }

        private void rodar()
        {
            string pasta = @"C:\Users\Dragleer\Documents\Side Walk\Fotos Teste\";
            string video = textBoxVideo.Text.Substring(textBoxVideo.Text.LastIndexOf(@"\") + 1).Replace(".avi", "");

            // create instance of video reader
            VideoFileReader reader = new VideoFileReader();
            // open video file
            reader.Open(textBoxVideo.Text);
            // check some of its attributes
            Console.WriteLine("width:  " + reader.Width);
            Console.WriteLine("height: " + reader.Height);
            Console.WriteLine("fps:    " + reader.FrameRate);
            Console.WriteLine("codec:  " + reader.CodecName);

            int divisor = (reader.Width / 320);
            int width = reader.Width / divisor;
            int height = reader.Height / divisor;
            int fps = reader.FrameRate / 10;

            frameNumber = 0;
            int frameCount = 0;

            //// read video frames
            while (true)
            {
                if (!pausa)
                {
                    using (var videoFrame = reader.ReadVideoFrame())
                    {
                        if (videoFrame == null || !rodando)
                            break;

                        var videoFrameCopia = (Bitmap)videoFrame.Clone();

                        if (salvando)
                        {
                            videoFrameCopia.Save(pasta + video + "_" + frameNumber + ".Png", ImageFormat.Png);
                        }

                        if (processando)
                        {


                            if (frameCount == fps)
                            {

                                Bitmap image = new Bitmap((Bitmap)videoFrameCopia.Clone(), new Size(width, height));

                                Thread thread = new Thread(() => processImageCor(image, frameNumber));
                                thread.Start();
                                frameCount = 0;
                            }


                            //if (frameCount == 10)
                            //{
                            //    Bitmap image = (Bitmap)videoFrameCopia.Clone();

                            //    Thread thread = new Thread(() => processImage(image));
                            //    thread.Start();
                            //    //processImage(videoFrameCopia);
                            //    frameCount = 0;
                            //}
                            frameCount++;
                        }


                        this.Invoke((MethodInvoker)delegate
                        {
                            frameNumber++;
                            labelCount.Text = frameNumber.ToString();
                            labelCount.Refresh();

                            pictureBoxVideo.Image = videoFrameCopia;
                            pictureBoxVideo.Refresh();

                        });

                        //videoFrame.Save(@"c:\tempo\"+i + ".bmp");

                        // dispose the frame when it is no longer required
                        videoFrame.Dispose();
                        videoFrameCopia.Dispose();

                    }
                }
            }
            reader.Close();
        }

        private void FormLerVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rodando)
            {
                button1_Click(null, null);
                e.Cancel = true;
            }
        }

        private void buttonPausa_Click(object sender, EventArgs e)
        {
            if (pausa)
            {
                pausa = false;
                buttonPausa.Text = "Pause";
            }
            else
            {
                pausa = true;
                buttonPausa.Text = "Continue";
            }
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            if (salvando)
            {
                salvando = false;
                buttonSalvar.Text = "Save";
            }
            else
            {
                salvando = true;
                buttonSalvar.Text = "Saving";
            }
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if (processando)
            {
                processando = false;
                buttonProcess.Text = "Process";
            }
            else
            {
                processando = true;
                buttonProcess.Text = "Stop Process";
            }
        }



    }
}

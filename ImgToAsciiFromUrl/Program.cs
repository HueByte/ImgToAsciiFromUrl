using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;

namespace ImgToAsciiFromUrl
{
    class Program
    {
        //url to img
        private const string _imgurl = "http://www.titikmall.store/wp-content/uploads/2019/01/236_2361875_pusheen_aka_the_cute_of_cuteness_by_favouritefi_cute_cat_drawing_png_4.jpg";   //url to img
        private const int _aWidth = 150;
        private static string[] _asciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };


        static void Main(string[] args)
        {
            Bitmap _resultImage = GetBitmapFromUrl(_imgurl);    //Bitmaping final img
            string art = MakeItArt(_resultImage);   //Converting to ascii
            Console.Write(art);
            Console.ReadLine();
        }

        private static Bitmap GetBitmapFromUrl(string url)
        {
            //Downloading and bitmaping img
            WebRequest _webr = WebRequest.Create(url);
            WebResponse _webresponse = _webr.GetResponse();
            Stream responseimage = _webresponse.GetResponseStream();
            Bitmap image = new Bitmap(responseimage);
            return image;
        }

        private static string MakeItArt(Bitmap _image)
        {
            _image = GetResized(_image, _aWidth); //making new size
            string ascii = asciiConverter(_image);
            return ascii;
        }

        private static Bitmap GetResized(Bitmap inputbit, int asciiWidth)
        {
            int asciiheight = 0;
            //New Height
            asciiheight = (int)Math.Ceiling((double)inputbit.Height * asciiWidth / inputbit.Width);
            //New bitmap with defined resolution
            Bitmap _result = new Bitmap(asciiWidth, asciiheight);
            Graphics _g = Graphics.FromImage((Image)_result);

            //interpolation
            _g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            _g.DrawImage(inputbit, 0, 0, asciiWidth, asciiheight);
            _g.Dispose();
            return _result;
        }

        private static string asciiConverter(Bitmap img)
        {
            bool toogle = false;
            StringBuilder singb = new StringBuilder();
            for (int h = 0; h < img.Height; h++)
            {
                for(int w = 0; w < img.Width; w++)
                {
                    Color pixel = img.GetPixel(w, h);
                    //find gray color
                    int red = (pixel.R + pixel.G + pixel.B) / 3;
                    int green = (pixel.R + pixel.G + pixel.B) / 3;
                    int blue = (pixel.R + pixel.G + pixel.B) / 3;
                    Color GrayColor = Color.FromArgb(red, green, blue);

                    if(!toogle)
                    {
                        int index = (GrayColor.R * 10) / 255;
                        singb.Append(_asciiChars[index]);
                    }
                }
                if (!toogle)
                {
                    singb.Append(Environment.NewLine);
                    toogle = true;
                }
                else
                {
                    toogle = false;
                }
            }

            return singb.ToString();

        }
    }
}

using System;
using System.Drawing;
using System.Reflection;
using System.Web;
using AForge.Imaging.Filters;
using IronOcr;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Utilities.Constants;
using LibraryManagementSystem.WebUI.Utilities.Logging;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Concrete {
    public class ImageProcessingManager : IImageProcessingService {
        public DataResult<string> ProcessImage(HttpPostedFileBase uploadFile) {
            try {
                Bitmap bmpPostedImage = new System.Drawing.Bitmap(uploadFile.InputStream);
                Bitmap newImage = new Bitmap(bmpPostedImage.Width, bmpPostedImage.Height);
                for (int i = 0; i < bmpPostedImage.Width; i++) {
                    for (int j = 0; j < bmpPostedImage.Height; j++) {
                        Color color = bmpPostedImage.GetPixel(i, j);
                        int newColor = (color.R + color.G + color.B) / 3;
                        newImage.SetPixel(i, j, Color.FromArgb(newColor, newColor, newColor));
                    }
                }
                bmpPostedImage = Sharpening(newImage).Data;
                AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };
                var results = OCR.Read(bmpPostedImage);
                string words = results.ToString();
                int index = words.IndexOf("ISBN") + 5;
                words = words.Substring(index, words.Length - index);
                index = words.IndexOf("\n");
                words = words.Substring(0, index - 1);
                for (int i = 0; i < words.Length; i++) {
                    int number = 0;
                    for (int j = 0; j < 10; j++) {
                        string ch = j.ToString();
                        if (words[i].Equals(ch[0])) {
                            number = 1;
                            break;
                        }
                    }
                    if (number == 0) {
                        words = words.Remove(i, 1);
                        i--;
                    }
                }
                return new SuccessDataResult<string>(words);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.ImageProcessing, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<string>(/*todo*/);
            }
        }

        private DataResult<Bitmap> Sharpening(Bitmap image) {
            try {
                Bitmap blurred = MedianFilter(image).Data;
                Bitmap edge = EdgeDetection(image, blurred).Data;
                return new SuccessDataResult<Bitmap>(SharpeImage(image, edge).Data);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.ImageProcessing, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Bitmap>(/*todo*/);
            }
        }

        private DataResult<Bitmap> MedianFilter(Bitmap image) {
            try {
                Bitmap imageX = new Bitmap(image);
                Bitmap bp = AForge.Imaging.Image.Clone(imageX, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Median filter = new Median();
                return new SuccessDataResult<Bitmap>(filter.Apply(bp));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.ImageProcessing, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Bitmap>(/*todo*/);
            }
        }

        private DataResult<Bitmap> SharpeImage(Bitmap OriginalImage, Bitmap Edge) {
            try {
                Color color1, color2, color;
                Bitmap Image;
                int width = OriginalImage.Width;
                int height = OriginalImage.Height;
                Image = new Bitmap(width, height);
                int R, G, B;
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        color1 = OriginalImage.GetPixel(x, y);
                        color2 = Edge.GetPixel(x, y);
                        R = color1.R + color2.R; G = color1.G + color2.G; B = color1.B + color2.B;
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        color = Color.FromArgb(R, G, B);
                        Image.SetPixel(x, y, color);
                    }
                }
                return new SuccessDataResult<Bitmap>(Image);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.ImageProcessing, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Bitmap>(/*todo*/);
            }
        }

        private DataResult<Bitmap> EdgeDetection(Bitmap OriginalImage, Bitmap BlurredImage) {
            try {
                Color color1, color2, color;
                Bitmap Image;
                int width = OriginalImage.Width;
                int height = OriginalImage.Height;
                Image = new Bitmap(width, height);
                int R, G, B;
                double db = 1.7;
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        color1 = OriginalImage.GetPixel(x, y);
                        color2 = BlurredImage.GetPixel(x, y);
                        R = Convert.ToInt16(db * (color1.R - color2.R));
                        G = Convert.ToInt16(db * (color1.G - color2.G));
                        B = Convert.ToInt16(db * (color1.B - color2.B));
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        color = Color.FromArgb(R, G, B);
                        Image.SetPixel(x, y, color);
                    }
                }
                return new SuccessDataResult<Bitmap>(Image);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.ImageProcessing, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Bitmap>(/*todo*/);
            }
        }
    }
}
using System;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Configuration;

namespace MekDB.Models
{
    public class ProcessImage
    {
        //variables used to control the size and quality of the image

        //File extensions
        private const string JpgEx = ".jpg";
        private const string JpegEx = ".jpeg";
        private const string JpeEx = ".jpe"; //also a jpg image
        private const string PngEx = ".png";
        private const string BmpEx = ".bmp";
        private const string GifEx = ".gif";

        //Mime data
        private const string JpgMime = "image/jpg";
        private const string JpegMime = "image/jpeg";
        private const string PJJpegMime = "image/pjpeg";
        private const string PngMime = "image/png";
        private const string XPngMime = "image/x-png";
        private const string BmpMime = "image/bmp";
        private const string GifMime = "image/gif";

        //the smallest allowed file
        private const int minimumFileSize = 512;

        //perform checks to make sure the file is a image
        public bool ValidateImage(HttpPostedFileBase File, bool CheckFileHeader)
        {
            //if the file if smaller then this we just ignore it
            if (File.ContentLength < minimumFileSize)
            {
                return false;
            }

            //first we check the mime data
            if (    
                File.ContentType.ToLower() != JpgMime      &&
                File.ContentType.ToLower() != JpegMime     &&
                File.ContentType.ToLower() != PJJpegMime   &&
                File.ContentType.ToLower() != PngMime      &&
                File.ContentType.ToLower() != XPngMime     &&
                File.ContentType.ToLower() != BmpMime      &&
                File.ContentType.ToLower() != GifMime
                )
            {
                //if none of these match we cancel the upload
                return false;
            }

            //then we check the file extension
            if (
                Path.GetExtension(File.FileName).ToLower() != JpegEx    &&
                Path.GetExtension(File.FileName).ToLower() != JpgEx     &&
                Path.GetExtension(File.FileName).ToLower() != JpeEx     &&
                Path.GetExtension(File.FileName).ToLower() != PngEx     &&
                Path.GetExtension(File.FileName).ToLower() != BmpEx     &&
                Path.GetExtension(File.FileName).ToLower() != GifEx     
                )
            {
                //if none of these match we cancel the upload
                return false;
            }

            //Check the file header to make sure it's not a malicious file in disguise
            if (CheckFileHeader)
            {
                try
                {
                    //first we check for malicious scripts
                    //https://stackoverflow.com/a/14587821
                    byte[] buffer = new byte[512];
                    File.InputStream.Read(buffer, 0, 512); //read first 512 bytes
                    string content = System.Text.Encoding.UTF8.GetString(buffer); //convert them into text
                    //do a regex match for commen malicious text injections
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    {
                        return false;
                    }

                    //then we check for malicious files by trying to read it as an image
                    using (var bitmap = new System.Drawing.Bitmap(File.InputStream))
                    {
                        //do nothing
                        //NOTE: Doing this set the file stream read position to the end of the file
                        //so you'll need to set it back to zero if you wish to read from it again

                        bitmap.GetThumbnailImage(0,0, null, IntPtr.Zero);
                    }

                }
                catch (NotSupportedException nsex)
                {
                    System.Diagnostics.Debug.WriteLine("###IsImageError: " + nsex);
                    return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("###IsImageError: " + ex);
                    return false;
                }
                finally
                {
                    File.InputStream.Position = 0; //reset stream position so we can read from it again
                }
            }

            //if everything is ok then we can assume that the file is a image
            return true;
        }

        //size down and convert the image to a compressed jpg to save space
        public byte[] ConvertImageToJpg(HttpPostedFileBase image)
        {
            //get values from Web.Config
            Size newImageSize;
            long imageQuality;
            try
            {
                newImageSize = new Size
                {
                    Width = int.Parse(ConfigurationManager.AppSettings["Width"]), /*get image size from Web.Config*/
                    Height = int.Parse(ConfigurationManager.AppSettings["Height"])
                };

                imageQuality = long.Parse(ConfigurationManager.AppSettings["Quality"]);
            }
            catch (ConfigurationException cex)
            {
                System.Diagnostics.Debug.WriteLine("####Invalid AppSettings Value" + cex.Message);
                throw cex;
            }
            catch (FormatException fex)
            {
                throw fex;
            }
            catch (Exception)
            {
                throw;
            }

            //convert to an image object
            Image inputImage = Image.FromStream(image.InputStream);
            //resize the image
            Image resizedImage = new Bitmap(inputImage, newImageSize);

            //get jpg enconding
            ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().ElementAt(1); //Jpg codec

            //used for setting the quality of the image, saving space
            Encoder qualityEncoder = Encoder.Quality;

            //array of encoder parameters
            EncoderParameters encoderParameters = new EncoderParameters(1);

            //tell the enconder what quality level we want. MUST BE CAST TO A LONG!!
            EncoderParameter encoderParameter = new EncoderParameter(qualityEncoder, (long)imageQuality); //<- worn't work with out the long cast

            //add our parameter to the parameter ""arary""
            encoderParameters.Param[0] = encoderParameter;

            //convert to jpg
            using (MemoryStream ms = new MemoryStream(0))
            {
                //convert to jpg and store it in the MemoryStream
                resizedImage.Save(ms, codec, encoderParameters);

                return ms.GetBuffer();
            }
        }
    }
}
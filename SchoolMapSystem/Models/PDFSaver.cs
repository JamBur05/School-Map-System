using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;


namespace SchoolMapSystem.Models
{
    class PDFSaver
    {
        private string filePath;

        internal static class User32
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
        }

        public void SaveScreenshotAsPdf(Window window, string fileName)
        {
            // Create a bitmap of the window
            var scale = 2;
            var bitmap = new Bitmap((int)(window.ActualWidth * scale), (int)(window.ActualHeight * scale));
            var graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen((int)window.Left, (int)window.Top, 0, 0, bitmap.Size);

            // Generate a file path based on the file name
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{fileName}.pdf");
            try
            {
                using (var document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(bitmap.Width / scale, bitmap.Height / scale)))
                {
                    // Save the bitmap as a PDF file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
                        document.Open();
                        var image = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp);
                        document.Add(image);
                        document.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        public string GetFilePath()
        {
            return filePath;
        }

    }
}

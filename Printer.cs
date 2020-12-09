using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Drawing;

namespace AvencaLib
{
    public static class AvencaPrinter
    {
        static private string output;
        static private int font_size;
        static private int pos_x;
        static private bool is_bold;

        private static bool PrintString(string printerName)
        {
            bool result = false;
            bool canPrint = false;

            try
            {
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    if (PrinterSettings.InstalledPrinters[i] == printerName)
                        canPrint = true;
                }
                if (canPrint)
                {
                    using (PrintDocument printDoc = new PrintDocument())
                    {
                        printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDoc_PrintString);
                        printDoc.PrinterSettings.PrinterName = printerName;
                        printDoc.Print();
                    }
                }
                else throw new Exception("Printer not found: " + printerName);
            }
            catch (Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
            return result;
        }

        private static bool PrintEtiqueta(string printerName)
        {
            bool result = false;
            bool canPrint = false;

            try
            {
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    if (PrinterSettings.InstalledPrinters[i] == printerName)
                        canPrint = true;
                }
                if (canPrint)
                {
                    using (PrintDocument printDoc = new PrintDocument())
                    {
                        printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDoc_PrintEtiqueta);
                        printDoc.PrinterSettings.PrinterName = printerName;
                        printDoc.Print();
                    }
                }
                else throw new Exception("Printer not found: " + printerName);
            }
            catch (Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
            return result;
        }

        public static bool PrintString(string printerName, string text, int fontSize = 16, bool bold = false, int x = 0)
        {
            output = text;
            font_size = fontSize;
            is_bold = bold;
            pos_x = x;

            return PrintString(printerName);
        }

        public static bool PrintStringList(string printerName, List<string> text, int fontSize = 16, bool bold = false, int x = 0)
        {
            output = text.ToString();

            return PrintString(printerName);
        }

        public static bool PrintEtiqueta(string printerName, string text, int fontSize = 20, bool bold = true, int x = 5)
        {
            output = text;
            font_size = fontSize;
            is_bold = bold;
            pos_x = x;

            return PrintEtiqueta(printerName);
        }

        public static bool PrintEtiqueta(string printerName, List<string> text, int fontSize = 20, bool bold = true, int x = 5)
        {
            output = text.ToString();

            return PrintEtiqueta(printerName);
        }

        private static void printDoc_PrintString(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font textFont = new System.Drawing.Font(FontFamily.GenericSansSerif, font_size, is_bold ? FontStyle.Bold : FontStyle.Regular);
            e.Graphics.DrawString(output, textFont, Brushes.Black, 0, 0);
        }

        private static void printDoc_PrintEtiqueta(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            RectangleF rf = new RectangleF(pos_x, 5, 80, 50);
            Rectangle r = new Rectangle(0, 0, 120, 50);
            Pen p = new Pen(Color.Gray);
            Font textFont = new System.Drawing.Font(FontFamily.GenericSansSerif, font_size, is_bold ? FontStyle.Bold : FontStyle.Regular);
            e.Graphics.DrawString(output, textFont, Brushes.Black, rf);
            e.Graphics.DrawRectangle(p, r);
        }
    }
}

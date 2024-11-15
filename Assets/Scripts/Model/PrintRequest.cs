using System;
using System.Drawing;
using UnityEngine;

public class PrintRequest
{

    public void Print(string imgPath, int printNum, Action<RequestResult> callback)
    {
        var ret = new RequestResult();

        try
        {
            Image img = Image.FromFile(imgPath);

            System.Drawing.Printing.PrintDocument printDocument = new();
            printDocument.PrintPage += (sender, e) =>
            {
                try
                {
                    for (int i = 0; i < printNum; i++)
                    {
                        float zoom = 1;
                        if (img.Width > e.Graphics.VisibleClipBounds.Width)
                        {
                            zoom = e.Graphics.VisibleClipBounds.Width / img.Width;
                        }
                        if (img.Height * zoom > e.Graphics.VisibleClipBounds.Height)
                        {
                            zoom = e.Graphics.VisibleClipBounds.Height / img.Height;
                        }
                        e.Graphics.DrawImage(img, 0, 0, img.Width * zoom, img.Height * zoom);
                        e.HasMorePages = false;
                    }
                }
                catch (Exception ex)
                {
                    ret.isValid = false;
                    ret.message = ex.Message;
                }
            };

            string printerName = GetDefaultPrinter();
            if (string.IsNullOrEmpty(printerName))
            {
                ret.isValid = false;
                ret.message = "プリンターが見つかりません。";
                throw new InvalidOperationException(ret.message);
            }

            printDocument.PrinterSettings.PrinterName = printerName;

            for (int i = 0; i < printNum; i++)
            {
                printDocument.Print();
            }

            ret.isValid = true;
            ret.message = "印刷が完了しました。";
        }
        catch (InvalidOperationException ex)
        {
            ret.isValid = false;
            ret.message = ex.Message;
        }
        catch (Exception ex)
        {
            ret.isValid = false;
            ret.message = ex.Message;
        }

        callback.Invoke(ret);
    }

    private string GetDefaultPrinter()
    {
        System.Drawing.Printing.PrinterSettings settings = new();
        return settings.PrinterName;
    }
}

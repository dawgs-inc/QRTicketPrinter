using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using UnityEngine;

public static class TicketComposer {
    public static async Task Compose(string scrQrFilePath, string distTicketFilePath, Ticket ticket)
    {
        await Task.Run(() =>
        {
            try
            {
                string baseImagePath = Constants.BASE_TICKET_PATH;

                using Bitmap baseImage = new Bitmap(baseImagePath);
                using Bitmap qrImage = new Bitmap(scrQrFilePath);
                using System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(baseImage);

                var collection = new System.Drawing.Text.PrivateFontCollection();
                collection.AddFontFile(Constants.FONT_FILE_PATH);
                var fontFamilies = collection.Families;
                var font = new System.Drawing.Font(fontFamilies[0], 18);

                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(ticket.createdAt)).LocalDateTime;
                string dateText = $"撮影可能日 : {dateTime.Year}/{dateTime.Month}/{dateTime.Day}";
                SizeF dateTextSize = g.MeasureString(dateText, font);
                string idText = $"ID : {ticket.id}";
                SizeF idTextSize = g.MeasureString(idText, font);

                g.DrawString(
                    dateText,
                    font,
                    Brushes.Black,
                    (baseImage.Width - dateTextSize.Width) * 0.5f,
                    baseImage.Height * 0.15f
                );

                g.DrawString(
                    idText,
                    font,
                    Brushes.Black,
                    (baseImage.Width - idTextSize.Width) * 0.5f,
                    baseImage.Height * 0.77f
                );

                Rectangle cropArea = new Rectangle(
                    (int)((qrImage.Width - qrImage.Width * 0.8f) * 0.5),
                    (int)((qrImage.Height - qrImage.Height * 0.8f) * 0.5),
                    (int)(qrImage.Width * 0.8f),
                    (int)(qrImage.Height * 0.8f)
                );
                using Bitmap cropQrImage = qrImage.Clone(cropArea, qrImage.PixelFormat);
                Vector2 qrImageSize = new(430, 430);

                g.DrawImage(
                    cropQrImage,
                    (baseImage.Width - qrImageSize.x) * 0.5f,
                    baseImage.Height * 0.505f,
                    qrImageSize.x,
                    qrImageSize.y
                );

                baseImage.Save(distTicketFilePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Common.Log($"エラーが発生しました: {ex.Message}");
                throw;
            }
        });
    }
}

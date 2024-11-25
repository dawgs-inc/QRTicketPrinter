using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using UnityEngine;

public static class TicketComposer {
    public static async Task Compose(string scrQrFilePath, string distTicketFilePath, string unixTimeStamp)
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

                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp)).LocalDateTime;

                g.DrawString(
                    $"撮影可能日：{dateTime.Year.ToString()}/{dateTime.Month.ToString()}/{dateTime.Day.ToString()}",
                    font,
                    Brushes.Black,
                    170,
                    250
                );

                Vector2 qrImageSize = new(500, 500);

                g.DrawImage(
                    qrImage,
                    (baseImage.Width - qrImageSize.x) * 0.5f,
                    baseImage.Height * 0.5f,
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

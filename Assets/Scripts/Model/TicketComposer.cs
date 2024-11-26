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
                float dateFontSize = Common.tomlRoot.Get<float>("date_font_size");
                float datePyScale = Common.tomlRoot.Get<float>("date_py_scale");
                float idFontSize = Common.tomlRoot.Get<float>("id_font_size");
                float idPyScale = Common.tomlRoot.Get<float>("id_py_scale");
                float qrSize = Common.tomlRoot.Get<float>("qr_size");
                float qrPyScale = Common.tomlRoot.Get<float>("qr_py_scale");
                float qrCropScale = Common.tomlRoot.Get<float>("qr_crop_scale");

                string baseImagePath = Constants.BASE_TICKET_PATH;

                using Bitmap baseImage = new Bitmap(baseImagePath);
                using Bitmap qrImage = new Bitmap(scrQrFilePath);
                using System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(baseImage);

                var collection = new System.Drawing.Text.PrivateFontCollection();
                collection.AddFontFile(Constants.FONT_FILE_PATH);
                var fontFamilies = collection.Families;
                var font = new System.Drawing.Font(fontFamilies[0], dateFontSize);
                var idTextFont = new System.Drawing.Font(fontFamilies[0], idFontSize);

                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(ticket.createdAt)).LocalDateTime;
                string dateText = $"撮影可能日 : {dateTime.Year}/{dateTime.Month}/{dateTime.Day}";
                SizeF dateTextSize = g.MeasureString(dateText, font);
                string idText = $"ID : {ticket.id}";
                SizeF idTextSize = g.MeasureString(idText, idTextFont);

                g.DrawString(
                    dateText,
                    font,
                    Brushes.Black,
                    (baseImage.Width - dateTextSize.Width) * 0.5f,
                    baseImage.Height * datePyScale
                );

                g.DrawString(
                    idText,
                    idTextFont,
                    Brushes.Black,
                    (baseImage.Width - idTextSize.Width) * 0.5f,
                    baseImage.Height * idPyScale
                );

                Rectangle cropArea = new Rectangle(
                    (int)((qrImage.Width - qrImage.Width * qrCropScale) * 0.5),
                    (int)((qrImage.Height - qrImage.Height * qrCropScale) * 0.5),
                    (int)(qrImage.Width * qrCropScale),
                    (int)(qrImage.Height * qrCropScale)
                );
                using Bitmap cropQrImage = qrImage.Clone(cropArea, qrImage.PixelFormat);
                Vector2 qrImageSize = new(qrSize, qrSize);

                g.DrawImage(
                    cropQrImage,
                    (baseImage.Width - qrImageSize.x) * 0.5f,
                    baseImage.Height * qrPyScale,
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

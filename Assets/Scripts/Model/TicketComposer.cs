using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using UnityEngine;

public static class TicketComposer{
    public static async Task Compose(string scrQrFilePath, string distTicketFilePath)
    {
        await Task.Run(() =>
        {
            try
            {
                string baseImagePath = Constants.BASE_TICKET_PATH;

                using (Bitmap baseImage = new Bitmap(baseImagePath))
                using (Bitmap qrImage = new Bitmap(scrQrFilePath))
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(baseImage))
                {
                    Vector2 qrImageSize = new(400, 400);

                    g.DrawImage(
                        qrImage,
                        (baseImage.Width - qrImageSize.x) * 0.5f,
                        baseImage.Height * 0.64f,
                        qrImageSize.x,
                        qrImageSize.y
                    );
                    g.Dispose();

                    baseImage.Save(distTicketFilePath, System.Drawing.Imaging.ImageFormat.Png);
                    baseImage.Dispose();
                    qrImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.Log($"エラーが発生しました: {ex.Message}");
                throw;
            }
        });
    }
}

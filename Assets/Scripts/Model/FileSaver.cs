using System;
using System.IO;
using System.Threading.Tasks;

public static class FileSaver
{
    public static async Task SaveQR(string base64String, string saveFilePath)
    {
        await Task.Run(() =>
        {
            try
            {
                int commaIndex = base64String.IndexOf(',');
                string base64 = base64String[(commaIndex + 1)..];
                byte[] qrData = Convert.FromBase64String(base64);

                if (qrData.Length == 0)
                {
                    throw new Exception("The downloaded file is empty.");
                }

                File.WriteAllBytesAsync(saveFilePath, qrData);
            }
            catch (Exception ex)
            {
                Common.Log($"エラー: {ex.Message}");
                throw;
            }
        });
    }
}

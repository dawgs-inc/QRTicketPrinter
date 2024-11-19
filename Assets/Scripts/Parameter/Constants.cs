using System.IO;
using UnityEngine;

public class Constants
{
    /// <summary>
    /// streamingAssetsのパス
    /// </summary>
    /// <value>Application.streamingAssetsPath</value>
    public static readonly string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;

    /// <summary>
    /// streamingAssetsのパス
    /// </summary>
    /// <value>Application.temporaryCachePath</value>
    public static readonly string CACHE_PATH = Application.temporaryCachePath;

    /// <summary>
    /// tomlファイル（設定ファイル）のパス
    /// </summary>
    /// <value></value>
    public static readonly string TOML_FILE_PATH = Path.Combine(STREAMING_ASSETS_PATH, "config.toml");

    /// <summary>
    /// チケット（レシート）のパス
    /// </summary>
    /// <value></value>
    public static readonly string BASE_TICKET_PATH = Path.Combine(STREAMING_ASSETS_PATH, "receipt.png");

    /// <summary>
    /// QRコードを一時的に保存するフォルダパス
    /// </summary>
    /// <value></value>
    public static readonly string QR_DIR_PATH = Path.Combine(CACHE_PATH, "qr");

    /// <summary>
    /// 合成後のレシートを一時的に保存するフォルダパス
    /// </summary>
    /// <value></value>
    public static readonly string TICKET_DIR_PATH = Path.Combine(CACHE_PATH, "ticket");
}

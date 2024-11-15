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
    /// tomlファイル（設定ファイル）のパス
    /// </summary>
    /// <value></value>
    public static readonly string TOML_FILE_PATH = Path.Combine(STREAMING_ASSETS_PATH, "config.toml");

    /// <summary>
    /// tomlファイル（設定ファイル）のパス
    /// </summary>
    /// <value></value>
    public static readonly string TICKET_PATH = Path.Combine(STREAMING_ASSETS_PATH, "receipt.png");
}

using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Nett;

public static class Common
{
    /// <summary>
    /// 設定ファイルのオブジェクト
    /// </summary>
    /// <value></value>
    public static TomlTable tomlRoot {get; private set;} = Toml.ReadFile(Constants.TOML_FILE_PATH);

    /// <summary>
    /// <para>呼び出し元のクラス名、 メソッド名をログ出力</para>
    /// <para>引数を付けた場合は、引数の中身を文字列として出力</para>
    /// </summary>
    /// <param name="message">ログ出力したい場合は文字列を入力 省略可能</param>
    public static void Log(string message = "")
    {
        // 1つ前のフレームを取得
        System.Diagnostics.StackFrame objStackFrame = new System.Diagnostics.StackFrame(1);

        // 呼び出し元のメソッド名を取得する
        string methodName = objStackFrame.GetMethod().Name;
        // 正規表現：2文字以上の英字
        methodName = Regex.Match(methodName, @"\b[a-zA-Z]{2,}\b").Value;

        // 呼び出し元のクラス名を取得する
        string className = objStackFrame.GetMethod().ReflectedType.FullName;
        // 正規表現：2文字以上の英字
        MatchCollection matches = Regex.Matches(className, @"\b[a-zA-Z]{2,}\b");
        // マッチした最後の要素を出力
        className = matches[matches.Count - 1].Value;

        string msg = "";
        if (message != "")
        {
            msg = "" + message;
        }

        Debug.Log($"*** [ {className} ] {methodName}() {msg} ***");
        // Debug.Log($"*** {msg} ***");
    }

    /// <summary>
    /// 指定されたフォルダ内のすべてのファイルとサブフォルダを削除します。
    /// </summary>
    /// <param name="folderPath">削除対象のフォルダのパス。</param>
    public static void ClearFolder(string folderPath)
    {
        try
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            // フォルダ内のすべてのファイルを削除
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                File.Delete(file);
                Common.Log($"delete file : {file}");
            }

            // フォルダ内のすべてのサブフォルダを削除
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (string directory in directories)
            {
                Directory.Delete(directory, true); // サブフォルダとその中身を再帰的に削除
                Common.Log($"delete dir : {directory}");
            }
        }
        catch (Exception ex)
        {
            Common.Log($"error: {ex.Message}");
        }
    }
}

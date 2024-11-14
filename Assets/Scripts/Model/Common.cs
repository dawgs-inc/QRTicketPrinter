using System;
using System.Collections;
using System.Collections.Generic;
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
}

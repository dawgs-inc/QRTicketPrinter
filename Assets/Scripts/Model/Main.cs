using System.IO;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        Common.Log();

        if (!Directory.Exists(Constants.QR_DIR_PATH))
        {
            Directory.CreateDirectory(Constants.QR_DIR_PATH);
        }

        if (!Directory.Exists(Constants.TICKET_DIR_PATH))
        {
            Directory.CreateDirectory(Constants.TICKET_DIR_PATH);
        }

        Common.ClearFolder(Constants.QR_DIR_PATH);
        Common.ClearFolder(Constants.TICKET_DIR_PATH);
    }
}

using UnityEngine;

public class Ticket
{
    public string id;
    public string base64QRString;
    public string createdAt;
    public string qrSaveFilePath;
    public string ticketSaveFilePath;

    public Ticket()
    {
        id = null;
        base64QRString = null;
        createdAt = null;
        qrSaveFilePath = null;
        ticketSaveFilePath = null;
    }
}

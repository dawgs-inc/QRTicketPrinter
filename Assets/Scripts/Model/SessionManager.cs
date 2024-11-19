using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    private PrintNumView printNumView;
    [SerializeField]
    private GameObject resultView;

    void Awake()
    {
        Common.Log();

        Auth auth = new();
        auth.RequestSignIn();
    }

    /// PrintNumView Events
    public void OnClickPlusButton(int num)
    {
        Common.Log($"+{num}");

        PrintNumCounter.UpdatePrintNum(num, PrintNumCounter.IncrementType.Plus);
        printNumView.SetPrintNumText(PrintNumCounter.GetPrintNum());
    }

    public void OnClickMinusButton(int num)
    {
        Common.Log($"-{num}");

        PrintNumCounter.UpdatePrintNum(num, PrintNumCounter.IncrementType.Minus);
        printNumView.SetPrintNumText(PrintNumCounter.GetPrintNum());
    }

    public async void OnClickOKButton()
    {
        Common.Log();

        string qrFilePath = Constants.QR_DIR_PATH + "/12345_qr.png";
        string ticketFilePath = Constants.TICKET_DIR_PATH + "/12345_ticket.png";

        await TicketComposer.Compose(qrFilePath, ticketFilePath);
    
        PrintRequest pr = new();
        pr.Print(ticketFilePath, PrintNumCounter.GetPrintNum(), ret =>
        {
            if (ret.isValid)
            {
                Common.Log("succeed!");
                Invoke("PrintSucceeded", 2);
            }
            else
            {
                Common.Log("faild!");
            }
        });
    }

    private void PrintSucceeded()
    {
        this.GetComponent<FadeTransition>().StartFadeTransition(resultView);
    }

    /// ResultView Events
    public void OnClickEndButton()
    {
        Common.Log();

        PrintNumCounter.Reset();
        printNumView.SetPrintNumText(PrintNumCounter.GetPrintNum());

        // Common.ClearFolder(Constants.QR_DIR_PATH);
        Common.ClearFolder(Constants.TICKET_DIR_PATH);
    }
}

using System.Threading.Tasks;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    private PrintNumView printNumView;

    [SerializeField]
    private GameObject resultView;
    private Auth auth;

    void Awake()
    {
        Common.Log();

        auth = new();
        auth.RequestSignIn(ret =>
        {
            if (!ret.isValid)
            {
                Common.Log("RequestSignIn faild!");
            }
        });
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

        TicketReuest ticketReuest = new(auth);
        RequestResult ticketReuestRet = await Task.Run<RequestResult>(() =>
        {
            return ticketReuest.Post(PrintNumCounter.GetPrintNum());
        });
        if (!ticketReuestRet.isValid)
        {
            Common.Log($"Failed to ticket request : {ticketReuestRet.message}");
            auth.RequestSignIn(ret =>
            {
                if (ret.isValid)
                {
                    OnClickOKButton();
                    return;
                }
                else
                {
                    Common.Log("RequestSignIn faild!");
                    return;
                }
            });
            return;
        }
        
        PrintRequest pr = new();
        pr.Print(ticketReuest.tickets, PrintNumCounter.GetPrintNum(), ret =>
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

        Common.ClearFolder(Constants.QR_DIR_PATH);
        Common.ClearFolder(Constants.TICKET_DIR_PATH);
    }
}

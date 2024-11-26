using System.Net;
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
                Common.Log("SignIn request faild!");
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

            if (ticketReuestRet.statusCode == HttpStatusCode.Unauthorized)
            {
                auth.RequestSignIn(ret =>
                {
                    if (ret.isValid)
                    {
                        OnClickOKButton();
                    }
                    else
                    {
                        Common.Log("SignIn request faild!");
                    }
                });
            }

            return;
        }
        
        PrintRequest pr = new();
        await pr.Print(ticketReuest.tickets, PrintNumCounter.GetPrintNum(), ret =>
        {
            if (ret.isValid)
            {
                Common.Log("print succeed!");
                Invoke("PrintSucceeded", 2);
            }
            else
            {
                Common.Log("print faild!");
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

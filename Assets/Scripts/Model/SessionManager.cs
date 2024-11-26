using System.Collections;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    private PrintNumView printNumView;

    [SerializeField]
    private ResultView resultView;
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
                        StartCoroutine(PrintFailed(2.0f, ret));
                    }
                });
            }

            StartCoroutine(PrintFailed(2.0f, ticketReuestRet));
            return;
        }
        
        PrintRequest pr = new();
        await pr.Print(ticketReuest.tickets, PrintNumCounter.GetPrintNum(), ret =>
        {
            if (ret.isValid)
            {
                Common.Log("print succeed!");
                StartCoroutine(PrintSucceeded(2.0f));
            }
            else
            {
                Common.Log("print faild!");
                StartCoroutine(PrintFailed(2.0f, ret));
            }
        });
    }

    IEnumerator PrintSucceeded(float delay)
    {
        yield return new WaitForSeconds(delay);

        resultView.SetResultText($"印刷が完了しました");
        GetComponent<FadeTransition>().StartFadeTransition(resultView.gameObject);
    }

    IEnumerator PrintFailed(float delay, RequestResult ret)
    {
        yield return new WaitForSeconds(delay);

        resultView.SetResultText($"印刷に失敗しました\n{(int)ret.statusCode} : {ret.message}");
        GetComponent<FadeTransition>().StartFadeTransition(resultView.gameObject);
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

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

    public void OnClickOKButton()
    {
        Common.Log();

        PrintRequest pr = new();
        pr.Print(Constants.TICKET_PATH, PrintNumCounter.GetPrintNum(), ret =>
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
    }
}

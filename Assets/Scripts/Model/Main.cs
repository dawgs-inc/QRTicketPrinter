using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        Common.Log();
    }

    public void OnClickButton()
    {
        PrintRequest pr = new();
        pr.Print(Constants.TICKET_PATH, 3, ret =>
        {
            if (ret.isValid)
            {
                Common.Log("succeed!");
            }
            else
            {
                Common.Log("faild!");
            }
        });
    }
}

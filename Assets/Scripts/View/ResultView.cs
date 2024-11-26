using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    public void SetResultText(string text)
    {
        resultText.text = text;
    }
}

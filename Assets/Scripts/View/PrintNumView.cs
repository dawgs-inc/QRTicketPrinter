using UnityEngine;
using UnityEngine.UI;

public class PrintNumView : MonoBehaviour
{
    	[SerializeField]
	    private Text printNumText;

        public void SetPrintNumText(int num)
        {
            printNumText.text = num.ToString();
        }
}

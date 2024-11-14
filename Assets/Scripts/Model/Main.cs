using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        Common.Log();

        Auth auth = new Auth();
        auth.RequestSignIn();
    }
}

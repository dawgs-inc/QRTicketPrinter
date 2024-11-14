using UnityEngine;

public class RequestResult
{
        public bool isValid;
        public string message;
        public bool isRetryable;
        public string stringValue;

        public RequestResult()
        {
            isValid = false;
            message = null;
            isRetryable = false;
            stringValue = null;
        }
}

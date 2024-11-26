using System.Net;
using UnityEngine;

public class RequestResult
{
        public bool isValid;
        public HttpStatusCode statusCode;
        public string message;
        public bool isRetryable;
        public string stringValue;

        public RequestResult()
        {
            isValid = false;
            statusCode = 0;
            message = null;
            isRetryable = false;
            stringValue = null;
        }
}

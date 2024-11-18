using UnityEngine;

public static class PrintNumCounter
{
    public enum IncrementType
    {
        Plus,
        Minus,
    }

    private const int MaximumNum = 50;

    private static int printNum = 1;

    public static void UpdatePrintNum(int num, IncrementType incrementType)
    {
        switch (incrementType)
        {
            case IncrementType.Plus:
                printNum += num;
                break;
            case IncrementType.Minus:
                printNum -= num;
                break;
            default:
               printNum += num;
               break;
        }
        
        if (printNum <= 0) {
            Reset();
        }
        else if (printNum > MaximumNum)
        {
            printNum = MaximumNum;
        }
    }

    public static int GetPrintNum()
    {
        return printNum;
    }

    public static void Reset()
    {
        printNum = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterPlugin : MonoBehaviour
{
    // Native methods declarations
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass printerJavaClass;
#endif

    // Initialize the printer plugin
    public static void Initialize()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        printerJavaClass = new AndroidJavaClass("com.yourpackage.PrinterPlugin");
#endif
    }

    // Discover available printers
    public static string[] DiscoverPrinters()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (printerJavaClass != null)
        {
            return printerJavaClass.CallStatic<string[]>("DiscoverPrinters");
        }
#endif

        // Return an empty array for unsupported platforms or errors
        return new string[0];
    }

    // Print the CSV content
    public static void PrintCSV(string printerIP, string csvContent)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (printerJavaClass != null)
        {
            printerJavaClass.CallStatic("PrintCSV", printerIP, csvContent);
        }
#endif
    }
}

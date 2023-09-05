using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;


public class PrinterManger : MonoBehaviour
{
    public string printerUrl = "http://192.168.15.42:3910/"; // Replace with the actual printer's IPP URL

    public void print()
    {
        string contentToPrint = "Hello, World!"; // Replace with your content

        // Call the plugin to print to the internet printer
        Printer.PrintToInternetPrinter(printerUrl, contentToPrint);
        Debug.Log(contentToPrint);  
    }
}

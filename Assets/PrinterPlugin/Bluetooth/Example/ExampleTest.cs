using System;
using System.Collections;
using System.Collections.Generic;
using TinyG.PrintBlueToothPlugin;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ExampleTest : ExampleBridge
{
    protected override void GetListDevice() { PrinterConnector.Instance.GetListDevice(); }
    protected override void Connect(string device) { PrinterConnector.Connect(device); }
    protected override void Disconnect() { PrinterConnector.Disconnect(); }
    protected override void AddLisener()
    {
        PrinterConnector.Instance.OnGetListDevice = OnGetListDevice;
        PrinterConnector.Instance.OnConnectSuscess = OnConnectSuscess;
        PrinterConnector.Instance.OnDisconnect = OnDisConnect;
        PrinterConnector.Instance.OnNotFoundDevice = OnNotFoundDevice;
        PrinterConnector.Instance.OnCanNotConnectDeviceCB = OnCanNotConnectDevice;
        PrinterConnector.Instance.OnPrintErrorCB = OnPrintErrorCB;
    }

    bool PrinterPrepare
    {
        get
        {
            return true;
        }
    }

    protected override void Start()
    {
        Permission.RequestUserPermission("android.permission.BLUETOOTH_CONNECT");
        TextTitle = "Printer Bluetooth 4/2023";
        UpdateTextTitle(TextTitle);
        base.Start();
    }

    #region PRINT COMMAND

    public override void PrintText()
    {
        if (PrinterPrepare)
        {
            PrinterConnector.SetAlign(AlignType.CENTER);
         //   PrintText(inputText.text);
            PrintText(PrintingCSV.csvContent);
            PrinterConnector.AddFeed();
        }
    }
    public override void PrintText(string text)
    {
        base.PrintText();
        PrinterConnector.PrintText(text);
    }
    public void PrintBytes(byte[] bytes)
    {
        PrinterConnector.PrintBytes(bytes);
    }

    public override void PrintImageTransparent()
    {
        if (PrinterPrepare)
        {
            base.PrintImageTransparent();
            byte[] imageData = textureTransparent.EncodeToPNG();
            PrinterConnector.SetAlign(AlignType.CENTER);
            PrinterConnector.PrintImage(imageData, TypePrintImage.RBGA); // test is transparent
            PrinterConnector.AddFeed(FeedType.LONG);
            PrinterConnector.AddFeed(FeedType.LONG);
        }
    }
    public override void PrintImage()
    {
        if (PrinterPrepare)
        {
            byte[] imageData = textureNew.EncodeToPNG();
            PrintImage(imageData);
        }
    }
    public override void PrintImage(byte[] imageData)
    {
        if (PrinterPrepare)
        {
            base.PrintImage(imageData);
            PrinterConnector.SetAlign(AlignType.CENTER);
            PrinterConnector.PrintImage(imageData);
            PrinterConnector.AddFeed(FeedType.LONG);
            PrinterConnector.AddFeed(FeedType.LONG);
        }
    }


    public override void PrintQRCode()
    {
        if (PrinterPrepare)
        {
            base.PrintQRCode();
            PrinterConnector.SetAlign(AlignType.LEFT);
            PrinterConnector.PrintQRcode(testQRcode.content, testQRcode.width);
            PrinterConnector.AddFeed(FeedType.LONG);
            PrinterConnector.AddFeed(FeedType.LONG);
        }
    }

    public override void PrintBarcode()
    {
        if (PrinterPrepare)
        {
            base.PrintBarcode();
            PrinterConnector.SetAlign(AlignType.RIGHT);
            PrinterConnector.PrintBarcode(testBarcode.content, testBarcode.width, testBarcode.height);
            PrinterConnector.AddFeed(FeedType.LONG);
            PrinterConnector.AddFeed(FeedType.LONG);
        }
    }

    public override void PrintAddFeed()
    {
        if (PrinterPrepare)
        {
            base.PrintAddFeed();
            PrinterConnector.AddFeed();
        }
    }
    public override void AddFeed(FeedType feedType)
    {
        base.AddFeed(feedType);
        PrinterConnector.AddFeed(feedType);
    }
    public override void PrintAddSmallFeed()
    {
        if (PrinterPrepare)
        {
            base.PrintAddSmallFeed();
            PrinterConnector.AddFeed(FeedType.SHORT);
        }
    }

    public override void PrintAddLongFeed()
    {
        if (PrinterPrepare)
        {
            base.PrintAddLongFeed();
            PrinterConnector.AddFeed(FeedType.LONG);
        }
    }

    public override void CutPage()
    {
        if (PrinterPrepare)
        {
            base.CutPage();
            PrinterConnector.CutPage();
        }
    }

    public override void SetAlign(AlignType alignType)
    {
        base.SetAlign(alignType);
        PrinterConnector.SetAlign(alignType);
    }
    public override void SetFormatText(FormatTextType formatTextType)
    {
        base.SetFormatText(formatTextType);
        PrinterConnector.SetFormatText(formatTextType);
    }
    public override void PrintExampleBill()
    {
        if (PrinterPrepare)
        {
            base.PrintExampleBill();
        }

    }
    #endregion
}

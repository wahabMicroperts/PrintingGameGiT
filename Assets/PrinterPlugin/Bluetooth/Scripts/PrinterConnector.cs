using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyG.PrintBlueToothPlugin
{

    public class PrinterConnector : IPrintConnector<PrinterConnector>
    {
        public string TargetPrinterAdress { get; set; }
        AndroidPluginCallback pluginCallback;

        // Use this for initialization
        protected override void Start()
        {
            //string listDevice = "Printer001#DC:0D:30:8B:C2:6E$";
            //Debug.Log(listDevice);
            //if (listDevice[listDevice.Length - 1] == '$')
            //{
            //    listDevice = listDevice.Substring(0, listDevice.Length - 1);
            //}
            //Debug.Log(listDevice);

            //List<string> list = new List<string>();
            //string[] arr = listDevice.Split('$');
            //list.AddRange(arr);
            //Debug.Log("list device = " + list.Count);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    Debug.Log(list[i]);
            //}

            ListDevice = new List<string>();
#if UNITY_EDITOR
            List<string> test = new List<string>()
            {
                "TM-m30_030387#00:01:90:97:5E:DB",
                "PG-9135#F7:C6:16:F8:DE:45",
                "Printer001#DC:0D:30:8B:C2:6E",
                "BT-80#66:12:CA:E2:FD:20",
                "Phòng ngủ#44:07:0B:AB:B6:0B"
            };
            ListDevice = test;
#endif
            base.Start();
        }


        #region INIT NATIVE CODE
        protected override void Init()
        {
            pluginCallback = new AndroidPluginCallback(OnConnected, OnDisconnectCB, OnGetListDeviceSuccess, OnBluetoothNotOpen, OnNotFoundAnyDevice, OnCanNotConnectDevice);
#if !UNITY_EDITOR && UNITY_ANDROID
        Debug.Log("Start");
        var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jc = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        jo = new AndroidJavaObject("com.tinyg.print_bluetooth_unity.PrinterConnectorBlueTooth");
        jo.Call("StartPrinter", pluginCallback, jc);
#endif
        }
        #endregion


        #region CONNECT AND DISCONNECT COMMAND
        public static void Connect(string address)
        {
            Instance.ConnectPrinter(address);
        }

        public static void Disconnect()
        {
            Instance.DisconnectPrinter();
        }
        #endregion


        #region CALLBACK FROM NATIVE
        public void OnConnected(string nameDevice, string address)
        {
            if (isConnected) return;
            isConnected = true;
            if (OnConnectSuscess != null) OnConnectSuscess.Invoke(nameDevice);
            TargetPrinterName = nameDevice;
            TargetPrinterAdress = address;
            Debug.Log("callback OnConnected ");
        }
        void OnBluetoothNotOpen()
        {
            if (OnNotFoundDevice != null) OnNotFoundDevice.Invoke();
            Debug.Log("callback OnBluetoothNotOpen ");
        }

        #endregion


        #region PRINT COMMAND
        /// <summary>
        /// Size max width = 576 and max height = 576
        /// Sometime depend by content, too much content -> big size, cannot transfer to mobile
        /// </summary>
        /// <param name="content">any text number - 1234567890</param>
        /// <param name="width">Max = 576</param>
        /// <param name="height">Max = 576</param>
        public static void PrintBarcode(String content, int width, int height)
        {
            Instance.PrintBarcodePrinter(content, width, height);
        }
        /// <summary>
        /// Size max = 576
        /// should be not too much content, the best length = 150
        /// </summary>
        /// <param name="content">string QR</param>
        /// <param name="size">Max = 576px - if size too big, it will be resize to 320px</param>
        public static void PrintQRcode(String content, int size)
        {
            Instance.PrintQRcodePrinter(content, size);
        }
        /// <summary>
        /// Max size = 576
        /// if size too big, it will be resize to 360px
        /// </summary>
        /// <param name="data">any byte array</param>
        public static void PrintImage(byte[] data)
        {
            Instance.PrintImagePrinter(data);
        }

        public static void PrintImage(byte[] data, TypePrintImage typePrintImage)
        {
            Instance.PrintImagePrinter(data, typePrintImage);
        }

        public static void PrintImage(byte[] data, TypePrintImage typePrintImage, ColorInkType colorType)
        {
            Instance.PrintImagePrinter(data, typePrintImage, colorType);
        }

        public static void PrintText(string data)
        {
            Instance.PrintTextPrinter(data);
        }
        public static void PrintBytes(byte[] bytes)
        {
            Instance.PrintBytesPrinter(bytes);
        }
        #endregion


        #region SUPPORT FUNCTION
        public void GetListDevice()
        {
            GetListDevices();
        }

        // Default Addfeed normal = 20px
        public static void AddFeed()
        {
            Instance.AddFeedPage();
        }

        public static void AddFeed(FeedType type)
        {
            Instance.AddFeedPage(type);
        }

        public static void SetFormatText(FormatTextType format)
        {
            Instance.SetFormatTextString(format);
        }

        public static void CutPage()
        {
            Instance.CutPages();
        }

        public static void SetAlign(AlignType align)
        {
            Instance.SetAlignLine(align);
        }

        public static bool CheckPrintable()
        {
            return Instance.IsPrintable;
        }
        #endregion
    }
}
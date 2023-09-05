
using System.Collections.Generic;
using System;
using UnityEngine;
using TinyG.PrintBlueToothPlugin;

public class IPrintConnector<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    protected virtual void Awake()
    {
        CheckInstance();

    }

    protected bool CheckInstance()
    {
        if (this == Instance) { return true; }
        Destroy(this);
        return false;
    }
    public List<string> ListDevice { get; set; } = new List<string>();
    public Action<List<string>> OnGetListDevice = null;
    public Action<string> OnConnectSuscess = null;
    public Action OnNotFoundDevice = null;
    public Action OnDisconnect = null;
    public Action OnCanNotConnectDeviceCB = null;
    public Action<PrintErrorCode> OnPrintErrorCB = null;

    public static AndroidJavaObject jc;
    public static AndroidJavaObject jo;

    public string TargetPrinterName { get; set; }
    protected bool isConnected = false;

    protected virtual void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    protected virtual void Init() { }
    #region CONNECT AND DISCONNECT COMMAND
    /// <summary>
    /// Use for Bluetooth
    /// </summary>
    /// <param name="address"></param>
    public virtual void ConnectPrinter(string address)
    {
#if !UNITY_EDITOR && UNITY_ANDROID

        Debug.Log("Connect");
        jo.Call("Connect", address);
#endif
    }

    /// <summary>
    /// User for USB OTG
    /// </summary>
    /// <param name="deviceId"></param>
    public virtual void ConnectPrinter(int deviceId)
    {
#if !UNITY_EDITOR && UNITY_ANDROID

        Debug.Log("Connect");
        jo.Call("Connect", deviceId);
#endif
    }

    public virtual void DisconnectPrinter()
    {
        Debug.Log("Disconnect click");
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("Disconnect");
#endif
    }

    public virtual bool PreparePrinter()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        return jo.Call<bool>("Prepare");
#endif
        return false;
    }
    #endregion


    #region CALLBACK FROM NATIVE
    protected void OnDisconnectCB()
    {
        isConnected = false;
        if (OnDisconnect != null) OnDisconnect.Invoke();
        Debug.Log("callback OnDisconnect ");
    }

    protected void OnGetListDeviceSuccess(List<string> listDevice)
    {
        this.ListDevice = listDevice;
        if (OnGetListDevice != null) OnGetListDevice.Invoke(listDevice);
        Debug.Log("callback OnGetListDeviceSuccess ");
    }

    protected void OnNotFoundAnyDevice()
    {
        if (OnNotFoundDevice != null) OnNotFoundDevice.Invoke();
        Debug.Log("callback OnNotFoundAnyDevice ");
    }
    protected void OnCanNotConnectDevice()
    {
        if (OnCanNotConnectDeviceCB != null) OnCanNotConnectDeviceCB.Invoke();
        Debug.Log("callback OnCanNotConnectDevice ");
    }

    protected void OnPrintError(int typeError)
    {
        if (OnPrintErrorCB != null) { OnPrintErrorCB.Invoke((PrintErrorCode)typeError); }
    }
    #endregion


    #region PRINT COMMAND

    public virtual void PrintBarcodePrinter(String content, int width, int height)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintBarcode", content, width, height);
#endif
    }

    public virtual void PrintQRcodePrinter(String content, int size)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintQRcode", content, size);
#endif
    }

    public virtual void PrintImagePrinter(byte[] data)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintImage", data);
#endif
    }

    public virtual void PrintImagePrinter(byte[] data, TypePrintImage typePrintImage)
    {
        int type = (int)typePrintImage;
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintImage", data, type);
#endif
    }

    public virtual void PrintImagePrinter(byte[] data, TypePrintImage typePrintImage, ColorInkType colorType)
    {
        int type = (int)typePrintImage;
        string color = colorType.ToString();
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintImage", data, type, color);
#endif
    }

    public virtual void PrintTextPrinter(string data)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintText", data);
#endif
    }

    public virtual void PrintBytesPrinter(byte[] bytes)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("PrintBytes", bytes);
#endif
    }

    #endregion


    #region SUPPORT FUNCTION
    public virtual void GetListDevices()
    {
        //Get List Device
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("GetListDevice");
#endif
    }

    // Default Addfeed normal = 20px
    public virtual void AddFeedPage()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("AddFeed");
#endif
    }

    public virtual void AddFeedPage(FeedType type)
    {
        string feed = type.ToString();
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("AddFeed", feed);
#endif
    }

    public virtual void SetFormatTextString(FormatTextType format)
    {
        string type = format.ToString();
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("SetFormatText", type);
#endif
    }

    public virtual void CutPages()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("CutPage");
#endif
    }

    public virtual void SetAlignLine(AlignType align)
    {
        int alignInt = (int)align;
#if !UNITY_EDITOR && UNITY_ANDROID
        jo.Call("SetAlign", alignInt);
#endif
    }

    public virtual bool IsPrintable => isConnected;
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AndroidPluginCallback : AndroidJavaProxy
{
    Action OnDeviceFoundCallback = null,
           OnDoneSearchingCallback = null,
           OnStartDisconnectCallback = null,
           OnDisconnectCallback = null,
           OnBluetoothNotOpenCallback = null,
           OnNotFoundAnyDeviceCallback = null,
           OnCanNotConnectDeviceCallback;
    Action<string, string> OnConnectedCallback = null;
    Action<List<string>> OnGetListDeviceSuccessCallback;

    public AndroidPluginCallback() : base("com.tinyg.print_bluetooth_unity.PluginCallback") { }

    public AndroidPluginCallback(
        Action<string, string> OnConnectedCallback,
        Action OnDisconnectCallback,
        Action<List<string>> OnGetListDeviceSuccessCallback,
        Action OnBluetoothNotOpenCallback = null,
        Action OnNotFoundAnyDeviceCallback = null,
        Action OnCanNotConnectDeviceCallback = null,
        //maybe not use
        Action OnDeviceFoundCallback = null,
        Action OnDoneSearchingCallback = null,
        Action OnStartDisconnectCallback = null) : base("com.tinyg.print_bluetooth_unity.PluginCallback")
    {
        this.OnDisconnectCallback = OnDisconnectCallback;
        this.OnConnectedCallback = OnConnectedCallback;

        this.OnGetListDeviceSuccessCallback = OnGetListDeviceSuccessCallback;
        this.OnBluetoothNotOpenCallback = OnBluetoothNotOpenCallback;
        this.OnNotFoundAnyDeviceCallback = OnNotFoundAnyDeviceCallback;
        this.OnCanNotConnectDeviceCallback = OnCanNotConnectDeviceCallback;

        this.OnDeviceFoundCallback = OnDeviceFoundCallback;
        this.OnDoneSearchingCallback = OnDoneSearchingCallback;
        this.OnStartDisconnectCallback = OnStartDisconnectCallback;
    }

    void OnDeviceFound()
    {
        if (OnDeviceFoundCallback != null)
            OnDeviceFoundCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnDeviceFound ");
    }
    void OnDoneSearching()
    {
        if (OnDoneSearchingCallback != null)
            OnDoneSearchingCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnDoneSearching ");
    }
    void OnStartDisconnect()
    {
        if (OnStartDisconnectCallback != null)
            OnStartDisconnectCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnStartDisconnect ");
    }

    void OnConnected(string nameDevice, string address)
    {
        if (OnConnectedCallback != null)
            OnConnectedCallback.Invoke(nameDevice, address);
        Debug.Log("AndroidPluginCallback callback OnConnected " + nameDevice + "   " + address);
    }
    void OnDisconnect()
    {
        if (OnDisconnectCallback != null)
            OnDisconnectCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnDisconnect ");
    }

    void OnGetListDeviceSuccess(string listDevice)
    {
        if(listDevice[listDevice.Length - 1] == '$')
        {
            listDevice = listDevice.Substring(0, listDevice.Length - 1);
        }
        List<string> list = new List<string>();
        string[] arr = listDevice.Split('$');
        list.AddRange(arr);
        if (OnGetListDeviceSuccessCallback != null)
            OnGetListDeviceSuccessCallback.Invoke(list);
        Debug.Log("AndroidPluginCallback callback OnGetListDeviceSuccess \n" + listDevice);
    }
    void OnBluetoothNotOpen()
    {
        if (OnBluetoothNotOpenCallback != null)
            OnBluetoothNotOpenCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnBluetoothNotOpen ");
    }
    void OnNotFoundAnyDevice()
    {
        if (OnNotFoundAnyDeviceCallback != null)
            OnNotFoundAnyDeviceCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnNotFoundAnyDevice ");
    }

    void OnCanNotConnectDevice()
    {
        if (OnCanNotConnectDeviceCallback != null) OnCanNotConnectDeviceCallback.Invoke();
        Debug.Log("AndroidPluginCallback callback OnCanNotConnectDevice ");
    }
}

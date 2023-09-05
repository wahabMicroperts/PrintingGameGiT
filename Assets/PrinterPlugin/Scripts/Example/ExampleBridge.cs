using System;
using System.Collections;
using System.Collections.Generic;
using TinyG.PrintBlueToothPlugin;
using UnityEngine;
using UnityEngine.UI;

public class ExampleBridge : MonoBehaviour
{
    public printing PrintingCSV;
    public Text textTitle;
    public Texture2D textureNew;
    public Texture2D textureTransparent;
    public Text textLog;
    public GameObject prefabloading;

    public GameObject listDevicePannel;
    public GameObject prefabbuttonItem;
    public Transform parentContent;

    public Text txtBarcode;
    public Text txtQRcode;
    public InputField inputText;

    public Button btnConnect;
    public Button btnDisconnect;
    public Button btnPrintText;
    public Image imageTexture;
    public Button btnPrintImage;
    public Button btnLoadGallery;
    public Dropdown dropdownImage;
    public Texture2D[] listTextures;

    public Button btnPrintImageTransparent;
    public Button btnPrintQRCode;
    public Button btnPrintBarcode;
    public Button btnPrintExBill;
    public Button btnPrintAddFeed;
    public Button btnPrintCutPage;

    public Button btnBack;
    public Button btnGetListDevice;

    protected GameObject loading;
    protected List<GameObject> listDeviceObj = new List<GameObject>();

    protected class Content
    {
        public string content;
        public int width;
        public int height;
        public Content(string content, int width, int height)
        {
            this.content = content;
            this.width = width;
            this.height = height;
        }
    }

    protected Content testQRcode = new Content("Hello Printer Plugin!", 500, 500);
    protected Content testBarcode = new Content("1234554321", 500, 200);
    public string TextTitle = "Printer Plugin USB V2.0";
    //public const string TexTitleBlueTooth = "Printer Bluetooth V2.0";

    protected void Log(string str)
    {
        textLog.text += ">> " + str + "\n";
    }

    // Use this for initialization
    protected virtual void Start()
    {

        btnConnect.onClick.AddListener(() =>
        {
            listDevicePannel.SetActive(true);
            loading = CreateLoading();
            StartCoroutine(WaitToPrintData(() =>
            {
                GetListDevice();
            }));
        });

        btnDisconnect.interactable = false;
        btnDisconnect.onClick.AddListener(() =>
        {
            loading = CreateLoading();
            StartCoroutine(WaitToPrintData(() =>
            {
                Disconnect();
            }));
        });

        btnPrintText.interactable = false;
        btnPrintText.onClick.AddListener(PrintText);

        btnPrintImage.interactable = false;
        btnPrintImage.onClick.AddListener(PrintImage);

        btnLoadGallery.interactable = false;
        btnLoadGallery.onClick.AddListener(PrintImageGallery);
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (var t in listTextures)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = t.name.ToString();
            options.Add(option);
        }

        dropdownImage.interactable = false;
        // Đưa danh sách tùy chọn vào dropdown
        dropdownImage.ClearOptions();
        dropdownImage.AddOptions(options);
        dropdownImage.onValueChanged.AddListener(ChangeDropdown);

        btnPrintImageTransparent.interactable = false;
        btnPrintImageTransparent.onClick.AddListener(PrintImageTransparent);

        txtQRcode.text = "Content: " + testQRcode.content + "\nSize: " + testQRcode.width + "px";
        btnPrintQRCode.interactable = false;
        btnPrintQRCode.onClick.AddListener(PrintQRCode);

        txtBarcode.text = "Content: " + testBarcode.content + "\nWidth: " + testBarcode.width + "px\nHeight: " + testBarcode.height + "px";
        btnPrintBarcode.interactable = false;
        btnPrintBarcode.onClick.AddListener(PrintBarcode);

        btnPrintExBill.interactable = false;
        btnPrintExBill.onClick.AddListener(PrintExampleBill);

        btnPrintAddFeed.interactable = false;
        btnPrintAddFeed.onClick.AddListener(PrintAddFeed);

        btnPrintCutPage.interactable = false;
        btnPrintCutPage.onClick.AddListener(CutPage);

        btnBack.onClick.AddListener(() =>
        {
            listDevicePannel.SetActive(false);
        });

        btnGetListDevice.onClick.AddListener(GetListDevice);
        AddLisener();
    }

    protected virtual void UpdateTextTitle(string title)
    {
        textTitle.text = title;
    }
    protected virtual void GetListDevice() { }
    protected virtual void Connect(string device) { }
    protected virtual void Disconnect() { }
    protected virtual void AddLisener() { }

    #region PRINT COMMAND

    public virtual void PrintText()
    {
        Log("Look Printer - Print Text................. ");
    }
    public virtual void PrintText(string text)
    {
    }

    public virtual void PrintImageTransparent()
    {
        Log("Look Printer - Print Image Transparent ................. ");
    }
    public virtual void PrintImage()
    {
        Log("Look Printer - Print Image ................. ");
    }
    public virtual void PrintImage(byte[] data)
    {
        Log("Look Printer - Print Image byte[]................. ");
    }

    public virtual void PrintImageGallery()
    {
        LoadImageFromGallery();
    }

    public virtual void PrintQRCode()
    {
        Log("Look Printer - PrintQRCode ................. ");
    }

    public virtual void PrintBarcode()
    {
        Log("Look Printer - PrintBarcode ................. ");
    }

    public virtual void PrintAddFeed()
    {
        Log("Look - PrintAddFeed ................. ");
    }
    public virtual void AddFeed(FeedType feedType) { }
    public virtual void PrintAddSmallFeed()
    {
        Log("Look - PrintAddFeed ................. ");
    }

    public virtual void PrintAddLongFeed()
    {
        Log("Look - PrintAddFeed ................. ");
    }

    public virtual void CutPage()
    {
        Log("Look - CutPage ................. ");
    }
    public virtual void SetAlign(AlignType alignType) { }
    public virtual void SetFormatText(FormatTextType formatTextType) { }

    public virtual void PrintExampleBill()
    {
        SetAlign(AlignType.CENTER);
        SetFormatText(FormatTextType.TEXT_FORMAT_HEADER_BOLD);
        PrintText(TextTitle); AddFeed(FeedType.SHORT);

        PrintText("-----------------------"); AddFeed(FeedType.SHORT);
        SetFormatText(FormatTextType.TEXT_FORMAT_DEFAULT);
        PrintText("Address: A1 - BCD 4 - Star world"); AddFeed(FeedType.LONG);
        SetFormatText(FormatTextType.TEXT_FORMAT_WIDTH);
        PrintText("Bill Gate"); AddFeed(FeedType.LONG);

        SetAlign(AlignType.LEFT);
        SetFormatText(FormatTextType.TEXT_FORMAT_DEFAULT);
        PrintText("Date : " + DateTime.Now.ToString()); PrintAddFeed();
        PrintText("Table: " + UnityEngine.Random.Range(10, 1000)); PrintAddFeed();

        SetAlign(AlignType.CENTER);
        SetFormatText(FormatTextType.TEXT_FORMAT_WIDTH);
        PrintText("Price tag"); PrintAddFeed();
        SetFormatText(FormatTextType.TEXT_FORMAT_DEFAULT);
        PrintText("----------------------------------"); PrintAddFeed();
        SetAlign(AlignType.LEFT);
        PrintText("Thit cho ham xuong : 10$"); PrintAddFeed();
        PrintText("Bong cai xao       : 4$"); PrintAddFeed();
        PrintText("Bau luoc hot vit   : 7$"); PrintAddFeed();
        PrintText("Bia 333            : 103$"); PrintAddFeed();
        PrintText("So huyet xao me    : 15$"); PrintAddFeed();
        PrintText("Thit lon luoc      : 12$"); PrintAddFeed();
        PrintText("Dua hau trang mieng: 8$"); PrintAddFeed();
        SetAlign(AlignType.CENTER);
        PrintText("----------------------------------"); PrintAddFeed();
        SetAlign(AlignType.LEFT);
        PrintText("Tong               : 159&"); PrintAddFeed();

        SetAlign(AlignType.CENTER);
        SetFormatText(FormatTextType.TEXT_FORMAT_SMALL_BOLD);
        PrintText("Thanks and gook luck!");
        PrintAddFeed(); PrintAddFeed();
        PrintAddFeed(); PrintAddFeed();
        PrintAddFeed(); PrintAddFeed();
        CutPage();
    }

    #endregion


    #region CALLBACK
    protected void OnGetListDevice(List<string> listDevice)
    {
        Log("Get List Sucess: list device = " + listDevice.Count);
        if (loading) Destroy(loading);
        foreach (var item in listDeviceObj)
        {
            Destroy(item);
        }
        listDeviceObj.Clear();

        for (int i = 0; i < listDevice.Count; i++)
        {
            var item = CreateObject(prefabbuttonItem, parentContent);
            item.GetComponentInChildren<Text>().text = listDevice[i];
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                ConnectPrinter(item.GetComponentInChildren<Text>().text);
            });
            listDeviceObj.Add(item);
        }
    }

    protected void ConnectPrinter(string address)
    {
        Log("Try connect to printer. address = " + address);
        string[] arr = address.Split('#');
        if (arr.Length > 0)
        {
            listDevicePannel.SetActive(false);
            loading = CreateLoading(); ;
            StartCoroutine(WaitToPrintData(() =>
            {
                Connect(arr[1]);
            }));
        }
    }

    public void OnConnectSuscess(string nameDevice)
    {
        Log("On Connect Printer " + nameDevice + " Suscess.");
        if (loading) Destroy(loading);
        btnDisconnect.interactable = true;
        btnConnect.interactable = false;

        btnPrintImage.interactable = true;
        btnLoadGallery.interactable = true;
        dropdownImage.interactable = true;
        btnPrintImageTransparent.interactable = true;
        btnPrintText.interactable = true;
        btnPrintQRCode.interactable = true;
        btnPrintBarcode.interactable = true;
        btnPrintExBill.interactable = true;
        btnPrintAddFeed.interactable = true;
        btnPrintCutPage.interactable = true;
    }

    protected void OnDisConnect()
    {
        Log("On Disconnect Device!");
        if (loading) Destroy(loading);
        btnConnect.interactable = true;

        btnPrintImage.interactable = false;
        btnLoadGallery.interactable = false;
        dropdownImage.interactable = false;
        btnPrintImageTransparent.interactable = false;
        btnPrintText.interactable = false;
        btnDisconnect.interactable = false;

        btnPrintQRCode.interactable = false;
        btnPrintBarcode.interactable = false;
        btnPrintExBill.interactable = false;
        btnPrintAddFeed.interactable = false;
        btnPrintCutPage.interactable = false;
    }
    protected void OnNotFoundDevice()
    {
        Log("On Not Found Device!");
        if (loading) Destroy(loading);

    }

    protected void OnCanNotConnectDevice()
    {
        Log("On Can Not Connect Device!");
        if (loading) Destroy(loading);
        //PopupManager.ShowPopupBase(Config.txtStatusCannotConnectPrinter);
    }
    protected void OnPrintErrorCB(PrintErrorCode code)
    {
        switch (code)
        {
            case PrintErrorCode.More_Than_Max_Length:
                Log("Bytes Array more than max length = 16384 \n Check width, heigth image over max size");
                break;
            case PrintErrorCode.Not_Found_Printer:
                Log("Not Found Printer, Attack usb printer");
                break;
        }
    }
    #endregion

    protected GameObject CreateLoading()
    {
        return CreateObject(prefabloading, FindObjectOfType<Canvas>().transform);
    }
    protected GameObject CreateObject(GameObject pre, Transform parent)
    {
        return Instantiate(pre, parent);
    }

    IEnumerator WaitToPrintData(Action runAction)
    {
        yield return new WaitForSeconds(.2f);
        runAction();
    }
    protected void ChangeDropdown(int value)
    {
        Debug.Log(" Value == " + value);
        byte[] imageData = listTextures[value].EncodeToPNG();
        PrintImage(imageData);
    }
    protected void LoadImageFromGallery()
    {
        int maxSize = 512;
        //NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        //{
        //    Debug.Log("Image path: " + path);
        //    if (path != null)
        //    {
        //        // Create Texture from selected image
        //        Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
        //        if (texture == null)
        //        {
        //            Debug.Log("Couldn't load texture from " + path);
        //            return;
        //        }
        //        imageTexture.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //        texture = GetImage(texture);
        //        Log("Look Printer - Print Image ................. ");
        //        byte[] imageData = texture.EncodeToPNG();
        //        PrintImage(imageData);
        //    }

        //});
    }
    protected Texture2D GetImage(Texture2D texture)
    {
        // Lấy dữ liệu raw của texture
        byte[] rawTextureData = texture.GetRawTextureData();

        // Xử lý dữ liệu raw ở đây
        // ...

        // Tạo texture mới và đưa dữ liệu vào
        Texture2D newTexture = new Texture2D(texture.width, texture.height);
        newTexture.LoadRawTextureData(rawTextureData);
        newTexture.Apply();
        return newTexture;
    }
}
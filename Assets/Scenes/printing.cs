using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;


public class printing : MonoBehaviour
{

    private string fileName = "data.csv"; // Name of the CSV file
    public TMP_InputField Name;
    public TMP_InputField OPertator;
    public TMP_InputField MachineId;
    public TMP_InputField Operater2;
    public TMP_InputField Location;


    [HideInInspector] public string csvContent;
    [HideInInspector] public string filePath;


    private void Start()
    {
        csvContent = null;

        // GenerateCSV();

    }

    [ContextMenu("GenrateFile")]
    public void GenerateCSV()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
       //  DirectoryHelper.GetAndroidExternalFilesDir();
#endif
        string names = Name.text;
        string operators = OPertator.text;
        string operators2 = Operater2.text;
        string machineid = MachineId.text;
        string locations = Location.text;

        DateTime currentDateTime = DateTime.Now;
        Debug.Log("Current Date and Time: " + currentDateTime);
        string dateTime = currentDateTime.ToString();
        // Sample data
        string[] time = { dateTime };
        string[] headers = { "Name", "Operator", "Operator2", "MachineId", "Location" };
        string[] row1 = { names, operators, operators2, machineid, locations };

        // string[] row2 = { "Jane Smith", "25", "Canada" };

        // Combine the data into CSV format
        csvContent = string.Join(",", headers) + "\n";
        csvContent += string.Join(",", row1) + "\n";
        csvContent += dateTime + "\n";

        filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(filePath, csvContent);
            Debug.Log("CSV file saved to internal storage: " + filePath);

              Application.OpenURL(filePath);
           // NativeControl.Instance().SaveCSVFileToGallery(csvContent,fileName, SaveImageLocation);



#if UNITY_ANDROID && !UNITY_EDITOR
         //   Application.OpenURL("/storage/emulated/0/");
            //Debug.Log("adnro");
#endif

        }
        catch (Exception e)
        {
            Debug.LogError("Error saving CSV file: " + e.Message);
        }
    }
    public void SaveImageLocation(string path)
    {
        Debug.Log("####SaveImageLocation#### " + path);
    }
}

    
 



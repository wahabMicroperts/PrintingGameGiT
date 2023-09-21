using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        int[] columnWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            int maxFieldWidth = Math.Max(headers[i].Length, row1[i].Length);
            columnWidths[i] = maxFieldWidth;
        }

        // Combine the data into CSV format with aligned columns
        csvContent = "";
        for (int i = 0; i < headers.Length; i++)
        {
            csvContent += headers[i].PadRight(columnWidths[i]);
            if (i < headers.Length - 1)
            {
                csvContent += " | ";
            }
        }
        csvContent += "\n";

        for (int i = 0; i < row1.Length; i++)
        {
            csvContent += row1[i].PadRight(columnWidths[i]);
            if (i < row1.Length - 1)
            {
                csvContent += " | ";
            }
        }
        csvContent += "\n";
        csvContent += dateTime + "\n";
        /*// Combine the data into CSV format
        csvContent = string.Join(" | ", headers) + "\n";
        csvContent += string.Join(" | ", row1) + "\n";
        csvContent += dateTime + "\n";*/



        try
        {
           
            //////////-----------------------------Testing For write and get--------------------

            string filePaths = Path.Combine(Application.temporaryCachePath, fileName);
            File.WriteAllText(filePaths, csvContent);

            // Export the file
            NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(filePaths, (success) => Debug.Log("File exported: " + success));

            /////---------------------------Pick File----------------
            NativeFilePicker.Permission permissions = NativeFilePicker.PickFile((path) =>
            {
                if (path == null)
                    Debug.Log("Operation cancelled");
                else
                    Debug.Log("Picked file: " + path);
            }, new string[] { fileName });

            Debug.Log("Permission result: " + permission);



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

    
 



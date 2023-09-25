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
    public TMP_InputField Credits;    

    [HideInInspector] public string headerContent = "";
    [HideInInspector] public  string row1Content = "";
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
        string credits  = Credits.text; 

        DateTime currentDateTime = DateTime.Now;
        Debug.Log("Current Date and Time: " + currentDateTime);
        string dateTime = currentDateTime.ToString();
        // Sample data
        string[] time = { dateTime };


        string[] headers = { "Name:", "Operator1:", "Operator2:", "MachineId:", "Location:","Credits" };
        string[] row1 = { names, operators, operators2, machineid, locations, credits };

        

        for (int i = 0; i < headers.Length || i < row1.Length; i++)
        {
            string header = i < headers.Length ? headers[i] : "";
            string value = i < row1.Length ? row1[i] : "";

            headerContent += header + "\n"; // Add each header to a new line
            row1Content += value + "\n";     // Add each row1 item to a new line
        }

        /*string[] headers = { "Name:", "Operator1:", "Operator2:", "MachineId:", "Location:" };
        string[] row1 = { names, operators, operators2, machineid, locations };

        int maxHeaderWidth = headers.Max(header => header.Length);
        int maxRow1Width = row1.Max(row => row.Length);

        csvContent = "";
        for (int i = 0; i < headers.Length || i < row1.Length; i++)
        {
            string header = i < headers.Length ? headers[i].PadRight(maxHeaderWidth) : string.Empty.PadRight(maxHeaderWidth);
            string value = i < row1.Length ? row1[i].PadRight(maxRow1Width) : string.Empty.PadRight(maxRow1Width); // Change PadRight to PadLeft here

            csvContent += header + "  " + value;

            if (i < headers.Length - 1 || i < row1.Length - 1)
            {
                csvContent += "\n"; // Move to the next line for the next set of columns
            }
        }
        csvContent += "\n" + "\n";*/
       // headerContent += "\n";
        headerContent += dateTime + "\n";
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

    
 



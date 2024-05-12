using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public static class FileManager
{
    public static bool WriteToFile(string filename, string data)
    {
        //Se descargan dentro de AppData\LocalLow\DefaultCompany\Pathfinding_game
        string fullPath = Path.Combine(Application.persistentDataPath, filename);
        
        try
        {
            if (!File.Exists(fullPath))
            {
                using (StreamWriter sw = File.CreateText(fullPath))
                {
                    sw.Write(data);
                }
            }
            else
            {
                File.WriteAllText(fullPath, data);
            }
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

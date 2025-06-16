using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string m_dataDirPath = "";
    private string m_dataFileName = "";
    private bool m_useEncryption = false;
    private readonly string m_encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        m_dataDirPath = dataDirPath;
        m_dataFileName = dataFileName;
        m_useEncryption = useEncryption;
    }

    public GameData Load()
    {
        // use Path.Combine to account for different OS,s having different path separators
        string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //optionally decrypt the data
                if(m_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //Deserialze the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // use Path.Combine to account for different OS,s having different path separators
        string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);
        try
        {
            //Create the directory the file will be written if it doesn't exist yet
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            //Optionally encrypt the data
            if(m_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //Write the serialized data to the file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char) (data[i] ^ m_encryptionCodeWord[i % m_encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

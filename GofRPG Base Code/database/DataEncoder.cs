using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// DataEncoder is a clss that is responsible for
/// Encoding and Decoding neccessary data for the
/// game.
/// </summary>
public class DataEncoder : Singleton<DataEncoder>
{
    private readonly string _key = "A60A5770FE5E7AB200BA9CFC94E4E8B0"; //set any string of 32 chars
    private readonly string _iv = "1234567887654321"; //set any string of 16 chars
    private static string _data;

    //Getters and Setters
    public static string GetData()
    {
        return _data;
    }

    void Start()
    {
        // if (!GameManager.Instance.encodedStreamingAssetsData)
        // {
        //     EncodeDatabaseFiles();
        //     GameManager.Instance.SetEncodedStreamingAssetsVariable();
        // }
    }

    /// <summary>
    /// Encodes all the data found in the
    /// streamingAssetsPath and places it in
    /// the persistentDataPath to use.
    /// </summary>
    public void EncodeDatabaseFiles()
    {

        int databaseIndex = 0;
        string saveDataPath = Units.SAVE_DATA_PATHS[databaseIndex];

        //if directory for databases already exists, delete old data inside
        if (Directory.Exists(Application.persistentDataPath + saveDataPath))
        {
            try
            {
                Debug.Log("Deleting paths... " + Application.persistentDataPath + saveDataPath);
                File.Delete(Application.persistentDataPath + saveDataPath);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UnauthorizedAccessException"))
                {
                    Debug.LogWarning("Unauthorized Access... need to use FileUtil...");
#if UNITY_EDITOR
                    FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + saveDataPath);
#endif
                }
            }
        }

        //if directory does not exist, create directory
        if (!Directory.Exists(Application.persistentDataPath + saveDataPath))
            Directory.CreateDirectory(Application.persistentDataPath + saveDataPath);

        //encode files from streamingAssetsPath to persistentDataPath
        foreach (string databasePath in Units.DATABASE_PATHS)
            EncodeAnyFile(Application.streamingAssetsPath, databasePath);
    }

    /// <summary>
    /// Encodes a particular file based on the 
    /// <paramref name="dataPath"/> and the 
    /// <paramref name="relativePath"/>, and places
    /// it inside the persistentDataPath location.
    /// </summary>
    /// <param name="dataPath">the directory used the place all the files</param>
    /// <param name="relativePath">the file and extension name</param>
    public void EncodeAnyFile(string dataPath, string relativePath)
    {
        Debug.Log("Encoding Paths: " + dataPath + relativePath);

        //take data from combined path
        // StartCoroutine(PullFileData(dataPath + relativePath));
        if (dataPath.Contains(Application.persistentDataPath))
            _data = File.ReadAllText(dataPath + relativePath);
        else
            StartCoroutine(PullFileData(dataPath + relativePath));

        string encryptedData = AESEncryption(_data);

        //write data inside persistent data path
        if (File.Exists(Application.persistentDataPath + relativePath))
            File.Delete(Application.persistentDataPath + relativePath);

        File.WriteAllText(Application.persistentDataPath + relativePath, encryptedData);
    }

    /// <summary>
    /// Decodes a particular file in the persistentDataPath
    /// location based on the <paramref name="path"/> and stores
    /// it inside the <c>_data</c> variable.
    /// </summary>
    /// <param name="path">relative path inside persistentDataPath</param>
    public void DecodePersistentDataFile(string path)
    {
        //take data from persistent assets path
        try
        {
            if (File.Exists(Application.persistentDataPath + path))
            {
                string encryptedData = File.ReadAllText(Application.persistentDataPath + path);
                _data = AESDecryption(encryptedData);
            }
            else
                Debug.LogWarning("WARNING: The file/location you are trying to decode: " + path + " does not exist...");
        }
        catch (Exception e)
        {
            if (e.Message.Contains("The input is not a valid Base-64 string as it contains a non-base 64 character"))
            {
                //This means file is not encoded and cannot be decoded.
                Debug.LogWarning("The file/location you wanted to decode: " + path + " was already dencoded...");
                _data = File.ReadAllText(Application.persistentDataPath + path);
                EncodeAnyFile(Application.persistentDataPath, path);
            }
            else
                throw e;
        }
    }

    public void GetStreamingAssetsFile(string path)
    {
        try
        {
            if (File.Exists(Application.streamingAssetsPath + path))
            {
                _data = File.ReadAllText(Application.streamingAssetsPath + path);
                Debug.Log("Retrieved data for " + path);
            }
            else
                Debug.LogWarning("file: " + Application.streamingAssetsPath + path + " does not exist!");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            _data = null;
        }
    }

    public IEnumerator GetStreamingAssetsFileWebGL(string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + path);

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                _data = www.downloadHandler.text;
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("ERROR: " + www.error);
                _data = null;
                break;
        }
    }

    /// <summary>
    /// Retrieves a row of data from based on the
    /// row's <paramref name="id"/>.
    /// </summary>
    /// <param name="id">the primary key for the row of data</param>
    /// <returns>a list of the row's data information seperated by commas.</returns>
    public string GetRowOfData(string id)
    {
        foreach (string row in _data.Split('\n'))
        {
            if (row.Contains(id))
                return row;
        }
        return null;
    }

    /// <summary>
    /// Retrieves several rows of data from based on the
    /// row's <paramref name="id"/>.
    /// </summary>
    /// <param name="id">the primary key for the row of data</param>
    /// <returns>a list of rows and their data information seperated by commas.</returns>
    public string[] GetRowsOfData(string id)
    {
        List<string> rows = new List<string>();

        foreach (string row in _data.Split('\n'))
        {
            if (row.Contains(id)) //TODO: change to row.Equals(id) and see what happens.
                rows.Add(row);
        }

        return rows.ToArray();
    }

    /// <summary>
    /// Retrieves all row data in the form of an array.
    /// </summary>
    /// <returns>a list of the row's data information seperated by commas.</returns>
    public string[] GetRowsOfData()
    {
        List<string> rows = new List<string>();

        int rowNum = 1;
        foreach (string row in _data.Split('\n'))
        {
            if (rowNum > 1 && row.Length > 0)
                rows.Add(row);
            rowNum++;
        }

        return rows.ToArray();
    }

    /// <summary>
    /// Clears the <c>_data</c> variable.
    /// </summary>
    public static void ClearData()
    {
        _data = "";
    }

    /// <summary>
    /// Pulls data from <paramref name="filePath"/> and
    /// stores it inside <c>_data</c> variable.
    /// </summary>
    /// <param name="filePath">full path to file</param>
    /// <returns></returns>
    private IEnumerator PullFileData(string filePath)
    {
        if (filePath.Contains("://"))
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(filePath);
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("ERROR WHEN GETTING DATA: " + webRequest.error);
                _data = null;
            }
            else
            {
                _data = webRequest.downloadHandler.text;
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
        else
        {
            Debug.LogWarning("Using non webgl method of reading data path " + filePath);
            if (File.Exists(filePath))
                _data = File.ReadAllText(filePath);
        }
    }

    /// <summary>
    /// Uses Advanced Encryption Standard to encrypt
    /// <paramref name="inputData"/>.
    /// </summary>
    /// <param name="inputData">data to be encrypted.</param>
    /// <returns>value of encrypted data</returns>
    private string AESEncryption(string inputData)
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider
        {
            BlockSize = 128,
            KeySize = 256,
            Key = ASCIIEncoding.ASCII.GetBytes(_key),
            IV = ASCIIEncoding.ASCII.GetBytes(_iv),
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7
        };

        byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(inputData);
        ICryptoTransform transform = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

        byte[] result = transform.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Uses Advanced Encryption Standard to decrypt
    /// <paramref name="inputData"/>.
    /// </summary>
    /// <param name="inputData">data to be decrypted.</param>
    /// <returns>value of decrypted data</returns>
    private string AESDecryption(string inputData)
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider
        {
            BlockSize = 128,
            KeySize = 256,
            Key = ASCIIEncoding.ASCII.GetBytes(_key),
            IV = ASCIIEncoding.ASCII.GetBytes(_iv),
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7
        };

        byte[] txtByteData = Convert.FromBase64String(inputData);
        ICryptoTransform transform = AEScryptoProvider.CreateDecryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

        byte[] result = transform.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return ASCIIEncoding.ASCII.GetString(result);
    }
}

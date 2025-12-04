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

    protected override void Awake()
    {
        base.Awake();
        EncodeAllFiles();
    }

    /// <summary>
    /// Encodes all the data found in the
    /// streamingAssetsPath and places it in
    /// the persistentDataPath to use.
    /// </summary>
    public void EncodeAllFiles()
    {
        //create direcories for encoded files
        foreach (string relativePath in Units.SAVE_DATA_PATHS)
        {
            //update databases by first deleting old data in game
            if (relativePath.Contains("database") && Directory.Exists(Application.persistentDataPath + relativePath))
            {
                try
                {
                    File.Delete(Application.persistentDataPath + relativePath);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("UnauthorizedAccessException"))
                    {
                        Debug.LogWarning("Unauthorized Access... need to use FileUtil...");
#if UNITY_EDITOR
                        FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + relativePath);
#endif
                    }
                }
            }

            if (!Directory.Exists(Application.persistentDataPath + relativePath))
                Directory.CreateDirectory(Application.persistentDataPath + relativePath);
        }

        //encode files from streamingAssetsPath to persistentDataPath
        foreach (string relativePath in Units.DATABASE_PATHS)
            EncodeFile(Application.streamingAssetsPath, relativePath);
    }

    /// <summary>
    /// Encodes a particular file based on the 
    /// <paramref name="dataPath"/> and the 
    /// <paramref name="relativePath"/>, and places
    /// it inside the persistentDataPath location.
    /// </summary>
    /// <param name="dataPath">the directory used the place all the files</param>
    /// <param name="relativePath">the file and extension name</param>
    public void EncodeFile(string dataPath, string relativePath)
    {
        //take data from combined path
        StartCoroutine(PullFileData(dataPath + relativePath));

        if (File.Exists(dataPath + relativePath))
            _data = File.ReadAllText(dataPath + relativePath);

        string encryptedData = AESEncryption(_data);

        //write data inside persistent data path
        if (File.Exists(dataPath + relativePath))
            File.Delete(Application.persistentDataPath + relativePath);

        File.WriteAllText(Application.persistentDataPath + relativePath, encryptedData);
    }

    /// <summary>
    /// Decodes a particular file in the persistentDataPath
    /// location based on the <paramref name="path"/> and stores
    /// it inside the <c>_data</c> variable.
    /// </summary>
    /// <param name="path">relative path inside persistentDataPath</param>
    public void DecodeFile(string path)
    {
        //take data from persistent assets path
        try
        {
            string encryptedData = File.ReadAllText(Application.persistentDataPath + path);
            _data = AESDecryption(encryptedData);
        }
        catch (Exception e)
        {
            if (!e.Message.Contains("The input is not a valid Base-64 string as it contains a non-base 64 character"))
            {
                Debug.LogWarning("WARNING: " + e.Message + "... Encoding and storing new files...");
                EncodeAllFiles();
                string encryptedData = File.ReadAllText(Application.persistentDataPath + path);
                _data = AESDecryption(encryptedData);
            }
            else
                throw e;
        }
    }

    /// <summary>
    /// Decodes a particular file in the persistentDataPath
    /// location based on the <paramref name="path"/> and stores
    /// it inside the <c>_data</c> variable. This also restores
    /// the previously encrypted file.
    /// </summary>
    /// <param name="path">relative path inside persistentDataPath</param>
    public void DecodeAndRestoreFile(string path)
    {
        string encryptedData = File.ReadAllText(Application.persistentDataPath + path);
        _data = AESDecryption(encryptedData);
        File.WriteAllText(Application.persistentDataPath + path, _data);
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
            if (row.Contains(id))
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
            UnityWebRequest webRequest = new UnityWebRequest(filePath);
            yield return webRequest;
            if (webRequest.result != UnityWebRequest.Result.Success)
                Debug.Log(webRequest.error);
            else
                _data = webRequest.downloadHandler.text;
        }
        else
            _data = File.ReadAllText(filePath);
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

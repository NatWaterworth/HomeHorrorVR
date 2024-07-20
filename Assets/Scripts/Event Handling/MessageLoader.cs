using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class MessageLoader : MonoBehaviour
{
    public static MessageLoader Instance { get; private set; }
    public string jsonFileName = "messages.json";

    private Dictionary<string, List<string>> messages;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMessages();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadMessages()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            messages = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(dataAsJson);
            Debug.Log("Messages loaded successfully.");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public List<string> GetMessages(string key)
    {
        if (messages != null && messages.ContainsKey(key))
        {
            return messages[key];
        }
        else
        {
            Debug.LogError("Key not found in messages.");
            return null;
        }
    }
}

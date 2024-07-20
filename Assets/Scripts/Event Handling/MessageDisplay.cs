using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MessageDisplay : MonoBehaviour
{
    // The TextMeshProUGUI object to update
    private TextMeshProUGUI targetTextComponent;

    // List of TMP_FontAsset objects to swap
    [SerializeField] List<TMP_FontAsset> fontAssets;

    void Start()
    {
        // Ensure that the messageLoader is assigned
        if (MessageLoader.Instance == null)
        {
            Debug.LogError("MessageLoader instance not found.");
            return;
        }

        // Create and configure TextMeshProUGUI component
        GameObject textObject = new GameObject("TextMeshPro Object");
        textObject.transform.SetParent(this.transform);
        targetTextComponent = textObject.AddComponent<TextMeshProUGUI>();

        // Set initial properties of TextMeshProUGUI
        targetTextComponent.fontSize = 36;
        targetTextComponent.alignment = TextAlignmentOptions.Center;
        LoadAndApplyMessages("avoidTheShadows");
    }

    void LoadAndApplyMessages(string messageKey)
    {
        List<string> messages = MessageLoader.Instance.GetMessages(messageKey);

        if (messages != null && messages.Count > 0)
        {
            // Apply the first message to the target TMP component
            targetTextComponent.text = messages[Random.Range(0, messages.Count-1)];

            // Swap the font asset if there are any font assets provided
            if (fontAssets != null && fontAssets.Count > 0)
            {
                targetTextComponent.font = fontAssets[Random.Range(0, fontAssets.Count - 1)]; // Swap to the first font asset in the list
            }

            Debug.Log("Message and font asset applied to TMP component successfully.");
        }
        else
        {
            Debug.LogError("No messages found or key not found.");
        }
    }

    public void SwapFontAsset(int index)
    {
        if (fontAssets != null && index >= 0 && index < fontAssets.Count)
        {
            targetTextComponent.font = fontAssets[index];
            Debug.Log("Font asset swapped successfully.");
        }
        else
        {
            Debug.LogError("Invalid font asset index.");
        }
    }
}

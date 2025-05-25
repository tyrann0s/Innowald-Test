using System;
using Managers;
using TMPro;
using UnityEngine;

public class ResGenPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ResourceManager resourceManager;

    private void Awake()
    {
        inputField.text = resourceManager.ResourceSpawnDelay.ToString();
    }

    public void ChangeResGenRate()
    {
        float value = float.Parse(inputField.text);
        if (value > 0) resourceManager.ResourceSpawnDelay = float.Parse(inputField.text);
    }
}

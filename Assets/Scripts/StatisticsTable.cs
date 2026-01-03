using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsTable : MonoBehaviour
{
    public GameObject tableRowPrefab;
    private List<DataManager.Player> _playerList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DataManager.Instance != null)
        {
            _playerList = DataManager.Instance.GetPlayerList();
            if (_playerList != null)
                GenerateTable();
        }
    }

    void GenerateTable()
    {
        
        foreach (DataManager.Player rowData in _playerList)
        {
            // Instantiate the prefab under the container
            GameObject newRow = Instantiate(tableRowPrefab, gameObject.transform);

            // Get the text components of the instantiated row's children
            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = rowData.name;
                texts[1].text = rowData.score.ToString();
            }
        }
    }
}

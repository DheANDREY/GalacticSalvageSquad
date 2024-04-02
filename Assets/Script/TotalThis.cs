using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TotalThis : MonoBehaviour
{
    public TextMeshProUGUI numberResources;
    public int itemNumber;
    // Update is called once per frame
    void Update()
    {
        numberResources.text = PlayerManager.resCounts[itemNumber-1].ToString();
    }
}

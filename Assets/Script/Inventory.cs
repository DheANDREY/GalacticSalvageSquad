using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; using TMPro;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    public TextMeshProUGUI[] numTotalItem;  public GameObject[] numTotal; 

    private void Update()
    {
        //for (int i = 0; i < slots.Length; i++)
        //{
        //    // Mencari indeks obstacle yang sesuai dengan slot ke-i
        //    int obstacleIndex = FindObstacleIndex(slots[i]);

        //    // Jika obstacleIndex valid, set text numTotalItem sesuai dengan resCounts obstacle tersebut
        //    if (obstacleIndex != -1)
        //    {
        //        numTotalItem[i].text = PlayerManager.resCounts[obstacleIndex].ToString();
        //    }
        //    else
        //    {
        //        // Jika tidak ditemukan obstacle yang sesuai, set text numTotalItem menjadi kosong
        //        numTotalItem[i].text = "";
        //    }
        //}
    }

    private int FindObstacleIndex(GameObject slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slot)
            {
                return i;
            }
        }

        // Jika tidak ditemukan, kembalikan nilai -1
        return -1;
    }
}

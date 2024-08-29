using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; transform.childCount > i; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
            menu.SetActive(true);
    }
}
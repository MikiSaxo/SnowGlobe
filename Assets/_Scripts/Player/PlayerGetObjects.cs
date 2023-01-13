using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   
public class PlayerGetObjects : MonoBehaviour
{
    public AudioManager audioManager;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Carotte>())
        {
            GetObject(0, collision.gameObject);
            audioManager.PlaySound("Item1");
        }
        if (collision.gameObject.GetComponent<Echarpe>())
        {
            GetObject(1, collision.gameObject);
            audioManager.PlaySound("Item1");
        }
        if (collision.gameObject.GetComponent<Chapeau>())
        {
            GetObject(2, collision.gameObject);
            audioManager.PlaySound("Item1");
        }
        else
        {
            audioManager.PlaySound("Collid");
        }
    }

    private void GetObject(int index, GameObject obj)
    {
        obj.SetActive(false);
        Manager.Instance.ObjectGet(index);
    }
}

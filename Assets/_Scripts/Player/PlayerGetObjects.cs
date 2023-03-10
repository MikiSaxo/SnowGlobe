using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGetObjects : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Carotte>())
        {
            GetObject(0, collision.gameObject);
            AudioManager.Instance.PlaySound("Item1");
        }

        if (collision.gameObject.GetComponent<Echarpe>())
        {
            GetObject(1, collision.gameObject);
            AudioManager.Instance.PlaySound("Item1");
        }

        if (collision.gameObject.GetComponent<Chapeau>())
        {
            GetObject(2, collision.gameObject);
            AudioManager.Instance.PlaySound("Item1");
        }
        else
        {
            AudioManager.Instance.PlaySound("Collid");
        }
    }

    private void GetObject(int index, GameObject obj)
    {
        obj.SetActive(false);
        Manager.Instance.ObjectGet(index);
    }
}
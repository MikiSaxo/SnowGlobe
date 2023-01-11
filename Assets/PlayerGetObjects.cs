using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
public class PlayerGetObjects : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Carotte>())
            print("carotte");
        if(collision.gameObject.GetComponent<Echarpe>())
            print("echarpe");
        if(collision.gameObject.GetComponent<Chapeau>())
            print("chapeau");
    }
}

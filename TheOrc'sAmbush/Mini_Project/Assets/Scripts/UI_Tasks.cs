using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tasks : MonoBehaviour
{
    private void Start()
    {
        //Invoke("SetActiveFalse", 1.2f);
        SetActiveFalse();
    }


    public  void SetActiveFalse()
    {
        //gameObject.SetActive(false);
        Destroy(this.gameObject,1.25f);
    }
}

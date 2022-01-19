using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }
    
    void Update()
    {
        tr.Rotate(0, 1, 0, Space.World); //아이템은 계속 y축 회전
       
    }

}

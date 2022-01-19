using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcCtrl : MonoBehaviour
{
    Rigidbody rb;
    public float force = 200; //움직이는 힘 정도


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("NpcMove", 1f, 2f); //2초마다 npc 움직임 효과용 스크립트 호출
    }
    
    void Update()
    {
        
    }

    void NpcMove()
    {
        rb.AddForce(0, force, 0); //npc가 위아래로 반복 이동하는 효과
    }


}

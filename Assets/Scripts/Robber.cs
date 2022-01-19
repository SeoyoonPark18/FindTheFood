using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour
{
    public int deathNum = 3; //총알 3번 맞으면 사망
    public GameObject hamburger; //음식 아이템 중 햄버거

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PBULLET") //만약 주인공 총알에 맞는다면
        {
            deathNum--; //목숨-1
            Destroy(collision.gameObject); //주인공 총알 제거
            if (deathNum == 0) //목숨이 0이 되었다면
            {
                Destroy(gameObject); //강도 제거(검거)
                hamburger.SetActive(true); //강도가 훔친 햄버거 획득!
            }
        }
    }
}

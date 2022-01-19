using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn2 : MonoBehaviour
{
    public GameObject enemy; //적 프리팹 저장용

    public Transform point; //지정한 포인트에 소환
    public float creatTime = 3.0f; //3초마다 적 캐릭터 소환

    void Start()
    {
        StartCoroutine(CreateEnemy()); //반복 호출
    }

    IEnumerator CreateEnemy()
    {
        while (true)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("ZOMBIE").Length; //적캐릭터인 좀비가 몇 마리 남았는지
            if (enemyCount == 0) //남아있지 않다면
            {
                yield return new WaitForSeconds(creatTime); //3초후
                Instantiate(enemy, point.position, point.rotation); //지정한 위치에 생성
            }
            else
            {
                yield return null;
            }
        }

    }
}

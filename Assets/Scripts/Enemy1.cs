using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int deathNum =3; //총알 3번 맞으면 사망
    public static int killedEnemy = 0; //전체 적 캐릭터 제거 횟수
    public GameObject enemy; //적 프리팹 저장용

    public Transform firePos; //총알 발사 위치
    public Transform enemyTr; //적(자신) 위치
    public Transform playerTr; //주인공 위치
    public GameObject bullet; //총알 프리팹
    public float shotInterval = 1; //1초마다 발사
    float shotTime = 0; //총알 발사 간격 조정용
    public float attackDist = 20; //공격 거리

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("PLAYER"); //주인공 오브젝트
        if (player != null) //있다면
        {
            playerTr = player.transform; //위치 받아오기
        }
    }

    void Update()
    {
        enemyTr.LookAt(playerTr); //시선을 player 향하게

        float dist = (playerTr.position - enemyTr.position).magnitude; //주인공과 적 사이 거리
        if (dist < attackDist) //지정한 거리보다 가깝다면
        {
            shotTime += Time.deltaTime; //발사 시간 간격 측정
            if (shotTime > shotInterval) //지정한 시간간격이 지났다면
            {
                int enemyCount = GameObject.FindGameObjectsWithTag("DEVIL").Length; //악마(자신)가 몇마리 남아있는지
                if (enemyCount != 0) //제거되기 전에만 공격가능하게
                {
                    Attack(); //공격하기
                    shotTime = 0; //측정간격은 다시 0으로
                }

            }
        }
    }
    void Attack()
    {
        Vector3 fireDir = (playerTr.position - firePos.position).normalized; //주인공 위치로 향하게끔
        GameObject obj = Instantiate(bullet); //총알 생성
        obj.transform.position = firePos.position; //발사 위치로 이동
        obj.GetComponent<Rigidbody>().AddForce(fireDir * 800); //주인공 향해 총알 발사
        Destroy(obj, 3f);  //3초후 총알 제거
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "PBULLET") //만약 주인공 총알에 맞는다면
        {
            deathNum--; //목숨-1
            Destroy(collision.gameObject);  //주인공 총알 제거
            if (deathNum == 0) //목숨이 0이 되면
            {
                killedEnemy++; //적 제거 횟수 증가
                print("적제거: " + killedEnemy); //콘솔창에 표시

                Destroy(enemy); //적 캐릭터 사망
                enemy.tag = "Untagged"; //사망시 태그 인식 방지
            }
        }
    }
}

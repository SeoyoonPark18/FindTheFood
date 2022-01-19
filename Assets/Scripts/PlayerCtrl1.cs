using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl1 : MonoBehaviour
{
    public float moveSpeed = 10.0f; //이동 속도
    public float rotSpeed = 5.0f; //회전 속도
    public float force = 300; //점프 힘

    Rigidbody rb;
    Transform tr;

    public Text foodItem; //아이템 획득현황 UI
    public int foodItemCount = 0; //아이템 획득 갯수

    public Text playerHpText; //주인공 체력 UI
    public int playerHp = 100; //주인공 체력

    public GameObject bullet; //주인공 총알
    public Transform firePos; //총알 발사 위치

    public Text NpcTalk; //대화창 텍스트
    public GameObject TalkPanel; //대화창 UI
    private string npcText; //npc 대사

    public GameObject hotdog; //음식 아이템
    public GameObject banana; //음식 아이템
    public GameObject finalObj; //숨겨놓은 오브젝트들(비활성화 상태)

    public Transform playerTr; //주인공 위치
    public Transform copTr; //경찰 npc
    public Transform docTr; //의사
    public Transform santaTr; //산타
    public Transform s1Tr; //축구선수1
    public Transform s2Tr; //축구선수2
    public Transform pilTr; //신사
    public Transform ghostTr; //유령

    public AudioClip getItem; //아이템 획득용 사운드
    public AudioClip shoot; //총알 발사 사운드
    AudioSource audioSrc; 

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        playerHpText.text = "주인공 체력: " + playerHp; //주인공 체력 업데이트

        //점프
        if (Mathf.Abs(rb.velocity.y) < 0.001f) //연속점프 방지
        {
            if (Input.GetKeyDown("space")) //스페이스바를 누르면
            {
                rb.AddForce(0, force, 0); //점프
            }
        }

        float h = Input.GetAxis("Horizontal"); //수평
        float v = Input.GetAxis("Vertical"); //수직
        float r = Input.GetAxis("Mouse X"); //마우스 좌우 이동 감지

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); //키보드 방향키로 이동 정도 감지
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime); //캐릭터 이동
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime*r); //캐릭터 회전

        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 버튼 클릭 시
        {
            GameObject obj = Instantiate(bullet, firePos.position, firePos.rotation); //총알 생성
            audioSrc.PlayOneShot(shoot, 1f); //총알 발사 사운드
            Destroy(obj, 3.0f); //3초 후 총알 제거
        }

        
       if(foodItemCount == 6) //음식 6개를 모두 얻는다면
        {
            SceneManager.LoadScene("GameClear"); //게임클리어 씬으로 이동
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "EBULLET") //적 총알에 닿는다면
        {
            playerHp -= 10; //주인공 체력 감소
            playerHpText.text = "주인공 체력: " + playerHp; //화면에 체력 표시
            Destroy(collision.gameObject); //총알 제거
            if (playerHp == 0) //체력이 0이 되었다면
            {
                SceneManager.LoadScene("GameOver"); //게임오버 씬이동
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ITEM") //아이템에 닿으면
        {
            other.gameObject.SetActive(false); //아이템 비활성화
            Destroy(other.gameObject); //아이템 제거
            foodItemCount++; //아이템 획득 갯수 증가
            foodItem.text = "음식 획득 " + foodItemCount + "/6"; //화면에 획득 갯수 표시
            audioSrc.PlayOneShot(getItem, 1f); //아이템 획득 사운드
        }

        if (other.gameObject.tag == "DOCTOR") //의사를 만나면
        {
            docTr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            npcText = "의사: 나를 찾아왔군. 체력을 회복시켜주지!"; //의사 대사
            StartCoroutine(_typing()); //타이핑효과
            playerHp = 100; //주인공 체력 회복
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
        if (other.gameObject.tag == "PILGRIM") //신사를 만나면
        {
            pilTr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            npcText = "신사: 내가 힌트를 주지. 유령을 찾아봐!"; //대사
            StartCoroutine(_typing()); //타이핑효과
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
        if (other.gameObject.tag == "COP") //경찰을 만나면
        {
            copTr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            if (Enemy1.killedEnemy < 5) //적을 5마리보다 덜 제거했다면
            {
                npcText = "경찰: 아직이군. 적을 5번 제거하고 나를 찾아오도록."; //대사
            }
            else //5마리 이상 제거했다면
            {
                npcText = "경찰: 적을 5번 이상 제거했군. 그렇다면 길을 열어주겠다."; //대사
                finalObj.SetActive(true);
            } 
            StartCoroutine(_typing()); //타이핑효과
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
        if (other.gameObject.tag == "SANTA") //산타를 만나면
        {
            santaTr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            npcText = "산타: 호호호 크리스마스 선물을 주지~! 시간 추가!"; //대사
            StartCoroutine(_typing()); //타이핑효과
            RemainTime.rTime += 10f; //시간 추가 코드
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
        if (other.gameObject.tag == "GHOST") //유령을 만나면
        {
            ghostTr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            npcText = "유령: 날 찾아와줘서 고마워! 너에게 선물을 줄게!"; //대사
            StartCoroutine(_typing()); //타이핑효과
            banana.SetActive(true); //바나나 활성화
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
        if (other.gameObject.tag == "SOCCER1") //축구선수1을 만나면
        {
            s1Tr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            npcText = "축구선수: 안녕! 나 핫도그 있는데 너도 먹을래?"; //대사
            StartCoroutine(_typing()); //타이핑효과
            if (hotdog != null) //핫도그 오브젝트가 있다면
            {
                hotdog.SetActive(true); //활성화
            }
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
        if (other.gameObject.tag == "SOCCER2") //축구선수2을 만나면
        {
            s2Tr.LookAt(playerTr); //시선을 주인공 향하게
            TalkPanel.SetActive(true); //대화창 활성화
            npcText = "축구선수: 강도에게 햄버거를 뺏겼어..."; //대사
            StartCoroutine(_typing()); //타이핑효과
            Invoke("RemovePanel", 5f); //5초 후 대화창 비활성화
        }
    }
    IEnumerator _typing()
    {
        yield return new WaitForSeconds(0f); //바로 대사 시작
        for(int i=0; i<= npcText.Length; i++)
        {
            NpcTalk.text = npcText.Substring(0, i); //한 글자씩 보이게
            yield return new WaitForSeconds(0.1f); //0.1초마다 글자 추가
        }
    }
   void RemovePanel()
    {
        TalkPanel.SetActive(false); //대화창 비활성화      
    }

}

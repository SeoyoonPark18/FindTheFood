using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RemainTime : MonoBehaviour
{
    public Text timeText; //시간 UI 표시용
    public static float rTime = 120f; //게임 제한시간 2분(120초)
    int min; //분 표시
    float sec; //초 표시

    void Update()
    {
        rTime -= Time.deltaTime; //남은 시간 계산
        if(rTime >= 60f) //60초 이상 남았다면
        {
            min = (int)rTime / 60; //분 단위 계산
            sec = rTime % 60; //초 단위 계산
            timeText.text = "남은 시간 " + min + "분 " + Mathf.Round(sec) + "초"; //시간 ui 표시
        }
        else //60초 미만으로 남았다면
        {
            timeText.text = "남은 시간 " + Mathf.Round(rTime) + "초"; //시간 ui 표시
            if (rTime < 0) //타임 오버 시
            {
                rTime = 0;
                timeText.text = "시간 종료";
                SceneManager.LoadScene("GameOver");//게임오버 씬 이동
            }
        }
        
        
    }
}

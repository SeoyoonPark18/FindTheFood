using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour { 

    public void reStartBtn() //재시작버튼
    {
    SceneManager.LoadScene("Game"); //게임씬 이동
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunGameManager : MonoBehaviour
{
    public static RunGameManager instance;
    TextMeshProUGUI timeText;
    public int waitSeconds = 3;
    public GameStateType gameStateType = GameStateType.NotInit;


    internal void StageClear()
    {
        gameStateType = GameStateType.Clear;
        Player.instance.OnStageClear();
        timeText.text = "Clear!";

    }

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this; //this는 변수, 함수 모두 접근 가능
    }
    
    [SerializeField] int point; 
    //다른 클래스에서 접근 못하게 하려면 public으로 안하고 시리얼라이즈필드를 사용 기본 값은 private이기 때문엥
    TextMeshProUGUI pointText;

    internal void AddCoin(int addPoint)
    {
        point += addPoint;
        pointText.text = point.ToString();
    }

    internal static bool IsPlaying()
    {
        return instance.gameStateType == GameStateType.Playing;
    }

    IEnumerator Start()
    {
        //시작하기 전에 카메라랑 캐릭터를 잠시 멈춰야함.
        gameStateType = GameStateType.Ready;

        //3초 카운트 다운 후 스타트 표시
        timeText = transform.Find("TimeText").GetComponent<TextMeshProUGUI>(); 
        //transform의 Find함수는 transform을 리턴하고 그 리턴한 트랜스폼에서 GetComponent하겠다는 의미
        pointText = transform.Find("PointText").GetComponent<TextMeshProUGUI>();


        for (int i = waitSeconds; i > 0; i--)
        {
            timeText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        timeText.text = "Start!";
        gameStateType = GameStateType.Playing;

        yield return new WaitForSeconds(0.5f); //0.5초 쉰 다음에 글자 사라지도록
        timeText.text = "";

        //카메라랑 캐릭터가 다시 움직이도록
    }

    //게임이 시작되었는지 멈춘 중인지 판단하여 상태로 동작하게 
    //문자만 있는 상태를 지정할 땐 항상 enum으롱



    public enum GameStateType
    {
        NotInit,
        Ready,
        Playing,
        Clear
    }

    // Update is called once per frame

}

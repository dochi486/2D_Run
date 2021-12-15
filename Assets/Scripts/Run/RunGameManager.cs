using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Run
{
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

        private void Awake()
        {
            instance = this; //this는 변수, 함수 모두 접근 가능
        }

        [SerializeField] int point;
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


        public enum GameStateType
        {
            NotInit,
            Ready,
            Playing,
            Clear
        }
    }
}
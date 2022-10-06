using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    /*  static: 자주변하지않는 일정한 값 혹은 설정 정보 , 인스턴스에 종속된 변수가아니라 스크립트 클래스 그 자체에 종속된 변수
     */
    public GameObject doorLocked;
    public static DoorButton instance;         // 도어버튼에 어디서든 접근할수있도록 인스턴스선언(다른스크립트포함) 
    public static Door door;              // 열고 닫을 문 , 버튼이 플레이어가 문에 가까이갔을때 인식할수있도록 트랜스폼으로 지정

    private void Awake()  //모든 변수와 게임의 상태를 초기화하기 위해서 호출된다. strat보다도 먼저호출된다. 
    {
        instance = this;         //instance : static DoorButton 변수  , this : 지금 실행중인 스크립트 
        door = null;
    }
    void Start()
    {
        gameObject.SetActive(false);  //주어진 값에 따라 게임 오브젝트를 활성화/비활성화 합니다.
    }

    
    public void Clicked()
    {
        if (door.isLocked == true)
        {
            doorLocked.SetActive(true);
            return;
        }
        if (door.isClose)  // 자식door오브젝트에서 Door스크립트를 받아오고 isClose가 false이면 열리고
        {
            door.Open();
        }
        else                                              // true면 닫는다.
        {
            door.Close();
        }
    }
}

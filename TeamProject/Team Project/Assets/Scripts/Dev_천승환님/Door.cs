using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public(접근제한자) : 접근제한자는 클래스 내의 멤버(필드,메서드,속성,이벤트등)의 접근을 제한하는 역할을 합니다., 클래스의 외부에서도 접근가능
 *접근 제한자 class 고유 식별자
 *MonoBehaviour : 유니티에서 생성하는 모든 스크립트를 상속받는 기본 클래스
 *상속: 먼저 구현해 놓은 스크립트를 통째로 받아와서 쉽게 사용해주는 기능*/

public class Door : MonoBehaviour //접근 제한자, 클래스명 : 상속     
{
    /*Transform : 오브젝트의 위치,회전,크기를 나타내는 컴포넌트
     * 컴포넌트 : 프로그래밍에 있어 재사용이 가능한 각각의 독립된 모듈
     * 모듈: 다중 파일을 다루는 일부 닷넷 바이너리
    */
    Transform doorPivot;         //doorPivot 트랜스폼


    public DoorButton doorButton;

    /* bool: 부울값 참과 거짓을 나타내는 구조체 형식
     * isClose : 필드 클래스 또는 구조체에 직접선언되는 모든 형식의 변수
     */
    public bool isClose;         //isClose 라는이름의 bool변수                             


    /*void(반환형식) : 로컬 함수의 반환 형식으로 사용되는 메서드가 값을 반환하지않도록 지정한다.
     * Start() : MonoBehaviour에 상속받는 스크립트 생성시에 존재하는 Start 메소드 
     */
    void Start()
    {
        doorPivot = transform.parent;                        //doorPivot = 부모오브젝트
        doorButton = DoorButton.instance;                    //도어버튼을 인스턴스로 접근 어떤 스크립트에서라도 접근할수있도록 만듬
        isClose = true;
    }

    /*void(반환형식) : 로컬 함수의 반환 형식으로 사용되는 메서드가 값을 반환하지않도록 지정한다.
     * Update() : 유니티 기본함수, 누가멈추지않는이상 매프레임마다 계속 호출되는 함수
     */
    void Update()
    {

    }
    /*private: 클래스의 내부에서만 접근가능 , 액세스중에서도 가장낮은 수준의 액서스 접근한정자를 사용하지않으면 기본값으로 private로 접근 수준이설정된다.
     * void : 반환형식
     * OnTriggerEnter : 게임오브젝트가 다른 게임오브젝트와 충돌하면 유니티는 OnTriggerEnter를 호출한다.
     */
    private void OnTriggerEnter(Collider other)               // 트리거 충돌을 이용해서 어떤 문을 열지 확인
    {
        doorButton.gameObject.SetActive(true);                // doorButton gameObject 활성화 
        DoorButton.door = doorPivot;                          // 트리거충돌로 들어갈 door에 DoorButton을 활성하
    }
    /* OnTriggerExit : 트리거를 떠나는 모든것을 파괴시킨다.
     * 
     */
    private void OnTriggerExit(Collider other)
    {

        doorButton.gameObject.SetActive(false);               //도어버튼 게임오브젝트 비활성화

        DoorButton.door = null;                             //없어도 되는코드 위에 도어버튼과 짝을 맞추기위해 넣었고 혹시모를 에러를 방지하기위해 넣음
    }
}

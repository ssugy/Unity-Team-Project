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
    DoorButton doorButton;
    [HideInInspector] public Quaternion closeRotation;
    [HideInInspector] public Quaternion openRotation;
    [HideInInspector] public bool isClose;

    public bool isLocked;


    /*void(반환형식) : 로컬 함수의 반환 형식으로 사용되는 메서드가 값을 반환하지않도록 지정한다.
     * Start() : MonoBehaviour에 상속받는 스크립트 생성시에 존재하는 Start 메소드 
     */
    void Start()
    {        
        doorPivot = transform.parent;                        //doorPivot = 부모오브젝트
        doorButton = DoorButton.instance;                    //도어버튼을 인스턴스로 접근 어떤 스크립트에서라도 접근할수있도록 만듬
        isClose = true;
        closeRotation = doorPivot.rotation;
        openRotation = closeRotation * Quaternion.Euler(0, 90f, 0);
    }    
    /*private: 클래스의 내부에서만 접근가능 , 액세스중에서도 가장낮은 수준의 액서스 접근한정자를 사용하지않으면 기본값으로 private로 접근 수준이설정된다.
     * void : 반환형식
     * OnTriggerEnter : 게임오브젝트가 다른 게임오브젝트와 충돌하면 유니티는 OnTriggerEnter를 호출한다.
     */
    private void OnTriggerEnter(Collider other)               // 트리거 충돌을 이용해서 어떤 문을 열지 확인
    {
        if (other.CompareTag("Player"))
        {
            doorButton.gameObject.SetActive(true);                // doorButton gameObject 활성화 
            DoorButton.door = this;
        }        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorButton.gameObject.SetActive(false);               //도어버튼 게임오브젝트 비활성화
            DoorButton.door = null; 
        }
    }
    public void Open()
    {
        StartCoroutine(DoorOpen());
    }
    public void Close()
    {
        StartCoroutine(DoorClose());
    }
    IEnumerator DoorOpen()
    {
        isClose = false;               
        for (int i = 0; i < 90; ++i)
        {
            doorPivot.rotation = Quaternion.RotateTowards(doorPivot.rotation, openRotation, 1f);
            yield return null;
        }
    }
    IEnumerator DoorClose()
    {
        isClose = true;              
        for (int i = 0; i < 90; ++i)
        {
            doorPivot.rotation = Quaternion.RotateTowards(doorPivot.rotation, closeRotation, 1f);
            yield return null;
        }
    }
}

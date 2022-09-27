using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    /*  static: ���ֺ������ʴ� ������ �� Ȥ�� ���� ���� , �ν��Ͻ��� ���ӵ� �������ƴ϶� ��ũ��Ʈ Ŭ���� �� ��ü�� ���ӵ� ����
     */
    public GameObject doorLocked;
    public static DoorButton instance;         // �����ư�� ��𼭵� �����Ҽ��ֵ��� �ν��Ͻ�����(�ٸ���ũ��Ʈ����) 
    public static Transform door;              // ���� ���� �� , ��ư�� �÷��̾ ���� �����̰����� �ν��Ҽ��ֵ��� Ʈ���������� ����

    private void Awake()  //��� ������ ������ ���¸� �ʱ�ȭ�ϱ� ���ؼ� ȣ��ȴ�. strat���ٵ� ����ȣ��ȴ�. 
    {
        instance = this;         //instance : static DoorButton ����  , this : ���� �������� ��ũ��Ʈ 
        door = null;
    }
    void Start() //
    {
        gameObject.SetActive(false);  //�־��� ���� ���� ���� ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ �մϴ�.
    }

    public void DoorOpen()
    {
        door.rotation = door.rotation * Quaternion.Euler(0, 90f, 0); //��������϶� ������ġ�� ���絵����ġ * y��90��  
        door.GetComponentInChildren<Door>().isClose = false; //�ڽ�door������Ʈ���� Door��ũ��Ʈ�� �޾ƿ��� isClose��� boolŸ�Ժ����� Door�� ���������Ƿ� false�� ������Ѵ�. 
    }
    public void DoorClose()
    {
        door.rotation = door.rotation * Quaternion.Euler(0, -90f, 0); //����Ŭ�����϶� ������ġ�� ���絵����ġ * y��-90��
        door.GetComponentInChildren<Door>().isClose = true; //�ڽ�door������Ʈ���� Door��ũ��Ʈ�� �޾ƿ��� isClose��� boolŸ�Ժ����� Door�� ���������Ƿ� true�� ������Ѵ�.
    }
    public void Door()
    {
        if (door.GetComponentInChildren<Door>().isLocked == true)
        {
            doorLocked.SetActive(true);
            return;
        }
        if (door.GetComponentInChildren<Door>().isClose)  // �ڽ�door������Ʈ���� Door��ũ��Ʈ�� �޾ƿ��� isClose�� false�̸� ������
        {
            DoorOpen();
        }
        else                                              // true�� �ݴ´�.
        {
            DoorClose();
        }
    }
}

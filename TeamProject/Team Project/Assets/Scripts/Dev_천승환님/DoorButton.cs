using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    /*  static: ���ֺ������ʴ� ������ �� Ȥ�� ���� ���� , �ν��Ͻ��� ���ӵ� �������ƴ϶� ��ũ��Ʈ Ŭ���� �� ��ü�� ���ӵ� ����
     */
    public GameObject doorLocked;
    public static DoorButton instance;         // �����ư�� ��𼭵� �����Ҽ��ֵ��� �ν��Ͻ�����(�ٸ���ũ��Ʈ����) 
    public static Door door;              // ���� ���� �� , ��ư�� �÷��̾ ���� �����̰����� �ν��Ҽ��ֵ��� Ʈ���������� ����

    private void Awake()  //��� ������ ������ ���¸� �ʱ�ȭ�ϱ� ���ؼ� ȣ��ȴ�. strat���ٵ� ����ȣ��ȴ�. 
    {
        instance = this;         //instance : static DoorButton ����  , this : ���� �������� ��ũ��Ʈ 
        door = null;
    }
    void Start()
    {
        gameObject.SetActive(false);  //�־��� ���� ���� ���� ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ �մϴ�.
    }

    
    public void Clicked()
    {
        if (door.isLocked == true)
        {
            doorLocked.SetActive(true);
            return;
        }
        if (door.isClose)  // �ڽ�door������Ʈ���� Door��ũ��Ʈ�� �޾ƿ��� isClose�� false�̸� ������
        {
            door.Open();
        }
        else                                              // true�� �ݴ´�.
        {
            door.Close();
        }
    }
}

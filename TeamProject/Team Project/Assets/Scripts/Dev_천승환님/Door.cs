using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public(����������) : ���������ڴ� Ŭ���� ���� ���(�ʵ�,�޼���,�Ӽ�,�̺�Ʈ��)�� ������ �����ϴ� ������ �մϴ�., Ŭ������ �ܺο����� ���ٰ���
 *���� ������ class ���� �ĺ���
 *MonoBehaviour : ����Ƽ���� �����ϴ� ��� ��ũ��Ʈ�� ��ӹ޴� �⺻ Ŭ����
 *���: ���� ������ ���� ��ũ��Ʈ�� ��°�� �޾ƿͼ� ���� ������ִ� ���*/

public class Door : MonoBehaviour //���� ������, Ŭ������ : ���     
{
    /*Transform : ������Ʈ�� ��ġ,ȸ��,ũ�⸦ ��Ÿ���� ������Ʈ
     * ������Ʈ : ���α׷��ֿ� �־� ������ ������ ������ ������ ���
     * ���: ���� ������ �ٷ�� �Ϻ� ��� ���̳ʸ�
    */
    Transform doorPivot;         //doorPivot Ʈ������
    DoorButton doorButton;
    [HideInInspector] public Quaternion closeRotation;
    [HideInInspector] public Quaternion openRotation;
    [HideInInspector] public bool isClose;

    public bool isLocked;


    /*void(��ȯ����) : ���� �Լ��� ��ȯ �������� ���Ǵ� �޼��尡 ���� ��ȯ�����ʵ��� �����Ѵ�.
     * Start() : MonoBehaviour�� ��ӹ޴� ��ũ��Ʈ �����ÿ� �����ϴ� Start �޼ҵ� 
     */
    void Start()
    {        
        doorPivot = transform.parent;                        //doorPivot = �θ������Ʈ
        doorButton = DoorButton.instance;                    //�����ư�� �ν��Ͻ��� ���� � ��ũ��Ʈ������ �����Ҽ��ֵ��� ����
        isClose = true;
        closeRotation = doorPivot.rotation;
        openRotation = closeRotation * Quaternion.Euler(0, 90f, 0);
    }    
    /*private: Ŭ������ ���ο����� ���ٰ��� , �׼����߿����� ���峷�� ������ �׼��� ���������ڸ� ������������� �⺻������ private�� ���� �����̼����ȴ�.
     * void : ��ȯ����
     * OnTriggerEnter : ���ӿ�����Ʈ�� �ٸ� ���ӿ�����Ʈ�� �浹�ϸ� ����Ƽ�� OnTriggerEnter�� ȣ���Ѵ�.
     */
    private void OnTriggerEnter(Collider other)               // Ʈ���� �浹�� �̿��ؼ� � ���� ���� Ȯ��
    {
        if (other.CompareTag("Player"))
        {
            doorButton.gameObject.SetActive(true);                // doorButton gameObject Ȱ��ȭ 
            DoorButton.door = this;
        }        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorButton.gameObject.SetActive(false);               //�����ư ���ӿ�����Ʈ ��Ȱ��ȭ
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

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
    public DoorButton doorButton;

    /* bool: �οﰪ ���� ������ ��Ÿ���� ����ü ����
     * isClose : �ʵ� Ŭ���� �Ǵ� ����ü�� ��������Ǵ� ��� ������ ����
     */
    public bool isClose;         //isClose ����̸��� bool����
    public bool isLocked;


    /*void(��ȯ����) : ���� �Լ��� ��ȯ �������� ���Ǵ� �޼��尡 ���� ��ȯ�����ʵ��� �����Ѵ�.
     * Start() : MonoBehaviour�� ��ӹ޴� ��ũ��Ʈ �����ÿ� �����ϴ� Start �޼ҵ� 
     */
    void Start()
    {
        doorPivot = transform.parent;                        //doorPivot = �θ������Ʈ
        doorButton = DoorButton.instance;                    //�����ư�� �ν��Ͻ��� ���� � ��ũ��Ʈ������ �����Ҽ��ֵ��� ����
        isClose = true;        
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
            DoorButton.door = doorPivot;
        }        
    }
    /* OnTriggerExit : Ʈ���Ÿ� ������ ������ �ı���Ų��.
     * 
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorButton.gameObject.SetActive(false);               //�����ư ���ӿ�����Ʈ ��Ȱ��ȭ

            DoorButton.door = null;                             //��� �Ǵ��ڵ� ���� �����ư�� ¦�� ���߱����� �־��� Ȥ�ø� ������ �����ϱ����� ����
        }
    }        
}

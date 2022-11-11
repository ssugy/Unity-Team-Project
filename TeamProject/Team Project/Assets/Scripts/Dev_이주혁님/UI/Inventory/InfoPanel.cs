using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ���� �г��� ����ϴ� ������Ʈ.
// �κ��丮�� �������� �����ϸ� ������ ���� �г��� ������, ���/����/����/�ı� ����� ����� �� �ִ�.
public class InfoPanel : MonoBehaviour
{    
    [Header("�гο� ǥ�õ� ����")]
    public Image icon;              // ������ ������.
    public Text nameText;           // ������ �̸�.
    public Text typeText;           // ������ Ÿ��.
    public Text explanationText;    // ������ ����.
    [Space(32)]
    [Header("���/�ı� ��ư")]
    public Button useButton;
    public Text useButtonText;
    public Button destroyButton;
    public Text destroyButtonText;
    [Header("�κ��丮 UI")]
    public InventoryUI iUi;

    // ������ �г��� ����ϴ� �޼ҵ�. �κ��丮�� �������� Ŭ���ϸ� �ش� ������ ������ ������ �޾ƿ� ����Ѵ�.
    public void SetInformation(Item _item, Slot _slot)
    {        
        useButton.onClick.RemoveAllListeners();         // ��� ��ư�� �Ҵ�� �޼ҵ带 �ʱ�ȭ.
        destroyButton.onClick.RemoveAllListeners();     // �ı� ��ư�� �Ҵ�� �޼ҵ带 �ʱ�ȭ.
        icon.sprite = _item.image;                      // ���õ� �������� �̹����� �������� ��ü.
        nameText.text = _item.name;                     // ���õ� �������� �̸����� �ؽ�Ʈ�� ��ü.
        explanationText.text = _item.explanation;       // ���õ� �������� �������� �ؽ�Ʈ�� ��ü.
        useButton.gameObject.SetActive(true);           // ��� ��ư�� Ȱ��ȭ��. (��� �������� ����� ��Ȱ��ȭ.)
        destroyButton.gameObject.SetActive(true);       // �ı� ��ư�� Ȱ��ȭ��. (������ ��� �������� �ı��� ��Ȱ��ȭ.)
        switch (_item.type)                             // ������ Ÿ�Կ� ���� �ٸ� ����� ����.
        {
            case ItemType.EQUIPMENT:
                typeText.text = "���";
                if (_item.equipedState == EquipState.UNEQUIPED)         // ���� ���� ���¸�.
                {
                    useButtonText.text = "����";
                    //useButton.onClick.AddListener(() => _item.Equip());
                    useButton.onClick.AddListener(() => iUi.Equip(_item, _slot));
                }
                else if (_item.equipedState == EquipState.EQUIPED)      // ���� ���¸�.
                {
                    destroyButton.gameObject.SetActive(false);
                    useButtonText.text = "����";
                    //useButton.onClick.AddListener(() => _item.Unequip());
                    useButton.onClick.AddListener(() => iUi.Unequip(_item, _slot));
                }                
                break;
            case ItemType.CONSUMABLE:
                typeText.text = "�Һ�";
                useButtonText.text = "���";
                useButton.onClick.AddListener(()=>_item.Use());     // ��� ��ư�� ������ �Һ� �������� ȿ���� �ߵ�.
                useButton.onClick.AddListener(() => Inventory.instance.RemoveItem(_item));      // ��� ��ư�� ������ �������� �����.
                break;
            case ItemType.INGREDIENTS:
                typeText.text = "���";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<����>";
                break;
        }                        
        destroyButton.onClick.AddListener(() => Inventory.instance.RemoveItem(_item));
    }   
}

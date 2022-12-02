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
    public GameObject QuickSlot;    // ������ ������ ��� ��ư
    [Space(32)]
    [Header("���/�ı� ��ư")]
    public Button useButton;
    public Text useButtonText;
    public Button destroyButton;
    public Text destroyButtonText;
    [Header("�κ��丮 UI")]
    public InventoryUI iUi;
    [Header("�߰��ɼ� UI")]
    public List<Text> options;
    public List<string> optionNames;

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
        QuickSlot.SetActive(false);
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
                QuickSlot.SetActive(true);
                typeText.text = "�Һ�";
                useButtonText.text = "���";
                useButton.onClick.AddListener(()=>_item.Use());     // ��� ��ư�� ������ �Һ� �������� ȿ���� �ߵ�.
                useButton.onClick.AddListener(() => JY_CharacterListManager.s_instance.invenList[0].RemoveItem(_item));      // ��� ��ư�� ������ �������� �����.
                break;
            case ItemType.INGREDIENTS:
                typeText.text = "���";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<����>";
                break;
        }
        // �߰� �ɼ� �� ǥ��
        ShowOptions(_item);

        destroyButton.onClick.AddListener(() => //Inventory.instance.RemoveItem(_item));
        iUi.DestroyItem(_item, _slot));
    }   

    private void ShowOptions(Item _item)
    {
        if (_item.type != ItemType.EQUIPMENT)
        {
            for (int i = 0; i < options.Count; i++)
            {
                options[i].gameObject.SetActive(false);
            }
            return;
        }
        if (_item.option != null && options.Count > 0)
        {
            for(int i = 0; i < options.Count; i++)
            {
                EquipOption.EquipAttrib attrib;
                float val;
                bool valid = _item.option.GetOptionValue(i, out attrib, out val);
                if (valid)
                {
                    options[i].gameObject.SetActive(true);
                    options[i].text = string.Format("{0}: {1:F2}%", optionNames[(int)attrib], val);
                }
                else
                {
                    options[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

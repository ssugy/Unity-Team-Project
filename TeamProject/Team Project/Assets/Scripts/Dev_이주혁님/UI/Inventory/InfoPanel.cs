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
    public Text levelText;          // ������ ���� ����.
    public Text explanationText;    // ������ ����.
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
    [Header("������ ���� �ν��Ͻ�")]
    public Button QuickSlot;                // ������ ������ ��� ��ư
    public JY_QuickSlot QuickslotScript;    //������ ��ũ��Ʈ

    // ������ �г��� ����ϴ� �޼ҵ�. �κ��丮�� �������� Ŭ���ϸ� �ش� ������ ������ ������ �޾ƿ� ����Ѵ�.
    public void SetInformation(Item _item)
    {        
        useButton.onClick.RemoveAllListeners();         // ��� ��ư�� �Ҵ�� �޼ҵ带 �ʱ�ȭ.
        destroyButton.onClick.RemoveAllListeners();     // �ı� ��ư�� �Ҵ�� �޼ҵ带 �ʱ�ȭ.
        QuickSlot.onClick.RemoveAllListeners();

        icon.sprite = _item.image;                      // ���õ� �������� �̹����� �������� ��ü.        
        explanationText.text = _item.explanation;       // ���õ� �������� �������� �ؽ�Ʈ�� ��ü.

        useButton.gameObject.SetActive(true);           // ��� ��ư�� Ȱ��ȭ��. (��� �������� ����� ��Ȱ��ȭ.)
        useButton.interactable = true;
        destroyButton.gameObject.SetActive(true);       // �ı� ��ư�� Ȱ��ȭ��. (������ ��� �������� �ı��� ��Ȱ��ȭ.)
        QuickSlot.gameObject.SetActive(false);

        switch (_item.type)                             // ������ Ÿ�Կ� ���� �ٸ� ����� ����.
        {
            case ItemType.EQUIPMENT:
                nameText.text = $"+{_item.enhanced} {_item.name}";  
                typeText.text = "���";
                levelText.gameObject.SetActive(true);
                levelText.text = $"���� {_item.level} �̻�\n���� ����";
                if (_item.equipedState == EquipState.UNEQUIPED)         // ���� ���� ���¸�.
                {
                    useButtonText.text = "����";
                    useButton.onClick.AddListener(() => _item.Equip());
                    // ��� ���� ������ ĳ���� �������� ������ ���� ��ư�� ��ȣ �ۿ��� �Ұ���.
                    if (_item.level > JY_CharacterListManager.s_instance.playerList[0].playerStat.level)
                        useButton.interactable = false;
                }
                else if (_item.equipedState == EquipState.EQUIPED)      // ���� ���¸�.
                {
                    destroyButton.gameObject.SetActive(false);
                    useButtonText.text = "����";
                    useButton.onClick.AddListener(() => _item.Unequip());                    
                }
                break;
            case ItemType.CONSUMABLE:
                nameText.text = _item.name;
                levelText.gameObject.SetActive(false);
                QuickSlot.gameObject.SetActive(true);
                if (_item.name.Equals(QuickslotScript.EquipItem.name))
                    QuickSlot.onClick.AddListener(() => UnequipQuickSlot());
                else if (!_item.name.Equals(QuickslotScript.EquipItem.name) || QuickslotScript.EquipItem==null) 
                    QuickSlot.onClick.AddListener(() => EquipQuickSlot(_item));
                typeText.text = "�Һ�";
                useButtonText.text = "���";
                useButton.onClick.AddListener(()=>_item.Use());     // ��� ��ư�� ������ �Һ� �������� ȿ���� �ߵ�.
                useButton.onClick.AddListener(() => JY_CharacterListManager.s_instance.invenList[0].RemoveItem(_item));      // ��� ��ư�� ������ �������� �����.
                break;
            case ItemType.INGREDIENTS:
                nameText.text = _item.name;
                levelText.gameObject.SetActive(false);
                typeText.text = "���";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<����>";
                break;
        }
        // �߰� �ɼ� �� ǥ��
        ShowOptions(_item);

        destroyButton.onClick.AddListener(() => iUi.DestroyItem(_item));
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
                if (_item.option.GetOptionValue(i, out EquipOption.EquipAttrib attrib, out float val))
                {
                    options[i].gameObject.SetActive(true);

                    switch (_item.option.tiers[i])
                    {
                        case EquipOption.EquipTier.Tres:
                            {
                                ColorUtility.TryParseHtmlString("#A240FFFF", out Color color);
                                options[i].color = color;
                                break;
                            }
                        case EquipOption.EquipTier.Duo:
                            {
                                ColorUtility.TryParseHtmlString("#406AFFFF", out Color color);
                                options[i].color = color;
                                break;
                            }
                        case EquipOption.EquipTier.Unus:
                            {
                                ColorUtility.TryParseHtmlString("#686868FF", out Color color);
                                options[i].color = color;
                                break;
                            }
                    }                    
                   
                    switch ((int)attrib)
                    {
                        // �ۼ�Ʈ�� �ƴ� ���� ��ġ�� �÷��ִ� �ɼǵ�.
                        case int n when (n == 2 || (n >= 4 && n <= 9) || n == 11):
                            {
                                options[i].text 
                                    = string.Format("{0}: {1}", optionNames[(int)attrib], (int)val);
                                break;
                            }
                        // �ۼ�Ʈ�� �ɷ�ġ�� ��½�Ű�� �ɼǵ�.
                        default:
                            {
                                options[i].text 
                                    = string.Format("{0}: {1:F2}%", optionNames[(int)attrib], val);
                                break;
                            }
                    }                    
                }
                else
                {
                    options[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void EquipQuickSlot(Item _item)
    {
        Debug.Log("���");
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        QuickslotScript.EquipQuickSlot(_item);
    }
    void UnequipQuickSlot()
    {
        Debug.Log("����");
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        QuickslotScript.ClearQuickSlot();
    }
}

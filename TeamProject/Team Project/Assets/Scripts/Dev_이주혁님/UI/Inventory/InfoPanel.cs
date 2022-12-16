using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 아이템 정보 패널을 담당하는 컴포넌트.
// 인벤토리의 아이템을 선택하면 아이템 정보 패널이 열리고, 사용/장착/해제/파괴 기능을 사용할 수 있다.
public class InfoPanel : MonoBehaviour
{    
    [Header("패널에 표시될 정보")]
    public Image icon;              // 아이템 아이콘.
    public Text nameText;           // 아이템 이름.
    public Text typeText;           // 아이템 타입.
    public Text levelText;          // 아이템 레벨 제한.
    public Text explanationText;    // 아이템 설명.
    [Space(32)]
    [Header("사용/파괴 버튼")]
    public Button useButton;
    public Text useButtonText;
    public Button destroyButton;
    public Text destroyButtonText;
    [Header("인벤토리 UI")]
    public InventoryUI iUi;
    [Header("추가옵션 UI")]
    public List<Text> options;
    public List<string> optionNames;
    [Header("퀵슬롯 관련 인스턴스")]
    public Button QuickSlot;                // 아이템 퀵슬롯 등록 버튼
    public JY_QuickSlot QuickslotScript;    //퀵슬롯 스크립트

    // 아이템 패널을 출력하는 메소드. 인벤토리의 아이템을 클릭하면 해당 슬롯의 아이템 정보를 받아와 출력한다.
    public void SetInformation(Item _item)
    {        
        useButton.onClick.RemoveAllListeners();         // 사용 버튼에 할당된 메소드를 초기화.
        destroyButton.onClick.RemoveAllListeners();     // 파괴 버튼에 할당된 메소드를 초기화.
        QuickSlot.onClick.RemoveAllListeners();

        icon.sprite = _item.image;                      // 선택된 아이템의 이미지로 아이콘을 교체.        
        explanationText.text = _item.explanation;       // 선택된 아이템의 설명으로 텍스트를 교체.

        useButton.gameObject.SetActive(true);           // 사용 버튼을 활성화함. (재료 아이템은 사용이 비활성화.)
        useButton.interactable = true;
        destroyButton.gameObject.SetActive(true);       // 파괴 버튼을 활성화함. (장착된 장비 아이템은 파괴가 비활성화.)
        QuickSlot.gameObject.SetActive(false);

        switch (_item.type)                             // 아이템 타입에 따라 다른 기능을 수행.
        {
            case ItemType.EQUIPMENT:
                nameText.text = $"+{_item.enhanced} {_item.name}";  
                typeText.text = "장비";
                levelText.gameObject.SetActive(true);
                levelText.text = $"레벨 {_item.level} 이상\n장착 가능";
                if (_item.equipedState == EquipState.UNEQUIPED)         // 장착 해제 상태면.
                {
                    useButtonText.text = "장착";
                    useButton.onClick.AddListener(() => _item.Equip());
                    // 장비 제한 레벨이 캐릭터 레벨보다 높으면 장착 버튼이 상호 작용이 불가됨.
                    if (_item.level > JY_CharacterListManager.s_instance.playerList[0].playerStat.level)
                        useButton.interactable = false;
                }
                else if (_item.equipedState == EquipState.EQUIPED)      // 장착 상태면.
                {
                    destroyButton.gameObject.SetActive(false);
                    useButtonText.text = "해제";
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
                typeText.text = "소비";
                useButtonText.text = "사용";
                useButton.onClick.AddListener(()=>_item.Use());     // 사용 버튼을 누르면 소비 아이템의 효과가 발동.
                useButton.onClick.AddListener(() => JY_CharacterListManager.s_instance.invenList[0].RemoveItem(_item));      // 사용 버튼을 누르면 아이템이 사라짐.
                break;
            case ItemType.INGREDIENTS:
                nameText.text = _item.name;
                levelText.gameObject.SetActive(false);
                typeText.text = "재료";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<오류>";
                break;
        }
        // 추가 옵션 값 표시
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
                        // 퍼센트가 아닌 고정 수치를 올려주는 옵션들.
                        case int n when (n == 2 || (n >= 4 && n <= 9) || n == 11):
                            {
                                options[i].text 
                                    = string.Format("{0}: {1}", optionNames[(int)attrib], (int)val);
                                break;
                            }
                        // 퍼센트로 능력치를 상승시키는 옵션들.
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
        Debug.Log("등록");
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        QuickslotScript.EquipQuickSlot(_item);
    }
    void UnequipQuickSlot()
    {
        Debug.Log("해제");
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        QuickslotScript.ClearQuickSlot();
    }
}

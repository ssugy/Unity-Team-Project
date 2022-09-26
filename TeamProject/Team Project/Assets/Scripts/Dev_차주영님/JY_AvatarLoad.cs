using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_AvatarLoad : AvatarSceneManager
{
    public static JY_AvatarLoad instance;
    public static JY_AvatarLoad s_instance { get { return instance; } }
    public GameObject origin;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        origin = GameObject.FindWithTag("Player");
        charMale = findGameObjectInChild("BaseCharacterM", origin.transform).gameObject;
        charFemale= findGameObjectInChild("BaseCharacterF", origin.transform).gameObject;
    }

    public Transform findGameObjectInChild(string nodename, Transform origin)
    {
        if (origin.name == nodename)
            return origin;
        for (int i = 0; i < origin.childCount; i++)
        {
            Transform childTr = findGameObjectInChild(nodename, origin.GetChild(i));
            if (childTr != null)
                return childTr;
        }
        return null;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void LoadModelData(int listNum)
    {
        if (JY_CharacterListManager.s_instance.selectNum == -1)
        {
            charMale.SetActive(false);
            charFemale.SetActive(false);
        }
        else
        {
            //gender °»½Å
            if (JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].gender == "M")
            {
                charMale.SetActive(false);
                charFemale.SetActive(true);
                //getSetCharacter("M"); //¿ëÈÆ
            }
            else
            {
                charMale.SetActive(true);
                charFemale.SetActive(false);
                //getSetCharacter("F"); //¿ëÈÆ
            }

            //Customizing °»½Å
            for(int i=0; i < 4;i++)
            {
                //¿ëÈÆ
                //DeleteSubOption(i);
                //{
                //    GameObject addedObj = targetScript.AddItem(targetScript.itemGroups[i],
                //    JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].characterAvatar[i]);
                //    if (i == 3)
                //    {
                //        GameObject child = addedObj.transform.GetChild(0).gameObject;
                //        //legOptions = child.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight();
                //    }
                //}
            }
        }
    }
}

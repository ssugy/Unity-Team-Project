using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    public void LoadScene(int index)
    {
        GameManager.s_instance.LoadScene(index);    // 이렇게 해야지 씬 이동 시 사라지는 객체의 버튼에 달 수 있다.
    }

    public void BGMSliderValueChanged(Slider slider)
    {
        AudioManager.s_instance.BGMSliderValueChanged(slider);
    }

    public void BGMMute(Toggle toggle)
    {
        AudioManager.s_instance.BGMMute(toggle);
    }

    public void EffectSliderValueChanged(Slider slider)
    {
        AudioManager.s_instance.EffectSliderValueChanged(slider);
    }

    public void EffectMute(Toggle toggle)
    {
        AudioManager.s_instance.EffectMute(toggle);
    }
}

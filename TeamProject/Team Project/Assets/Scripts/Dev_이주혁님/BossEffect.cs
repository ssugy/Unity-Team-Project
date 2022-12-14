using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffect : MonoBehaviour
{
    public void EffectOff()
    {       
        gameObject.SetActive(false);
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }

    // �ش� �ݹ� �Լ��� �ڽ� ������Ʈ ���� ��� ��ƼŬ�� ����� ���� ������ �����.
    private void OnParticleSystemStopped()
    {        
        InstanceManager.s_instance.BossEffectList.Remove(this);        
        gameObject.SetActive(false);
       
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }
}

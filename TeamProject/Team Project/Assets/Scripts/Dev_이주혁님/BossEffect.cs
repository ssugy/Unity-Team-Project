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

    // 해당 콜백 함수는 자식 오브젝트 포함 모든 파티클의 재생이 끝난 다음에 실행됨.
    private void OnParticleSystemStopped()
    {        
        InstanceManager.s_instance.BossEffectList.Remove(this);        
        gameObject.SetActive(false);
       
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }
}

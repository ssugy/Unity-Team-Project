using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public void EffectOff()
    {
        // SkillEffectList에서 Remove를 하지 않는 것은
        // List에 대한 foreach문 내부에서 Remove나 Add가 발생할 시 Invalid Error가 발생하기 때문.
        gameObject.SetActive(false);
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }
    
    // 해당 콜백 함수는 자식 오브젝트 포함 모든 파티클의 재생이 끝난 다음에 실행됨.
    private void OnParticleSystemStopped()
    {
        // 인스턴스 매니저의 스킬 이펙트 리스트에서 해당 이펙트를 삭제.
        InstanceManager.s_instance.SkillEffectList.Remove(this);
        // 이펙트 오브젝트 비활성화. (이펙트가 루프하지 않으므로 불필요하지만 시인성을 위해 사용)
        gameObject.SetActive(false);
        // 가비지 콜렉팅 오브젝트의 자식으로 설정. 및 가비지 콜렉션에 등록.
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }
}

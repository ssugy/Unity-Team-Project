using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public void EffectOff()
    {
        // SkillEffectList���� Remove�� ���� �ʴ� ����
        // List�� ���� foreach�� ���ο��� Remove�� Add�� �߻��� �� Invalid Error�� �߻��ϱ� ����.
        gameObject.SetActive(false);
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }
    
    // �ش� �ݹ� �Լ��� �ڽ� ������Ʈ ���� ��� ��ƼŬ�� ����� ���� ������ �����.
    private void OnParticleSystemStopped()
    {
        // �ν��Ͻ� �Ŵ����� ��ų ����Ʈ ����Ʈ���� �ش� ����Ʈ�� ����.
        InstanceManager.s_instance.SkillEffectList.Remove(this);
        // ����Ʈ ������Ʈ ��Ȱ��ȭ. (����Ʈ�� �������� �����Ƿ� ���ʿ������� ���μ��� ���� ���)
        gameObject.SetActive(false);
        // ������ �ݷ��� ������Ʈ�� �ڽ����� ����. �� ������ �ݷ��ǿ� ���.
        transform.SetParent(GarbageCollecting.s_instance.transform);
        GarbageCollecting.s_instance.garbage.Add(gameObject);
    }
}

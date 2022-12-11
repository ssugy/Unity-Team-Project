using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    
    public ParticleSystem commonHitEffectPrefab;
    

    public void PlayHitEffect(Vector3 pos, Quaternion normal, Transform parent = null)
    {
        var targetPrefab = commonHitEffectPrefab;

        var effect = Instantiate(targetPrefab, pos, normal);

        if (parent != null) effect.transform.SetParent(parent);

        effect.Play();
    }
}
using UnityEngine;

public class HumanoidInvincible : MonoBehaviour
{
    [SerializeField]
    private HumanoidMaterials m_materials = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    private float m_invincibleTime = 0.0f;
    private float m_materialsTime = 0.0f;

    private bool m_isInvincible = false;

    public bool IsInvincible {  get { return m_isInvincible; } }

    private void Update()
    {
        if (!m_isInvincible)
        {
            return;
        }

        if (m_invincibleTime < 0.0f)
        {
            FinishInvicible();
            return;
        }

        InvincibleEffect();
    }

    /// <summary>
    /// 無敵時間中に点滅する処理を行います。
    /// </summary>
    private void InvincibleEffect()
    {
        m_invincibleTime -= Time.deltaTime;
        m_materialsTime += Time.deltaTime * m_humanoidData.Params.InvinceibleSpeed;
        if ((int)m_materialsTime % 2 == 1) // 奇数の秒数でマテリアルをチカチカさせる
        {
            m_materialsTime = 0.0f;
            m_materials.SwitchingMeshEveryTime();
        }
    }

    /// <summary>
    /// 無敵時間開始処理を行います。
    /// </summary>
    public void StartInvincible()
    {
        m_isInvincible = true;
        m_invincibleTime = m_humanoidData.Params.InvinceibleTime;
        m_materialsTime = 0.0f;
    }

    /// <summary>
    /// 無敵時間終了処理を行います。
    /// </summary>
    private void FinishInvicible()
    {
        m_materials.InitializeMesh();
        m_isInvincible = false;
    }
}

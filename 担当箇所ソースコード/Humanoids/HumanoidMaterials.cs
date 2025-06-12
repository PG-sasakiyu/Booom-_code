using UnityEngine;

/// <summary>
/// ヒューマノイドのマテリアルをプレイヤーごとに制御するクラスです。
/// </summary>
public class HumanoidMaterials : MonoBehaviour
{
    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private Material[] m_humanoidMaterials = new Material[PlayerManager.PlayerMax];

    [SerializeField]
    private SkinnedMeshRenderer[] m_humanoidMeshs = new SkinnedMeshRenderer[HumanoidMeshMax];

    private const int HumanoidMeshMax = 6;

    private void Start()
    {
        InitializeMesh(m_selfData.Number);
    }

    /// <summary>
    /// 自身のプレイヤー番号に合わせてメッシュのマテリアルを適用させる処理を行います
    /// </summary>
    /// <param name="selfNum"> 自分自身のプレイヤー番号 </param>
    private void InitializeMesh(int selfNum)
    {
        for(int i = 0; i < HumanoidMeshMax; i++)
        {
            m_humanoidMeshs[i].material = m_humanoidMaterials[selfNum];
        }
    }

    /// <summary>
    /// メッシュの表示、非表示を呼び出す度に切り替えます
    /// </summary>
    public void SwitchingMeshEveryTime()
    {
        for (int i = 0; i < HumanoidMeshMax; i++)
        {
            m_humanoidMeshs[i].enabled = !m_humanoidMeshs[i].enabled;
        }
    }

    /// <summary>
    /// メッシュの初期化を行います。
    /// </summary>
    public void InitializeMesh()
    {
        for (int i = 0; i < HumanoidMeshMax; i++)
        {
            m_humanoidMeshs[i].enabled = true;
        }
    }
}
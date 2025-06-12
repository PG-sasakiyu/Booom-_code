using UnityEngine;

/// <summary>
/// 大砲のマテリアルをプレイヤーごとに制御するクラスです。
/// </summary>
public class CannonMaterials : MonoBehaviour
{
    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private Material[] m_cannonBaseMaterial = new Material[PlayerManager.PlayerMax];
    [SerializeField]
    private Material[] m_cannonCleannessMaterial = new Material[PlayerManager.PlayerMax];
    [SerializeField]
    private Material[] m_cannonHumanoidMaterial = new Material[PlayerManager.PlayerMax];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonBaseMesh = new SkinnedMeshRenderer[BaseMeshMax];
    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonCleannessMesh = new SkinnedMeshRenderer[CleannessMeshMax];
    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonHumanoidMesh = new SkinnedMeshRenderer[HumanoidMeshMax];

    private const int BaseMeshMax = 3;
    private const int CleannessMeshMax = 1;
    private const int HumanoidMeshMax = 6;

    private void Start()
    {
        SetBaseMesh(m_selfData.Number);
        SetCleannessMesh(m_selfData.Number);
        SetHumanoidMesh(m_selfData.Number);
    }

    private void SetBaseMesh(int playerNum)
    {
        for (int i = 0; i < m_cannonBaseMesh.Length; i++)
        {
            m_cannonBaseMesh[i].material = m_cannonBaseMaterial[playerNum];
        }
    }

    private void SetCleannessMesh(int playerNum)
    {
        for (int i = 0; i < m_cannonCleannessMesh.Length; i++)
        {
            m_cannonCleannessMesh[i].material = m_cannonCleannessMaterial[playerNum];
        }
    }

    private void SetHumanoidMesh(int playerNum)
    {
        for (int i = 0; i < m_cannonHumanoidMesh.Length; i++)
        {
            m_cannonHumanoidMesh[i].material = m_cannonHumanoidMaterial[playerNum];
        }
    }
}

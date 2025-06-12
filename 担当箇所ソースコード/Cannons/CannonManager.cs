using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private Transform m_bombManager = null;

    public static readonly int CannonMax = 2;

    public Transform BombManager { get { return m_bombManager; } private set { m_bombManager = value; } }

    /// <summary>
    /// 大砲の動作可能なのかを制御します。
    /// </summary>
    public void SetOperable(bool isEnabled)
    {
        for(int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            var cannonMover = m_playerManager.CannonInstances[i].GetComponent<CannonMover>();
            cannonMover.SetOperable(isEnabled);

            var cannonBombShoot = m_playerManager.CannonInstances[i].GetComponent<CannonBombShoot>();
            cannonBombShoot.SetOperable(isEnabled);
        }
    }
}

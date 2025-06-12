using UnityEngine;

public class HumanoidManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    public static readonly int HumanoidMax = 2;

    /// <summary>
    /// ヒューマノイドが動作可能なのかを制御します。
    /// </summary>
    public void SetOperable(bool isEnabled)
    {
        for(int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            var humanoidMover = m_playerManager.HumanoidInstances[i].GetComponent<HumanoidMover>();
            humanoidMover.SetOperable(isEnabled);
        }
    }
}

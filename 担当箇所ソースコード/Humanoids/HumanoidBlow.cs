using UnityEngine;

public class HumanoidBlow : MonoBehaviour
{
    [SerializeField]
    private HumanoidAnimator m_animator = null;

    [SerializeField]
    private Renderer m_humanoidRenderer = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    private bool m_isBlow = false;

    private Vector3 m_blowDirection = Vector3.zero;

    private HumanoidMover m_targetMover = null;

    private void Update()
    {
        if (!m_isBlow)
        {
            return;
        }

        // ヒューマノイドが画面外に出たら
        if(!m_humanoidRenderer.isVisible)
        {
            InitializeFinishBlow();
            return;
        }
        BlowOperation();
    }

    /// <summary>
    /// 爆風で触れた物体が吹き飛ぶ方向を計算します
    /// </summary>
    private Vector3 GetBlowDirection(Vector3 targetPos)
    {
        var direction = transform.position - targetPos;
        float directionY = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);
        return new Vector3(direction.x, directionY, 0.0f).normalized;
    }

    /// <summary>
    /// 爆風を受けたヒューマノイドの初期化処理を行います
    /// </summary>
    public void InitializeStartBlow(Vector3 targetPos, HumanoidMover targetMover)
    {
        if (m_isBlow)
        {
            return;
        }
        
        m_targetMover = targetMover;
        IntiializeHumanoid();

        m_blowDirection = GetBlowDirection(targetPos);
    }

    /// <summary>
    /// ヒューマノイド物理演算の初期化を行います
    /// </summary>
    private void IntiializeHumanoid()
    {
        m_isBlow = true;
        m_targetMover.SetOperable(false);
        m_targetMover.SetPhysicalOperable(false);

        // 吹き飛びアニメーション開始
        m_animator.ChangeTopState(HumanoidAnimator.TopState.Blow);
        m_animator.ChangeUnderState(HumanoidAnimator.UnderState.Blow);
    }

    /// <summary>
    /// 吹き飛び処理終了時にヒューマノイドの初期化を行います
    /// </summary>
    private void InitializeFinishBlow()
    {
        m_isBlow = false;
        m_targetMover.SetOperable(true);
        m_targetMover.SetPhysicalOperable(true);

        // アニメーション遷移可能状態にする
        m_animator.TransferableState(top: HumanoidAnimator.TopState.Blow);
        m_animator.TransferableState(under: HumanoidAnimator.UnderState.Blow);
    }

    /// <summary>
    /// 吹き飛び処理を行います。
    /// </summary>
    private void BlowOperation()
    {
        this.transform.position += (m_blowDirection * m_humanoidData.Params.BlowSpeed) * Time.deltaTime;
    }
}
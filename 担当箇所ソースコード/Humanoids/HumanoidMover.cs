using UnityEngine;
using static HumanoidAnimator;

public class HumanoidMover : MonoBehaviour
{
    [SerializeField]
    private HumanoidData m_humanoidData = null;

    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private BoxCollider m_collider = null;

    [SerializeField]
    private HumanoidAnimator m_animator = null;

    private Transform m_parentTrans = null;

    private float m_movementValue = 0.0f;
    private float m_stunExitTIme = 0.0f;

    private bool m_isStun = false;
    private bool m_isOperable = true;

    private const float FallingMinValue = 0.1f;
    private const float RayDistance = 0.6f;

    private static readonly Vector3 FixedRayStartPos = new Vector3(0.0f, 0.5f, 0.0f);

    private void Awake()
    {
        m_parentTrans = this.transform.parent;
    }

    private void Update()
    {
        if(!m_isOperable)
        {
            if (m_isStun)
            {
                CountStunTime();
            }
            return;
        }

        if (CanMove())
        {
            MoveOperation();
        }

        if(CanJump())
        {
            JumpOperation();
        }

        if(!IsGrounded() && m_rigidbody.velocity.y <= FallingMinValue)
        {
            m_animator.ChangeTopState(TopState.Falling);
            m_animator.ChangeUnderState(UnderState.Falling);
        }
    }

    /// <summary>
    /// ヒューマノイドの移動処理を行います
    /// </summary>
    private void MoveOperation()
    {
        var speedX = m_humanoidData.Params.MoveSpeed * m_movementValue;
        this.transform.position += new Vector3(speedX, 0.0f, 0.0f) * Time.deltaTime;

        m_animator.ChangeTopState(TopState.Move);
        m_animator.ChangeUnderState(UnderState.Move);
    }

    /// <summary>
    /// プレイヤーが移動できる状態かを確認します
    /// </summary>
    private bool CanMove()
    {
        m_movementValue = m_inputData.GetHumanoidMoveInput(m_selfData.Number);
        if (m_movementValue > InputData.MovementDeadZoneRange || m_movementValue < -InputData.MovementDeadZoneRange)
        {
            return true;
        }

        m_animator.TransferableState(top: TopState.Move);
        m_animator.TransferableState(under: UnderState.Move);

        return false;
    }

    /// <summary>
    /// ジャンプ挙動の処理を行います
    /// </summary>
    private void JumpOperation()
    {
        m_rigidbody.AddForce(new Vector3(0.0f, m_humanoidData.Params.PowerJump, 0.0f), ForceMode.Impulse);

        m_animator.ChangeTopState(TopState.Jump);
        m_animator.ChangeUnderState(UnderState.Jump);
    }

    /// <summary>
    /// ジャンプ挙動を実行できる状態か確認します。
    /// </summary>
    private bool CanJump()
    {
        if(!IsGrounded())
        {
            return false;
        }

        if(m_inputData.WasPressedActionInput(InputData.ActionsType.Jump, m_selfData.Number))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ヒューマノイドが現在地面に着地しているかを調べます
    /// </summary>
    /// <returns> true-> 着地中 false-> 空中 </returns>
    private bool IsGrounded()
    {
        // Ray発射位置設定
        var startPos = this.transform.position + FixedRayStartPos;

        // 左側の発射位置からワールド下向きに飛ばすRayを作成
        var leftRay = new Ray(new Vector3(startPos.x - 0.2f, startPos.y, startPos.z), transform.up * -1);
        if(Physics.Raycast(leftRay, out RaycastHit leftHitInfo, RayDistance))
        {
            if(IsHitGroundOrStand(leftHitInfo))
            {
                return true;
            }
        }

        // 右側の発射位置からワールド下向きに飛ばすRayを作成
        var rightRay = new Ray(new Vector3(startPos.x + 0.2f, startPos.y, startPos.z), transform.up * -1);
        if (Physics.Raycast(rightRay, out RaycastHit rightHitInfo, RayDistance))
        {
            if(IsHitGroundOrStand(rightHitInfo))
            {
                return true;
            }
        }
        
        // 空中にいる場合は親子関係削除
        this.transform.SetParent(m_parentTrans);
        return false;
    }

    private bool IsHitGroundOrStand(RaycastHit hitInfo)
    {
        // Rayと接触したのが地面であれば
        if (TagManager.Instance.SearchedTagName(hitInfo.collider.gameObject, TagManager.Type.Ground))
        {
            // 地面にいる場合は親子関係削除
            this.transform.SetParent(m_parentTrans);

            m_animator.TransferableState(top: TopState.Falling);
            m_animator.TransferableState(under: UnderState.Falling);
            return true;
        }

        // Rayと接触したのが足場であれば
        if (TagManager.Instance.SearchedTagName(hitInfo.collider.gameObject, TagManager.Type.Stand))
        {
            this.transform.SetParent(hitInfo.transform);

            // 落下アニメーションから次アニメーションへ遷移可能にする
            m_animator.TransferableState(top: TopState.Falling);
            m_animator.TransferableState(under: UnderState.Falling);
            return true;
        }
        return false;
    }

    /// <summary>
    /// スタン時間の経過を処理します。
    /// </summary>
    private void CountStunTime()
    {
        if(m_stunExitTIme > 0.0f)
        {
            m_stunExitTIme -= Time.deltaTime;
        }
        else
        {
            m_isStun = false;
            SetOperable(true);
        }
    }

    /// <summary>
    /// 操作可能状態の指定します
    /// </summary>
    public void SetOperable(bool isEnabled)
    {
        m_isOperable = isEnabled;
    }

    /// <summary>
    /// 物理挙動の有無を指定します
    /// </summary>
    public void SetPhysicalOperable(bool isActive)
    {
        m_rigidbody.useGravity = isActive;
        m_rigidbody.isKinematic = !isActive;

        m_collider.enabled = isActive;
    }

    /// <summary>
    /// スタン状態を開始します。
    /// </summary>
    public void StunStart(float stun)
    {
        m_stunExitTIme = stun;
        m_isOperable = false;
        m_isStun = true;
    }
}

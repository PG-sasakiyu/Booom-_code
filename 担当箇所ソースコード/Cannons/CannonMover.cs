using UnityEngine;

public class CannonMover : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    private float m_movementValue = 0.0f;

    private bool m_isOperable = true;

    private static readonly float ReverseAngle = 180.0f;

    private void Awake()
    {
        InitializeRotation();
    }

    void Update()
    {
        if(!m_isOperable)
        {
            return;
        }

        if (CanMove())
        {
            MoveOperation();
        }
    }

    /// <summary>
    /// 大砲がステージ向きになるよう回転します
    /// </summary>
    private void InitializeRotation()
    {
        var eulerAngles = this.transform.eulerAngles;
        if(this.transform.position.x <= 0.0f)
        {
            eulerAngles.y = ReverseAngle;
        }
        this.transform.eulerAngles = eulerAngles;
    }

    /// <summary>
    /// 大砲プレイヤーの移動処理を行います
    /// </summary>
    private void MoveOperation()
    {
        var position = this.transform.position;
        var moveSpeed = (m_movementValue * m_cannonData.Params.MoveSpeed) * Time.deltaTime;

        position.y += moveSpeed;
        if(position.y < m_cannonData.Positions.MovePosMinY)
        {
           position.y = m_cannonData.Positions.MovePosMinY;
        }

        if(position.y > m_cannonData.Positions.MovePosMaxY)
        {
           position.y = m_cannonData.Positions.MovePosMaxY;
        }

        this.transform.position = position;
    }

    /// <summary>
    /// 大砲プレイヤーが移動可能か調べます
    /// </summary>
    private bool CanMove()
    {
        m_movementValue = m_inputData.GetCannonMoveInput(m_selfData.Number);
        if (m_movementValue >= InputData.MovementDeadZoneRange || m_movementValue <= -InputData.MovementDeadZoneRange)
        {
            return true;
        }

        return false;
    }

    public void SetOperable(bool isEnabled)
    {
        m_isOperable = isEnabled;
    }
}
using UniRx;
using UnityEngine;

public class HumanoidDirectionRotator : MonoBehaviour
{
    [SerializeField]
    private Transform m_modelTransform = null;

    [SerializeField]
    private HumanoidAnimator m_animator = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    public ReactiveProperty<bool> IsRight { get; private set; } = new ReactiveProperty<bool>(false);

    private void Start()
    {
        IsRight.Subscribe(msg => SetBodyAngle()).AddTo(this);
        IsRight.Subscribe(msg => m_animator.SwitchMirroring()).AddTo(this);
    }

    private void Update()
    {
        SerachDirection();
    }

    /// <summary>
    /// 現在の向きを調べます
    /// </summary>
    private void SerachDirection()
    {
        var movementValue = m_inputData.GetHumanoidMoveInput(m_selfData.Number);
        if (movementValue <= InputData.MovementDeadZoneRange && movementValue >= -InputData.MovementDeadZoneRange)
        {
            return;
        }

        if (movementValue > 0.0f)
        {
            IsRight.Value = true;
        }
        else
        {
            IsRight.Value = false;
        }
        return;
    }

    /// <summary>
    /// プレイヤーの動きに合わせて体の角度を変更します
    /// </summary>
    private void SetBodyAngle()
    {
        if(IsRight.Value)
        {
            m_modelTransform.rotation = Quaternion.Euler(0.0f, m_humanoidData.Params.RightBodyAngle, 0.0f);
            return;
        }
        m_modelTransform.rotation = Quaternion.Euler(0.0f, m_humanoidData.Params.LeftBodyAngle, 0.0f);
    }
}

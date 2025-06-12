using UnityEngine;

[CreateAssetMenu(fileName = "HumanoidData", menuName = "ScriptableObjects/HumanoidData", order = 1)]
public class HumanoidData : ScriptableObject
{
    [SerializeField]
    private PositionData m_positionData = new PositionData();
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    public PositionData Positions { get { return m_positionData; } private set { m_positionData = value; } }
    public ParamsData Params { get { return m_paramsData; } private set { m_paramsData = value; } }

    [System.Serializable]
    public class PositionData
    {
        [SerializeField, Header("ゲーム開始時座標を設定")]
        private Vector2[] m_startPos = new Vector2[HumanoidManager.HumanoidMax];

        [SerializeField, Header("リスポーン座標を設定")]
        private Vector2[] m_respawnPos = new Vector2[HumanoidManager.HumanoidMax];

        public Vector2[] StartPos { get { return m_startPos; } private set { m_startPos = value; } }
        public Vector2[] RespawnPos { get { return m_respawnPos; } private set { m_respawnPos = value; } }
    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("左右の移動速度")]
        private float m_moveSpeed = 0.0f;

        [SerializeField, Header("ジャンプの強さ（高さ）")]
        private float m_powerJump = 0.0f;

        [SerializeField, Header("右向いた時の角度")]
        private float m_rightBodyAngle = 0.0f;

        [SerializeField, Header("左向いた時の角度")]
        private float m_leftBodyAngle = 0.0f;

        [SerializeField,Header("復帰後無敵時間")]
        private float m_invincibleTime = 0.0f;

        [SerializeField, Header("点滅速度")]
        private float m_invincibleSpeed = 0.0f;

        [SerializeField, Header("リスポーンまでの時間")]
        private float m_respwanTime = 0.0f;

        [SerializeField, Header("吹き飛び速度")]
        private float m_blowSpeed = 0.0f;

        public float MoveSpeed { get { return m_moveSpeed; } private set { m_moveSpeed = value; } }
        public float PowerJump { get { return m_powerJump; } private set { m_powerJump = value; } }
        public float RightBodyAngle { get { return m_rightBodyAngle; } private set { m_rightBodyAngle = value; } }
        public float LeftBodyAngle { get { return m_leftBodyAngle; } private set { m_leftBodyAngle = value; } }
        public float InvinceibleTime { get { return m_invincibleTime; } private set { m_invincibleTime = value; } }
        public float InvinceibleSpeed { get { return m_invincibleSpeed; } private set { m_invincibleSpeed = value; } }
        public float RespawnTime { get { return m_respwanTime; } private set { m_respwanTime = value; } }
        public float BlowSpeed { get { return m_blowSpeed; } private set { m_blowSpeed = value; } }
    }
}

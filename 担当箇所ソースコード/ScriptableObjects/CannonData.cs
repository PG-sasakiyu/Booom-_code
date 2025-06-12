using UnityEngine;

[CreateAssetMenu(fileName = "NewCannonData", menuName = "ScriptableObjects/CannonDeta")]
public class CannonData : ScriptableObject
{
    [SerializeField]
    private PositionData m_positionData = new PositionData();

    [SerializeField]
    private ParamsData m_paramData = new ParamsData();

    public PositionData Positions { get { return m_positionData; } set { value = m_positionData; } }
    public ParamsData Params { get { return m_paramData; } set { value = m_paramData; } }

    [System.Serializable]
    public class PositionData
    {
        [SerializeField, Header("ゲーム開始時座標を設定")]
        private Vector2[] m_startPos = new Vector2[CannonManager.CannonMax];

        [SerializeField, Header("移動できるY座標の最小値")]
        private float m_movePosMinY = 0.0f;
        
        [SerializeField, Header("移動できるY座標の最大値")]
        private float m_movePosMaxY = 0.0f;

        public Vector2[] StartPos { get { return m_startPos; } private set { m_startPos = value; } }
        public float MovePosMinY { get { return m_movePosMinY; } private set { m_movePosMinY = value; } }
        public float MovePosMaxY { get { return m_movePosMaxY; } private set { m_movePosMaxY = value; } }
    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("移動速度")]
        private float m_moveSpeed = 0.0f;

        [SerializeField, Header("爆弾の最小発射速度")]
        private float m_shootSpeedMin = 0.0f;

        [SerializeField, Header("爆弾の最大発射速度")]
        private float m_shootSpeedMax = 0.0f;

        [SerializeField, Header("爆弾の飛距離増加速度")]
        private float m_shootChargeSpeed = 0.0f;

        [SerializeField, Header("爆弾の発射角度")]
        private float m_shootAngle = 0.0f;

        [SerializeField, Header("爆弾を込めれる弾倉数")]
        private int m_magazineMax = 0;

        [SerializeField,Header("爆弾のリロード時間（秒）")]
        private float m_reloadTime = 0.0f;

        [SerializeField, Header("大砲の冷却時間（という名の発射間隔）")]
        private float m_cookingOffTime = 0.0f;

        public float MoveSpeed { get { return m_moveSpeed; } private set { m_moveSpeed = value; } }
        public float ShootSpeedMin { get { return m_shootSpeedMin; } private set { m_shootSpeedMin = value; } }
        public float ShootSpeedMax { get { return m_shootSpeedMax; } private set { m_shootSpeedMax = value; } }
        public float ShootChargeSpeed { get { return m_shootChargeSpeed; } private set { m_shootChargeSpeed = value; } }
        public float ShootAngle { get { return m_shootAngle; } private set { m_shootAngle = value; } }
        public int MagazineMax { get { return m_magazineMax; } private set { m_magazineMax = value; } }
        public float ReloadTime { get { return m_reloadTime; } private set { m_reloadTime = value; } }
        public float CookingOffTime { get { return m_cookingOffTime; } private set { m_cookingOffTime = value; } }
    }
}
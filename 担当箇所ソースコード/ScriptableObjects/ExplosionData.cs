using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosionData", menuName = "ScriptableObjects/ExplosionData", order = 1)]
public class ExplosionData : ScriptableObject
{
    [SerializeField]
    private ParamsData m_paramsData = null;

    [SerializeField]
    private BlowData m_blowData = new BlowData();

    [SerializeField]
    private EffectData m_effectData = null;

    public ParamsData Params { get {  return m_paramsData; } private set { value = m_paramsData; } }
    public BlowData Blow { get { return m_blowData; } private set { value = m_blowData; } }
    public EffectData Effect { get { return m_effectData; } private set { m_effectData = value; } }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("爆風の判定が実際に発生するまでのディレイ")]
        private float m_colliderActivateDelayTime = 0.0f;

        [SerializeField, Header("爆風の持続フレーム数")]
        private int m_durationFrameCount = 0;

        [SerializeField, Header("エフェクト含めすべての再生が終了するまでの時間")]
        private float m_effectEndTime = 0.0f;

        public float ColliderActivateDelayTime { get { return m_colliderActivateDelayTime; } private set { value = m_colliderActivateDelayTime; } }
        public int DurationFrameCount { get { return m_durationFrameCount; } private set { value = DurationFrameCount; } }
        public float EffectEndTime { get { return m_effectEndTime; } private set { value = m_effectEndTime; } }
    }

    [System.Serializable]
    public class BlowData
    {
        [SerializeField, Header("吹き飛び後の減速開始するまでの時間")]
        private float m_decelerationStartTime = 3.2f;

        [SerializeField, Header("吹き飛び後の減速しきるまでの時間")]
        private float m_decelerationTime = 1.5f;

        [SerializeField, Header("吹き飛び中に操作が効かない時間")]
        private float m_cantInputTime = 1.0f;

        [SerializeField, Header("吹き飛び中の減速率")]
        private float m_decelerationRate = 0.5f;

        public float DecelerationStartTime { get { return m_decelerationStartTime; } private set { value = m_decelerationStartTime; } }
        public float DecelerationTime { get { return m_decelerationTime; } private set { value = m_decelerationTime; } }
        public float CantInputTime { get { return m_cantInputTime; } private set { value = m_cantInputTime; } }
        public float DecelerationRate { get { return m_decelerationRate; } private set { value = m_decelerationRate; } }
    }

    [System.Serializable]
    public class EffectData
    {
        [SerializeField, Header("爆発の初期サイズ")]
        private float m_scaleMin = 0.0f;

        [SerializeField, Header("爆発の最大サイズ")]
        private float m_scaleMax = 0.0f;

        [SerializeField, Header("最大サイズまで大きくなる時間")]
        private float m_scaleChangeTime = 0.0f;

        [SerializeField, Header("最大サイズを維持する時間")]
        private float m_scaleMaxTime = 0.0f;

        [SerializeField,Header("エフェクト終了時の消滅にかかる時間")]
        private float m_effectEndTime = 0.0f;

        public float ScaleMin { get { return m_scaleMin; } private set { m_scaleMin = value; } }
        public float ScaleMax { get { return m_scaleMax; } private set { m_scaleMax = value; } }
        public float ScaleChangeTime { get { return m_scaleChangeTime; } private set { m_scaleChangeTime = value; } }
        public float ScaleMaxTime { get { return m_scaleMaxTime;} private set { m_scaleMaxTime = value; } }
        public float EffectEndTime { get { return m_effectEndTime; } private set { m_effectEndTime = value; } }
    }
}

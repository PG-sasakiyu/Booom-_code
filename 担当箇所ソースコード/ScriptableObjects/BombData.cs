using UnityEngine;

[CreateAssetMenu(fileName = "NewBombData", menuName = "ScriptableObjects/BombData", order = 1)]
public class BombData : ScriptableObject
{
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    public ParamsData Params { get { return m_paramsData; } private set { value = m_paramsData; } }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("爆風による吹き飛びの強さ")]
        private float m_explosionPower = 0.0f;

        [SerializeField, Header("爆弾の爆風の広さ")]
        private float m_explosionRange = 0.0f;

        public float ExplosionPower { get { return m_explosionPower; } private set { value = m_explosionPower; } }
        public float ExplosionRange { get { return m_explosionRange; } private set { value = m_explosionRange; } }
    }
}
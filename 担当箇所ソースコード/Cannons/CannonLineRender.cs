using UnityEngine;

public class CannonLineRender : MonoBehaviour
{
    [SerializeField]
    private CannonBombShoot m_cannonBombShoot = null;

    [SerializeField]
    private LineRenderer m_lineRenderer = null;

    [SerializeField,Header("軌道描画の最長距離までの密度")]
    private int m_lineRenderCount = 0;

    [SerializeField,Header("軌道描画の距離")]
    private float m_lineLength = 0.0f;

    private Vector3 m_shootVelocity = Vector3.zero;
    private Vector3 m_lineStartPosition = Vector3.zero;

    /// <summary>
    /// 鉛直投げ上げ公式の1/2
    /// </summary>
    private static readonly float m_oneHalf = 0.5f;

    private void Update()
    {
        if (m_cannonBombShoot.IsShoot)
        {
            m_lineRenderer.enabled = true;
            GetRenderParabola();
        }
        else
        {
            //ボタンが押されていなければ描画しない。
            m_lineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// 爆弾の発射軌道を描画させる処理をします
    /// </summary>
    private void GetRenderParabola()
    {
        m_shootVelocity = m_cannonBombShoot.ShootVelocity;
        m_lineStartPosition = m_cannonBombShoot.ShootInitialPosition;
        m_lineRenderer.positionCount = m_lineRenderCount;

        //描画座標の間隔
        float timeStep = m_lineLength / m_lineRenderCount;
        bool draw = false;

        for (int i = 0; i < m_lineRenderCount; i++)
        {
            //renderTime 　開始位置からの経過時間
            float renderTime = timeStep * i;
            //endTime 　renderTimeの次の描画座標の経過時間
            float endTime = renderTime + timeStep;

            SetLineRendererPosition(i, renderTime);
            draw = IsHitObject(renderTime, endTime);

            // 衝突したら描画をやめる。
            if (draw)
            {
                m_lineRenderer.positionCount = i;
                break;
            }
        }
    }

    /// <summary>
    /// m_lineRendererの座標に描画する処理を行います。
    /// </summary>
    /// <param name="renderTime">描画座標の開始位置からの経過時間</param>
    private void SetLineRendererPosition(int index, float renderTime)
    {
        m_lineRenderer.SetPosition(index, GetRenderPointPosition(renderTime));
    }

    /// <summary>
    /// 描画したLineの衝突判定を調べます。
    /// </summary>
    private bool IsHitObject(float renderTime, float endTime)
    {
        //始終点の座標
        Vector3 startPosition = GetRenderPointPosition(renderTime);
        Vector3 endPosition = GetRenderPointPosition(endTime);

        // 衝突判定があったらtrueを返す
        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            if(TagManager.Instance.SearchedTagName(hitInfo.transform.gameObject, TagManager.Type.Bomb))
            {
                return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 鉛直投げ上げの公式を利用
    /// 開始位置 + 初速度*時間 + 1/2 * 2^2 * 重力
    /// </summary>
    private Vector3 GetRenderPointPosition(float time)
    {
        return (m_lineStartPosition + ((m_shootVelocity * time) + (m_oneHalf * time * time) * Physics.gravity));
    }
}

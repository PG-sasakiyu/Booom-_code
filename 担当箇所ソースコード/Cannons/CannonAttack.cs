using System.Collections;
using UnityEngine;

public class CannonAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bombPrefab = null;

    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private Transform m_generatePosTrans = null;

    private int m_currentMagazine = 0;

    private float m_reloadTimer = 0.0f;

    private bool m_isCoockingOff = false;
    private bool m_isOperable = true;

    private const int FireableCount = 1;
    
    private void Awake()
    {
        m_currentMagazine = m_cannonData.Params.MagazineMax;
    }

    void Update()
    {
        if(!m_isOperable)
        {
            return;
        }

        if (CanAttack())
        {
            AttackOperation();
        }
        ReloadingCannon();
    }

    /// <summary>
    /// 爆弾発射する攻撃の処理を行います
    /// </summary>
    private void AttackOperation()
    {
        // 爆弾を生成
        var instance = Instantiate(m_bombPrefab, m_generatePosTrans.position, Quaternion.identity);

        // 爆弾発射SE再生
        SoundEffectManager.Instance.OnPlayOneShot((int)SoundEffectManager.MainScenePattern.Cannon);

        // 発射口の冷却開始
        StartCoroutine(CookingOffCannon());

        // 所持弾数を一発減らす
        m_currentMagazine--;
    }

    /// <summary>
    /// 現在攻撃できる状態かを調べます
    /// </summary>
    private bool CanAttack()
    {
        if (!m_inputData.WasPressedActionInput(InputData.ActionsType.Attack, m_selfData.Number)) 
        {
            return false;
        }

        // 所持弾数が1発以上あり、大砲の冷却が完了していたら
        if (m_currentMagazine > FireableCount || !m_isCoockingOff)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 大砲のリロード処理を行います（弾倉に空きがあればリロードします）
    /// </summary>
    private void ReloadingCannon()
    {
        // 既に弾倉が埋まっていたらreturn
        if (m_currentMagazine >= m_cannonData.Params.MagazineMax)
        {
            return;
        }

        m_reloadTimer += Time.deltaTime;
        if (m_reloadTimer > m_cannonData.Params.ReloadTime)
        {
            m_currentMagazine++;
            m_reloadTimer = 0.0f;
        }
    }

    /// <summary>
    /// 大砲の発射口を冷却する処理を行います
    /// </summary>
    private IEnumerator CookingOffCannon()
    {
        m_isCoockingOff = true;
        yield return new WaitForSeconds(m_cannonData.Params.CookingOffTime);
        m_isCoockingOff = false;
    }

    /// <summary>
    /// 攻撃可能の有無を変更します。
    /// </summary>
    public void SetOperable(bool isEnabled)
    {
        m_isOperable = isEnabled;
    }
}

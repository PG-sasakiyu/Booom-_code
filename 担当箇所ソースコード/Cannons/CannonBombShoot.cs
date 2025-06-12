using UnityEngine;
using UnityEngine.UI;
using static InputData;

public class CannonBombShoot : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private Transform m_shootTransform = null;

    [SerializeField]
    private GameObject m_bombrefab = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private Slider m_gaugeSlider = null;

    [SerializeField]
    private Image m_gaugeImage = null;

    [SerializeField,Header("発射の強さ")]
    private float m_shootPower = 0.0f;

    [SerializeField]
    private float m_bombStockTime = 0.0f;

    [SerializeField]
    private float m_cannonCookingOffTime = 0.0f;

    private bool m_isShoot = false;
    private bool m_isOperable = false;
    private bool m_isGaugeChargeUp = true;

    private Vector3 m_shootInitialPosition = Vector3.zero;
    private Vector3 m_shootVelocity = Vector3.zero;

    public Vector3 ShootVelocity { get { return m_shootVelocity; } private set { m_shootVelocity = value; } }
    public Vector3 ShootInitialPosition { get { return m_shootInitialPosition; } private set { m_shootInitialPosition = value; } }
  
    public bool IsShoot { get { return m_isShoot; } private set { m_isShoot = value; } }

    private void Start()
    {
        m_cannonManager = GetComponentInParent<CannonManager>();
        m_shootTransform.Rotate(0.0f,0.0f, m_cannonData.Params.ShootAngle);
        ShootInitialPosition = m_shootTransform.transform.position;
    }

    private void Update()
    {
        if (!m_isOperable)
        {
            return;
        }

        m_gaugeSlider.value = m_bombStockTime / m_cannonData.Params.MagazineMax;
        ChangeGaugeColor();

        if (m_cannonCookingOffTime < m_cannonData.Params.CookingOffTime)
        {
            m_cannonCookingOffTime += Time.deltaTime;
            return;
        }

        if (m_inputData.WasPressedActionInput(ActionsType.Attack, m_selfData.Number)
         && CanShootBomb())
        {
            ShootPowerCharge();
        }

        if (!m_inputData.WasPressedActionInput(ActionsType.Attack, m_selfData.Number) && IsShoot)
        {
            ShootBombOperation();
        }

        if(m_bombStockTime < m_cannonData.Params.MagazineMax)
        {
            m_bombStockTime += Time.deltaTime / m_cannonData.Params.ReloadTime;
        }
    }

    /// <summary>
    /// ボタンが押されている間、飛距離を伸ばす処理を行います。
    /// </summary>
    private void ShootPowerCharge()
    {
        ChargeShootPower();
        ShootInitialPosition = m_shootTransform.transform.position;
        ShootVelocity = m_shootTransform.up * m_shootPower;
        IsShoot = true;
    }

    /// <summary>
    /// 飛距離の増減処理を行います。
    /// </summary>
    private void ChargeShootPower()
    {
        if (m_cannonData.Params.ShootSpeedMax > m_shootPower && m_isGaugeChargeUp)
        {
            m_shootPower += m_cannonData.Params.ShootChargeSpeed * Time.deltaTime;
        }
        else
        {
            m_isGaugeChargeUp = false;
        }

        if (m_shootPower > m_cannonData.Params.ShootSpeedMin && !m_isGaugeChargeUp)
        {
            m_shootPower -= m_cannonData.Params.ShootChargeSpeed * Time.deltaTime;
        }
        else
        {
            m_isGaugeChargeUp = true;
        }
    }

    /// <summary>
    ///ボムを投げる処理を行います。
    /// </summary>
    private void ShootBombOperation()
    {
        SoundEffectManager.Instance.OnPlayOneShot((int)SoundEffectManager.MainScenePattern.Cannon);
        var rigidbody = GenerateBomb().GetComponent<Rigidbody>();
        rigidbody.AddForce(ShootVelocity * rigidbody.mass, ForceMode.Impulse);
        m_shootPower = m_cannonData.Params.ShootSpeedMin;
        IsShoot = false;
        m_cannonCookingOffTime = 0.0f;
        m_bombStockTime--;
    }

    /// <summary>
    /// ゲージの色を変更します。
    /// </summary>
    private void ChangeGaugeColor()
    {
        if (m_bombStockTime >= m_cannonData.Params.MagazineMax - 1.0f)
        {
            m_gaugeImage.color = Color.green;
        }
        else if (m_bombStockTime >= m_cannonData.Params.MagazineMax - 2.0f)
        {
            m_gaugeImage.color = Color.yellow;
        }
        else
        {
            m_gaugeImage.color = Color.red;
        }
    }

    /// <summary>
    /// ボムを生成し、bombのGameObjectを返します。
    /// </summary>
    private GameObject GenerateBomb()
    {
        var bomb = Instantiate(m_bombrefab, m_shootTransform.position, Quaternion.identity, m_cannonManager.BombManager);
        return bomb;
    }

    /// <summary>
    /// 爆弾の弾が発射可能なのかを調べます
    /// </summary>
    private bool CanShootBomb()
    {
        if(m_bombStockTime >= 1.0f)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 爆弾を発射可能なのかを制御します
    /// </summary>
    public void SetOperable(bool isEnabled)
    {
        m_isOperable = isEnabled;
    }
}

using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RoundUIController : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 0.0f;

    [SerializeField]
    private RoundManager m_roundManager = null;

    [SerializeField]
    private ScreenFade m_screenFade = null;

    [SerializeField]
    private Image[] m_faceIconImage = new Image[PlayerManager.PlayerMax];

    [SerializeField]
    private Sprite[] m_faceIconSprite = new Sprite[PlayerManager.PlayerMax];

    [SerializeField]
    private List<Image> m_teamConfirmImage = new List<Image>();

    [SerializeField]
    private List<Image> m_roundTextImage = new List<Image>();

    [SerializeField]
    private Image m_playerTeamImage = null;

    private bool m_isMove = false;
    private RectTransform m_rectTransform = null;
    private Vector3 m_moveTerminalPos = new Vector3(960.0f, 540.0f, 0.0f);

    private const float AlmostDestination = 0.05f;

    public ReactiveProperty<bool> IsArrived { get; private set; } = new ReactiveProperty<bool>(false);

    public enum TextType
    {
        Ready,
        Go,
        Finish
    }

    private void Start()
    {
        IsArrived.Subscribe(msg => StartCoroutine(m_roundManager.PrepareRoundSequence())).AddTo(this);
        m_rectTransform = m_teamConfirmImage[RoundManager.CurrentRound].transform.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!m_screenFade.IsFinishFadeIn)
        {
            return;
        }

        if(IsArrived.Value || m_isMove)
        {
            return;
        }
        StartMoveTeamConfirmUI();
    }

    private void FixedUpdate()
    {
        if (!m_isMove)
        {
            return;
        }
        MoveTeamConfirmUI();
    }

    /// <summary>
    /// 画面左からスライドさせ画像を登場させる移動処理を行います
    /// </summary>
    private void MoveTeamConfirmUI()
    {
        // ほぼ目的地に近い距離になったら移動を終了し、終点値に座標を固定する
        if (Mathf.Abs(m_rectTransform.position.y - m_moveTerminalPos.y) <= AlmostDestination)
        {
            m_rectTransform.position = m_moveTerminalPos;
            IsArrived.Value = true;
            m_isMove = false;
            return;
        }

        var nextFramePos = Vector3.MoveTowards
        (
            m_rectTransform.position, m_moveTerminalPos, m_moveSpeed
        );
        m_rectTransform.position = nextFramePos;
    }

    /// <summary>
    /// プレイヤーの顔アイコンをシャッフルする処理を行います
    /// </summary>
    public IEnumerator ShuffleFaceIcon(float shuffleTime)
    {
        for (int i = 0; i < m_faceIconImage.Length; i++)
        {
            m_faceIconImage[i].enabled = true;
        }

        var startTime = Time.time;
        while(true)
        {
            var elapsedTime = Time.time - startTime;
            if (elapsedTime > shuffleTime)
            {
                break;
            }

            for (int i = 0; i < m_faceIconImage.Length; i++)
            {
                var randomValue = Random.Range(0, PlayerManager.PlayerMax);
                m_faceIconImage[i].sprite = m_faceIconSprite[randomValue];
            }

            yield return null;
        }
    }

    /// <summary>
    /// ラウンド開始のテキストを表示、非表示を設定できます
    /// </summary>
    public IEnumerator DrawRoundStartText(TextType typeNum, float drawTime)
    {
        m_roundTextImage[(int)typeNum].enabled = true;

        yield return new WaitForSeconds(drawTime);
        m_roundTextImage[(int)typeNum].enabled = false;
    }

    /// <summary>
    /// チーム確認画像のスライド移動の初期化を行います
    /// </summary>
    public void StartMoveTeamConfirmUI()
    {
        m_isMove = true;
        m_teamConfirmImage[RoundManager.CurrentRound].enabled = true;
    }

    /// <summary>
    /// プレイヤーの顔アイコンを表示する処理を行います
    /// </summary>
    public void DrawFaceIcon()
    {
        for (int i = 0; i < m_faceIconImage.Length; i++)
        {
            m_faceIconImage[i].enabled = true;
        }
    }

    /// <summary>
    /// プレイヤーの顔アイコンを非表示にする処理を行います
    /// </summary>
    public IEnumerator HiddenFaceIcon(float drawTime)
    {
        yield return new WaitForSeconds(drawTime);

        for (int i = 0; i < m_faceIconImage.Length; i++)
        {
            m_faceIconImage[i].enabled = false;
        }
    }

    /// <summary>
    /// 表示させるプレイヤーアイコンを設定します
    /// </summary>
    public void SetFaceIconSprite()
    {
        var alphaNum = TeamGenerator.AlphaTeamNumber;
        var bravoNum = TeamGenerator.BravoTeamNumber;
        for (int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            m_faceIconImage[i].sprite = m_faceIconSprite[bravoNum[i]]; 
            m_faceIconImage[i + TeamGenerator.MembersCount].sprite = m_faceIconSprite[alphaNum[i]];
        }
    }

    /// <summary>
    /// ConfirmUIの表示を制御します。
    /// </summary>
    public void HiddenTeamConfirmUI()
    {
        m_teamConfirmImage[RoundManager.CurrentRound].enabled = false;
    }

    /// <summary>
    /// TeamUIの表示を制御します。
    /// </summary>
    public void SetTeamUI(bool isEnabled)
    {
        m_playerTeamImage.enabled = isEnabled;
    }
}
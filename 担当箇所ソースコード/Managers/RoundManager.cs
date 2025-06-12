using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private float m_textDrawDelayTime = 0.0f;

    [SerializeField]
    private float m_ReadytextDrawDelayTime = 0.0f;

    [SerializeField]
    private HumanoidManager m_humanoidManager = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private BombDestroyer m_bombManager = null;

    [SerializeField]
    private EffectManager m_effectManager = null;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private ScreenFade m_screenFade = null;

    [SerializeField]
    private GameTimer m_gameTimer = null;

    [SerializeField]
    private ScoreManager m_scoreManager = null;

    [SerializeField]
    private DrawScoreText m_drawScoreText = null;

    [SerializeField]
    private CrownGenerator m_crownGenerator = null;

    [SerializeField]
    private SpecialCrownGenerator m_specialCrownGenerator = null;

    [SerializeField]
    private RoundUIController m_roundUIController = null;

    [SerializeField]
    private SurviveScoreManager m_surviveScoreManager = null;

    public static int CurrentRound { get; private set; }

    public enum RoundState
    {
        One,
        Two,
        Max
    }

    private void Start()
    {
        m_humanoidManager.SetOperable(false);
        m_cannonManager.SetOperable(false);
    }

    private void SwitchNextRound()
    {
        CurrentRound++;
        m_sceneChanger.LoadSpecifiedScene(SceneChanger.SceneName.Main);
    }

    private void InitializeRoundStart()
    {
        m_gameTimer.StartCountDown();
        m_drawScoreText.SetDrawing(true);
        StartCoroutine(m_crownGenerator.GenerateStart());
        StartCoroutine(m_specialCrownGenerator.GenerateStartSpecialCrown());
        m_surviveScoreManager.enabled = true;
    }

    /// <summary>
    /// ラウンド開始までの準備を行います
    /// </summary>
    public IEnumerator PrepareRoundSequence()
    {
        if (!m_roundUIController.IsArrived.Value)
        {
            yield break;
        }

        BackGroundMusicManager.Instance.OnPlayOneShot(BackGroundMusicManager.MusicName.TeamAndRoll);
        if (CurrentRound == (int)RoundState.One)
        {
            // ラウンド1のみ行うシャッフルとチーム確認
            m_roundUIController.DrawFaceIcon();
            m_roundUIController.SetFaceIconSprite();
            yield return StartCoroutine(m_roundUIController.HiddenFaceIcon(m_textDrawDelayTime));

            m_roundUIController.HiddenTeamConfirmUI();
            m_roundUIController.SetTeamUI(true);
            yield return StartCoroutine(m_roundUIController.HiddenFaceIcon(m_textDrawDelayTime));
        }

        if (CurrentRound == (int)RoundState.Two)
        {
            yield return StartCoroutine(m_roundUIController.HiddenFaceIcon(m_textDrawDelayTime));

            m_roundUIController.HiddenTeamConfirmUI();
        }
        
        m_roundUIController.SetTeamUI(false);

        BackGroundMusicManager.Instance.StartVolumeFadeOut();
        SoundEffectManager.Instance.OnPlayOneShot((int)SoundEffectManager.MainScenePattern.Ready);
        yield return StartCoroutine(m_roundUIController.DrawRoundStartText(RoundUIController.TextType.Ready, m_ReadytextDrawDelayTime));

        m_humanoidManager.SetOperable(true);
        m_cannonManager.SetOperable(true);

        BackGroundMusicManager.Instance.OnPlayOneShot(BackGroundMusicManager.MusicName.MainBGM);
        yield return StartCoroutine(m_roundUIController.DrawRoundStartText(RoundUIController.TextType.Go, m_ReadytextDrawDelayTime));

        InitializeRoundStart();
    }

    /// <summary>
    /// ラウンド終了演出を出し、ラウンドの初期化処理を行います
    /// </summary>
    public IEnumerator InitializeRoundFinish()
    {
        if(!m_gameTimer.IsTimeLimit.Value)
        {
            yield break;
        }

        m_scoreManager.OffGetPoint();
        m_humanoidManager.SetOperable(false);
        m_cannonManager.SetOperable(false);
        m_bombManager.DestoroyBomb();

        m_effectManager.OnPlayEffect(Vector3.zero, 0.0f, EffectManager.Type.Finish);
        yield return StartCoroutine(m_roundUIController.DrawRoundStartText(RoundUIController.TextType.Finish, m_textDrawDelayTime));

        if (CurrentRound == (int)RoundState.One)
        {
            SwitchNextRound();
            yield break; // returnと同義
        }

        CurrentRound = 0; // ラウンド数を初期化する
        m_sceneChanger.LoadNextScene();
    }
}
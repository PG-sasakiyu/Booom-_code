using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct InstancesInfo
{
    public GameObject m_instance;
    public Vector2[] m_positions;
    public List<int> m_index;
    public Transform m_parent;

    public InstancesInfo(GameObject assignInstance,Vector2[] assignPos, List<int> assignIndex, Transform assignParent)
    {
        m_instance = assignInstance;
        m_positions = assignPos;
        m_index = assignIndex;
        m_parent = assignParent;
    }
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_humanoidPrefab = null;

    [SerializeField]
    private GameObject m_cannonPrefab = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private Transform m_humanoidParent = null;

    [SerializeField]
    private Transform m_cannonParent = null;

    private const string PlayerStringName = "Player";

    #region プロパティ値格納変数
    private GameObject[] m_instances = new GameObject[PlayerMax];
    private GameObject[] m_humanoidInstances = new GameObject[HumanoidManager.HumanoidMax];
    private GameObject[] m_cannonInstances = new GameObject[CannonManager.CannonMax];
    #endregion

    public static readonly int PlayerMax = 4;

    public InstancesInfo HummanoidInfo { get; private set; } = new InstancesInfo();
    public InstancesInfo CannonInfo { get; private set; } = new InstancesInfo();

    public GameObject[] HumanoidInstances { get { return m_humanoidInstances; } private set { m_humanoidInstances = value; } }
    public GameObject[] CannonInstances { get { return m_cannonInstances; } private set { m_cannonInstances = value; } }
    public GameObject[] Instances { get { return m_instances; } private set { m_instances = value; } }

    private void Awake()
    {
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            InitializeInstancesInfo(TeamGenerator.AlphaTeamNumber, TeamGenerator.BravoTeamNumber);
            GeneratingPlayer(HummanoidInfo, CannonInfo);
        }

        // 引数で渡すプレファブと座標を入れ替え（攻守交替）
        if(RoundManager.CurrentRound == (int)RoundManager.RoundState.Two)
        {
            InitializeInstancesInfo(TeamGenerator.BravoTeamNumber, TeamGenerator.AlphaTeamNumber);
            GeneratingPlayer(CannonInfo, HummanoidInfo);
        }

        AssignInstances();
    }

    /// <summary>
    /// プレイヤーを生成させる処理を行わせます
    /// </summary>
    private void GeneratingPlayer(InstancesInfo alphaTeamInfo, InstancesInfo bravoTeamInfo)
    {
        var alphaTeamName = TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Alpha];
        GenerateTeam(alphaTeamName, alphaTeamInfo.m_index, alphaTeamInfo.m_instance, alphaTeamInfo.m_positions, alphaTeamInfo.m_parent);

        var bravoTeamName = TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Bravo];
        GenerateTeam(bravoTeamName, bravoTeamInfo.m_index, bravoTeamInfo.m_instance, bravoTeamInfo.m_positions, bravoTeamInfo.m_parent);
    }

    /// <summary>
    /// チーム毎にプレイヤーを生成させる命令を実行します
    /// </summary>
    private void GenerateTeam(string teamName,List<int> teamNumbers, GameObject prefab, Vector2[] positions, Transform parent)
    {
        for (int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            SpawningPlayer(teamName, teamNumbers[i], prefab, positions[i], parent);
        }
    }

    /// <summary>
    /// プレイヤーを生成する処理を行います
    /// </summary>
    private void SpawningPlayer(string teamName, int teamNum, GameObject prefab, Vector3 position, Transform parent)
    {
        Instances[teamNum] = Instantiate(prefab, position, Quaternion.identity, parent);
        SetPlayerName(teamNum);
        SetSelfData(teamName, teamNum);
    }

    /// <summary>
    /// プレイヤーのオブジェクト名を設定します
    /// </summary>
    private void SetPlayerName(int selfNum)
    {
        var playerNum = selfNum + 1;
        Instances[selfNum].name = PlayerStringName + playerNum;
    }

    /// <summary>
    /// 自分自身のデータを登録します。
    /// </summary>
    private void SetSelfData(string selfTeamName, int selfNum)
    {
        var selfData = Instances[selfNum].GetComponent<SelfData>();
        if (selfData != null)
        {
            selfData.SetNumber(selfNum);
            selfData.SetTeamName(selfTeamName);
        }
    }

    /// <summary>
    /// ヒューマノイド、キャノンに分別しインスタンスを設定します
    /// </summary>
    private void AssignInstances()
    {
        for(int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            HumanoidInstances[i] = Instances[HummanoidInfo.m_index[i]];
            CannonInstances[i] = Instances[CannonInfo.m_index[i]];
        }
    }

    /// <summary>
    /// インスタンスの詳細データの初期化を行います。
    /// </summary>
    private void InitializeInstancesInfo(List<int> humanoidIndex, List<int> cannonIndex)
    {
        HummanoidInfo = new InstancesInfo(m_humanoidPrefab, m_humanoidData.Positions.StartPos, humanoidIndex, m_humanoidParent);
        CannonInfo = new InstancesInfo(m_cannonPrefab, m_cannonData.Positions.StartPos, cannonIndex, m_cannonParent);
    }
}

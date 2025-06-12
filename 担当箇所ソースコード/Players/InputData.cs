using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputData : MonoBehaviour
{
    [SerializeField, Header("キーボード操作可能設定")]
    private bool m_isOpreableKeyboard = true;

    private Dictionary<int, Dictionary<KeyCode, ActionsType>> keyMap = new Dictionary<int, Dictionary<KeyCode, ActionsType>>();

    private const float KeyboardMovementValueMax = 1.0f;

    public static readonly float MovementDeadZoneRange = 0.2f;
    public static readonly float UISwitchDeadZoneRange = 0.3f;
    public static readonly float ThrowDeadZoneRange = 0.7f;

    public enum ActionsType
    {
        Jump,
        Attack,
        LeftOrDown,
        RightOrUp
    }
    public enum MenuInteractionInput
    {
        Decision,
        SwitchLeft,
        SwitchRight,
    }

    private void Awake()
    {
        keyMap = new Dictionary<int, Dictionary<KeyCode, ActionsType>>
        {
            { 0,
                new Dictionary<KeyCode, ActionsType>
                {
                    { KeyCode.W, ActionsType.Jump },
                    { KeyCode.S, ActionsType.Attack },
                    { KeyCode.A, ActionsType.LeftOrDown },
                    { KeyCode.D, ActionsType.RightOrUp }
                }
            },
            { 1,
                new Dictionary<KeyCode, ActionsType>
                {
                    { KeyCode.Y, ActionsType.Jump },
                    { KeyCode.H, ActionsType.Attack },
                    { KeyCode.G, ActionsType.LeftOrDown },
                    { KeyCode.J, ActionsType.RightOrUp }
                }
            },
            { 2,
                new Dictionary<KeyCode, ActionsType>
                {
                    { KeyCode.P, ActionsType.Jump },
                    { KeyCode.Semicolon, ActionsType.Attack },
                    { KeyCode.L, ActionsType.LeftOrDown },
                    { KeyCode.Quote, ActionsType.RightOrUp }
                }
            },
            { 3,
                new Dictionary<KeyCode, ActionsType>
                {
                    { KeyCode.UpArrow, ActionsType.Jump },
                    { KeyCode.DownArrow, ActionsType.Attack },
                    { KeyCode.LeftArrow, ActionsType.LeftOrDown },
                    { KeyCode.RightArrow, ActionsType.RightOrUp }
                }
            }
        };
    }

    /// <summary>
    /// プレイヤー番号からペア登録されているコントローラー名を取得します
    /// </summary>
    /// <returns></returns>
    private string ConvertPlayerNumberToControllerName(int playerNum)
    {
        return ControllerManager.Instance.ControllerMap.FirstOrDefault(x => x.Value == playerNum).Key;
    }

    /// <summary>
    /// プレイヤーのアクションボタンを押されたか調べます
    /// </summary>
    private bool WasPressedActionButton(ActionsType currentType, int playerNum)
    {
        var controllerName = ConvertPlayerNumberToControllerName(playerNum);
        foreach (var gamepad in Gamepad.all)
        {
            if(gamepad.name != controllerName)
            {
                continue;
            }

            switch (currentType)
            {
                case ActionsType.Jump:
                    return gamepad.aButton.wasPressedThisFrame;

                case ActionsType.Attack:
                    return gamepad.aButton.isPressed;

                default:
                    return false;
            }
        }
        return false;
    }

    /// <summary>
    /// キーボードのアクションボタンが押されたか調べます
    /// </summary>
    private bool WasPressedActionsKey(ActionsType actionType, int playerNum)
    {
        if (!keyMap.ContainsKey(playerNum))
        {
            return false;
        }

        foreach (var currentType in keyMap[playerNum])
        {
            // ジャンプは長押し判定を取得しないためreturnする
            if(currentType.Value == ActionsType.Jump && actionType == ActionsType.Jump)
            {
                return WasPressedActionsKeyDown(currentType);
            }


            // 引数指定されたキーが押されているか総当たりします（長押し）
            if (currentType.Value == actionType && Input.GetKey(currentType.Key))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// キーボードの単押しアクションボタンが押されたか調べます
    /// </summary>
    private bool WasPressedActionsKeyDown(KeyValuePair<KeyCode, ActionsType> currentType)
    {
        if(Input.GetKeyDown(currentType.Key))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// コントローラーの左スティックの入力値を調べます
    /// </summary>
    /// <returns> 左スティックの入力値をVector2で返します </returns>
    private Vector2 GetLeftStickValue(int playerNum)
    {
        // プレイヤー番号のコントローラー名を取得する
        var controllerName = ConvertPlayerNumberToControllerName(playerNum);
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.name != controllerName)
            {
                continue;
            }

            return gamepad.leftStick.ReadValue();
        }
        return Vector2.zero;
    }

    /// <summary>
    /// ヒューマノイドの運動量を調べます
    /// </summary>
    /// <returns> 引数番号のヒューマノイドの運動量をVector2で返します </returns>
    public float GetHumanoidMoveInput(int playerNum)
    {
        if (WasPressedActionsKey(ActionsType.LeftOrDown, playerNum) && m_isOpreableKeyboard)
        {
            return -KeyboardMovementValueMax;
        }

        if(WasPressedActionsKey(ActionsType.RightOrUp, playerNum) && m_isOpreableKeyboard)
        {
            return KeyboardMovementValueMax;
        }
        return GetLeftStickValue(playerNum).x;
    }

    /// <summary>
    /// 大砲の移動運動量を調べます
    /// </summary>
    /// <returns> 引数番号の大砲の運動量をVector2で返します </returns>
    public float GetCannonMoveInput(int playerNum)
    {
        if(WasPressedActionsKey(ActionsType.LeftOrDown, playerNum) && m_isOpreableKeyboard)
        {
            return -KeyboardMovementValueMax;
        }

        if(WasPressedActionsKey(ActionsType.RightOrUp, playerNum) && m_isOpreableKeyboard)
        {
            return KeyboardMovementValueMax;
        }
        return GetLeftStickValue(playerNum).y;
    }

    /// <summary>
    /// プレイヤーのアクション入力が行われたか調べます
    /// </summary>
    public bool WasPressedActionInput(ActionsType currentType, int playerNum)
    {
        if (WasPressedActionButton(currentType, playerNum))
        {
            return true;
        }

        if (!m_isOpreableKeyboard)
        {
            return false;
        }

        if (WasPressedActionsKey(currentType, playerNum))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// メニュー入力操作が行われたか調べます
    /// </summary>
    public bool WasPressedMenuInteractionInput(MenuInteractionInput currentType, int playerNum)
    {
        switch (currentType)
        {
            case MenuInteractionInput.Decision:
                return Gamepad.all[playerNum].bButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame;

            case MenuInteractionInput.SwitchLeft:
                return Gamepad.all[playerNum].leftShoulder.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame;

            case MenuInteractionInput.SwitchRight:
                return Gamepad.all[playerNum].rightShoulder.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;

            default:
                return false;
        }
    }
}
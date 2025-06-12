using UnityEngine;

public class BombDestroyer : MonoBehaviour
{
    /// <summary>
    /// シーン上に存在するすべて爆弾を削除します
    /// </summary>
    public void DestoroyBomb()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

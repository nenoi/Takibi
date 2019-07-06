using UnityEngine;
using UnityEngine.UI;

public class SoulManager : MonoBehaviour {

    // オブジェクト参照
    private GameObject gameManager; // ゲームマネージャー

    public Sprite[] soulPicture = new Sprite[2]; // 魂の絵

    public enum SOUL_KIND { // 魂の種類を定義
        BLUE,
        RED,
    }

    private SOUL_KIND soulKind; // 魂の種類

    private void Start() {
        gameManager = GameObject.Find("GameManager");
    }

    // 魂取得
    public void TouchSoul() {
        // 左クリック中のみ魂を取得できる
        if (Input.GetMouseButton(0) == false) {
            return;
        }

        switch(soulKind) {
            case SOUL_KIND.BLUE:
                gameManager.GetComponent<GameManager>().IncrementScore(1);
                break;
            case SOUL_KIND.RED:
                gameManager.GetComponent<GameManager>().IncrementScore(10);
                break;
        }

        Destroy(this.gameObject);
    }

    // 魂の種類を設定
    public void SetKind(SOUL_KIND kind) {
        soulKind = kind;

        switch(soulKind) {
            case SOUL_KIND.BLUE:
                GetComponent<Image>().sprite = soulPicture[0];
                break;
            case SOUL_KIND.RED:
                GetComponent<Image>().sprite = soulPicture[1];
                break;
        }
    }
}

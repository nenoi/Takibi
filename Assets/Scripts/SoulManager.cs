using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoulManager : MonoBehaviour {

    // オブジェクト参照
    private GameObject gameManager; // ゲームマネージャー

    public Sprite[] soulPicture = new Sprite[3]; // 魂の絵

    public enum SOUL_KIND { // 魂の種類を定義
        BLUE,
        GREEN,
        YELLOW,
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

        RectTransform rect = GetComponent<RectTransform>();

        // 魂の軌跡設定
        Vector3[] path = {
            new Vector3(rect.localPosition.x * 1.5f, 0f, 0f), // 中間点
            new Vector3(0f, -128f, 0f),
        };

        // DOTWeen を使ったアニメ作成
        rect.DOLocalPath(path, 0.5f, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(AddSoulPoint);
        // 同時にサイズも変更
        rect.DOScale(
            new Vector3(0.5f, 0.5f, 0f),
            0.5f
            );

        //switch(soulKind) {
        //    case SOUL_KIND.BLUE:
        //        gameManager.GetComponent<GameManager>().IncrementScore(1);
        //        break;
        //    case SOUL_KIND.GREEN:
        //        gameManager.GetComponent<GameManager>().IncrementScore(10);
        //        break;
        //    case SOUL_KIND.YELLOW:
        //        gameManager.GetComponent<GameManager>().IncrementScore(20);
        //        break;
        //}

        //Destroy(this.gameObject);
    }

    // 魂アニメ終了後にポイント加算処理をする
    private void AddSoulPoint() {
        switch(soulKind) {
            case SOUL_KIND.BLUE:
                gameManager.GetComponent<GameManager>().IncrementScore(1);
                break;
            case SOUL_KIND.GREEN:
                gameManager.GetComponent<GameManager>().IncrementScore(10);
                break;
            case SOUL_KIND.YELLOW:
                gameManager.GetComponent<GameManager>().IncrementScore(20);
                break;
        }

        Destroy(this.gameObject);
    }

    // 魂の種類を設定
    public void SetKind(SOUL_KIND kind) {
        soulKind = kind;

        //switch (soulKind) {
        //    case SOUL_KIND.BLUE:
        //        GetComponent<Image>().sprite = soulPicture[0];
        //        break;
        //    case SOUL_KIND.GREEN:
        //        GetComponent<Image>().sprite = soulPicture[1];
        //        break;
        //    case SOUL_KIND.YELLOW:
        //        GetComponent<Image>().sprite = soulPicture[2];
        //        break;
        //}
    }
}

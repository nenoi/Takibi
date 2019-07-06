using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
    // 定数定義
    private const int MAX_SOUL = 10;    // 魂最大数
    private const int RESPAWN_TIME = 1; // 魂が発生する秒数
    private const int MAX_LEVEL = 3;    // 最大のモノノケレベル

    // オブジェクト参照
    public GameObject soulPrefab; // 魂プレハブ
    public GameObject textClearPrefab; // クリアテキストプレハブ
    public GameObject canvasGame; // ゲームキャンバス
    public GameObject textScore;  // スコアテキスト
    public GameObject mononokeManager; // モノノケマネージャー
    public GameObject kaeru; // カエル

    public AudioClip getScoreSE; // 効果音：スコアゲット
    public AudioClip levelUpSE;  // 効果音：レベルアップ
    public AudioClip clearSE;    // 効果音：クリア

    // メンバ変数
    private int score = 0;       // 現在のスコア
    private int nextScore = 10; // レベルアップまでに必要なスコア

    private int currentSoul = 0; // 現在の魂数

    private int mononokeLevel = 0; // モノノケレベル

    private DateTime lastDateTime; // 前回、魂を生成した時間

    private int[] nextScoreTable = new int[] { 10, 100, 200, 99999 }; // レベルアップ値

    private bool clearFlag = false; // クリアフラグ

    private AudioSource audioSource; // オーディオソース

    private void Start() {
        // オーディオソース取得
        audioSource = this.gameObject.GetComponent<AudioSource>();
        currentSoul = 0;
        // 初期魂生成
        for (int i = 0; i < currentSoul; i++) {
            CreateSoul();
        }

        // 初期設定
        lastDateTime = DateTime.UtcNow;
        nextScore = nextScoreTable[mononokeLevel];

        RefreshScoreText();
    }

    private void Update() {
        // RESPAWN_TIME に設定した時間が経過するごとに、魂を生成する
        if (currentSoul < MAX_SOUL) {
            TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

            if (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME)) {
                while (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME)) {
                    CreateNewSoul();
                    timeSpan -= TimeSpan.FromSeconds(RESPAWN_TIME);
                }
            }
        }
    }

    // 新しい魂の生成
    public void CreateNewSoul() {
        lastDateTime = DateTime.UtcNow;
        if (currentSoul >= MAX_SOUL) {
            return;
        }
        CreateSoul();
        currentSoul++; // 魂を１つ増やす
    }

    // 魂生成
    public void CreateSoul() {
        GameObject soul = Instantiate(soulPrefab);
        soul.transform.SetParent(canvasGame.transform, false);
        soul.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-300.0f, 300.0f),
            UnityEngine.Random.Range(-140.0f, -500.0f),
            0f);

        // 魂の種類を設定
        int kind = UnityEngine.Random.Range(0, mononokeLevel + 1);
        switch(kind) {
            case 0:
                soul.GetComponent<SoulManager>().SetKind(SoulManager.SOUL_KIND.BLUE);
                break;
            case 1:
                soul.GetComponent<SoulManager>().SetKind(SoulManager.SOUL_KIND.GREEN);
                break;
            case 2:
                soul.GetComponent<SoulManager>().SetKind(SoulManager.SOUL_KIND.YELLOW);
                break;
            case 3:
                soul.GetComponent<SoulManager>().SetKind(SoulManager.SOUL_KIND.YELLOW);
                break;
        }
    }

    // スコアを増やす（魂入手）
    public void IncrementScore(int getScore) {
        audioSource.PlayOneShot(getScoreSE);

        // カエルが魂を取り込むアニメ再生
        AnimatorStateInfo stateInfo =
            kaeru.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash ==
            Animator.StringToHash("Base Layer.get@kaeru")) {
            // すでに再生中なら先頭から
            kaeru.GetComponent<Animator>().Play(stateInfo.fullPathHash, 0, 0.0f);
        }
        else {
            kaeru.GetComponent<Animator>().SetTrigger("isGetScore");
        }

        if (score < nextScore) {
            score += getScore;
            // レベルアップ値を超えないよう制限
            if (score > nextScore) {
                score = nextScore;
            }

            MononokeLevelUp();
            RefreshScoreText();

            // ゲームクリア判定
            if (mononokeLevel == MAX_LEVEL) {
                if (clearFlag == false) {
                    clearFlag = true;
                    ClearEffect();
                }
            }

        }

        currentSoul--; // 魂を1つ減らす
    }

    // スコアテキスト更新
    void RefreshScoreText() {
        textScore.GetComponent<Text>().text =
            "魂  " + score + " / " + nextScore;
    }

    // モノノケレベル管理
    private void MononokeLevelUp() {
        if (score >= nextScore) {
            if (mononokeLevel < MAX_LEVEL) {
                mononokeLevel++;
                score = 0;

                MononokeLevelUpEffect();

                nextScore = nextScoreTable[mononokeLevel];
                mononokeManager.GetComponent<MononokeManager>().SetMononokeVisible(mononokeLevel);
            }
        }
    }

    // レベルアップ時の演出
    private void MononokeLevelUpEffect() {
        audioSource.PlayOneShot(levelUpSE);
    }

    // モノノケが全員集まった時の演出
    private void ClearEffect() {
        //TODO:
        GameObject textClear = Instantiate(textClearPrefab);
        textClear.transform.SetParent(canvasGame.transform, false);

        Destroy(textClear, 5.0f);

        audioSource.PlayOneShot(clearSE);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MononokeManager : MonoBehaviour {
    // オブジェクト参照
    public GameObject[] mononokeObject;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetMononokeVisible(int level) {
        // レベルに応じてモノノケを表示する
        mononokeObject[level - 1].SetActive(true);
    }
}

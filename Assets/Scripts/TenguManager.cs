using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenguManager : MonoBehaviour
{

    public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CallClearEffect() {
        gameManager.GetComponent<GameManager>().ClearEffect();
    }
}

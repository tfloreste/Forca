using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitAfterTime : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //Application.Quit();
        Invoke("fechaOJogo", 5.0f); // Encerra o jogo
    }

    void fechaOJogo()
    {
        Application.Quit();
    }


}

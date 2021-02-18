using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject letra;            // prefab da letra no Game
    public GameObject centro;          // Objeto de texto que indica o centro da tela

    private string palavraOculta = "";  // palavra a ser descoberta
    // private string[] palavrasOcultas = new string[] {"carro", "elefante", "futebol" };  // Array de palavras ocultas
    private int tamanhoPalavraOculta;   // tamanho desta palavra
    private char[] letrasOcultas;               // letras da palavra oculta
    private bool[] letrasDescobertas;           // indicador de quais letras foram descobertas

    private int numTentativas;                  // Armazena as tentativas válidas da rodada
    private int maxNumTentativas;               // Número máximo de tentantativas para Forca ou Salvação
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");
        
        InitGame();
        InitLetras();

        numTentativas = 0;
        maxNumTentativas = 10;
        score = 0;

        UpdateNumTentativas();  
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i = 0; i < numLetras; i++){
            Vector3 novaPosicao;
            novaPosicao = new Vector3(
                centro.transform.position.x + ((i - numLetras/2.0f)*80), 
                centro.transform.position.y, 
                centro.transform.position.z);
            GameObject l = (GameObject) Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i+1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    void InitGame()
    {
        // int numeroAleatorio = Random.Range(0, palavrasOcultas.Length);
        // palavraOculta = palavrasOcultas[numeroAleatorio];
        palavraOculta = PegaUmaPalavraDoArquivo();

        tamanhoPalavraOculta = palavraOculta.Length; // determina-se o número de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();    // transforma-se a palavra em maíuscula
        letrasOcultas = new char[tamanhoPalavraOculta];     // Instancia-se o array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalavraOculta]; // Instancia-se o array bool do indicador de letras certas
        
        letrasOcultas = palavraOculta.ToCharArray();        // copia-se a palavra no array de letras
    }

    void checkTeclado()
    {
        if(Input.anyKeyDown) {
            char letraTeclado = Input.inputString.ToCharArray()[0];
            int letraTeladoComoInt = System.Convert.ToInt32(letraTeclado);

            if(letraTeladoComoInt >= 97 && letraTeladoComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();

                if(numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_forca");
                }

                for (int i = 0; i < tamanhoPalavraOculta; i++)
                {
                    if(!letrasDescobertas[i])
                    {
                        letraTeclado = System.Char.ToUpper(letraTeclado);
                        if(letrasOcultas[i] == letraTeclado)
                        {
                            letrasDescobertas[i]= true;
                            GameObject.Find("letra" + (i+1)).GetComponent<Text>().text = letraTeclado.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            VerificaSePalavraDescoberta();
                        }
                    }
                }
            }

        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
    }

    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;
        for (int i = 0; i < tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }

        if(condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("Lab1_salvo");
        }
    }

    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length);

        return palavras[palavraAleatoria];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float Size = 0;

    public void PlayBig(){
        SceneManager.LoadScene("Three Circles Test");
        Size = 1;
        }
    public void PlaySmall(){
        SceneManager.LoadScene("Three Circles Test");
        Size = 2;
        }
    public void Options() {
            Debug.Log("Options");
        }

    public void QuitGame(){
            Debug.Log("QUIT");
            SceneManager.LoadScene("Quit");
        }
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


}


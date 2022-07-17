using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StarMenu : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    VisualElement root;
    void Start()
    {
        UIDocument menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;
        RegisterButtonCallbacks();

    }

    public void RegisterButtonCallbacks()
    {
        //ui_start_button
        Button startButton = root.Q<Button>("ui_start_button");
        if (startButton == null)
        {
            Debug.Log("crap");
        }

        startButton.RegisterCallback<ClickEvent>((evt =>
        {
            SceneManager.LoadScene("GameScene");
            Debug.Log("here");
        }));
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
}

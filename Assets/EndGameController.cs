using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    
    
    [SerializeField] GameObject newEmpireButton;
    // Start is called before the first frame update
    void Start()
    {
        newEmpireButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            GameCenter.instance.EndTurn();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            GameObject.Find("BottomPanel").GetComponentInChildren<UITabController>().NavigateRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Find("BottomPanel").GetComponentInChildren<UITabController>().NavigateLeft();
        }
    }
}
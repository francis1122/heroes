using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;

public class ResourceFloatingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //this.
        
        StartCoroutine(DestroyStuff());
        StartCoroutine(Fade(GetComponent<TextMeshProUGUI>(), 2.2f));
        StartCoroutine(MoveFromTo(this.transform
            , transform.position
            , transform.position + new Vector3(0, 180, 0),
            80.0f));
    }

    public void InitializeText(ResourceBundle resourceBundle)
    {
        var textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = "<#00FF00>" + resourceBundle.GetResourceStringDisplay() + "</color>";
    }

    // Update is called once per frame
    IEnumerator DestroyStuff()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject.Destroy(this.transform.parent.gameObject);
    }

    IEnumerator Fade(TextMeshProUGUI textMesh, float time)
    {
        //var textComponent = GetComponent<TextMeshProUGUI>();
        //textComponent.alpha -= .01f;
        //  float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;


        float timeElapsed = 0.0f;
        float valueToLerp = 1.0f;

        float delayTime = 5f;
        while (true)
        {
            //textMesh.alpha -= 2f;
            valueToLerp = Mathf.Lerp(1, 0, timeElapsed/time);
            timeElapsed += Time.deltaTime;
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, valueToLerp);

            // t += step; // Goes from 0 to 1, incrementing by step each time
            //   objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate(); // Leave the routine and return here in the next frame
        }

        //objectToMove.position = b;
    }

    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 3.5f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate(); // Leave the routine and return here in the next frame
        }

        objectToMove.position = b;
    }
}
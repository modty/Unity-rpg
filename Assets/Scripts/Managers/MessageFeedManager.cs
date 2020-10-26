using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour {

    private static MessageFeedManager instance;

    [SerializeField]
    private GameObject messagePrefab;

    public static MessageFeedManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageFeedManager>();
            }
       
            return instance;
        }

    }

    public void WriteMessage(string message)
    {

       GameObject go = Instantiate(messagePrefab, transform);

        go.GetComponent<Text>().text = message;

        go.transform.SetAsFirstSibling();

        Destroy(go, 2);

    }

    public void WriteMessage(string message, Color color)
    {

        GameObject go = Instantiate(messagePrefab, transform);
        Text t = go.GetComponent<Text>();

        t.text = message;
        t.color = color;

        go.transform.SetAsFirstSibling();

        Destroy(go, 2);

    }
}

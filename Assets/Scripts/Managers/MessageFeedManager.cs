using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 39

public class MessageFeedManager : MonoBehaviour {

    private static MessageFeedManager instance;

    [SerializeField]
    private GameObject messagePrefab;

    public static MessageFeedManager MyInstance
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
}

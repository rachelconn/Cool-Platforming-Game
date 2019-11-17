using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public GameObject ob;

    // Start is called before the first frame update
    void Start()
    {
        Button button = ob.transform.Find("Close").GetComponent<Button>();
        button.onClick.AddListener( Change );
    }

    void Change()
    {
        SceneManager.LoadScene("Test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

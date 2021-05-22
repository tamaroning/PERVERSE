using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sounddontremove : MonoBehaviour
{
    static sounddontremove _instance = null;
    static sounddontremove instance
    {
        get { return _instance ?? (_instance = FindObjectOfType<sounddontremove>()); }
    }

    void Awake()
    {
        if (this != instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    void OnDestroy()
    {
        if (this == instance) _instance = null;

    }

    // Update is called once per frame
    void Update()
    {

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Title" && sceneName != "automaticselectlevel")
        {
            Destroy(gameObject);
        }
    }
}

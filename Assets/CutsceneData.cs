using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneData : MonoBehaviour
{
    public static CutsceneData Instance;
    public List<int> CutsceneIndexList;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        CutsceneIndexList = new();
        Instance = this;
        DontDestroyOnLoad(gameObject);    
    }
}

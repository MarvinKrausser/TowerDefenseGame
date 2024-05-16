using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreEndScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = LevelData.score.ToString();
    }
}

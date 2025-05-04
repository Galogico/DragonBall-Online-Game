using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JumpModeScript : MonoBehaviour
{
    PlayerMovement player;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.JumpMode == true){
            text.SetText("Jump Mode: Jump");
        }else{
            text.SetText("Jump Mode: Fly");
        }
    }
}

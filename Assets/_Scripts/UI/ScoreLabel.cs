using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreLabel : MonoBehaviour
{
    private TMP_Text label;
    public int Score { get; set; }
    private void Awake() {
		label = GetComponent<TMP_Text>();
    }
    private void Update() {
        label.text = Score.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using Thry.BeerPong;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreAnnouncement : UdonSharpBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private TextMeshProUGUI _textScore;
    [SerializeField]
    private TextMeshProUGUI _textSkill;
    [SerializeField]
    private ThryBP_Player _player;

    private int _lastScore = -1;

    public void ShowScore(int score)
    {
        if(score == _lastScore)
            return;
        _lastScore = score;

        string s = score.ToString("D4");
        _textScore.text = s;
        float skill = _player.LastAimAssistStrength * 100;
        _textSkill.text = skill.ToString("F0") + "%";
        _animator.SetTrigger("show");
    }

    public void ResetScore()
    {
        _lastScore = -1;
    }
}

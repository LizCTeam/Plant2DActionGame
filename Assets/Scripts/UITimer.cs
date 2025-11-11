using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] Text _TimerText;
    [SerializeField] private float _timeLimit = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeLimit -= Time.deltaTime;

        if (_timeLimit < 0f)
        {
            _timeLimit = 0f;
        }

        //Žc‚èŽžŠÔ‚ð®”‚Å•\Ž¦
        _TimerText.text = _timeLimit.ToString("F0");
    }
}

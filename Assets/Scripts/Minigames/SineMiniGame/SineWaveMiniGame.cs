using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveMiniGame : MonoBehaviour
{
    const float SINE_BUFFER_AMOUNT = 5f;

    [Range(0.1f, 1f)]
    [SerializeField] float deadZoneCheck = 0.3f;

    [SerializeField] float frequencySpeed = 5f;
    [SerializeField] float amplifierSpeed = 10f;

    private SineWaveGenerator recalibrationSineWave;
    private SineWaveGenerator playerSineWave;

    public bool Completed = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        Completed = false;
        foreach (Component child in GetComponentsInChildren<SineWaveGenerator>())
        {
            if (child.name == "RecalibrationSineWave")
            {
                recalibrationSineWave = child.GetComponent<SineWaveGenerator>();
                recalibrationSineWave.Randomise();
            }
            else
            {
                playerSineWave = child.GetComponent<SineWaveGenerator>();
                playerSineWave.Randomise();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horLeft = Input.GetAxisRaw("Horizontal");
        float horRight = Input.GetAxisRaw("Right Horizontal");

        float verLeft = Input.GetAxisRaw("Vertical");
        float verRight = Input.GetAxisRaw("Right Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            if (CompareSineWaves())
            {
                Completed = true;
                this.enabled = false;
            }
        }

        if (horLeft > deadZoneCheck
            || horLeft < -deadZoneCheck)
        {
            var value = horLeft * frequencySpeed * Time.deltaTime;
            playerSineWave.ManipulateFrequency(value);
        }
        

        if (verRight > deadZoneCheck
            || verRight < -deadZoneCheck)
        {
            var value = verRight * amplifierSpeed * Time.deltaTime;
            playerSineWave.ManipulateAmplifier(value);
        }
    }

    private bool CompareSineWaves()
    {
        bool success = false;

        success = playerSineWave.Frequency >= recalibrationSineWave.Frequency - SINE_BUFFER_AMOUNT
                  && playerSineWave.Frequency <= recalibrationSineWave.Frequency + SINE_BUFFER_AMOUNT;

        success = playerSineWave.Amplifier >= recalibrationSineWave.Amplifier - SINE_BUFFER_AMOUNT
                  && playerSineWave.Amplifier <= recalibrationSineWave.Amplifier + SINE_BUFFER_AMOUNT;
        return success;
    }
}

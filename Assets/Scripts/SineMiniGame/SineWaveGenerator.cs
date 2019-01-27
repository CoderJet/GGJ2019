using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveGenerator : MonoBehaviour
{
    const int WAVELENGTH_MIN_VALUE = 200;
    const int WAVELENGTH_MAX_VALUE = 400;

    const float FREQ_MIN_VALUE = 2.5f;
    const float FREQ_MAX_VALUE = 10f;

    const float AMP_MIN_VALUE = 100f;
    const float AMP_MAX_VALUE = 250f;

    public float Frequency
    {
        get
        {
            return frequency;
        }
    }


    public float Amplifier
    {
        get
        {
            return amplifier;
        }
    }


    [SerializeField] private Gradient WaveColour;

    [Range(WAVELENGTH_MIN_VALUE, WAVELENGTH_MAX_VALUE)]
    [SerializeField]
    private int wavelength = WAVELENGTH_MAX_VALUE;

    [Range(FREQ_MIN_VALUE, FREQ_MAX_VALUE)]
    [SerializeField] private float frequency = FREQ_MIN_VALUE;
    
    [Range(AMP_MIN_VALUE, AMP_MAX_VALUE)]
    [SerializeField] private float amplifier = AMP_MIN_VALUE;

    private float thickness = 0.2f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.colorGradient = WaveColour;
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
            lineRenderer.positionCount = wavelength;
            lineRenderer.useWorldSpace = false;
        }
        Randomise();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Randomise();
        }

        if (wavelength != lineRenderer.positionCount)
            lineRenderer.positionCount = wavelength;

        int i = 0;
        while (i < wavelength)
        {
            Vector3 pos = new Vector3(((transform.position.x + i) * wavelength) / frequency, Mathf.Sin(i + Time.time) * amplifier, 0);
            lineRenderer.SetPosition(i, pos);
            i++;
        }
    }

    private void Randomise()
    {
        //wavelength = Random.Range(WAVELENGTH_MIN_VALUE, WAVELENGTH_MAX_VALUE);
        frequency = Random.Range(FREQ_MIN_VALUE, FREQ_MAX_VALUE);
        amplifier = Random.Range(AMP_MIN_VALUE, AMP_MAX_VALUE);
    }

    public void ManipulateFrequency(float value)
    {
        frequency += value;
        frequency = Mathf.Clamp(frequency, FREQ_MIN_VALUE, FREQ_MAX_VALUE);
    }

    public void ManipulateAmplifier(float value)
    {
        amplifier += value;
        amplifier = Mathf.Clamp(amplifier, AMP_MIN_VALUE, AMP_MAX_VALUE);
    }
}

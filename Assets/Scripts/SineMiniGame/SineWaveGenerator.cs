using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveGenerator : MonoBehaviour
{
    const float FREQ_MIN_VALUE = 10f;
    const float FREQ_MAX_VALUE = 20f;

    const float AMP_MIN_VALUE = 50f;
    const float AMP_MAX_VALUE = 100f;

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

    [Range(200f, 400f)]
    [SerializeField]
    private int wavelength = 400;

    [Range(FREQ_MIN_VALUE, FREQ_MAX_VALUE)]
    [SerializeField] private float frequency = 50;
    
    [Range(AMP_MIN_VALUE, AMP_MAX_VALUE)]
    [SerializeField] private float amplifier = 2f;

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
    }
    void Update()
    {
        if (wavelength != lineRenderer.positionCount)
            lineRenderer.positionCount = wavelength;

        if (thickness != lineRenderer.endWidth)
        {
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
        }

        int i = 0;
        while (i < wavelength)
        {
            Vector3 pos = new Vector3(((transform.position.x + i) * wavelength) / frequency, Mathf.Sin(i + Time.time) * amplifier, 0);
            lineRenderer.SetPosition(i, pos);
            i++;
        }
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

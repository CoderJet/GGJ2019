using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveGenerator : MonoBehaviour
{
    const float FREQ_MIN_VALUE = 1f;
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

    public Camera main;

    [SerializeField] private bool calculateRandom = false;

    [SerializeField] private Gradient WaveColour;
    
    [SerializeField] private int pixelWidth = 0;

    [Range(FREQ_MIN_VALUE, FREQ_MAX_VALUE)]
    [SerializeField] private float frequency = FREQ_MIN_VALUE;
    
    [Range(AMP_MIN_VALUE, AMP_MAX_VALUE)]
    [SerializeField] private float amplifier = AMP_MIN_VALUE;

    [SerializeField] private float stepSize = 0.1f;

    public bool InvertDirection= false;

    private float thickness = 0.2f;

    private LineRenderer lineRenderer;

    

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (main != null)
            pixelWidth = main.scaledPixelWidth;

        if (lineRenderer != null)
        {
            lineRenderer.colorGradient = WaveColour;
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
            lineRenderer.positionCount = pixelWidth;
            lineRenderer.useWorldSpace = false;
        }

        // Wavelength is the number of peaks + troughs
        
        if (calculateRandom)
            Randomise();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Randomise();
        }

        if (pixelWidth != lineRenderer.positionCount)
            lineRenderer.positionCount = pixelWidth;

        //step_size = (float)wavelength / (1 / pixelWidth);
        stepSize = 1;

        int i = 0;
        //float position_step = 0f;
        while (i < pixelWidth)
        {
            Vector3 pos = new Vector3(i, Mathf.Sin(((float)i / ((float)pixelWidth / (6f * (float)frequency))) + (Time.time * (InvertDirection ? -1 : 1))) * amplifier);
            lineRenderer.SetPosition(i, pos);

            //float xPos = (transform.position.x + (i * step_size)) / frequency;
            //Vector3 pos = new Vector3(xPos, Mathf.Sin((i * step_size) + Time.time) * amplifier, 0);
            //lineRenderer.SetPosition(i, pos);
            //i++;
            //position_step += step_size;

            //float xPos = ((transform.position.x - position_step) * i);
            //Vector3 pos = new Vector3(xPos, Mathf.Sin(i + Time.time) * amplifier, 0);
            //lineRenderer.SetPosition(i, pos);
            i++;
            //position_step += step_size;
        }
    }

    public void Randomise()
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

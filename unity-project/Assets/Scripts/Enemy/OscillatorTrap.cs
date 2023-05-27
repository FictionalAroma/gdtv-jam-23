using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatorTrap : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period; // growing over time
        const float tau = Mathf.PI * 2; // constant value 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1
        movementFactor = (rawSinWave + 1f) / 2f; // recalculated to go from 0 to 1
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}

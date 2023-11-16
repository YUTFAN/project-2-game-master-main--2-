using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    new private Light light;
    private GameObject player;
    private float X;

    [SerializeField] private float intensity = 0;

    void Start()
    {
        if (intensity == 0) intensity = 1;
        X = transform.position.x;
        intensity = 1;
        light = GetComponent<Light>();
        light.intensity = intensity;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = (1 - Mathf.Abs(X - player.transform.position.x) / 20) * intensity;
    }

}

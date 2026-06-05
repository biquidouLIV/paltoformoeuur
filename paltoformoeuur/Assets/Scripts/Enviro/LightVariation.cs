using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightVariation : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float minIntensityValue;
    [SerializeField] private float maxIntensityValue;
    private Light2D lightScript;
        
    private void Start()
    {
        lightScript = gameObject.GetComponent<Light2D>();
        StartCoroutine(Variation());
    }

    private IEnumerator Variation()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            lightScript.intensity = Random.Range(minIntensityValue, maxIntensityValue);
        }
    }
}

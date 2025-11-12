using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class VehicleAudio : MonoBehaviour
{
    public VehicleController vehicleController;
    public AudioSource audioSource;
    public AudioClip startEngineClip;
    public AudioClip engineSoundClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vehicleController.CurrentInput.y != 0)
        {
            //加速or減速
            ChangeEnginePitch(Mathf.Abs(vehicleController.CurrentInput.y));
        }
        else
        {
            //減少
            ChangeEnginePitch(Mathf.Abs(vehicleController.CurrentInput.y));
        }
    }

    public void StartEngine(float delayTime)
    {
        StartCoroutine(AudioSwitchDelay(startEngineClip, engineSoundClip, delayTime));
    }
    public void OffEngine()
    {
        StartCoroutine(AudioOff(3f));
    }
    public void ChangeEnginePitch(float value)
    {
        audioSource.pitch = 1 + value;
        audioSource.pitch = Math.Clamp(audioSource.pitch, 1, 2);
    }

    IEnumerator AudioSwitchDelay(AudioClip clip1, AudioClip clip2, float DelayValue)
    {
        audioSource.clip = clip1;
        audioSource.Play();
        yield return new WaitForSeconds(DelayValue);

        audioSource.Stop();
        audioSource.clip = clip2;
        audioSource.Play();
    }
    IEnumerator AudioOff(float duration)
    {
        if (audioSource.clip != null)
        {
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float value = Mathf.Lerp(1, 0, timer / duration);
                audioSource.volume = value;
                yield return null;
            }

            if (audioSource.volume <= 0)
            {
                audioSource.volume = 1;
                audioSource.clip = null;
                audioSource.Stop();
            }
        }
    }
}

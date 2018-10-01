using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class RowOSounds : MonoBehaviour {


  #pragma warning disable 0649
  
  [SerializeField]
  private AudioClip[] clips;
  
  [SerializeField]
  private Button exampleButton;
  
  [SerializeField]
  private Color color1;
  
  [SerializeField]
  private Color color2;
    
  #pragma warning restore 0649
  
  private AudioSource _audioSource;
  
  
  void Start() {
    _audioSource = GetComponent<AudioSource>();
    
    for (int n = 0; n < clips.Length; ++n) {
      Button b = Instantiate<Button>(exampleButton, exampleButton.transform.parent);
      b.transform .localScale      = Vector3.one;
      b.transform .localPosition   = Vector3.zero;
      int lambdaCapture = n;
      b.onClick.AddListener(() => OnClickSoundButton(lambdaCapture));
      b.targetGraphic.color = InactiveColorForButton(n);
      b.gameObject.SetActive(true);
    }
    
    exampleButton.gameObject.SetActive(false);
  }
  
  private Color BaseColorForButton(int n) {
    float f = (float)n / (float)(clips.Length - 1);
    return Color.Lerp(color1, color2, f);
  }
  
  private Color PeakColorForButton(int n) {
    return Color.Lerp(BaseColorForButton(n), Color.white, 0.75f);
  }

  private Color InactiveColorForButton(int n) {
    return BaseColorForButton(n) * 0.8f;
  }
  
  int lastPlayingSound = -1;
  void OnClickSoundButton(int n) {
    if (lastPlayingSound == n) {
      _audioSource.Stop();
      lastPlayingSound = -1;
      transform.GetChild(n + 1).GetComponent<Button>().targetGraphic.color = InactiveColorForButton(n);
    }
    else {
      _audioSource.clip = clips[n];
      _audioSource.Play();
      if (lastPlayingSound != -1) {
        transform.GetChild(lastPlayingSound + 1).GetComponent<Button>().targetGraphic.color = InactiveColorForButton(lastPlayingSound);
      }
      lastPlayingSound = n;
    }
  }


  float _amp = 0.0f;

  private void Update() {
    const int numSamps = 2048;
    float[] r = new float[numSamps];
    float[] l = new float[numSamps];
    _audioSource.GetOutputData(r, 0);
    _audioSource.GetOutputData(l, 0);
    double amp = 0.0f;
    for (int n = 0; n < numSamps; ++n) {
      amp += Mathf.Abs(r[n]);
      amp += Mathf.Abs(l[n]);
    }
    amp /= (double)numSamps * 2.0;
    
    _amp = _amp * 0.7f + (float)amp;
    
    if (lastPlayingSound != -1) {
      Button curButton = exampleButton.transform.parent.GetChild(lastPlayingSound + 1).GetComponent<Button>();
      curButton.targetGraphic.color = Color.Lerp(BaseColorForButton(lastPlayingSound), PeakColorForButton(lastPlayingSound), _amp);
    }
   
    
  }
}

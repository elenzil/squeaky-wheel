using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class AudioButtonCtlr : MonoBehaviour {

  private Button btnTheButton;
  
  #pragma warning disable 0649
  
  [SerializeField]
  private AudioSource audioSource;
  
  [SerializeField]
  private AudioClip[] audioClips;
  
  [SerializeField]
  private Text txtClipNum;
  
  #pragma warning restore 0649
  
  private int clipNum = 0;


  void Start() {
  
    btnTheButton = GetComponent<Button>();
    
    btnTheButton.onClick.AddListener(OnTheButton);
    
    UpdateClipLabel();
  }
  
  
  void OnTheButton() {
    clipNum = (clipNum + 1) % (audioClips.Length + 1);
    if (clipNum == 0) {
      audioSource.Stop();
    }
    else {
      audioSource.clip = audioClips[clipNum - 1];
      audioSource.Play();
    }
    
    UpdateClipLabel();
  }
  
  void UpdateClipLabel() {
    txtClipNum.transform.parent.gameObject.SetActive(clipNum != 0);
    txtClipNum.text = clipNum.ToString();
  }
}

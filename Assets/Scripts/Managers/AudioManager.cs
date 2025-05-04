using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource SFXObject;
    private void Awake(){
        if(instance == null){
            instance = this;
        }
    }
    public void PlaySFXClip(AudioClip Aclip, Transform spawnTransform){
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = Aclip;
        float clipLenth = audioSource.clip.length;
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLenth);
    }
}

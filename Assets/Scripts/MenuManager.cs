using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Sprite[] difficulties;
    [SerializeField] private Sprite[] sounds;
    [SerializeField] private Image difficulyButton;
    [SerializeField] private Image soundButton;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject options;
    private AudioSource tapSound;

    private void Start()
    {
        tapSound = GetComponent<AudioSource>();
    }

    public void Play()
    {
        TapSound();
        SceneManager.LoadScene("LevelScene");
    }

    public void ChangeDifficulty()
    {
        TapSound();
        DataHolder.difficulty++;
        if (DataHolder.difficulty > 2) DataHolder.difficulty = 0;
        difficulyButton.sprite = difficulties[DataHolder.difficulty];
    }

    public void SoundsControl()
    {
        DataHolder.soundsOn = !DataHolder.soundsOn;
        TapSound();
        soundButton.sprite = DataHolder.soundsOn ? sounds[0] : sounds[1];
    }

    public void Options(bool opening)
    {
        TapSound();
        menu.SetActive(!opening);
        options.SetActive(opening);
    }

    private void TapSound()
    {
        if (DataHolder.soundsOn) tapSound.Play();
    }
}

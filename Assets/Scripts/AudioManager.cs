using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    
    private bool onTutorial;
    private bool onDungeon;
    private bool onBoss;

    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    void Start() {
        Play("StartMenuMusic");
    }

    void Update() {
        if ((SceneManager.GetActiveScene().name == "Dungeon1" || SceneManager.GetActiveScene().name == "DungeonLevel2.0") && !onDungeon) {
            Sound s = Array.Find(sounds, sound => sound.name == "DungeonBGM");
            // Stops the tutorial music 
            StopByName("TutorialBGM");
            StopByName("BossMusic");
            
            Play("DungeonBGM");
            onTutorial = false;
            onDungeon = true;
            onBoss = false;
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial1" && !onTutorial) {
            StopByName("StartMenuMusic");
            StopByName("BossMusic");
            StopByName("DungeonBGM");

            Play("TutorialBGM");
            onTutorial = true;
            onDungeon = false;
            onBoss = false;
           
        }
        else if (SceneManager.GetActiveScene().name == "THEFINALBOSS(pleasedontedititwithoutlmk)" && !onBoss)
        {
            StopByName("DungeonBGM");
            StopByName("StartMenuMusic");
            StopByName("TutorialBGM");

            Play("BossMusic");
            onTutorial = false;
            onDungeon = false;
            onBoss = true;
            
        }
    }
    
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.Play();
    }
    public void StopByName(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.Stop();
    }
}

using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    string curr_scene = "NULL";
    string[] dungeon_levels = { "Dungeon1", "Dungeon2", "Dungeon3" };
    string[] tutorial_levels = { "Tutorial1", "Tutorial2", "Tutorial3" };
    string[] boss_levels = { "THEFINALBOSS(pleasedontedititwithoutlmk)" };


    void Awake()
    {
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

    void Update()
    {
        updateBGM();
    }

    public void ChangeMasterVolume(float val)
    {
        AudioListener.volume = val;
    }

    void updateBGM()
    {
        string active_scene = SceneManager.GetActiveScene().name;
        if (curr_scene != active_scene)
        {
            if (dungeon_levels.Contains(active_scene))
            {
                StopByName("StartMenuMusic");
                StopByName("BossMusic");
                StopByName("TutorialBGM");

                Play("DungeonBGM");
            }
            else if (tutorial_levels.Contains(active_scene))
            {
                StopByName("StartMenuMusic");
                StopByName("BossMusic");
                StopByName("DungeonBGM");

                Play("TutorialBGM");

            }
            else if (boss_levels.Contains(active_scene))
            {
                StopByName("DungeonBGM");
                StopByName("StartMenuMusic");
                StopByName("TutorialBGM");

                Play("BossMusic");
            }
            else
            {
                StopByName("DungeonBGM");
                StopByName("StartMenuMusic");
                StopByName("TutorialBGM");
                StopByName("BossMusic");

                Play("StartMenuMusic");
            }
            curr_scene = active_scene;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.Play();
    }
    public void StopByName(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.Stop();
    }
    public void SetSoundVolume(string name, float vol)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.volume = vol;
    }

    public void SetPitchVolume(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.pitch = pitch;
    }

    public bool isStillPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return false;
        }
        return s.source.isPlaying;

    }

    public void setSoundLooping(string name, bool loop) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found");
            return;
        }
        s.source.loop = loop;

    }

}

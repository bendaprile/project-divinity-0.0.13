using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NonDiegeticController : MonoBehaviour
{
    [SerializeField] float HardSwitchDuration = 1f;
    [SerializeField] float SwitchDuration = 3f;

    [SerializeField] private AudioSource[] audioSources;

    private float audioLevel;

    [SerializeField] private List<AudioClip> ApocMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> ApocCombatMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> WinterMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> WinterCombatMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> StormMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> StormCombatMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> ForestMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> ForesCombatMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> CityMusic = new List<AudioClip>();
    [SerializeField] private List<AudioClip> CityCombatMusic = new List<AudioClip>();

    private List<AudioClip> removedPlaylist = null;
    private List<AudioClip> currentPlaylist = null;
    private List<AudioClip> nextPlaylist = null;
    private int playlistIter;

    private CombatChecker CC;

    private bool using_CustomPlaylist;
    private bool in_menu;
    private float kill_until;
    private Zones current_zone = Zones.Apoc;

    private bool current_mainAudioSource;
    private bool audio_killed;

    void Start()
    {
        Assert.IsTrue(audioSources[0].volume == audioSources[1].volume);
        audioLevel = audioSources[0].volume;
        CC = GameObject.Find("Player").GetComponentInChildren<CombatChecker>();
    }


    public void ChangeAudioSpecific(List<AudioClip> data, bool SourceIsMenu = false)
    {
        if (SourceIsMenu)
        {
            in_menu = true;
            if (using_CustomPlaylist)
            {
                removedPlaylist = currentPlaylist;
            }
        }
        else
        {
            using_CustomPlaylist = true;
        }

        nextPlaylist = data;
        audioSources[b2i(!current_mainAudioSource)].clip = nextPlaylist[0]; //Dont play until no audio
        audioSources[b2i(!current_mainAudioSource)].time = 0;
        kill_until = Time.unscaledTime + HardSwitchDuration;
    }

    public void ChangeAudioGeneral() //TODO MAKE BASED ON LOCATION
    {
        if (in_menu)
        {
            in_menu = false;
            if (using_CustomPlaylist)
            {
                ChangeAudioSpecific(removedPlaylist);
            }
        }
        else
        {
            using_CustomPlaylist = false;
        }

        audioSources[b2i(!current_mainAudioSource)].time = 0;
        kill_until = Time.unscaledTime + HardSwitchDuration;
    }

    private int b2i(bool data)
    {
        if (data)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    private void MatchingAudioHelper(List<AudioClip> Swap)
    {
        if(Swap == currentPlaylist)
        {
            return;
        }

        nextPlaylist = Swap;
        audioSources[b2i(!current_mainAudioSource)].clip = nextPlaylist[playlistIter]; //All the songs will match
        audioSources[b2i(!current_mainAudioSource)].time = audioSources[b2i(current_mainAudioSource)].time;
        audioSources[b2i(!current_mainAudioSource)].volume = 0;
        audioSources[b2i(!current_mainAudioSource)].Play();
    }

    private void AllAudioHelper_final()
    {
        currentPlaylist = nextPlaylist;
        nextPlaylist = null;
        current_mainAudioSource = !current_mainAudioSource;
    }


    private void Update()
    {
        if(Time.unscaledTime < kill_until)
        {
            if(audioSources[0].volume > 0)
            {
                audioSources[0].volume -= audioLevel * Time.unscaledDeltaTime / HardSwitchDuration;
            }

            if(audioSources[1].volume > 0)
            {
                audioSources[1].volume -= audioLevel * Time.unscaledDeltaTime / HardSwitchDuration;
            }

            audio_killed = true;
            return;
        }


        if (!in_menu && !using_CustomPlaylist && nextPlaylist == null) //Swaps music based on the zone
        {
            if (CC.enemies_nearby)
            {
                if (current_zone == Zones.Apoc)
                {
                    MatchingAudioHelper(ApocCombatMusic);
                }
            }
            else
            {
                if (current_zone == Zones.Apoc)
                {
                    MatchingAudioHelper(ApocMusic);
                }
            }
        }

        if(audio_killed) //Happens once
        {
            audio_killed = false;
            audioSources[b2i(!current_mainAudioSource)].Play();
        }

        if (nextPlaylist != null) //Switches between master and slave 
        {
            audioSources[b2i(current_mainAudioSource)].volume -= audioLevel * Time.unscaledDeltaTime / SwitchDuration;
            audioSources[b2i(!current_mainAudioSource)].volume += audioLevel * Time.unscaledDeltaTime / SwitchDuration;
            if (audioSources[b2i(!current_mainAudioSource)].volume >= 1)
            {
                AllAudioHelper_final();
            }
        }


        if (!audioSources[b2i(current_mainAudioSource)].isPlaying && currentPlaylist != null) //Next track
        {
            playlistIter = (playlistIter + 1) % currentPlaylist.Count;
            audioSources[b2i(current_mainAudioSource)].clip = currentPlaylist[playlistIter];
            audioSources[b2i(current_mainAudioSource)].time = 0;
            audioSources[b2i(current_mainAudioSource)].Play();
            if(nextPlaylist != null)
            {
                audioSources[b2i(!current_mainAudioSource)].clip = nextPlaylist[playlistIter];
                audioSources[b2i(!current_mainAudioSource)].time = 0;
                audioSources[b2i(!current_mainAudioSource)].Play();
            }
        }
    }

}

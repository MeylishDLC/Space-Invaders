using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Sound
{
    public class SoundManager: MonoBehaviour
    {
        [BankRef]
        public List<string> Banks;
        
        private Dictionary<string, EventInstance> eventInstances;
        private EventInstance musicEventInstance;

        private void Awake()
        {
            LoadBanks();
            eventInstances = new Dictionary<string, EventInstance>();
        }

        public void InitializeMusic(string musicName, EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicName,musicEventReference);
            musicEventInstance.start();
        }

        public bool HasMusic(string musicName)
        {
            return eventInstances.ContainsKey(musicName);
        }
        
        public void StopMusic(string musicName, STOP_MODE stopMode)
        {
            if (!eventInstances.ContainsKey(musicName))
            {
                Debug.LogError($"No music with name {musicName} was found");
            }

            var instance = eventInstances[musicName];
            instance.stop(stopMode);
            instance.release();
            eventInstances.Remove(musicName);
        }

        public void PauseMusic(string musicName, bool paused)
        {
            eventInstances[musicName].setPaused(paused);
        }
        private EventInstance CreateInstance(string instanceName, EventReference eventReference)
        {
            var eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(instanceName, eventInstance);
            return eventInstance;
        }

        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }
        private void CleanUp()
        {
            if (eventInstances is null)
            {
                return;
            }
            foreach (EventInstance eventInstance in eventInstances.Values)
            {
                eventInstance.stop(STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        private void LoadBanks()
        {
            foreach (var b in Banks)
            {
                RuntimeManager.LoadBank(b, true);
                Debug.Log("Loaded bank " + b);
            }

            RuntimeManager.CoreSystem.mixerSuspend();
            RuntimeManager.CoreSystem.mixerResume();
        }
    }
}
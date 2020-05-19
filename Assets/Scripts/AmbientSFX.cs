using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class AmbientSFX : MonoBehaviour
    {
        public AudioSource daySfx;
        public AudioSource nightSfx;

        public bool dayTime = true;
        public bool nightTime = false;

        public float time = 0.0f;
        public float dayDuration = 10.0f;
        public float nightDuration = 10.0f;

        //public AudioClip dayAudio;
        //public AudioClip nightAudio;

        // Start is called before the first frame update
        void Start()
        {
            daySfx = GetComponent<AudioSource>();
            nightSfx = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            time = time + Time.deltaTime;
            if (nightTime == false)
            {
                DaySfx();
            }
            if (dayTime == false)
            {
                NightSfx();
            }
        }

        void DaySfx()
        {
            daySfx.Play();
            if (time > dayDuration)
            {
                dayTime = false;
                nightTime = true;
            }
        }

        void NightSfx()
        {
            nightSfx.Play();
            if (time > dayDuration + nightDuration)
            {
                nightTime = false;
                dayTime = true;
                time = 0;
            }
        }
    }


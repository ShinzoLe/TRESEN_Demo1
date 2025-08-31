using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    public bool loop;

    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Danh sách âm thanh")]
    public List<Sound> sounds = new List<Sound>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Khởi tạo AudioSource cho từng sound
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Phát âm thanh theo tên
    public void Play(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"[AudioManager] Không tìm thấy âm thanh: {name}");
            return;
        }
        s.source.Play();
    }

    // Phát một lần (không bị ngắt nếu đang phát)
    public void PlayOneShot(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        if (s == null || s.clip == null) return;
        s.source.PlayOneShot(s.clip);
    }

    // Dừng âm thanh
    public void Stop(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        if (s != null && s.source.isPlaying)
            s.source.Stop();
    }

    // Kiểm tra đang phát không
    public bool IsPlaying(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        return (s != null && s.source.isPlaying);
    }
}

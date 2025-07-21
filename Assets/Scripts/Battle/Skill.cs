using Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SkillFrame
{
    public uint ms;
    public uint duration;
    public Vector3 Offset;
    public Vector3 Rotation;
    public Vector3 Scale;
}

[Serializable]
public class Skill
{
    [SerializeField] List<SkillFrame> frames = new();
    public List<SkillFrame> Frames => frames.ToList();
    [NonSerialized] public Transform Transform;
    [NonSerialized] public AttackBox AttackBox;
}

public class SkillController : IController
{
    Skill skill;
    float firedTime;
    int pos;
    uint ms;

    List<SkillFrame> frames => skill?.Frames ?? new();
    public bool Alive => IsInitialized && pos != -2;
    public bool IsInitialized => skill.Transform != null && skill.AttackBox != null && skill.Frames.Count > 0;
    public void Initialize(Skill skill, Transform transform, AttackBox attackBox)
    {
        this.skill = skill;
        this.skill.Transform = transform;
        this.skill.AttackBox = attackBox;
    }
    public void Fire()
    {
        firedTime = Time.time;
        pos = -1;
        ms = uint.MaxValue;
    }
    public void Update()
    {
        if (!IsInitialized) return;
        if (!UpdateMS()) return;

        if (pos == frames.Count - 1 && frames.Last().ms + frames.Last().duration <= ms)
        {
            OnDeadSkill();
            return;
        }

        if (UpdatePos())
        {
            OnUpdateSkillFrame();
        }
    }
    bool UpdateMS()
    {
        uint ms = (uint)((Time.time - firedTime) * 1000);
        bool updated = this.ms != ms;
        this.ms = ms;
        return updated;
    }
    bool UpdatePos()
    {
        int prePos = pos;
        for (int i = Mathf.Max(0, pos); i < frames.Count; i++)
        {
            if (ms < frames[i].ms) break; else pos = i;
        }
        bool updated = prePos != pos;
        return updated;
    }
    void OnDeadSkill()
    {
        pos = -2;
    }
    void OnUpdateSkillFrame()
    {
        var frame = frames[pos];
        skill.Transform.localPosition = frame.Offset;
        skill.Transform.localEulerAngles = frame.Rotation;
        skill.Transform.localScale = frame.Scale;
        skill.AttackBox.SetAttackWindow(frame.duration / 1000f);
        skill.AttackBox.OpenAttackWindow();
    }
}
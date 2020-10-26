using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Debuffs
{
    public abstract class Debuff
    {

        public float Duration { get; set; }

        public float ProcChance { get; set; }

        public float Elapsed { get; set; }

        public Character Character { get; protected set; }

        public Image Icon { get; set; }

        public abstract string Name
        {
            get;
        }

        public Debuff(Image image)
        {
            this.Icon = image;
        }

        public virtual void Apply(Character character)
        {
            this.Character = character;
            character.ApplyDebuff(this);
            UIManager.Instance.AddDebuffToTargetFrame(this);
        }

        public virtual void Remove()
        {
            Character.RemoveDebuff(this);
            Elapsed = 0;
        }

        public virtual void Update()
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= Duration)
            {
                Remove();
            }
        }

        public abstract Debuff Clone();
    }
}

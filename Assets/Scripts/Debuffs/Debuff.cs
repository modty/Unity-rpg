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

        public float MyDuration { get; set; }

        public float ProcChance { get; set; }

        public float Elapsed { get; set; }

        public Character MyCharacter { get; protected set; }

        public Image MyIcon { get; set; }

        public abstract string Name
        {
            get;
        }

        public Debuff(Image image)
        {
            this.MyIcon = image;
        }

        public virtual void Apply(Character character)
        {
            this.MyCharacter = character;
            character.ApplyDebuff(this);
            UIManager.MyInstance.AddDebuffToTargetFrame(this);
        }

        public virtual void Remove()
        {
            MyCharacter.RemoveDebuff(this);
            Elapsed = 0;
        }

        public virtual void Update()
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= MyDuration)
            {
                Remove();
            }
        }

        public abstract Debuff Clone();
    }
}

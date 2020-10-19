using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Debuffs
{
    class IgniteDebuff : Debuff
    {
        public float MyTickDamage { get; set; }

        public override string Name
        {
            get { return "Ignite"; }
        }

        private float elapsed;

        public  IgniteDebuff(Image icon) : base(icon)
        {
            MyDuration = 20;
        }

        public override void Update()
        {
            elapsed += Time.deltaTime;

            if (elapsed >= MyDuration/MyTickDamage)
            {
                MyCharacter.TakeDamage(MyTickDamage, null);
                elapsed = 0;
            }

           base.Update();
        }

        public override void Remove()
        {
            elapsed = 0;
            base.Remove();
        }

        public override Debuff Clone()
        {
            IgniteDebuff clone = (IgniteDebuff)this.MemberwiseClone();
            return clone;
        }
    }
}

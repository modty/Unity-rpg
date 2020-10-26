using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts.Debuffs
{
    class PermafrostDebuff : Debuff
    {
        public float SpeedReduction { get; set; }

        public override string Name => "Permafrost";

        public PermafrostDebuff(Image icon) : base(icon)
        {
            Duration = 3;
        }

        public override void Apply(Character character)
        {

            if (character.CurrentSpeed >= character.Speed)
            {
                character.CurrentSpeed = character.Speed - (character.Speed * (SpeedReduction / 100));
                base.Apply(character);
            }
      
        }

        public override void Remove()
        {
            Character.CurrentSpeed = Character.Speed;
            base.Remove();
        }

        public override Debuff Clone()
        {
            PermafrostDebuff clone = (PermafrostDebuff)this.MemberwiseClone();

            return clone;
        }
    }
}

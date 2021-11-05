using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGame.Model
{
    public abstract class Spell
    {
        public int RequiredMana { get; }

        public Spell(int requiredMana) => this.RequiredMana = requiredMana;

        public abstract void Cast(Wizard target);
    }

    public class Paralyze : Spell
    {
        public Paralyze() 
            : base(20)
        {
        }

        public override void Cast(Wizard target) => target.AddEffect(new ParalizedTimedEffect(2));
    }

    public class Untouchable : Spell
    {
        public Untouchable() 
            : base(30)
        {
        }

        public override void Cast(Wizard target) => target.AddEffect(new UntouchableTimedEffect(3));
    }

    public class Uncastable : Spell
    {
        public Uncastable() 
            : base(40)
        {
        }

        public override void Cast(Wizard target) => target.AddEffect(new UncastableTimedEffect(4));
    }
}

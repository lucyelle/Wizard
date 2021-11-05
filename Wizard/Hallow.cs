using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGame.Model
{
    public abstract class Hallow
    {
        public abstract void Apply(Wizard wizard);
    }

    public class Cloak : Hallow
    {
        public override void Apply(Wizard wizard) => wizard.IsUntouchable = true;
    }

    public class Ring : Hallow
    {
        public override void Apply(Wizard wizard) => wizard.RegenerationSpeed *= 2;
    }

    public class Dagger : Hallow
    {
        public override void Apply(Wizard wizard) => wizard.AttackDamage *= 2;
    }
}

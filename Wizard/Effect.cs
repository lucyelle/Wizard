using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGame.Model
{
    public abstract class TimedEffect
    {
        private int remainingTime;

        public TimedEffect(int remainingTime)
        {
            this.remainingTime = remainingTime;
        }

        protected abstract void Apply(Wizard wizard);

        public bool TryApply(Wizard wizard)
        {
            if (this.remainingTime-- == 0) return false;

            Apply(wizard);
            return true;
        }
    }

    public class ParalizedTimedEffect : TimedEffect
    {
        public ParalizedTimedEffect(int remainingTime) : base(remainingTime)
        {
        }

        protected override void Apply(Wizard wizard)
        {
            wizard.IsParalized = true;
        }
    }

    public class UntouchableTimedEffect : TimedEffect
    {
        public UntouchableTimedEffect(int remainingTime) : base(remainingTime)
        {
        }

        protected override void Apply(Wizard wizard)
        {
            wizard.IsUntouchable = true;
        }
    }

    public class UncastableTimedEffect : TimedEffect
    {
        public UncastableTimedEffect(int remainingTime) : base(remainingTime)
        {
        }

        protected override void Apply(Wizard wizard)
        {
            wizard.IsUncastable = true;
        }
    }
}

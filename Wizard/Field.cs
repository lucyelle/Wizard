using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGame.Model
{
    public class Field
    {
        public IReadOnlyList<Wizard> WizardsHere => this.wizardsHere;
        public IList<Field> Neighbours => this.neighbors;

        private List<Wizard> wizardsHere = new();
        private List<Field> neighbors = new();

        public void RemoveWizard(Wizard w) => this.wizardsHere.Remove(w);

        public virtual void AcceptWizard(Wizard wizard)
        {
            this.wizardsHere.Add(wizard);
        }
    }

    public class Cave : Field
    {
        private Spell spell;

        public Cave(Spell spell) => this.spell = spell;

        public override void AcceptWizard(Wizard wizard)
        {
            base.AcceptWizard(wizard);
            wizard.LearnSpell(this.spell);
        }
    }

    public class Forest : Field
    {
        private Hallow? hallow;

        public Forest(Hallow? hallow) => this.hallow = hallow;

        public override void AcceptWizard(Wizard wizard)
        {
            base.AcceptWizard(wizard);
            if (this.hallow is not null)
            {
                wizard.ReceiveHallow(this.hallow);
                this.hallow = null;
            }
        }
    }
}

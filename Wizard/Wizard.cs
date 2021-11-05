using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGame.Model
{
    public class Wizard
    {
        private HashSet<Spell> learnedSpells = new();
        private List<TimedEffect> appliedEffects = new();
        private List<Hallow> ownedHallows = new();

        public int Mana { get; private set; } = 100;
        public int MaxMana { get; } = 100;
        public Field CurrentField { get; private set; }
        public IReadOnlySet<Spell> LearnedSpells => this.learnedSpells;
        public IReadOnlyList<Hallow> OwnedHallows => this.ownedHallows;
        public int RegenerationSpeed { get; set; } = 1;
        public int AttackDamage { get; set; } = 1;
        public bool IsParalized { get; set; }
        public bool IsUncastable { get; set; }
        public bool IsUntouchable { get; set; }

        public Wizard(Field currentField)
        {
            this.CurrentField = currentField;
            this.CurrentField.AcceptWizard(this);
        }

        public void Move(Field field)
        {
            if (this.IsParalized) return;
            if (this.CurrentField.Neighbours.Contains(field))
            {
                this.CurrentField.RemoveWizard(this);
                field.AcceptWizard(this);
                this.CurrentField = field;
            }
        }

        public void Cast(Spell spell, Wizard target)
        {
            if (this.IsParalized) return;
            if (spell.RequiredMana > this.Mana) return;
            if (!this.LearnedSpells.Contains(spell)) return;
            if (target.CurrentField != this.CurrentField) return;
            if (target.IsUncastable) return;

            spell.Cast(target);
        }

        public void TakeDamage(int amount)
        {
            if (this.IsUntouchable) return;
            this.Mana = Math.Max(this.Mana - amount, 0);
            if (this.Mana == 0) this.appliedEffects.Add(new ParalizedTimedEffect(20));
        }

        public void Attack(Wizard target)
        {
            if (this.CurrentField != target.CurrentField) return;
            target.TakeDamage(this.AttackDamage);
        }

        public void LearnSpell(Spell spell) => this.learnedSpells.Add(spell);

        public void ReceiveHallow(Hallow hallow) => this.ownedHallows.Add(hallow);

        public Hallow? LoseHallow()
        {
            if (!this.IsParalized) return null;
            if (this.ownedHallows.Count == 0) return null;
            var h = this.ownedHallows[^1];
            this.ownedHallows.RemoveAt(this.ownedHallows.Count - 1);
            return h;
        }

        public void StealHallow(Wizard target)
        {
            if (target.CurrentField != this.CurrentField) return;
            var h = target.LoseHallow();
            if (h is not null) this.ReceiveHallow(h);
        }

        public void AddEffect(TimedEffect effect) => this.appliedEffects.Add(effect);

        public void Update()
        {
            this.ResetAppliedEffects();
            this.UpdateEffects();
            this.RegenerateMana();
        }

        private void ResetAppliedEffects()
        {
            this.IsParalized = false;
            this.IsUntouchable = false;
            this.IsUncastable = false;
            this.RegenerationSpeed = 1;
            this.AttackDamage = 1;
        }

        private void UpdateEffects()
        {
            for (var i = 0; i < this.appliedEffects.Count;)
            {
                if (this.appliedEffects[i].TryApply(this)) ++i;
                else this.appliedEffects.RemoveAt(i);
            }

            for (var i = 0; i < this.ownedHallows.Count; ++i)
            {
                this.ownedHallows[i].Apply(this);
            }
        }

        private void RegenerateMana()
        {
            if (this.IsParalized) return;
            this.Mana = Math.Min(this.Mana + this.RegenerationSpeed, this.MaxMana);
        }
    }
}

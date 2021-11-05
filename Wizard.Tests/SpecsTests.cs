using System;
using System.Collections.Generic;
using System.Linq;
using WizardGame.Model;
using Xunit;

namespace WizardGame.Tests
{
    public class SpecsTests
    {
        class TestSpell : Spell
        {
            public List<Wizard> CastedOn { get; } = new();

            public TestSpell(int req)
                : base(req)
            {
            }

            public override void Cast(Wizard target) => this.CastedOn.Add(target);
        }

        [Fact]
        public void BarlangFaláhozÉrveVarázslatotOlvasunk()
        {
            /*
            Wizard     Untouchable
              v          v
             F1 -- F2 -- C
             */

            var untouchable = new Untouchable();
            var f1 = new Field();
            var f2 = new Field();
            var c = new Cave(untouchable);

            f1.Neighbours.Add(f2);
            f2.Neighbours.Add(f1);
            f2.Neighbours.Add(c);
            c.Neighbours.Add(f2);

            var wizard = new Wizard(f1);

            Assert.Equal(wizard.CurrentField, f1);
            Assert.Contains(wizard, f1.WizardsHere);
            Assert.Empty(wizard.LearnedSpells);

            // Invalid move
            wizard.Move(c);

            Assert.Equal(wizard.CurrentField, f1);
            Assert.Contains(wizard, f1.WizardsHere);
            Assert.Empty(wizard.LearnedSpells);

            // Valid move
            wizard.Move(f2);

            Assert.Equal(wizard.CurrentField, f2);
            Assert.DoesNotContain(wizard, f1.WizardsHere);
            Assert.Contains(wizard, f2.WizardsHere);
            Assert.Empty(wizard.LearnedSpells);

            // Valid move
            wizard.Move(c);

            Assert.Equal(wizard.CurrentField, c);
            Assert.DoesNotContain(wizard, f1.WizardsHere);
            Assert.DoesNotContain(wizard, f2.WizardsHere);
            Assert.Contains(wizard, c.WizardsHere);
            Assert.Single(wizard.LearnedSpells);
            Assert.IsType<Untouchable>(wizard.LearnedSpells.First());
        }

        [Fact]
        public void NemTanultaMegAVarázslatot()
        {
            var spell = new TestSpell(10);
            var f = new Field();
            var w = new Wizard(f);
            
            w.Cast(spell, w);

            Assert.Empty(spell.CastedOn);
        }

        [Fact]
        public void MegtanultaAVarázslatot()
        {
            var spell = new TestSpell(10);
            var f = new Cave(spell);
            var w = new Wizard(f);

            w.Cast(spell, w);

            Assert.Single(spell.CastedOn);
            Assert.Equal(w, spell.CastedOn[0]);
        }

        [Fact]
        public void NemUgyanottÁllAKétWizard()
        {
            var spell = new TestSpell(10);
            var f1 = new Cave(spell);
            var f2 = new Field();
            var w1 = new Wizard(f1);
            var w2 = new Wizard(f2);

            w1.Cast(spell, w2);

            Assert.Empty(spell.CastedOn);
        }

        [Fact]
        public void UgyanottÁllAKétWizard()
        {
            var spell = new TestSpell(10);
            var f = new Cave(spell);
            var w1 = new Wizard(f);
            var w2 = new Wizard(f);

            w1.Cast(spell, w2);

            Assert.Single(spell.CastedOn);
            Assert.Equal(w2, spell.CastedOn[0]);
        }

        [Fact]
        public void NincswElégMana()
        {
            var spell = new TestSpell(1000);
            var f = new Cave(spell);
            var w = new Wizard(f);

            w.Cast(spell, w);

            Assert.Empty(spell.CastedOn);
        }

        [Fact]
        public void WizardTámadniPróbálDeNemUgyanottVannak()
        {
            var f1 = new Field();
            var f2 = new Field();

            var w1 = new Wizard(f1);
            var w2 = new Wizard(f2);

            w1.Attack(w2);

            Assert.Equal(100, w1.Mana);
            Assert.Equal(100, w2.Mana);
        }

        [Fact]
        public void WizardTámad()
        {
            var f = new Field();

            var w1 = new Wizard(f);
            var w2 = new Wizard(f);

            w1.Attack(w2);

            Assert.Equal(100, w1.Mana);
            Assert.Equal(99, w2.Mana);
        }

        [Fact]
        public void WizardTámadÉsBénít()
        {
            var f = new Field();

            var w1 = new Wizard(f);
            var w2 = new Wizard(f);

            w2.TakeDamage(99);
            w1.Attack(w2);

            Assert.Equal(100, w1.Mana);
            Assert.Equal(0, w2.Mana);

            w2.Update();
            Assert.True(w2.IsParalized);
        }

        [Fact]
        public void WizardFelveszEgyEreklyét()
        {
            /*
            Wizard     Ring
              v          v
             F1 -- F2 -- F
             */

            var ring = new Ring();
            var f1 = new Field();
            var f2 = new Field();
            var c = new Forest(ring);

            f1.Neighbours.Add(f2);
            f2.Neighbours.Add(f1);
            f2.Neighbours.Add(c);
            c.Neighbours.Add(f2);

            var wizard = new Wizard(f1);

            Assert.Empty(wizard.OwnedHallows);

            wizard.Move(f2);

            Assert.Empty(wizard.OwnedHallows);

            wizard.Move(c);

            Assert.Single(wizard.OwnedHallows);
            Assert.IsType<Ring>(wizard.OwnedHallows.First());
        }

        [Fact]
        public void WizardFelveszBemegyAzErdobeDeNincsOttHallow()
        {
            /*
            Wizard     Ring
              v          v
             F1 -- F2 -- F
             */
            
            var f1 = new Field();
            var f2 = new Field();
            var c = new Forest(null);

            f1.Neighbours.Add(f2);
            f2.Neighbours.Add(f1);
            f2.Neighbours.Add(c);
            c.Neighbours.Add(f2);

            var wizard = new Wizard(f1);

            Assert.Empty(wizard.OwnedHallows);

            wizard.Move(f2);

            Assert.Empty(wizard.OwnedHallows);

            wizard.Move(c);

            Assert.Empty(wizard.OwnedHallows);
        }
    }
}

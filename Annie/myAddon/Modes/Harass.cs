using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
// Using the config like this makes your life easier, trust me
using Settings = myAddon.Config.Modes.Harass;

namespace myAddon.Modes
{
    public sealed class Harass : ModeBase
    {
    	private	float Stacks = Player.GetBuff("pyromania").Count;
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on harass mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = false;
            if (Settings.UseQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                var Mob = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range).Where(minion => minion.Health <= Player.Instance.GetSpellDamage(minion,SpellSlot.Q));
                if (target != null && Stacks >= 2 && Settings.UseQ && Player.Instance.ManaPercent > Settings.Mana)
                {
                	Orbwalker.DisableAttacking = true;
                    Q.Cast(target);
                }
                else if  (Mob != null && Settings.Farming)
                {
                	Orbwalker.DisableAttacking = true;
                	Q.Cast(Mob.First());
                }
            }
            if (Settings.UseW && Settings.UseW && W.IsReady())
            {
            	var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            	var predW  = W.GetPrediction(target).CastPosition;
            	if (target != null && !Q.IsReady())
            	{
            		W.Cast(predW);
            	}
            }
            
        }
    }
}

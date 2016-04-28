using System;
using System.Linq;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass
namespace myAddon
{
    // I can't really help you with my layout of a good config class
    // since everyone does it the way they like it most, go checkout my
    // config classes I make on my GitHub if you wanna take over the
    // complex way that I use
    public static class Config
    {
        private const string MenuName = "Annie";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Lucky4's Annie");
            Menu.AddLabel("To change the menu, please have a look at the");
            Menu.AddLabel("Config.cs class inside the project, now have fun!");

            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                // Initialize the menu
                Menu = Config.Menu.AddSubMenu("Modes,modes");

                // Initialize all modes
                // Combo
                Combo.Initialize();
                Menu.AddSeparator();

                // Harass
                Harass.Initialize();
                LaneClear.Initialize();
                LastHit.Initialize();
                Misc.Initialize();
                Draws.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }

                static Combo()
                {
                    // Initialize the menu values
                    Menu.AddGroupLabel("Combo");
                    _useQ = Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Use W"));
                    _useR = Menu.Add("comboUseR", new CheckBox("Use R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                public static bool UseQ
                {
                    get { return Menu["harassUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseW
                {
                    get { return Menu["harassUseW"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool Farming
                {
                    get { return Menu["farming"].Cast<CheckBox>().CurrentValue; }
                }
                public static int Mana
                {
                    get { return Menu["harassMana"].Cast<Slider>().CurrentValue; }
                }

                static Harass()
                {
                    // Here is another option on how to use the menu, but I prefer the
                    // way that I used in the combo class
                    Menu.AddGroupLabel("Harass");
                    Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu.Add("harassUseW", new CheckBox("Use W"));
                    Menu.Add("farming", new CheckBox("Farm(op)"));
                    Menu.Add("harassMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {
                }
            }
           public static class LaneClear
            {
                public static bool UseQ
                {
                    get { return Menu["clearUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseW
                {
                    get { return Menu["clearUseW"].Cast<CheckBox>().CurrentValue; }
                }

                static LaneClear()
                {
                    Menu.AddGroupLabel("LaneClear");
                    Menu.Add("clearUseQ", new CheckBox("Use Q"));
                    Menu.Add("clearUseW", new CheckBox("Use W"));
                }

                public static void Initialize()
                {
                }
           }
          	public static class LastHit
            {
                public static bool UseQ
                {
                    get { return Menu["lastUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                static LastHit()
                {
                    Menu.AddGroupLabel("LastHit");
                    Menu.Add("lastUseQ", new CheckBox("Use Q"));
                }

                public static void Initialize()
                {
                }
           }
          	
          public static class Misc
            {
                public static bool UseE
                {
                    get { return Menu["autoE"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool AutoStacks
                {
                	get { return Menu["autoStacks"].Cast<CheckBox>().CurrentValue;}
                }
                static Misc()
                {
                    Menu.AddGroupLabel("Misc");
                    Menu.Add("autoE", new CheckBox("Auto E"));
                    Menu.Add("autoStacks", new CheckBox("Auto Stacks"));
                    Menu.Add("gapClose", new CheckBox("Anti-Gapclose"));
                }

                public static void Initialize()
                {
                }
           }
         public static class Draws
			{
         		private const int BarWidth = 106;
        		private const int LineThickness = 10;
        		private static Vector2 BarOffset = new Vector2(0, 16);
				private static Color _drawingColor;
        		public static Color DrawingColor
       			{
        			get { return _drawingColor; }
            		set { _drawingColor = Color.FromArgb(170, value); }
        		}
        		private static Color _drawQ;
        		public static Color  DrawQ
       			{
        			get { return _drawQ; }
            		set { _drawQ = Color.FromArgb(170, value); }
        		}
        		private static Color _drawW;
        		public static Color  DrawW
       			{
        			get { return _drawW; }
            		set { _drawW = Color.FromArgb(170, value); }
        		}
         		private static Color _drawR;
        		public static Color  DrawR
       			{
        			get { return _drawR; }
            		set { _drawR = Color.FromArgb(170, value); }
        		}
         		public static bool DrawDmg
                {
                    get { return Menu["drawDmg"].Cast<CheckBox>().CurrentValue; }
                }
				static Draws()
				{
					Menu.AddGroupLabel("Draw");
					Menu.Add("drawDmg", new CheckBox("Draw Dmg"));
				}
				public static void Initialize()
				{	
					DrawQ = Color.RoyalBlue;
					DrawW = Color.Teal;
					DrawR = Color.IndianRed;
					DrawingColor = Color.DarkBlue;
					Drawing.OnEndScene += OnEndScene;
				}
				private static void OnEndScene(EventArgs args)
				{
					foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsHPBarRendered && u.IsValid))
                	{
                    	var damageQ = Player.Instance.GetSpellDamage(unit, SpellSlot.Q);
						var damageW = Player.Instance.GetSpellDamage(unit, SpellSlot.W);
						var damageR = Player.Instance.GetSpellDamage(unit, SpellSlot.R);
                    	if (damageQ +  damageW + damageR <= 0)
                    	{
                        continue;
                    	}
                    	var damagePercentageQ = ((unit.TotalShieldHealth() - damageQ) > 0 ? (unit.TotalShieldHealth() - damageQ) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    	var damagePercentageW = ((unit.TotalShieldHealth() - damageW) > 0 ? (unit.TotalShieldHealth() - damageW) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    	var damagePercentageR = ((unit.TotalShieldHealth() - damageR) > 0 ? (unit.TotalShieldHealth() - damageR) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var currentHealthPercentage = unit.TotalShieldHealth() / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    	var startPointQ = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentageQ * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    	var startPointW = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentageW * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    	var startPointR = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentageR * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        var endPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1, (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        if (Config.Modes.Draws.DrawDmg)
                        {
                        Drawing.DrawLine(startPointQ, endPoint, LineThickness, DrawQ);
                        Drawing.DrawLine(startPointW, endPoint, LineThickness, DrawW);
                        Drawing.DrawLine(startPointR, endPoint, LineThickness, DrawR);
                        }
					}
				}
			}
        }
    }
}

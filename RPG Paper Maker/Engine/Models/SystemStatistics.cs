﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemStatistics : SuperListItem
    {
        public static int MAX_STATISTICS = 999;
        public Dictionary<string, string> Names;
        public SystemGraphic Bar;

        #region Game over 

        public GameOverOptions AllGameOverOptions;
        [Serializable]
        public class GameOverOptions
        {
            public bool NoImplication;
            public bool AllHeroes;
            public List<int> HeroesSelected;
            public Comparaison Comparaison;
            public int Value;
            public Measure Measure;

            // -------------------------------------------------------------------
            // Constructor
            // -------------------------------------------------------------------

            public GameOverOptions(bool noImplication = true, bool allHeroes = true, List<int> heroesSelected = null, Comparaison comparaison = Comparaison.Equal, int value = 0, Measure measure = Measure.Percent)
            {
                NoImplication = noImplication;
                AllHeroes = allHeroes;
                HeroesSelected = heroesSelected;
                Comparaison = comparaison;
                Value = value;
                Measure = measure;
            }

            // -------------------------------------------------------------------
            // CreateCopy
            // -------------------------------------------------------------------

            public GameOverOptions CreateCopy()
            {
                return new GameOverOptions(NoImplication, AllHeroes, (HeroesSelected == null) ? null : new List<int>(HeroesSelected), Comparaison, Value, Measure);
            }
        }

        #endregion


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public SystemStatistics(int id) : this(id, WANOK.GetDefaultNames(), new SystemGraphic(GraphicKind.Bar, new object[] { new int[] { 0, 0, 1, 1 } }), new GameOverOptions())
        {

        }

        public SystemStatistics(int id, Dictionary<string, string> names, SystemGraphic bar, GameOverOptions gameOverOptions)
        {
            Id = id;
            Names = names;
            Bar = bar;
            SetName();
            AllGameOverOptions = gameOverOptions;
        }

        public void SetName()
        {
            Name = Names[WANOK.CurrentLang];
        }

        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public override SuperListItem CreateCopy()
        {
            return new SystemStatistics(Id, new Dictionary<string, string>(Names), Bar.CreateCopy(), AllGameOverOptions.CreateCopy());
        }

        // -------------------------------------------------------------------
        // GetDefaultStatistics
        // -------------------------------------------------------------------

        public static List<SystemStatistics> GetDefaultStatistics()
        {
            List<SystemStatistics> list = new List<SystemStatistics>();

            list.Add(new SystemStatistics(1, WANOK.GetDefaultNames("HP"), new SystemGraphic("hpBar.png", true, GraphicKind.Bar, new object[] { 68, 4, 56, 8 }), new GameOverOptions(false, true, null, Comparaison.Equal, 0, Measure.Unit)));
            list.Add(new SystemStatistics(2, WANOK.GetDefaultNames("MP"),new SystemGraphic("mpBar.png", true, GraphicKind.Bar, new object[] { 68, 4, 56, 8 }), new GameOverOptions(true)));
            list.Add(new SystemStatistics(3, WANOK.GetDefaultNames("SP"),new SystemGraphic("spBar.png", true, GraphicKind.Bar, new object[] { 68, 4, 56, 8 }),new GameOverOptions(true)));

            return list;
        }
    }
}

using BullsAndCows;
using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsCowsTest.Mocks
{
    internal class MockUI : IUI
    {
        public string get { get; set; }
        public string put { get; set; }
        public bool cont { get; set; }
        public bool Continue()
        {
            return cont;
        }

        public string GetInput()
        {
            return get;
        }

        public void Output(string s)
        {
            put = s;
        }

        public void ShowHiscores(IEnumerable<PlayerData> hiscores)
        {
        }
    }
}

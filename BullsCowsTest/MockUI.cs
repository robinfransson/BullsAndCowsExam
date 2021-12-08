using BullsAndCows;
using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsCowsTest
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

        public string GetString()
        {
            return get;
        }

        public void PutString(string s)
        {
            put = s;
        }
    }
}

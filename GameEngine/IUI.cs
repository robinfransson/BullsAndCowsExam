using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IUI
    {
        void PutString(string s);
        string GetString();
        bool Continue();
    }
}

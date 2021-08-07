using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JET.Modding
{
    public abstract class JetMod
    {
        /// <summary>
        /// Called when your mod is first loaded.
        /// </summary>
        protected internal abstract void Initialize();
    }
}

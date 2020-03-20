using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    /// <summary>
    /// Inherit from this in order to be able to use the wheelselection. 
    /// </summary>
    public interface IWeightable
    {
        int Chance { get; set; }
    }
}

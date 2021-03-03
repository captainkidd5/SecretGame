using Penumbra;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.LightStuff
{
    //entities which block lighting should inherit from this.
    public interface ILightBlockable
    {
        Hull Hull { get; set; }
        void UpdateHullPosition();
        void LoadPenumbra( Stage stage);

    }
}

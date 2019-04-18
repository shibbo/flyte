using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt
{
    public class MaterialBase
    {
        public enum Type
        {
            Wii = 0,
            _3DS = 1,
            Switch =2
        }

        public virtual string getName() { return mMaterialName; }

        public string mMaterialName;

        public Type getType() { return mMaterialType; }
        public void setType(Type type) { mMaterialType = type; }

        Type mMaterialType;
    }
}

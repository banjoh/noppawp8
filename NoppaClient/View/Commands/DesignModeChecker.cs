using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.View
{
    public class DesignToolChecker
    {
        public bool IsInDesignTool { get { return System.ComponentModel.DesignerProperties.IsInDesignTool; } }
        public bool IsNotInDesignTool { get { return !IsInDesignTool; } }
    }
}
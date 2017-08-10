using Hale.Lib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Data.Entities.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public class Function
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public string Identifier { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModuleFunctionType Type { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Module Module { get; set; }
    }

    /*
    /// <summary>
    /// 
    /// </summary>
    public class FunctionType
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static int Action = 1;
        
        /// <summary>
        /// 
        /// </summary>
        public static int Check = 2;

        /// <summary>
        /// 
        /// </summary>
        public static int Info = 3;
    }
    */
}

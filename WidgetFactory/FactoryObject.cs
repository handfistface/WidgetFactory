using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetFactory
{
    /// <summary>
    /// public class FactoryObject
    /// Author: John Kirschner
    /// Date: 07-28-2018
    /// Details:
    ///     The interface that will be inherited to generate the object definition
    ///     The object name will need implemented
    ///     The components will be conditionally implemented (say someone buys a part, well that part is not manufactured at the factory
    ///     so the class realizes that the components will not need bulit prior and will just include them in the build process)
    /// Thread safe
    /// </summary>
    public class FactoryObject
    {
        #region public string s_Name
        /// <summary>
        /// The name of the object to be created or consumed in the factory
        /// </summary>
        public string s_Name
        {
            get
            {
                string s_Temp;      //temporary storage which will be returned
                //lock the object to be thread safe
                lock (_o_NameLock)
                    s_Temp = _s_Name;       //get the contents from the storage variable
                return s_Temp;      //return the temporary storage
            }
            set
            {
                //lock the object to be thread safe
                lock (_o_NameLock)
                    _s_Name = value;        //set the storage variable to the contents passed to this method
            }
        }
        private string _s_Name;     //storage variable for s_Name
        private object _o_NameLock;     //lock for _s_Name
        #endregion public string s_Name
        private Stack<FactoryObject> sfo_Components;     //holds the components required to construct the specified item

        #region public FactoryObject(string s_Name)
        /// <summary>
        /// public FactoryObject(string s_Name)
        /// Constructor for the class
        /// Takes the name as a parameter, will leave the components stack empty to show that there 
        /// are no components required to build the object
        /// </summary>
        /// <param name="s_Name">The name of the factory object</param>
        public FactoryObject(string name)
        {
            s_Name = name;
            sfo_Components = new Stack<FactoryObject>();        //create a new empty stack
        }
        #endregion public FactoryObject(string s_Name)

        #region public GetComponents()
        /// <summary>
        /// Stack(FactoryObject) GetComponents()
        /// Returns the stack of components required to build this item
        /// Can be an empty stack
        /// Returns a new stack compared to the one in this class, this is so that the stack access is thread safe
        /// </summary>
        /// <returns>The stack of items that is required to build this item</returns>
        Stack<FactoryObject> GetComponents()
        {
            Stack<FactoryObject> sfo_Ret = new Stack<FactoryObject>();       //the return for this stack
            //loop through each item in the stack
            foreach(FactoryObject fo in sfo_Components)
            {
                sfo_Ret.Push(fo);       //push the factory object onto the top of the stack
            }
            return sfo_Ret;     //return the stack of factory objects
        }
        #endregion public GetComponents()

        #region public void AddComponent(FactoryObject fo_Component)
        /// <summary>
        /// public void AddComponent(FactoryObject fo_Component)
        /// This method adds an item to the stack of factory objects
        /// </summary>
        /// <param name="fo_Component">The component that will be added to the list</param>
        public void AddComponent(FactoryObject fo_Component)
        {
            sfo_Components.Push(fo_Component);
        }
        #endregion public void AddComponent(FactoryObject fo_Component)
    }
}

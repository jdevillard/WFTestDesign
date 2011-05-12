using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.WFTestDesign_Integration
{


    //Custom Attibutes
    [ProvideToolboxItems(1)]
    [ProvideToolboxFormat("CF_WORKFLOW_4")]


    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidWFTestDesign_IntegrationPkgString)]
    public sealed class WFTestDesign_IntegrationPackage : Package
    {
        string CategoryName = "WFTestDesign";
        string ActivityName = "MonActiviteBizUnit";

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public WFTestDesign_IntegrationPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));

            ToolboxInitialized += new EventHandler(AddActivityToToolbox);
            ToolboxUpgraded += new EventHandler(AddActivityToToolbox);
        }



        void AddActivityToToolbox(object sender, EventArgs e)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Function AddActivityToToolbox"));

            IVsToolbox toolbox = (IVsToolbox)this.GetService(typeof(SVsToolbox));
            if (toolbox != null)
            {
                toolbox.AddTab(CategoryName);

                Assembly ass = Assembly.LoadWithPartialName("WFTestDesign.Activities");

                
                Func<Assembly, System.Collections.Generic.IEnumerable<Type>> getitems = assembly =>
                    from type in assembly.GetTypes()
                    where type.IsPublic
                    && !type.IsNested
                    && !type.IsAbstract
                    && typeof(System.Activities.Activity).IsAssignableFrom(type)
                    orderby type.Name
                    select type;

               List<Type> ass_type = getitems(ass).ToList();


               // Type[] types = ass.GetTypes();

               foreach (Type _type in ass_type)
                {
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "get assembly : " +  _type.Name));


                    OleDataObject dataObject = new OleDataObject();
                    //AssemblyName assemblyName = typeof(WFTestDesign.Activities.File.FileCreate).Assembly.GetName();


                    AssemblyName assemblyName = _type.Assembly.GetName();
                    assemblyName.CodeBase = @"E:\Dev\VS2010\WFTestDesign\WFTestDesign.Activities\bin\Debug\WFTestDesign.Activities.dll";
                    dataObject.SetData("AssemblyName", assemblyName);
                    dataObject.SetData("CF_WORKFLOW_4", "MapperActivity");
                    dataObject.SetData("WorkflowItemTypeNameFormat", _type.AssemblyQualifiedName);
                    TBXITEMINFO[] toolboxItemInfo = new TBXITEMINFO[1];
                    toolboxItemInfo[0].bstrText = _type.Name;
                    //Bitmap bmp = new System.Drawing.Bitmap(16, 16);
                   
                    
                    object[] attrs = _type.GetCustomAttributes(typeof(System.Drawing.ToolboxBitmapAttribute), true); ;  // Reflection.
                   /*
                    if (attrs.Length != 0)
                    {
                        ToolboxBitmapAttribute attr = (ToolboxBitmapAttribute) attrs[0];
                        Trace.WriteLine(" Attributes : " + attr.ToString());
                        toolboxItemInfo[0].hBmp = (new Bitmap(attr.GetImage(attr.GetType()))).GetHbitmap();
                        //Image img =  ToolboxBitmapAttribute.GetImageFromResource(typeof(WFTestDesign.Activities.File.FileCreate), "", false);
                        //Bitmap bmp = new Bitmap(img);
                    
                    }
                   
                    if (attrs.Length == 0)
                    {
                    Trace.WriteLine("No Bitmap Attributes : " );
                    Trace.WriteLine("Lenght Activity CustomAttributes: " + (typeof(System.Activities.Activity)).GetCustomAttributes(typeof(System.Drawing.ToolboxBitmapAttribute), true).Length);
                    foreach (var item in (typeof(System.Activities.Activity)).GetCustomAttributes(typeof(System.Drawing.ToolboxBitmapAttribute), true))
                    {
                         Trace.WriteLine("Custom Attributes : " + item.ToString());
                    }
                    
                        
                        /*
                    object DefaultBitmap = (typeof(System.Activities.Activity)).GetCustomAttributes(typeof(System.Drawing.ToolboxBitmapAttribute), true)[0];
                   ToolboxBitmapAttribute DefaultBitmapAttribute = (ToolboxBitmapAttribute) DefaultBitmap;
                   IntPtr DefaultIntPtr = (new Bitmap(DefaultBitmapAttribute.GetImage(DefaultBitmapAttribute.GetType()))).GetHbitmap();
                   toolboxItemInfo[0].hBmp = DefaultIntPtr;
                        
                   }*/

                    bool isIconDefault = true;
                        // Displaying output.
                    foreach (System.Drawing.ToolboxBitmapAttribute attr in attrs)
                    {
                        Trace.WriteLine(" Attributes : " + attr.ToString());
                        toolboxItemInfo[0].hBmp = (new Bitmap(attr.GetImage(attr.GetType()))).GetHbitmap();
                        isIconDefault = false;
                    }

                    if (isIconDefault)
                        toolboxItemInfo[0].hBmp = Resources.DefaultIco.GetHbitmap();


                    toolbox.AddItem(dataObject, toolboxItemInfo, CategoryName);







                }
            }   
          

            
                
                

                
            

           
            
                

            // Using reflection.
           
        }



           

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

        }
        #endregion

    }
}

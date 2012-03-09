using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.ServiceModel.Syndication;
using System.Reflection;
using System.Diagnostics;
using System.Net;

namespace Microsoft.WFTestDesign_Integration
{
    static class UpdateChecker
    {
        public static updatedVersion CheckForUpdates(string feedUrl, UpdateFilter filter)     
        {
            // NOTE: Requires a reference to System.ServiceModel.dll      
            var formatter = new Atom10FeedFormatter();  
             try
            { 
                 // Read the feed         
                var reader = XmlReader.Create(feedUrl);
                formatter.ReadFrom(reader);  
            }
            catch (Exception e)
            {
                Trace.WriteLine(String.Format("Error while trying to check updade : {0} " ,e.Message));
                return null;
            }
                     
            
            // Get the version specified in AssemblyInfo.cs        
            var currentVersion = new Version(version.fileversion);//Assembly.GetExecutingAssembly().GetName().Version;        
            
            // Linq magic         
            var update = (from i in formatter.Feed.Items            
            where new Version(i.Title.Text) > currentVersion
            //&& i.Categories.Any(c => CanUpgradeTo(c.Name, filter))
                          orderby i.LastUpdatedTime descending
                              select i).FirstOrDefault();

            if (update != null)
            {
                return new updatedVersion()
                {
                    downloadurl = update.Links.Single().Uri.ToString(),
                    oldversion = currentVersion.ToString(),
                    newversion = update.Title.Text,
                    content = update.Summary.Text
                };

            }
            else
                return null;
        }    
        
        /*
        static bool CanUpgradeTo(string status, UpdateFilter filter)  
        {        
            // Bitwise magic  
            return true;//(int)filter & (int)Enum.Parse(typeof(UpdateFilter), status, true) != 0; 
        }
        */
    }

    enum ReleaseStatus {
        Stable = 1, 
        Beta = 2, 
        Alpha = 4 
    }
    enum UpdateFilter { 
        Stable = ReleaseStatus.Stable,
        Beta = Stable | ReleaseStatus.Beta,
        Alpha = Beta | ReleaseStatus.Alpha
    } 

    public class updatedVersion
    {
        public string oldversion { get; set; }
        public string newversion { get; set; }
        public string downloadurl {get;set;}
        public string content { get; set; }
    }

}

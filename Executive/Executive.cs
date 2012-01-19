////////////////////////////////////////////////////////////////////////////
// Executive.cs - Starter Executive for Project #2                        //
//Author:Mukund Narayana Murthy SUID:50361-4612                           //
// SOURCE:Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2011 //
///////////////////////////////////////////////////////////////////////////

/*
 * Module Operations:
 * ------------------
 * This module defines the following class:
 *   Program  - a class that acts a central controlling Package in Ensuring the Functioning 
 *   of the Source Code Publisher
 */
/* Required Files:
 * ===============
 * -----------------------------
 * Module            File Names
 * -----------------------------       
 * Executive    --   Executive.cs
 * FileFinder   --   Navigate.cs
 * Locator      --   IRulesandAction.cs,Parser.cs,RulesandActions.cs,ScopeStack.cs,Semi.cs,Toker.cs  
 * TextInserter --   Inserter.cs,Publish.cs
 *   
 *   
 * Maintenance History:
 * --------------------
 * Version 1.0 Release 10/10/2011.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Project2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path; 
            if (args.Length > 0)
                path = args[0];
            else
                path = Directory.GetCurrentDirectory();
            Publish pub = new Publish();
            pub.setPubDir("./publish");
            pub.setDupDir("../../duplicate");
            pub.setTemplate("../../Resources/Template.htm"); 
            Navigate nav = new Navigate();
            nav.go(path, "*.cs");
            List<string> files = nav.getSources(); // To Obtain list of fully qualified filenames to be processed
            DirectoryInfo di1;
                
                if (!Directory.Exists(pub.getDupDir()))
                    di1 = Directory.CreateDirectory(pub.getDupDir());
                else
                    di1 = new DirectoryInfo(pub.getDupDir());
                    foreach (string file in files)
                    {
                        FileInfo File = new FileInfo(file);
                        DirectoryInfo di;
          
                        if (!Directory.Exists(pub.getPubDir()))
                            di = Directory.CreateDirectory(pub.getPubDir());
                        else
                        di = new DirectoryInfo(pub.getPubDir());
                            try
                            {
                                List<CodeAnalysis.Elem> table = pub.Locate(File);//find locations where spans need to be inserted
                                if (table != null)
                                {
                                    FileInfo dupFile = pub.Spans(table, File);
                                    if (dupFile != null)
                                    {
                                        pub.makePage(dupFile, di); 
                                    }
                                }     
                            } 
                            catch(Exception ex)
                            {
                                Console.WriteLine("An error occured: " + ex.Message);
                                continue;
                            }
                        pub.CreateDirList(di);       
                    }
        }
    }
}

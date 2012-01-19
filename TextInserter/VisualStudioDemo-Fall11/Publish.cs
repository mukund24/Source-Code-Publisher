/////////////////////////////////////////////////////////////////////////////////
// Publish.cs - handles conversion to html for single file                    //
//              Author:Mukund Narayana Murthy SUID:50361-4612               //
// Source :Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2011//
///////////////////////////////////////////////////////////////////////////

/*
 * Module Operations:
 * ------------------
 * This module defines the following class:
 *   Publish  - a collection functions Used  to PUBLISH source Code Files to HTML Pages.
 * 
 * Functions List:
 * --------------
 * 1)SetTemplate()    -- Sets the value of the Template.htm Path for the final HTML published Page
 * 
 * 2)GetTemplate()    -- Get the value of the Template.htm path to be used by the Inserter
 * 
 * 3)SetPubDir()      -- Sets the value of the PUblished Directory.
 * 
 * 4)GetPubDir()      -- Get the value of the Published Directory.
 * 
 * 5)SetDupDir()      -- Sets the value of the Duplicate Directory (a temporary file store into which Span Tags would get inserted)
 * 
 * 6)GetDupDir()      -- Get the value of the  Duplicate Directory.
 * 
 * 7)makePage(FileInfo file,DirectoryInfo di) -- A function responsible to Publish one source code file at a time.
 * 
 * 8)CreateDirList(DirectoryInfo di)  -- A funtion Responsible to create a directory listing of all the published files
 * 
 * 9)List<CodeAnalysis.Elem> Locate(FileInfo f)   -- A function used to locate the begin and end of Types in the Source Code File.
 * 
 * 10)FileInfo Spans(List<CodeAnalysis.Elem> e,FileInfo f) -- A function that Iterates through the File line by line inorder to Insert Spans.
 * 
 * 11)InsertSpans(List<CodeAnalysis.Elem> e, ref int count, ref int StartFlag, ref int EndFLag, ref string Ospan, 
 * ref string line, string Mspan, StreamReader sr, StreamWriter sw) -- Inserts Spans at appropriate Locations in the Duplicate File
 * 
 * 12)WriteLines(string Ospan, int flag1,int flag2, string line, StreamWriter sw) -- Writes lines from the file as per the Inserted Span Tags.
 * 
 * 13)CopyResources(DirectoryInfo di) -- Copies the JavaScript and CSS file resources needed by the Published Html pages into 
 * the respective JS and CSS directories 
 */
/* 
 * 
 * Required Files: 
 * ===============
 * -----------------------------
 * Module            File Names
 * -----------------------------       
 * Locator      --   IRulesandAction.cs,Parser.cs,RulesandActions.cs,ScopeStack.cs,Semi.cs,Toker.cs  
 * TextInserter --   Inserter.cs,Publish.cs
 *   
 *   
 * Maintenance History:
 * --------------------
 * Version 1.0 Release 10/10/2011.
 * 
 * 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Project2
{
    public class Publish
    {
     string template;
     string PubDir;
     string DupDir;
         public void setTemplate(string templfile)
         {
            template = templfile;
         }

         public string getTemplate()
         {
             return template;
         }
         public void setPubDir(string path)
         {
             PubDir = path;
         }

        public string getPubDir()
        {
            return PubDir;
        }   


      //////////////////////////////////////////////////////////////////
     //Sets the Duplicate Directory of Source Code Files in which <span> 
    //tags are inserted before getting Published
   //////////////////////////////////////////////////////////////////// 
    
        public void setDupDir(string dupfile)
        {
            DupDir = dupfile;
        }

    //Get Duplicate Files Directory.

        public string getDupDir()
        {
            return DupDir;
        }
      
   //Publishes one source Code File at a time.

        public void makePage(FileInfo file,DirectoryInfo di)
        {
          // call to TextInserter to create a Clickable webpage.
        
            Inserter Ins = new Inserter();
            Ins.TextInsertion(getTemplate(), file.FullName,di.Name + "\\" + file.Name + ".htm");
    
        }

        public void CreateDirList( DirectoryInfo di)
        {
            string Line = "<html>" + "<head>" + "<title>Directory Listing </title>" + "</head>" + "<body>" + "<table>";
        
            TextWriter tw = new StreamWriter(di.FullName + "\\dirlist.htm");
            tw.WriteLine(Line);
            //Get the list of html files in the Published Directory
            FileInfo[] files=di.GetFiles("*.htm");
      
                foreach (FileInfo f in files)
                {
                    Line= "<tr><th>" + "<a href=" +f.Name +">" + f.Name+ "</a>" + "</th><th>" + f.Length + 
                        "</th>" + "<th>" + f.CreationTime + "</th></tr>";
                    tw.WriteLine(Line);
                }   
        
                    Line = "</table>" + "</body>" + "</html>";
                    tw.WriteLine(Line);
                    tw.Close();
        
                    DirectoryInfo dup=new DirectoryInfo(getDupDir());
                    FileInfo[] dupfiles = dup.GetFiles("*.cs");
                foreach (FileInfo d in dupfiles)
                {
                    try
                    {
                        d.Delete();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occured: " + ex.Message);
                        return;
                    }
                }
                    CopyResources(di); 
        }
    
       ///////////////////////////////////////////////////////////////////////////////
      //Locate does the job of locating the namespaces,functions and Class ranges.  //
    //It returns Table containing inforamtion about the Type,Name Begin Line and    //
     //Ending Lines of the above mentioned ranges.                                // 
     /////////////////////////////////////////////////////////////////////////////

        public List<CodeAnalysis.Elem> Locate(FileInfo file)
        {
            CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
            semi.displayNewLines = false;
            
            if (!semi.open(file.FullName as string))
             {
               return null;
             }
                CodeAnalysis.BuildCodeAnalyzer builder = new CodeAnalysis.BuildCodeAnalyzer(semi);
                CodeAnalysis.Parser par = builder.build();

                while (semi.getSemi())
                par.parse(semi);
                CodeAnalysis.Repository Rep = CodeAnalysis.Repository.getInstance();
            
                //Store the Begin,End locations alone with Name and Type Information
                List<CodeAnalysis.Elem> table = Rep.locations;
                semi.close();
                return Rep.locations;
        }

      //Funciton that Inserts the Span tags in the Duplicate copies of the Source Code files before 
      //passing the to MakePage for publishing
        public FileInfo Spans(List<CodeAnalysis.Elem> e,FileInfo f)
        {   
            int count = 0,StartFlag=0,EndFLag=0,Mflag=0;
            string Ospan, line, Mspan = "<span class=\"" + "manpage" + "\">";
            StreamReader sr = null;
            
            StreamWriter sw = null;
                try
                {   
                    sr = new StreamReader(f.FullName);
                    sw = new StreamWriter(DupDir + "\\" + f.Name);            
                }
                catch(Exception ex)
                {
                    Console.WriteLine("An error occured: " + ex.Message);
                    return null;
                }
                FileInfo dupFile = new FileInfo(DupDir + "\\" + f.Name);    //Get Duplicate File  
                
                do
                {
                    Mflag=StartFlag = EndFLag = 0;
                    Ospan = "<span class=\"";
                    line = sr.ReadLine();
                    line = System.Net.WebUtility.HtmlEncode(line);
                    count++;
                    InsertSpans(e, ref count, ref StartFlag, ref EndFLag, ref Ospan, ref line, Mspan, sr, sw);
                        if((StartFlag==0)&&(EndFLag==0))
                        sw.WriteLine(line);
                } while (line!=null);
                sr.Close();
                sw.Close();
                return dupFile; //To be used by the MakePage inorder to Publish the SOURCE CODE files one by one
        }

        // Inserts Span Tags as per the specified locations from the List<CodeAnalysis.Elem> Table //
        
        private void InsertSpans(List<CodeAnalysis.Elem> e, ref int count, ref int StartFlag, ref int EndFLag, ref string Ospan, ref string line, string Mspan, StreamReader sr, StreamWriter sw)
        {
            foreach (CodeAnalysis.Elem m in e)
            {
                if (line != null && line.Contains("/*") && count < e[0].begin)// Check For Beginnning of ManPages to insert Spans
                {
                    sw.WriteLine(Mspan);
                    while (!line.Contains("using System"))
                    {
                        sw.WriteLine(line);
                        count++;
                        line = sr.ReadLine();
                        line = System.Net.WebUtility.HtmlEncode(line);
                    }
                    sw.WriteLine("</span>");
                    break;
                }
                else if (count == m.begin) //Check for Beginning of Namespaces/Classes/Functions 
                {
                    Ospan += m.type + "\">";
                    StartFlag = 1;
                    WriteLines(Ospan, StartFlag, EndFLag, line, sw);
                    break;
                }
                else if (count == m.end) //Check for Endings of Namespaces/Classes/Functions 
                {
                    EndFLag = 1;
                    WriteLines(Ospan, StartFlag, EndFLag, line, sw);
                    break;
                }
            } //Foreach loop ends here.
        }

        public void WriteLines(string Ospan, int flag1,int flag2, string line, StreamWriter sw)
        {
            string Cspan = "</span><span>";
            string Espan = "</span>";
                if (flag1 == 1)
                {
                    sw.Write(Ospan);
                    sw.WriteLine(line);
                    sw.Write(Cspan);        
                }
                else if (flag2 == 1)
                {
                    sw.WriteLine(line);
                    sw.Write(Espan);
                }      
        }//Writelines Function Ends

        public void CopyResources(DirectoryInfo di)// Copy resources of JavaScript and CSS to be used by the HTML files
        {
            if (!di.CreateSubdirectory("CSS").Exists)
            {
                di.CreateSubdirectory("CSS");
            }
            if (!di.CreateSubdirectory("js").Exists)
            {
                di.CreateSubdirectory("js");

            }
            DirectoryInfo Res = new DirectoryInfo("../../Resources");
            FileInfo[] Resources = Res.GetFiles();

            foreach (FileInfo f in Resources)
            {
                if (f.Extension == ".js")
                {
                    f.CopyTo(di.FullName + "\\" + "js" + "\\" + f.Name,true);
                }
                if (f.Extension == ".css")
                {
                    f.CopyTo(di.FullName + "\\" + "CSS" + "\\" + f.Name,true);
                }
            }
        }


#if(TEST_PUBLISHER)
        static void Main(string[] args)
        {
            Console.Write("\n Testing Text Inserter And Publisher");
            Console.Write("\n ======================\n");
            DirectoryInfo di, di1;
            Publish pub = new Publish();
            pub.setPubDir("./publish");
            pub.setDupDir("./duplicate");
            pub.setTemplate("../../Resources/template.htm");

                   if (!Directory.Exists(pub.getPubDir()))
                        di = Directory.CreateDirectory(pub.getPubDir());
                   else
                        di = new DirectoryInfo(pub.getPubDir());
            
            pub.getDupDir();
            Console.WriteLine(di);
                    if (!Directory.Exists(pub.getDupDir()))
                        di1 = Directory.CreateDirectory(pub.getDupDir());
                    else
                        di1 = new DirectoryInfo(pub.getDupDir());
                    
            Console.WriteLine(di1);
            FileInfo f = new FileInfo("../../Publish.cs");
            
            List<CodeAnalysis.Elem> table=pub.Locate(f);

            FileInfo dupFile=pub.Spans(table, f);

            StreamReader sr = new StreamReader(dupFile.FullName);

            string line = sr.ReadToEnd();
            Console.WriteLine(line);

                    if (dupFile != null)
                    {
                        pub.makePage(dupFile, di);
                    }
            sr.Close();
            pub.CreateDirList(di);
            Console.WriteLine("File Published in the following directory:");
            Console.WriteLine("===========================================");
            Console.WriteLine(pub.getPubDir());
            Console.WriteLine("===========================================");
        }
#endif
  }
}


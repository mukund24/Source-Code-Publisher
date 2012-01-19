//////////////////////////////////////////////////////////////////////////////
// Inserter.cs - Demo inserting one file into the body of another           //
// Author:Mukund Narayana Murthy SUID:50361-4612                            //
// Source:Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2011   //
//////////////////////////////////////////////////////////////////////////////
/*
 * Module Opearations:
 * -------------------
 * This Module Defines the Following Class
 * 1)Inserter -- Provides the functions needed to insert the SOURCE CODE FILE having the Span Tags at appropriate
 * locations to be inserted into the HTML TEMPLATE FILE.
 * 
 * Functions Included:
 * ------------------
 * TextInsertion(string Template, string Inserted, string Result) -- Inserts the SOURCE CODE FILE between the 
 * <pre></pre>(preformat tags) of the template.htm file.
 * 
 * Required Files: 
 * ---------------
 * 1) Publish.cs 2) Inserter.cs
 * 
 * * Maintenance History:
 * --------------------
 * Version 1.0 Release 10/10/2011.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Project2
{
  public class Inserter
  {
        public void TextInsertion(string Template, string Inserted, string Result)
        {
          StreamReader templateRdr = null;
          StreamReader insertedRdr = null;
          StreamWriter result = null;
          try
          {
            templateRdr = new StreamReader(Template);
            insertedRdr = new StreamReader(Inserted);
            result = new StreamWriter(Result);
          }
          catch
          {
            Console.Write("\n\n  could not read or write the requested files\n\n");
            return;
          }
          do
          {
            string line = templateRdr.ReadLine();
        
            result.WriteLine(line);
            if (line == null)
              break;
            if (line.IndexOf("<pre>") != -1)
            {
              do
              {
                string inline = insertedRdr.ReadLine();
                if (inline == null)
                  break;
                result.WriteLine(inline);
              } while (true);
              insertedRdr.Close();
            }
          } while (true);
          templateRdr.Close();
          result.Close();
        
        }
    
  }
}

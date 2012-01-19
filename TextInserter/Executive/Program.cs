///////////////////////////////////////////////////////////////////////
// Program.cs - Demo inserting one file into the body of another     //
//                                                                   //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2011   //
///////////////////////////////////////////////////////////////////////
/*
 * Makes reference to VisualStudioDemo-Fall11 project
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Executive
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.Write("\n  Demonstrating Text Insertion");
      Console.Write("\n ==============================\n");

      if (args.GetLength(0) < 3)
      {
        Console.Write(
          "\n  Please enter name of template, inserted, and to file names\n\n"
        );
        return;
      }
      VisualStudioDemo_Fall11.Inserter insrtr 
        = new VisualStudioDemo_Fall11.Inserter();
      insrtr.TextInsertion(args[0], args[1], args[2]);
    }
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Classes_To_Forms
{
    class Program
    {

        static void Main(string[] args)
        {
            if(args.Length == 0)
            {

                Console.WriteLine();
                Console.WriteLine(@"    ____ _____     _____");
                Console.WriteLine(@"   / ___|_   _|__ |  ___|");
                Console.WriteLine(@"  | |     | |/ _ \| |_");
                Console.WriteLine(@"  | |___  | | (_) |  _|");
                Console.WriteLine(@"   \____| |_|\___/|_|");

                Console.WriteLine();
                Console.WriteLine("    Usage: ctof class.cs");
                Console.WriteLine();
                Console.WriteLine("    Description :");
                Console.WriteLine();
                Console.WriteLine("      Create Win dows Form with components the and form");
                Console.WriteLine();
                Console.WriteLine("     GitHub : https://github.com/AndetonAf/ClassToForm");
                Console.WriteLine();

                return;
            }

            if (args.Length >= 1 && File.Exists(args[0]))
            {
                Console.WriteLine(args[0]);
                Generate(args[0]);
            }
            else
            {
                Console.WriteLine("File not found.");
            }

        }

        static void Generate(string path)
        {
            string _namespace = GetNamespace(path);
            List<string> variables = GetVariables(path);
            string get = Get(path, variables);
            string set = Set(path, variables);
            GenerateForm(path, _namespace, get, set);
            string componentsInitiate = ComponentsInitiate(variables);
            string componentsSet = ComponentsSet(variables);
            string componentsVariable = ComponentsVariable(variables);
            string componentsAddControl = ComponentsAddControl(variables);
            GenerateDesigner(path, _namespace, componentsInitiate, componentsSet, componentsAddControl, componentsVariable);
        }

        static string GetNamespace(string path)
        {
            string _namespace = "";
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            while ((line = file.ReadLine()) != null)
            {
                if (line.IndexOf("namespace") > -1) {
                    _namespace = line.Split(" ")[1];
                }
            }
            file.Close();
            return _namespace;
        }

        static List<string> GetVariables(string path)
        {
            List<string> variables = new List<string>();
            bool insideClass = false;
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            while ((line = file.ReadLine()) != null)
            {
                if (line.IndexOf("class") > -1)
                {
                    insideClass = !insideClass;
                }
                if (line.IndexOf(";") > -1 && insideClass)
                {
                    variables.Add(line.Remove(line.Length - 1).Split(" ")[line.Split(" ").Length - 1]);
                }
            }
            file.Close();
            return variables;
        }

        static string Get(string path, List<string> variables)
        {
            FileInfo file = new FileInfo(path);
            string name = file.Name.Split('.')[0];
            string componentsInitiate = "\r\n\t\tpublic void Get()\r\n\t\t{\r\n";
            foreach (string i in variables)
            {
                string _i = i.First().ToString().ToUpper() + i.Substring(1);
                componentsInitiate += "\t\t\tdata." + i + " = textBox"+_i+".Text;\n";
            }
            componentsInitiate += "\t\t}\r\n";
            return componentsInitiate;
        }
        static string Set(string path, List<string> variables)
        {
            FileInfo file = new FileInfo(path);
            string name = file.Name.Split('.')[0];
            string componentsInitiate = "\r\n\t\tpublic void Set()\r\n\t\t{\r\n";
            foreach (string i in variables)
            {
                string _i = i.First().ToString().ToUpper() + i.Substring(1);
                componentsInitiate += "\t\t\ttextBox" + _i+".Text = data."+i+";\r\n";
            }
            componentsInitiate += "\t\t}\r\n";
            return componentsInitiate;
        }

        static void GenerateForm(string path, string _namespace, string get, string set)
        {
            FileInfo file = new FileInfo(path);
            string name = file.Name.Split('.')[0];
            string form = "using System.Windows.Forms;\r\n\r\nnamespace " + _namespace + "\r\n{\r\n\tpublic partial class FormClass" + name + ": Form\r\n\t{\r\n\t\t"+"public "+name+ " data = new " + name + "();\r\n\t\tpublic FormClass" + name + "()\r\n\t\t{\r\n\t\t\tInitializeComponent();\r\n\t\t}" + get + set + "\r\n\t}\r\n}";

            string output = @file.DirectoryName+"\\FormClass" + file.Name;
            Console.WriteLine("Created FormClass" + file.Name);

            System.IO.File.WriteAllText(output, form);
        }

        static string ComponentsInitiate(List<string> variables)
        {
            string componentsInitiate = "";
            foreach (string i in variables)
            {
                string name = i.First().ToString().ToUpper() + i.Substring(1);
                componentsInitiate += "\t\t\tthis.label" + name + " = new System.Windows.Forms.Label();\r\n";
                componentsInitiate += "\t\t\tthis.textBox" + name + " = new System.Windows.Forms.TextBox();\r\n";
            }
            return componentsInitiate;
        }
        static string ComponentsSet(List<string> variables)
        {
            string componentsSet = "";
            foreach (string i in variables)
            {
                int y = 25 * (variables.IndexOf(i) + 1);
                string name = i.First().ToString().ToUpper() + i.Substring(1);
                componentsSet += "\t\t\t// \r\n";
                componentsSet += "\t\t\t// label" + name + "\r\n";
                componentsSet += "\t\t\t// \r\n";
                componentsSet += "\t\t\tthis.label" + name + ".AutoSize = true;\r\n";
                componentsSet += "\t\t\tthis.label"+ name + ".Location = new System.Drawing.Point("+ 20 + ", "+ (y + 3) + ");\r\n";
                componentsSet += "\t\t\tthis.label"+ name + ".Name = \"label"+ name + "\";\r\n";
                componentsSet += "\t\t\tthis.label"+ name + ".Size = new System.Drawing.Size(35, 13);\r\n";
                componentsSet += "\t\t\tthis.label"+ name + ".TabIndex = 0;\r\n";
                componentsSet += "\t\t\tthis.label"+ name + ".Text = \"" + name + "\";\r\n";
                componentsSet += "\t\t\t// \r\n";
                componentsSet += "\t\t\t// textBox" + name + "\r\n";
                componentsSet += "\t\t\t// \r\n";
                componentsSet += "\t\t\tthis.textBox" + name + ".Location = new System.Drawing.Point("+ 60 + ","+ y + ");\r\n";
                componentsSet += "\t\t\tthis.textBox"+ name + ".Name = \"textBox" + name + "\";\r\n";
                componentsSet += "\t\t\tthis.textBox"+ name + ".Size = new System.Drawing.Size(100, 20);\r\n";
                componentsSet += "\t\t\tthis.textBox" + name + ".TabIndex = 1;\r\n";
            }
            return componentsSet;
        }
        static string ComponentsVariable(List<string> variables)
        {
            string componentsVariable = "";
            foreach (string i in variables)
            {
                string name = i.First().ToString().ToUpper() + i.Substring(1);
                componentsVariable += "\t\tprivate System.Windows.Forms.Label label" + name + ";\r\n";
                componentsVariable += "\t\tprivate System.Windows.Forms.TextBox textBox" + name + ";\r\n";
            }
            return componentsVariable;
        }
        static string ComponentsAddControl(List<string> variables)
        {
            string componentsAddControl = "";
            foreach (string i in variables)
            {
                string name = i.First().ToString().ToUpper() + i.Substring(1);
                componentsAddControl += "this.Controls.Add(this.label" + name + ");\r\n";
                componentsAddControl += "this.Controls.Add(this.textBox" + name + ");\r\n";
            }
            return componentsAddControl;
        }
        static void GenerateDesigner(string path, string _namespace, string componentsInitiate, string componentsSet, string componentsAddControl, string componentsVariable)
        {
            FileInfo file = new FileInfo(path);
            string name = file.Name.Split('.')[0];
            string designer = "namespace "+_namespace+ "\r\n{\r\n\tpartial class FormClass" + name+"\r\n\t{\r\n\t\t/// <summary>\r\n\t\t/// Required designer variable.\r\n\t\t/// </summary>\r\n\t\tprivate System.ComponentModel.IContainer components = null;\r\n\r\n\t\t/// <summary>\r\n\t\t/// Clean up any resources being used.\r\n\t\t/// </summary>\r\n\t\t/// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>\r\n\t\tprotected override void Dispose(bool disposing)\r\n\t\t{\r\n\t\t\tif (disposing && (components != null))\r\n\t\t\t{\r\n\t\t\t\tcomponents.Dispose();\r\n\t\t\t}\r\n\t\t\tbase.Dispose(disposing);\r\n\t\t}\r\n\r\n\t\t#region Windows Form Designer generated code\r\n\r\n\t\t/// <summary>\r\n\t\t/// Required method for Designer support - do not modify\r\n\t\t/// the contents of this method with the code editor.\r\n\t\t/// </summary>\r\n\t\tprivate void InitializeComponent()\r\n\t\t{\r\n\t\t\t"+componentsInitiate+"\r\n\t\t\tthis.SuspendLayout();\r\n\t\t\t"+componentsSet+"\r\n\t\t\t// \r\n\t\t\t// "+name+"\r\n\t\t\t// \r\n\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);\r\n\t\t\tthis.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;\r\n\t\t\tthis.ClientSize = new System.Drawing.Size(800, 450);\r\n\t\t\t "+ componentsAddControl + "\r\n\t\t\tthis.Name = \""+name+"\";\r\n\t\t\tthis.Text = \""+name+"\";\r\n\t\t\tthis.ResumeLayout(false);\r\n\r\n\t\t}\r\n\r\n\t\t#endregion\r\n\t\t\r\n\t\t" + componentsVariable + "\r\n\t}\r\n}";
            string output = @file.DirectoryName+"\\FormClass" + name + ".Designer.cs";
            System.IO.File.WriteAllText(output, designer);
            Console.WriteLine("Created FormClass" + name + ".Designer.cs");
        }

    }
}

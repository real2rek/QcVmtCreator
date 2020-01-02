using System;
using System.IO;
using System.Collections;

namespace QcVmtCreator
{
    class Program
    {
        static class Globals
        {
            public static string userName = "";
            public static string prefix = "";
            public static string searchPath = "";
        }

        ArrayList files = new ArrayList();
        ArrayList materials = new ArrayList();
        ArrayList physModels = new ArrayList();

        public void getName()
        {
            Program program = new Program();
            Console.WriteLine("Please enter your name:");
            Globals.userName = Console.ReadLine();
            program.getPath();
        }

        public void getPath()
        {
            Program program = new Program();

            Console.WriteLine("Please enter SMD files location:");
            Globals.searchPath = Console.ReadLine();
            if (!Directory.Exists(Globals.searchPath))
            {
                program.getPath();
                return;
            }
            program.getFiles(Globals.searchPath);
        }

        public void addFile(string file)
        {
            files.Add(file);
        }

        public void getMaterials(string file)
        {
            foreach (string line in File.ReadLines(file))
            {              
                if (line.Contains(Globals.prefix))
                {
                    if (!materials.Contains(line))
                    {
                        materials.Add(line);
                    }
                }
            }
        }

        public void printMaterials()
        {
            Console.WriteLine("Detected material names:");
            materials.Sort();
            //Displaying all materials 
            foreach (Object obj in materials)
            {
                Console.WriteLine(obj);
            }
        }

        public void getPhysModels(string fileName)
        {
            physModels.Add(fileName);
        }

        public void printPhysModels()
        {
            Console.WriteLine("Detected phys models:");
            physModels.Sort();
            //Displaying all phys models
            foreach (Object obj in physModels)
            {
                Console.WriteLine(obj);
            }
        }

        public void getFiles(string searchPath)
        {
            Program program = new Program();
            string[] files = Directory.GetFiles(searchPath);

            Globals.prefix = "";
            ///Detecting file prefix
            for (int j = 0; j < files.Length; j++)
            {
                string fileName = System.IO.Path.GetFileName(files[j]);
                if (fileName == "idle.smd" || fileName.Contains(".vmt") )
                {
                    continue;
                }
                int index = fileName.IndexOf('_');
                Globals.prefix = fileName.Substring(0, index + 1);
                Console.WriteLine("Detected prefix: " + Globals.prefix);
                break;
            }

            ///Cycling through all the files
            Console.WriteLine("Detected smd files:");
            for (int i = 0; i < files.Length; i++)
            {
                string pathDisp = files[i];
                string extension = Path.GetExtension(pathDisp);
                string fileName = System.IO.Path.GetFileName(files[i]);
                //Checking if we are processing .smd file (also not checking idle.smd)
                if (extension == ".smd" && fileName != "idle.smd")
                {
                    Console.WriteLine(fileName);
                    program.addFile(fileName);
                    program.getMaterials(files[i]);
                }
                //Chcking if we have some phys models
                if (fileName.Contains("_p.smd"))
                {
                    program.getPhysModels(fileName);
                }

                if (i == files.Length - 1)
                {
                    program.printMaterials();
                    program.printPhysModels();
                    program.generateQc();
                    program.generateMaterials();
                }
            }
        }

        public void generateQc()
        {
            Program program = new Program();
            Console.WriteLine("Generating QC files");

            //Printing all files
            foreach (Object obj in files)
            {
                string file = obj.ToString();
                if (file.Contains("_p.smd"))
                {
                    continue;
                }
                Console.WriteLine(obj);
            }

            //Creating qcs
            string qc = "";
            Globals.prefix = Globals.prefix.Replace("_", "");
            Directory.CreateDirectory(Globals.searchPath + "/QcVmtGenerator/qc/");
            foreach (Object obj in files)
            {
                qc = System.IO.File.ReadAllText(@"qc.qc");             
                string fileName = obj.ToString();
                string modelName = fileName.Replace(".smd", "");

                if (modelName.Contains("_p")) //if its model with physics create appropriate qc entry
                {
                    fileName = fileName.Replace("_p", "");
                    modelName = modelName.Replace("_p", "");
                    qc = System.IO.File.ReadAllText(@"qc_p.qc");
                    qc = qc.Replace("<name>", Globals.userName);
                    qc = qc.Replace("<prefix>", Globals.prefix);
                    qc = qc.Replace("<filename>", modelName);
                    qc = qc.Replace("<physmodel>", modelName + "_p");

                    File.WriteAllText(Globals.searchPath + "/QcVmtGenerator/qc/" + modelName + ".qc", qc);
                    continue;
                }

                qc = qc.Replace("<name>", Globals.userName);
                qc = qc.Replace("<prefix>", Globals.prefix);
                qc = qc.Replace("<filename>", modelName);
           
                File.WriteAllText(Globals.searchPath + "/QcVmtGenerator/qc/" + modelName + ".qc", qc);
            }
        }

        //Using arrayList of materials to create vmts
        public void generateMaterials()
        {
            Program program = new Program();
            Console.WriteLine("Generating materials");
            Directory.CreateDirectory(Globals.searchPath + "/QcVmtGenerator/vmt/materials/" + Globals.userName + "/" + Globals.prefix + "/");

            foreach (Object obj in materials)
            {
                string file = obj.ToString();
                if (file.Contains("_p.smd"))
                {
                    continue;
                }
                Console.WriteLine(obj);

                string materialName = obj.ToString();
                string materialBase = "";
                string materialNameInner = "";

                materialBase = System.IO.File.ReadAllText(@"mat_base.vmt");
                materialNameInner = materialName;

                if (materialName.Contains("gold"))
                {
                    materialBase = System.IO.File.ReadAllText(@"mat_gold.vmt");
                    materialNameInner = materialName.Replace("_gold", "");
                }
                if (materialName.Contains("glow"))
                {
                    materialBase = System.IO.File.ReadAllText(@"mat_glow.vmt");
                    materialNameInner = materialName.Replace("_glow", "");
                }

                materialBase = materialBase.Replace("<name>", Globals.userName);
                materialBase = materialBase.Replace("<prefix>", Globals.prefix);
                materialBase = materialBase.Replace("<filename>", materialNameInner);

                File.WriteAllText(Globals.searchPath + "/QcVmtGenerator/vmt/materials/" + Globals.userName + "/" + Globals.prefix + "/" + materialName + ".vmt", materialBase);
            }
        }

        public void endProgram()
        {

        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Console.WriteLine("Welcome to QC and VMT creator by 2REK 2019.");

            program.getName();
            var materials = new ArrayList();
        }

    }
}

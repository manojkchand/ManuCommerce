using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
//using System.Windows.Forms;
using Flyweight;

namespace Flyweight
{
    public class ProgramMainClient
    {
        static FlyweightFactory album = new FlyweightFactory();

        public static Dictionary<string, List<string>> allGroups = new Dictionary<string, List<string>>();

        public void LoadGroups(string path)
        {
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(path);//Assuming Test is your Folder
            System.IO.FileInfo[] Files = d.GetFiles("*");
            List<string> Members = new List<string>();
            foreach (var file in Files)
            {
                Members.Add(file.Name);

            }
            var myGroups = new[]{
                new {Name= "Garden", Members}
            };
            
            foreach (var g in myGroups)
            {
                if (allGroups.ContainsKey(g.Name))
                {
                    allGroups.Remove(g.Name);
                }
                allGroups.Add(g.Name, new List<string>());
                foreach (string filename in g.Members)
                {
                    allGroups[g.Name].Add(filename);
                    album[filename].Load(path,filename);
                }
            }
        }
        //will used to show the output to client
        public void CreateGroups(string ThumbnailPath)
        {
            Flyweight.Empty(ThumbnailPath);
            int row = 0;
            foreach (string g in allGroups.Keys)
            {
                int col = 0;
                //e.Graphics.DrawString(g, new Font("Arial", 16), new SolidBrush(Color.Black), new PointF(0, row * 130 + 10));
                foreach (string filename in allGroups[g])
                {
                    album[filename].Create(ThumbnailPath, "thumb_"+ filename);
                    col++;
                }
                row++;
            }
        }
    }
}

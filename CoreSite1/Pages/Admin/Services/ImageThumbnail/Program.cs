using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Flyweight
{
    public interface IFlyweight
    {
        void Load(string path,string filename);
        void Create(string ThumbnailPath, string filename);

    }

    //intrinsic state //extrinsic state class/struct is not created it would have shown the real picture.
    public struct Flyweight : IFlyweight
    {
        Image pThmbnai;
        public void Load(string path,string filename)
        {
            pThmbnai = new Bitmap(path + filename).GetThumbnailImage(100, 100, null, new IntPtr());

        }

        public void Create(string ThumbnailPath, string filename )//PaintEventArgs e, int row, int col)
        {
            pThmbnai.Save(ThumbnailPath + filename, System.Drawing.Imaging.ImageFormat.Jpeg);
            pThmbnai.Dispose();
        }

        public static void Empty(string ThumbnailPath)
        {//"wwwroot/Images_thumb/flyweight-images/thumbnail/"
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ThumbnailPath);
            foreach (System.IO.FileInfo file in di.GetFiles()) file.Delete();

            foreach (System.IO.DirectoryInfo dir in di.GetDirectories()) dir.Delete(true);
            
            //foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            //foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }

    public class FlyweightFactory
    {
        //keeps an indexed list of IFlyweight objects in existence
        Dictionary <string,IFlyweight> flyweights=new Dictionary<string,IFlyweight>();

        public FlyweightFactory()
        {
            flyweights.Clear();
        }

        public IFlyweight this[string index]
        {
            get
            {
                if (!flyweights.ContainsKey(index))
                    flyweights[index] = new Flyweight(); 
                    return flyweights[index];       
            }
        }
    }
}

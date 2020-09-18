using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace WindowsPrompt
{

    public partial class Form1 : Form
    {
        
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        

        public Form1()
        {
            InitializeComponent();
            
            UpdateTitleBar();
            
            UpdateFormBackGround();
            UpdateBackGroundColor();

        }

        private void UpdateTitleBar()
        {
          
        }

        private void UpdateFormBackGround()
        {
           // this.BackColor = Color.Red;
            this.BackgroundImage = Properties.Resources.photo1;
            
            
        }
       
       
        private void UpdateBackGroundColor()
        {
            ControlCollection allControls= (ControlCollection)Controls;
            try
            {
                foreach (Control control in allControls)
                {
                    if(control is Label)
                    {
                        ((Label)control).ForeColor = Color.Red;
                    }
                    if(control is Button)
                    {
                        ((Button)control).ForeColor = Color.Red;
                    }

                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        
        public static Image GetImage()
        {
            BrandingPolicy brandingPolicy = GetBrandingPolicyData();
            Image image2 = null;
            byte[] bytes = Convert.FromBase64String(brandingPolicy.BackgroundImage);
            using (var ms = new MemoryStream(bytes))
            {
                image2 = new Bitmap(ms);
            }

            return image2;
        }
        public static BrandingPolicy GetBrandingPolicyData()
        {
            string policyObjectFilePath = @"C:\Program Files (x86)\Novell\ZENworks\conf\BrandingPolicy\BrandingPolicy.xml";
            BrandingPolicy policyData = DeserializeObjectFromXmlFile<BrandingPolicy>(policyObjectFilePath);
            return policyData;
        }
        public static T DeserializeObjectFromXmlFile<T>(string filePath)
        {            
            T outObject = default(T);
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                string xmlString = xmlDoc.OuterXml;

                using (StringReader reader = new StringReader(xmlString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (XmlReader xmlReader = new XmlTextReader(reader))
                    {
                        outObject = (T)serializer.Deserialize(xmlReader);
                        xmlReader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                
            }
            return outObject;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
            
        }
    }
}

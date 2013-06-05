using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace XmlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTypes.LevelType testData = new DataTypes.LevelType();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("tutorial2.xml", settings))
            {
                IntermediateSerializer.Serialize(writer, testData, null);
            }
        }
    }
}

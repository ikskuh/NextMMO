using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NextMMO
{
	public static class Xml
	{
		public static string Serialize(this object obj)
		{
			XmlSerializer ser = new XmlSerializer(typeof(object));
			using (var sw = new StringWriter(CultureInfo.InvariantCulture))
			{
				ser.Serialize(sw, obj);
				return sw.ToString();
			}
		}

		public static T Deserialize<T>(this string xml)
		{
			XmlSerializer ser = new XmlSerializer(typeof(T));
			using (var sr = new StringReader(xml))
			{
				return (T)ser.Deserialize(sr);
			}
		}
	}
}

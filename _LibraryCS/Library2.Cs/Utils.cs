﻿using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Library2.Cs
{
    public static class Utils
    {
        /// <summary>
        /// Remove all xmlns:* instances from the passed XmlDocument to simplify our xpath expressions
        /// </summary>
        public static XDocument RemoveNamespaces(XDocument oldXml)
        {
            // FROM: http://social.msdn.microsoft.com/Forums/en-US/bed57335-827a-4731-b6da-a7636ac29f21/xdocument-remove-namespace?forum=linqprojectgeneral
            try
            {
                XDocument newXml = XDocument.Parse(Regex.Replace(
                    oldXml.ToString(),
                    @"(xmlns:?[^=]*=[""][^""]*[""])",
                    "",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline)
                );
                return newXml;
            }
            catch (XmlException error)
            {
                throw new XmlException(error.Message + " at Utils.RemoveNamespaces");
            }
        }

        /// <summary>
        /// Remove all xmlns:* instances from the passed XmlDocument to simplify our xpath expressions
        /// </summary>
        public static XDocument RemoveNamespaces(string oldXml)
        {
            XDocument newXml = XDocument.Parse(oldXml);
            return RemoveNamespaces(newXml);
        }

        /// <summary>
        /// Converts a string that has been HTML-enconded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="escapedString">String to decode.</param>
        /// <returns>Decoded (unescaped) string.</returns>
        public static string UnescapeString(string escapedString)
        {
            return HttpUtility.HtmlDecode(escapedString);
        }
    }
}
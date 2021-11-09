using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netdockerworker
{
    public static class WebElementExtensions
    {
        public static IWebElement GetElementParentByClass(this IWebElement element, string elementType)
        {
            try
            {
                IWebElement parent = element;
                do
                {
                    parent = parent.FindElement(By.XPath("./parent::*")); //parent relative to current element

                } while (!parent.GetType().Equals(elementType));

                return parent;
            }
            catch
            {
                return null;
            }
        }
    }
}

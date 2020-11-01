using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SyncfusionMvcApplication3
{
    public partial class ComboBoxController: Controller
    {
        public ActionResult ComboBoxFeatures()
        {
List<CountryList> country = new List<CountryList>();
            List<groupsList> group = new List<groupsList>();
            group.Add(new groupsList { parentId = "a", text = "Group A" });
            group.Add(new groupsList { parentId = "b", text = "Group B" });
            group.Add(new groupsList { parentId = "c", text = "Group C" });
            group.Add(new groupsList { parentId = "d", text = "Group D" });
            group.Add(new groupsList { parentId = "e", text = "Group E" });
            ViewBag.datasource = group;
ViewBag.datasource1 = CountryList.GetCountries();
return View();
         } 
    }
public class CountryList
    {
        public int value { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public string sprite { get; set; }
        public string category { get; set; }
        public string iconclass { get; set; }
        public static List<CountryList> GetCountries()
        {
            List<CountryList> country = new List<CountryList>();
            country.Add(new CountryList { value = 11, parentId = "a", text = "Algeria", sprite = "flag-dz", iconclass = "flag flag-dz", category = "A"});
            country.Add(new CountryList { value = 12, parentId = "a", text = "Armenia", sprite = "flag-am", iconclass = "flag flag-am", category = "A" });
            country.Add(new CountryList { value = 13, parentId = "a", text = "Bangladesh", sprite = "flag-bd", iconclass = "flag flag-bd", category = "B" });
            country.Add(new CountryList { value = 14, parentId = "a", text = "Cuba", sprite = "flag-cu", iconclass = "flag flag-cu", category = "C" });
            country.Add(new CountryList { value = 15, parentId = "b", text = "Denmark", sprite = "flag-dk", iconclass = "flag flag-dk", category = "D" });
            country.Add(new CountryList { value = 16, parentId = "b", text = "Egypt", sprite = "flag-eg", iconclass = "flag flag-eg", category = "E" });
            country.Add(new CountryList { value = 17, parentId = "c", text = "Finland", sprite = "flag-fi", iconclass = "flag flag-fi", category = "F" });
            country.Add(new CountryList { value = 18, parentId = "c", text = "India", sprite = "flag-in", iconclass = "flag flag-in", category = "I" });
            country.Add(new CountryList { value = 19, parentId = "c", text = "Malaysia", sprite = "flag-my", iconclass = "flag flag-my", category = "M" });
            country.Add(new CountryList { value = 20, parentId = "d", text = "New Zealand", sprite = "flag-nz", iconclass = "flag flag-nz", category = "N" });
            country.Add(new CountryList { value = 21, parentId = "d", text = "Norway", sprite = "flag-no", iconclass = "flag flag-no", category = "N" });
            country.Add(new CountryList { value = 22, parentId = "d", text = "Poland", sprite = "flag-pl", iconclass = "flag flag-pl", category = "P" });
            country.Add(new CountryList { value = 23, parentId = "e", text = "Romania", sprite = "flag-ro", iconclass = "flag flag-ro", category = "R" });
            country.Add(new CountryList { value = 24, parentId = "e", text = "Singapore", sprite = "flag-sg", iconclass = "flag flag-sg", category = "S" });
            country.Add(new CountryList { value = 25, parentId = "e", text = "Thailand", sprite = "flag-th", iconclass = "flag flag-th", category = "T" });
            country.Add(new CountryList { value = 26, parentId = "e", text = "Ukraine", sprite = "flag-ua", iconclass = "flag flag-ua", category = "U" });
            return country;
        }
    }
    public class groupsList
    {
        public string text { get; set; }
        public string parentId { get; set; }
        public int id { get; set; }
        public int parent { get; set; }
    }
}

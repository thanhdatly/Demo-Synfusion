using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Syncfusion.SpellChecker.Base;
using SyncfusionMvcApplication3.Models;


namespace SyncfusionMvcApplication3.WebAPI
{
    public class SpellCheckController : ApiController
    {
        internal SpellCheckerBase baseDictionary, customDictionary;

        private readonly string _customFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Custom.dic"); // Here we need to specify the corresponding file name
        private readonly List<Status> errorWords = new List<Status>();

        SpellCheckController()
        {
            if (baseDictionary == null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Default.dic");
                // Here we need to specify the corresponding file name
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                baseDictionary = new SpellCheckerBase(stream);
            }
            this.CustomFileRead();
        }



        [AcceptVerbs("Get", "Post")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object AddToDictionary(string callback, string data)
        {
            var serializer = new JavaScriptSerializer();
            Actions args = (Actions)serializer.Deserialize(data, typeof(Actions));
            if (args.CustomWord != null)
            {
                this.AddToCustomDictionary(args.CustomWord);
            }
            HttpContext.Current.Response.Write(string.Format("{0}({1});", callback, serializer.Serialize(args.CustomWord)));
            return string.Empty;
        }

        [AcceptVerbs("Get", "Post")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CheckWords(string callback, string data)
        {
            var serializer = new JavaScriptSerializer();
            Actions args = (Actions)serializer.Deserialize(data, typeof(Actions));

            if (args.RequestType == "checkWords")
            {
                baseDictionary.IgnoreAlphaNumericWords = args.Model.IgnoreAlphaNumericWords;
                baseDictionary.IgnoreEmailAddress = args.Model.IgnoreEmailAddress;
                baseDictionary.IgnoreMixedCaseWords = args.Model.IgnoreMixedCaseWords;
                baseDictionary.IgnoreUpperCaseWords = args.Model.IgnoreUpperCase;
                baseDictionary.IgnoreUrl = args.Model.IgnoreUrl;
                baseDictionary.IgnoreFileNames = args.Model.IgnoreFileNames;
                var errorWords = this.SplitWords(args.Text);
                HttpContext.Current.Response.Write(string.Format("{0}({1});", callback, serializer.Serialize(errorWords)));
            }
            else if (args.RequestType == "getSuggestions")
            {
                var suggestions = baseDictionary.GetSuggestions(args.ErrorWord);
                HttpContext.Current.Response.Write(string.Format("{0}({1});", callback, serializer.Serialize(suggestions)));
            }
            return string.Empty;
        }


        #region helpers
        private void CustomFileRead()
        {
            Stream stream1 = new FileStream(_customFilePath, FileMode.Open, FileAccess.ReadWrite);
            customDictionary = new SpellCheckerBase(stream1);
        }
        private bool CheckWord(string word)
        {
            var flag = false;
            if (baseDictionary.HasError(word))
            {
                flag = true;
                using (StreamReader sr = new StreamReader(_customFilePath))
                {
                    var contents = sr.ReadToEnd();
                    if (contents != "")
                    {
                        flag = customDictionary.HasError(word) ? true : false;
                    }
                }
            }
            return flag;
        }

        private void AddToCustomDictionary(string word)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_customFilePath, true))
            {
                file.Write(word + Environment.NewLine);
            }
            this.CustomFileRead();
        }

        private List<Status> SplitWords(string text)
        {
            var words = text.Split(null);
            foreach (var word in words)
            {
                string textWord;
                Uri uriResult;
                bool checkUrl = Uri.TryCreate(word, UriKind.Absolute, out uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (checkUrl)
                {
                    textWord = word;
                    if (this.CheckWord(textWord))
                    {
                        this.GetStatus(textWord);
                    }
                }
                else if (Regex.IsMatch(word,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase))
                {
                    textWord = word;
                    if (this.CheckWord(textWord))
                    {
                        this.GetStatus(textWord);
                    }
                }
                else if (Regex.IsMatch(word, @"[a-zA-Z0-9_$\-\.\\]*\\[a-zA-Z0-9_$\-\.\\]+"))
                {
                    textWord = word;
                    if (this.CheckWord(textWord))
                    {
                        this.GetStatus(textWord);
                    }
                }
                else
                {
                    if (word.EndsWith(".") || word.EndsWith(","))
                    {
                        textWord = word.Remove(word.Length - 1);
                    }
                    else if (word.Contains('\t') || word.Contains('\n') || word.Contains('\r'))
                    {
                        textWord = Regex.Replace(word, @"\t|\n|\r", "");
                    }
                    else
                    {
                        textWord = word;
                    }
                    this.GetStatus(textWord);
                }
            }
            return errorWords;
        }

        private List<Status> GetStatus(string textWord)
        {
            var splitWords = Regex.Replace(textWord, @"[^0-9a-zA-Z\'_]", " ").Split(null);
            foreach (var inputWord in splitWords)
            {
                if (this.CheckWord(inputWord))
                {
                    errorWords.Add(new Status
                    {
                        ErrorWord = inputWord
                    });
                }
            }
            return errorWords;
        }
        #endregion
    }
}
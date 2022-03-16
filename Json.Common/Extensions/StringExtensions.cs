using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Json.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string initial)
            => Regex.Replace(initial,
                             // (Match any non punctuation) & then ignore any punctuation
                             @"([^\p{Pc}]+)[\p{Pc}]*",
                             new MatchEvaluator(mtch =>
                             {
                                 var word = mtch.Groups[1].Value.ToLower();

                                 return $"{Char.ToUpper(word[0])}{word[1..]}";
                             }));
    }
}

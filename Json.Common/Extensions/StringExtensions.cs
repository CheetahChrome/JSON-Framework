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
        /// <summary>
        /// Make a word pascal case if it is joined by an underscore or dash.
        /// </summary>
        /// <param name="initial">Initial string</param>
        /// <returns>A pascal cased word if it was originally joined by a dash or underscore.</returns>
        public static string ToPascalCase(this string initial)
        {
            var checkPattern = @"\p{Pc}";
            var pattern = @"([^\p{Pc}]+)[\p{Pc}]*";

            if (!Regex.IsMatch(initial, checkPattern)) // Ignore any words that are not joined.
                return initial;

            return Regex.Replace(initial,
                                     // (Match any non punctuation) & then ignore any punctuation
                                     pattern,
                                     new MatchEvaluator(mtch =>
                                     {
                                         var word = mtch.Groups[1].Value.ToLower();

                                         return $"{Char.ToUpper(word[0])}{word[1..]}";
                                     }));
        }

        /// <summary>
        /// Take a pascal cased string and make the first character lowercase and return.
        /// </summary>
        public static string ToFirstCharLower(this string initial)
            => Regex.Replace(initial, @"^(\w)", new MatchEvaluator((mtch) => mtch.Value.ToLower()));
    }
}

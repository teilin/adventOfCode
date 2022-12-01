using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static List<string> validItemTypes = new List<string>
    {
        "byr",
        "iyr",
        "eyr",
        "hgt",
        "hcl",
        "ecl",
        "pid",
        "cid"
    };
    static IList<bool> validationPassport = new List<bool>();
    static bool Validate(List<string> passport)
    {
        var passElm = new Dictionary<string,string>();
        foreach(var passPortLine in passport)
        {
            if(string.IsNullOrEmpty(passPortLine)) continue;
            var elm = passPortLine.Split(' ', StringSplitOptions.TrimEntries);
            foreach(var e in elm)
            {
                var elmType = e.Substring(0, e.IndexOf(":"));
                var t = e.IndexOf(":")+1;
                var elmValue = e.Substring(e.IndexOf(":")+1);
                if(validItemTypes.Contains(elmType)) passElm.Add(elmType, elmValue);
            }
        }
        return isValid(passElm);
    }

    static bool isValid(Dictionary<string,string> elmTypes)
    {
        if(elmTypes.Count == validItemTypes.Count) return hasValidData(elmTypes);
        else if(elmTypes.Count == validItemTypes.Count-1)
        {
            if(!elmTypes.ContainsKey("cid")) return hasValidData(elmTypes);
        }
        return false;
    }

    static bool hasValidData(Dictionary<string,string> elmtypes)
    {
        var numValid = 0;
        var isNumeric = false;
        foreach(var item in elmtypes)
        {
            switch(item.Key)
            {
                case "byr":
                    var byr = 0;
                    isNumeric = int.TryParse(item.Value, out byr);
                    if(isNumeric && item.Value.Length==4 && (byr >= 1920 && byr <= 2002)) numValid++;
                    break;
                case "iyr":
                    var iyr = 0;
                    isNumeric = int.TryParse(item.Value, out iyr);
                    if(isNumeric && item.Value.Length==4 && (iyr >= 2010 && iyr <= 2020)) numValid++;
                    break;
                case "eyr":
                    var eyr = 0;
                    isNumeric = int.TryParse(item.Value, out eyr);
                    if(isNumeric && item.Value.Length==4 && (eyr >= 2020 && eyr <= 2030)) numValid++;
                    break;
                case "hgt":
                    if(item.Value.EndsWith("cm") || item.Value.EndsWith("in"))
                    {
                        var unit = item.Value.Substring(item.Value.Length-2);
                        var height = item.Value.Replace(unit, "");
                        var h = 0;
                        isNumeric = int.TryParse(height, out h);
                        if(unit=="cm")
                            if(isNumeric && (h >= 150 && h <= 193)) numValid++;
                        if(unit=="in")
                            if(isNumeric && (h >= 59 && h <= 76)) numValid++;
                    }
                    break;
                case "hcl":
                    if(item.Value.StartsWith("#"))
                    {
                        var tmp = item.Value.Substring(1);
                        var isMatch = Regex.IsMatch(tmp, "^[a-f0-9]*$");
                        if(isMatch && tmp.Length==6) numValid++;
                    }
                    break;
                case "ecl":
                    var list = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                    if(list.Contains(item.Value)) numValid++;
                    break;
                case "pid":
                    var pid = 0;
                    isNumeric = int.TryParse(item.Value, out pid);
                    if(isNumeric && item.Value.Length==9) numValid++;
                    break;
                case "cid":
                    break;
                default:
                    break;
            }
        }
        return numValid==7;
    }

    static void Parse(string[] input)
    {
        var passPort = new List<string>();
        foreach(var line in input)
        {
            passPort.Add(line);
            if(string.IsNullOrEmpty(line))
            {
                validationPassport.Add(Validate(passPort));
                passPort.Clear();
            }
        }
        validationPassport.Add(Validate(passPort));
    }

    static void Main(string[] args)
    {
        var inputFile = File
            .ReadAllLines(@"passports2.txt");

        Parse(inputFile);
        var count = validationPassport.Count(c => c == true);
        Console.WriteLine(count);
    }
}
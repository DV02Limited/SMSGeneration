using System;
using System.Text.RegularExpressions;


namespace SMSGeneration
{
    public class SetVar
    {
        public string Smsnumber { get; set; }

        public string CreateSmsNumber(string emailBody)
        {
            Match match = new Regex("Usage for\\s(.\\d+)").Match(new HtmlToText().ConvertHtml(emailBody)); // Find mobile number in usage alert received from Daisy
            string pattern = "^0";
            string replacement = "44";
            string input = match.Groups[1].ToString();
            this.Smsnumber = new Regex(pattern).Replace(input, replacement); // Drop '0' from mobile number and convert into E.164 format
            return Smsnumber;
        }

        public string CreateSmsText(string Smsnumber, string emailBody)
        {
            // Create SMS text message for monetary usage alert and return it back to MiCC Enterprise
            return $"Dear Customer, Usage Limit for {this.Smsnumber} has exceeded £{(Decimal.Parse(new Regex("exceeded\\s£(.\\d+\\.\\d+)").Match(new HtmlToText().ConvertHtml(emailBody)).Groups[1].Value) * new Decimal(2))} Total Cost";
        }

        public string CreateSmsTextData(string emailBody)
        {
            // Create SMS text message for data usage alert and return it back to MiCC Enterprise
            return $"Dear Customer, Your data limit of {new Regex("exceeded\\s(.*)\\sTotal Data").Match(new HtmlToText().ConvertHtml(emailBody)).Groups[1].Value}MB has been reached and an Auto-Bar applied";
        }

        public string Customer(string emailBody)
        {
            Match match = new Regex("Cost Centre:\\s+(.*) > >").Match(new HtmlToText().ConvertHtml(emailBody)); // Find customer from received Daisy email
            if (match.Success)
                return match.Groups[1].Value; //return customer name back to MiCC Enterprise
            return "Error";
        }

        public string UsageType(string emailBody)
        {
            Match match = new Regex("Usage Alert - (.*)").Match(new HtmlToText().ConvertHtml(emailBody)); // Get usage alert type from received Daisy email
            if (match.Success)
                return match.Groups[1].Value; // Send back to MiCC Enterprise if usage alert if either cost or data
            return "Error";
        }

        public string Date(string emailBody)
        {
            Match match = new Regex("triggered on(.*)").Match(new HtmlToText().ConvertHtml(emailBody));
            if (match.Success)
                return match.Groups[1].Value;
            return "Error";
        }

        public string User(string emailBody)
        {
            Match match = new Regex("\\((.*)\\)").Match(new HtmlToText().ConvertHtml(emailBody)); // Get mobile user name from received Daisy email
            if (match.Success)
                return match.Groups[1].Value;
            return "Error";
        }

        public string DataUsage(string emailBody)
        {
            return new Regex("exceeded\\s(.*)\\sTotal Data").Match(new HtmlToText().ConvertHtml(emailBody)).Groups[1].Value;
        }
        
        public string MoneyUsage(string emailBody)
        {
            return (Decimal.Parse(new Regex("exceeded\\s£(.\\d+\\..\\d+)").Match(new HtmlToText().ConvertHtml(emailBody)).Groups[1].Value) * new Decimal(2)).ToString();
        }

    }
}

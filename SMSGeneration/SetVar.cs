using System.Text.RegularExpressions;


namespace SMSGeneration
{
    public class SetVar
    {
        public string Smsnumber { get; set; }

        public string CreateSmsNumber(string emailBody)
        {
            // Convert HTML Body Text into plain text
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);

            //Extract SMS number from email
            var mobileregex = new Regex(@"Usage for\s(.\d+)");
            var mobilematch = mobileregex.Match(emailBodyText);

            // Convert UK national SMS number into international format
            string pattern = "^0";
            string replacement = "44";
            string nationalSms = (mobilematch.Groups[1].ToString());
            Regex convertRgx = new Regex(pattern);
            Smsnumber = convertRgx.Replace(nationalSms, replacement);

            //Return SMS number back to MiCC Enterprise script
            return Smsnumber;
        }

        public string CreateSmsText(string emailBody)
        {
            // Convert HTML Body Text into plain text
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);

            //Extract usage limit from email
            var costregex = new Regex(@"exceeded\s£(.\d\..\d)");
            var costmatch = costregex.Match(emailBodyText);

            //Calculate customer usage limit
            decimal saleprice = decimal.Parse(costmatch.Groups[1].Value) * 2;

            //Create SMS Text
            string smstext = ($"Dear Customer, Usage Limit for {Smsnumber} has exceeded £{saleprice} Total Cost");

            //Return SMS text back to MiCC Enterprise
            return smstext;
        }

        public string CreateSmsTextData(string emailBody)
        {
            // Convert HTML Body text to plain text
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);

            // Extract Data Limit from email
            var dataregex = new Regex(@"exceeded (.*) Data");
            var datamatch = dataregex.Match(emailBodyText);

            // Create SMS Text
            string smstext = ($"Dear Customer, Your data limit of {datamatch.Groups[1].Value}MB has been reached and an Auto-Bar applied");
            // return text string
            return smstext;
        }

        public string Customer(string emailBody)
        {
            // Convert HTML Body to Plain text
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);

            Regex regex = new Regex(@"Cost Centre: (.*) > >");
            Match Customer = regex.Match(emailBodyText);

            if (Customer.Success)
            {
                return (Customer.Groups[1].Value);
            }
            else
            {
                return "Error";
            }

        }

        public string UsageType(string emailBody)
        {
            // Convert HTML Body to Plain text
            HtmlToText html = new HtmlToText();
            string emailBodyText = html.ConvertHtml(emailBody);

            // Extract Usage Type
            Regex regex = new Regex(@"Usage Alert - (.*)");
            Match Usage = regex.Match(emailBodyText);

            if (Usage.Success)
            {
                return (Usage.Groups[1].Value);
            }
            else
            {
                return "Error";
            }

        }

        public string Date(string emailBody)
        {
            HtmlToText html = new HtmlToText();
            string emailBodyText = html.ConvertHtml(emailBody);
            Regex regex = new Regex(@"triggered on(.*)");
            Match Date = regex.Match(emailBodyText);
            if (Date.Success)
            {
                return (Date.Groups[1].Value);
            }
            else
            {
                return "Error";
            }
        }

        public string User(string emailBody)
        {
            HtmlToText html = new HtmlToText();
            string emailBodyText = html.ConvertHtml(emailBody);
            Regex regex = new Regex(@"\((.*)\)");
            Match Username = regex.Match(emailBodyText);
            if (Username.Success)
            {
                return (Username.Groups[1].Value);
            }
            else
            {
                return "Error";
            }
        }

        public string DataUsage(string emailBody)
        {
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);
            Regex regex = new Regex(@"exceeded (.*) Data");
            Match Usage = regex.Match(emailBodyText);
            return (Usage.Groups[1].Value);
        }
        
        public string MoneyUsage(string emailBody)
        {
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);
            var costregex = new Regex(@"exceeded\s£(.\d\..\d)");
            var costmatch = costregex.Match(emailBodyText);

            //Calculate customer usage limit
            decimal saleprice = decimal.Parse(costmatch.Groups[1].Value) * 2;
            return saleprice.ToString();
        }

    }
}

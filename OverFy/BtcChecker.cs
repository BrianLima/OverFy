using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace OverFy
{
    static class BtcChecker
    {
        public static string GetPrice(string currency)
        {
            dynamic b;
            using (WebClient w = new WebClient())
            {
                b = JsonConvert.DeserializeObject(w.DownloadString(new Uri("https://blockchain.info/pt/ticker")));
            }
            switch (currency)
            {
                case "USD": return "1 BTC = " + b.USD.symbol + " " + Math.Round(Decimal.Parse(b.USD.last.ToString()), 2);
                case "JPY": return "1 BTC = " + b.JPY.symbol + " " + Math.Round(Decimal.Parse(b.JPY.last.ToString()), 2);
                case "CNY": return "1 BTC = " + b.CNY.symbol + " " + Math.Round(Decimal.Parse(b.CNY.last.ToString()), 2);
                case "SGD": return "1 BTC = " + b.SGD.symbol + " " + Math.Round(Decimal.Parse(b.SGD.last.ToString()), 2);
                case "HKD": return "1 BTC = " + b.HKD.symbol + " " + Math.Round(Decimal.Parse(b.HKD.last.ToString()), 2);
                case "CAD": return "1 BTC = " + b.CAD.symbol + " " + Math.Round(Decimal.Parse(b.CAD.last.ToString()), 2);
                case "NZD": return "1 BTC = " + b.NZD.symbol + " " + Math.Round(Decimal.Parse(b.NZD.last.ToString()), 2);
                case "AUD": return "1 BTC = " + b.AUD.symbol + " " + Math.Round(Decimal.Parse(b.AUD.last.ToString()), 2);
                case "CLP": return "1 BTC = " + b.CLP.symbol + " " + Math.Round(Decimal.Parse(b.CLP.last.ToString()), 2);
                case "GBP": return "1 BTC = " + b.GBP.symbol + " " + Math.Round(Decimal.Parse(b.GBP.last.ToString()), 2);
                case "DKK": return "1 BTC = " + b.DKK.symbol + " " + Math.Round(Decimal.Parse(b.DKK.last.ToString()), 2);
                case "SEK": return "1 BTC = " + b.SEK.symbol + " " + Math.Round(Decimal.Parse(b.SEK.last.ToString()), 2);
                case "ISK": return "1 BTC = " + b.ISK.symbol + " " + Math.Round(Decimal.Parse(b.ISK.last.ToString()), 2);
                case "CHF": return "1 BTC = " + b.CHF.symbol + " " + Math.Round(Decimal.Parse(b.CHF.last.ToString()), 2);
                case "BRL": return "1 BTC = " + b.BRL.symbol + " " + Math.Round(Decimal.Parse(b.BRL.last.ToString()), 2);
                case "EUR": return "1 BTC = " + b.EUR.symbol + " " + Math.Round(Decimal.Parse(b.EUR.last.ToString()), 2);
                case "RUB": return "1 BTC = " + b.RUB.symbol + " " + Math.Round(Decimal.Parse(b.RUB.last.ToString()), 2);
                case "PLN": return "1 BTC = " + b.PLN.symbol + " " + Math.Round(Decimal.Parse(b.PLN.last.ToString()), 2);
                case "THB": return "1 BTC = " + b.THB.symbol + " " + Math.Round(Decimal.Parse(b.THB.last.ToString()), 2);
                case "KRW": return "1 BTC = " + b.KRW.symbol + " " + Math.Round(Decimal.Parse(b.KRW.last.ToString()), 2);
                case "TWD": return "1 BTC = " + b.TWD.symbol + " " + Math.Round(Decimal.Parse(b.TWD.last.ToString()), 2);
                default: return "";
            }
        }
    }
}

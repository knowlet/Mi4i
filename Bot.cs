using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Mi4i
{
    class Bot
    {
        private CookieContainer cc;
        private SpWebClient spwc;
        private string user, pwd;
        public Bot()
        {
            this.cc = new CookieContainer();
            this.spwc = new SpWebClient(cc);
            this.spwc.Headers.Add(HttpRequestHeader.Accept, "*/*");
            // this.spwc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            this.spwc.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-TW,zh;q=0.8,en-US;q=0.6,en;q=0.4");
            // this.spwc.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
            this.spwc.Headers.Add(HttpRequestHeader.Referer, "https://account.xiaomi.com/pass/serviceLogin");
            this.spwc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.52 Safari/537.36");
            // this.spwc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            Console.WriteLine("歡迎使用小米手機自動搶購程式");
        }

        public void setUserData(string user, string pwd)
        {
            this.user = user;
            this.pwd = pwd;
        }

        public void login()
        {
            spwc.DownloadString("https://account.xiaomi.com/pass/serviceLogin");
            string capCode = "";
            NameValueCollection data = new NameValueCollection();
            data.Add("user", this.user);
            data.Add("_json", "true");
            data.Add("pwd", this.pwd);
            data.Add("callback", "https://account.xiaomi.com");
            data.Add("sid", "passport");
            data.Add("qs", "%3Fsid%3Dpassport");
            data.Add("hidden", "");
            data.Add("_sign", "KKkRvCpZoDC+gLdeyOsdMhwV0Xg=");
            data.Add("serviceParam", "{\"checkSafePhone\":false}");
            data.Add("captCode", "");
            do
            {
                data.Set("captCode", capCode);
                byte[] buf = spwc.UploadValues("https://account.xiaomi.com/pass/serviceLoginAuth2", data);
                string ret = Encoding.UTF8.GetString(buf);
                // string ret = spwc.UploadString(loginUri, param);
                if (ret.Contains("成功")) break;
                byte[] buffer = spwc.DownloadData("https://account.xiaomi.com" + Regex.Match(ret, @"/pass/getCode\?icodeType=login&\d*\.?\d+").Value);
                string path = Environment.CurrentDirectory + "\\Captcha.jpg";
                File.WriteAllBytes(path, buffer);
                Console.WriteLine("請輸入 " + path + " 驗證碼");
                capCode = Console.ReadLine();
            } while (true);
        }

        public bool buy()
        {
            string cart = spwc.DownloadString("http://buy.mi.com/tw/cart/add/4153500002");
            if (cart.Contains("商品沒有庫存了"))
            {
                Console.WriteLine("商品沒有庫存了");
                return false;
            }
            Console.WriteLine("成功添加 小米手機 4i 黑灰色 32GB 到購物車");
            string ret = spwc.DownloadString("https://buy.mi.com/tw/buy/checkout");
            string address_id = Regex.Match(ret, "address_id\":\"(\\d+)").Groups[1].Value;
            NameValueCollection data = new NameValueCollection();
            data.Add("Checkout[best_time]", "1");
            data.Add("Checkout[invoice_type]", "1");
            data.Add("Checkout[invoice_title]", "");
            data.Add("Checkout[invoice_company_code]", "");
            data.Add("Checkout[email]", this.user);
            data.Add("Checkout[is_donate]", "0");
            data.Add("Checkout[couponsValue]", "0");
            data.Add("Checkout[couponsType]", "no");
            data.Add("Checkout[address_id]", address_id);
            data.Add("Checkout[submit]", "成立訂單");
            data.Add("needCity", "0");
            spwc.UploadValues("https://buy.mi.com/tw/buy/checkout", data);
            Console.WriteLine("購買完成！請前往付款頁面：");
            Console.WriteLine(spwc.ResponseUri.ToString());
            return true;
        }
    }
}

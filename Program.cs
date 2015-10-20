using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi4i
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.setUserData("f108854e@opayq.com", "xenl.48dfz");
            bot.login();
            Console.WriteLine("使用倒數功能請輸入日期（如不需要請按Enter繼續）");
            string t = Console.ReadLine();
            if (!t.Equals(String.Empty))
            {
                DateTime dt = Convert.ToDateTime(t);
                TimeSpan ts = dt.Subtract(DateTime.Now);
                Console.WriteLine(ts.TotalSeconds);
                while (ts.TotalSeconds > 0)
                {
                    Console.Write("\r");
                    Console.Write("倒數：" + ts.TotalSeconds + "\t");
                    ts = dt.Subtract(DateTime.Now);
                }
                Console.WriteLine("\r倒數完成\t\t");
            }
            Console.WriteLine("請選擇購買方式：\n\t[1] 買一隻\n\t[2] 一直買");
            int choose = 0;
            Int32.TryParse(Console.ReadLine(), out choose);
            switch (choose)
            {
                case 1:
                    while (!bot.buy()) ;
                    break;
                case 2:
                    while (true)
                        bot.buy();
            }
            Console.WriteLine("感謝您的使用，我們下次再見");
            Console.ReadLine();
        }
    }
}

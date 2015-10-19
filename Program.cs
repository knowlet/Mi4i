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

using System;
using System.IO;
using System.Net;

namespace FacebookApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rnd = new Random();

            var accountId = "100013726390504";
            var interlocutor = "100013532889680";
            var body = "Hello!";
            var masssageId = "61959509515404160" + rnd.Next(10, 99);



            var url = "https://www.facebook.com/messaging/send/?dpr=1";

            var parameters =
                "&client=mercury" +
                "&action_type=ma-type:user-generated-message" +
                "&body=" + body +
                "&ephemeral_ttl_mode=0" +
                //"&force_sms=true" +
                "&has_attachment=false" +
                "&message_id=" + masssageId +
                "&offline_threading_id=" + masssageId +
                "&other_user_fbid=" + interlocutor +
                "&source=source:titan:web" +
                "&signature_id=6ec32383" +
                "&specific_to_list[0]=fbid:" + interlocutor +
                "&specific_to_list[1]=fbid:" + accountId +
                "&timestamp=1477228400458" + //+
                "&ui_push_phase=V3" +
                
                "&__user=" + accountId +
                "&__a=1" +
                "&__dyn=aihoFeyfyGmagngDxyG9gigmzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dgjyR88y8ixvrzEgVrDG4XzFE8oiGt0h9VojxCVEiGqdgkxvDAzUO5u5od8a8Cium8yUgx66EK2m5FUgC_UrQ4bBG7awxV8F3qzE" + //+
                "&__af=o" + //1
                "&__req=96" + //x
                "&__be=-1" +
                "&__pc=PHASED:DEFAULT" +
                "&fb_dtsg=AQHXI-PMhJk_:AQEgGrp3bo1j" +
                "&ttstamp=2658169885487834570706751586581701191115310411557738383" + //+
                "&__rev=2638327" + //+
                "&__srp_t=1477219353"; //+

            var coockies = "locale=ru_RU;" +
                           //"av=1;" +
                           "datr=rREdV7eqIbgh25Gal6Q9J9ZC;" +
                           "sb=ZA1EV8pIHILc8m8MpfzyOHHA;" +
                           "c_user=100013726390504;" +
                           "xs=40%3AGL9vgtMygUyU1A%3A2%3A1476897375%3A-1;" +
                           "fr=0almW4fUXZ8jobYqn.AWXAeGfL9FtndbxqHgYYZ2BDmLc.BXRA1l.uM.AAA.0.0.BYDLBc.AWVG6oTs;" + //+
                           "csm=2;" +
                           "s=Aa4JxX8cIBNkkbrq.BYB6pf;" +
                           "pl=n;" +
                           "lu=ggAF8SUUdyIdnGp-mB0GMomA;" +
                           "p=-2;" +
                           "act=1477224479925%2F25;" +
                           "wd=1920x974;" +
                           "presence=EDvF3EtimeF1477225047EuserFA21B13726390504A2EstateFDt2F_5bDiFA2user_3a1B13532889680A2ErF1C_5dElm2FA2user_3a1B13532889680A2Euct2F1477218753058EtrFnullEtwF3602623524EatF1477225047083G477225047117CEchFDp_5f1B13726390504F26CC;";
           
            var wb = new WebClient();
            wb.Headers.Add(HttpRequestHeader.Cookie, coockies);
            wb.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wb.Headers[HttpRequestHeader.Accept] = "*/*";
            //wb.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
            wb.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            wb.Headers[HttpRequestHeader.Referer] = "https://www.facebook.com/profile.php?id=100013532889680&sk=about"; //+
            wb.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
        
            string HtmlResult = wb.UploadString(url, parameters);

            
            var path = @"answer.txt";
            var sw = new StreamWriter(path, false);
            sw.WriteLine(HtmlResult);
        }
    }
}





//            var parameters =
//                "&client=mercury" +
//                "&action_type=ma-type:user-generated-message" +
//                "&body=Hello" +
//                "&ephemeral_ttl_mode=0" +
//                "&force_sms=true" +
//                "&has_attachment=false" +
//                "&message_id=6194557044580485791" +
//                "&offline_threading_id=6194557044580485791" +
//                "&other_user_fbid=223500233" +
//                "&source=source:titan:web" +
//                "&signature_id=6f66bfb5" +
//                "&specific_to_list[0]=fbid:223500233" +
//                "&specific_to_list[1]=fbid:100013726390504" +
//                "&timestamp=1476818665788" +
//                "&ui_push_phase=V3" +
//                "&__user=100013726390504" +
//                "&__a=1" +
//                "&__dyn=aihoFeyfyGmagngDxyG8EigmzFEbFbGA8Ay8Z9LFwxBxCbzEeAq2i5U4e2CEaUgxebkwy8wGFeex2uVWxeUWq264EK14DBwJKq4GCzEkxvDAzUO5u5o5S9ADBy8K48hxGbwYDx2r_xLggKm7U9eiax6ew" +
//                "&__af=-1" +
//                "&__req=x" +
//                "&__be=-1" +
//                "&__pc=PHASED:DEFAULT" +
//                "&fb_dtsg=AQHXI-PMhJk_:AQEgGrp3bo1j" +
//                "&ttstamp=2658172887345807710474107955865816910371114112519811149106" +
//                "&__rev=2631735" +
//                "&__srp_t=1476897375";
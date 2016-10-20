namespace DataBase.Models
{
    public class CookiesDbModel
    {
        private string _locale;
        private string _av;
        private string _datr;
        private string _sb;
        private string _cUser;
        private string _xs;
        private string _fr;
        private string _csm;
        private string _s;
        private string _pl;
        private string _lu;
        private string _p;
        private string _act;
        private string _wd;
        private string _presence;

        public long Id { get; set; }

        public long? AccountId { get; set; }

        public AccountDbModel Account { get; set; }

        public string Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = "locale=" + value;
            }
        }

        public string Av
        {
            get
            {
                return _av;
            }
            set
            {
                _av = "av=" + value;
            }
        }

        public string Datr
        {
            get
            {
                return _datr;
            }
            set
            {
                _datr = "datr=" + value;
            }
        }

        public string Sb
        {
            get
            {
                return _sb;
            }
            set
            {
                _sb = "sb=" + value;
            }
        }

        public string CUser
        {
            get
            {
                return _cUser;
            }
            set
            {
                _cUser = "cuser=" + value;
            }
        }

        public string Xs
        {
            get
            {
                return _xs;
            }
            set
            {
                _xs = "xs=" + value;
            }
        }

        public string Fr
        {
            get
            {
                return _fr;
            }
            set
            {
                _fr = "fr=" + value;
            }
        }

        public string Csm
        {

            get
            {
                return _csm;
            }
            set
            {
                _csm = "csm=" + value;
            }
        }

        public string S
        {
            get
            {
                return _s;
            }
            set
            {
                _s = "s=" + value;
            }
        }

        public string Pl
        {
            get
            {
                return _pl;
            }
            set
            {
                _pl = "pl=" + value;
            }
        }

        public string Lu
        {
            get
            {
                return _lu;
            }
            set
            {
                _lu = "lu=" + value;
            }
        }

        public string P
        {
            get
            {
                return _p;
            }
            set
            {
                _p = "p=" + value;
            }
        }

        public string Act
        {
            get
            {
                return _act;
            }
            set
            {
                _act = "act=" + value;
            }
        }

        public string Wd
        {
            get
            {
                return _wd;
            }
            set
            {
                _wd = "wd=" + value;
            }
        }

        public string Presence
        {
            get
            {
                return _presence;
            }
            set
            {
                _presence = "presence=" + value;
            }
        }
    }
}
namespace DataBase.QueriesAndCommands.Queries.UrlParameters.Models
{
    public class GeneralUrlParametersModel: IUrlParameters
    {
        private string _userId;
        private string _a;
        private string _dyn;
        private string _af;
        private string _req;
        private string _be;
        private string _pc;
        private string _fbDtsg;
        private string _ttstamp;
        private string _rev;
        private string _srpT;

        public string UserId
        {
            get { return "__user=" + _userId; }
            set { _userId = value; }
        }

        public string A
        {
            get { return "__a=" + _a; }
            set { _a = value; }
        }

        public string Dyn
        {
            get { return "__dyn=" + _dyn; }
            set { _dyn = value; }
        }

        public string Af
        {
            get { return "__af=" + _af; }
            set { _af = value; }
        }

        public string Req
        {
            get { return "__req=" + _req; }
            set { _req = value; }
        }

        public string Be
        {
            get { return "__be" + _be; }
            set { _be = value; }
        }

        public string Pc
        {
            get { return "__pc=" + _pc; }
            set { _pc = value; }
        }

        public string FbDtsg
        {
            get { return "fb_dtsg=" + _fbDtsg; }
            set { _fbDtsg = value; }
        }

        public string Ttstamp
        {
            get { return "ttstamp=" + _ttstamp; }
            set { _ttstamp = value; }
        }

        public string Rev
        {
            get { return "__rev=" + _rev; }
            set { _rev = value; }
        }

        public string SrpT
        {
            get { return "__srp_t=" + _srpT; }
            set { _srpT = value; }
        }
    }
}
